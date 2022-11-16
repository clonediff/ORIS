using HttpServer2.ServerInfrstructure.CookiesAndSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace HttpServer2.MyCookieValues
{
    internal class SessionIdCookie : ICookieValue
    {
        public bool IsAuthorize { get; set; }
        public int Id { get; set; }

        public Cookie AsCookie(TimeSpan expires)
        {
            var value = JsonSerializer.Serialize(this);
            return new Cookie { Name = "SessionId", Value = value, Expires = DateTime.Now + expires };
        }
    }
}
