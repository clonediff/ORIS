using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.ServerInfrstructure.CookiesAndSessions
{
    public interface ICookieValue
    {
        Cookie AsCookie(TimeSpan expires);
    }
}
