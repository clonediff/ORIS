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
    public class CheckCookie<T> : Attribute
        where T : ICookieValue
    {
        public string PropertyName { get; }
        public object? Value { get; }

        public CheckCookie(string name, object? value)
        {
            PropertyName = name;
            Value = value;
        }
    }
}
