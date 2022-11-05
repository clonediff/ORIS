using HttpServer2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HttpServer2
{
    public class MyORM
    {
        public string connectionString;
        static Dictionary<Type, string> tableNames;
        static Dictionary<PropertyInfo, string> columnNames;
        static Dictionary<(Type table, string column), PropertyInfo> propertyByColumnName;
        Dictionary<PropertyInfo, int> columnInTable;

        static MyORM()
        {
            tableNames = new();
            columnNames = new();
            propertyByColumnName = new();

            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.GetInterfaces().Any(i => i.Equals(typeof(IModel))));

            foreach (var type in types)
            {
                var tableName = type
                    .GetCustomAttributes<Table>(false)
                    .FirstOrDefault(new Table(type.Name + "s"));
                tableNames[type] = tableName.Name;
            }

            var properties = types.SelectMany(x => x.GetProperties());
            foreach (var property in properties)
            {
                var columnName = property
                    .GetCustomAttributes<Column>(false)
                    .FirstOrDefault(new Column(property.Name));
                columnNames[property] = columnName.Name;
                var key = (property.DeclaringType, columnName.Name);
                if (propertyByColumnName.ContainsKey(key))
                    throw new InvalidConstraintException("Внутри одной модели свойства должны иметь разные названия для столбцов");
                propertyByColumnName[key] = property;
            }
        }

        public MyORM(string connectionString)
        {
            this.connectionString = connectionString;
            //DBTablesCreator.CreateTables(tableNames, columnNames, this.connectionString);
            columnInTable = new();
        }

        public int ExecuteNonQuery(string query)
        {
            int affectedRows = default;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCommand(query, connection);
                affectedRows = cmd.ExecuteNonQuery();
            }
            return affectedRows;
        }

        public int Insert<T>(T obj)
            where T : IModel
        {
            if (obj is null)
                throw new ArgumentNullException();
            var properties = typeof(T).GetProperties().Where(x => x.Name != "Id");
            var propertyNamesInTable = properties.Select(x => columnNames[x]);
            var query = $"INSERT INTO {tableNames[typeof(T)]}\n" +
                $"VALUES ({string.Join(", ", properties.Select(x => $"'{x.GetValue(obj)}'"))})";

            return ExecuteNonQuery(query);
        }

        public int Update<T>(T obj)
            where T : IModel
        {
            if (obj is null)
                throw new ArgumentNullException();
            var properties = typeof(T).GetProperties().Where(x => x.Name != "Id");
            var propertyNamesInTable = properties.Select(x => columnNames[x]);
            var query = $"UPDATE {tableNames[typeof(T)]}\n" +
            $"SET {string.Join(", ", properties.Zip(propertyNamesInTable).Select(x => $"{x.Second} = '{x.First.GetValue(obj)}'"))}" +
            $"WHERE Id = {obj.Id}";

            return ExecuteNonQuery(query);
        }

        public int Delete<T>(T obj)
            where T : IModel
        {
            if (obj is null)
                throw new ArgumentNullException();
            var properties = typeof(T).GetProperties();
            var propertyNamesInTable = properties.Select(x => columnNames[x]);
            var query = $"DELETE FROM {tableNames[typeof(T)]}\n" +
            $"WHERE {string.Join(" AND ", properties.Zip(propertyNamesInTable).Select(x => $"{x.Second} = '{x.First.GetValue(obj)}'"))}";

            return ExecuteNonQuery(query);
        }

        public IEnumerable<T> Select<T>()
            where T : IModel
        {
            var result = new List<T>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string selectCmd = $"SELECT * FROM {tableNames[typeof(T)]}";
                var cmd = new SqlCommand(selectCmd, connection);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var properties = typeof(T).GetProperties();
                    var propertyNamesInTable = properties.Select(x => columnNames[x]);

                    while (reader.Read())
                    {
                        var objT = Activator.CreateInstance(typeof(T));
                        foreach (var property in properties.Zip(propertyNamesInTable))
                            property.First.SetValue(objT, reader.GetValue(reader.GetOrdinal(property.Second)));
                        result.Add((T)objT);
                    }
                }
            }

            return result;
        }
    }
}
