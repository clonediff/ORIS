using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.ServerInfrstructure.CookiesAndSessions
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CheckCookie : Attribute
    {
        ICookieValue CookieValue { get; }

        public CheckCookie(ICookieValue cookieValue) => CookieValue = cookieValue;
    }
}
