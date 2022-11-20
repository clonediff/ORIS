using HttpServer2.ServerInfrstructure.CookiesAndSessions;
using HttpServer2.ServerInfrstructure.ServerResponse;
using HttpServer2.ServerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace HttpServer2.MyCookieValues
{
    internal class SessionIdCookie : ICookieValue
    {
        public bool IsAuthorize { get; set; }
        public int? Id { get; set; }

        [JsonIgnore]
        public IControllerResult IfNotExists { get; } = new NotAuthorized();

        public Cookie AsCookie(TimeSpan expires)
        {
            var value = CookieValueSerializer.Serialize(this);
            return new Cookie { Name = "SessionId", Value = value, Expires = DateTime.Now + expires };
        }
    }
}
