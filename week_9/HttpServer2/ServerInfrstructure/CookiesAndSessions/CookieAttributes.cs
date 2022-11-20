using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.ServerInfrstructure.CookiesAndSessions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckCookie : Attribute
    {
        public Type Type { get; }

        public string PropertyName { get; }
        public object? Value { get; }

        public CheckCookie(Type type, string name, object? value)
        {
            Type = type;
            PropertyName = name;
            Value = value;
        }
    }
}
