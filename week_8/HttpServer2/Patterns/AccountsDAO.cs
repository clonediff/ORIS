using HttpServer2.Models;

namespace HttpServer2
{
    public class AccountsDAO : IAccountsDAO
    {
        private MyORM orm = new MyORM(Constants.SteamDBConnectionString);

        public int Create(Account obj)
            => orm.Insert(obj);

        public int Delete(Account obj)
            => orm.Delete(obj);

        public IEnumerable<Account> GetAll()
            => orm.Select<Account>();

        public Account? GetById(int id)
            => GetAll().FirstOrDefault(a => a.Id == id);

        public int Update(Account obj)
            => orm.Update(obj);
    }
}
