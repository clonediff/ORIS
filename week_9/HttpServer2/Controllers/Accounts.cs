using HttpServer2.Attributes;
using HttpServer2.Models;
using HttpServer2.ServerInfrstructure.CookiesAndSessions;
using HttpServer2.ServerInfrstructure.CookiesAndSessions.Attributes;
using HttpServer2.ServerInfrstructure.ServerResponse;
using HttpServer2.ServerResponse;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace HttpServer2.Controllers
{
    [ApiController]
    public class Accounts
    {
        MyORM orm = new MyORM();

        [HttpGET("/")]
        public IEnumerable<Account> GetAccounts()
        {
            return orm.Select<Account>();
        }

        [HttpGET("/{id}")]
        public Account? GetAccountById(int id)
        {
            return orm
                .Select<Account>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        [HttpPOST("/")]
        public IControllerResult PostAccounts(string login, string password)
        {
            var account = orm.Select<Account>().Where(x => x.Login == login && x.Password == password).FirstOrDefault();
            
            if 
            var cookie = (account is not null, new SessionIdCookie { IsAuthorize = true, Id = account?.Id ?? -1 }, TimeSpan.FromMinutes(5));
            return new CookieResult(cookie);
        }
    }
}
