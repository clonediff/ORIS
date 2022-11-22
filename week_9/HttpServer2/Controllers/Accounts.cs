using HttpServer2.Attributes;
using HttpServer2.Models;
using HttpServer2.MyCookieValues;
using HttpServer2.ServerInfrstructure.CookiesAndSessions;
using HttpServer2.ServerInfrstructure.ServerResponse;
using HttpServer2.ServerResponse;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpServer2.Controllers
{
    [ApiController]
    public class Accounts
    {
        MyORM orm = MyORM.Instance;



        [HttpGET("/")]
        [CheckCookie(typeof(SessionIdCookie), nameof(SessionIdCookie.IsAuthorize), true)]
        public IEnumerable<Account> GetAccounts()
        {
            return orm.Select<Account>();
        }

        [HttpGET("/info")]
        [CheckCookie(typeof(SessionIdCookie), nameof(SessionIdCookie.IsAuthorize), true)]
        public Account? GetAccountInfo(
            [FromCookie(typeof(SessionIdCookie), nameof(SessionIdCookie.Id))] int id)
        {
            return orm
                .Select<Account>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        [HttpGET("/{id}")]
        [CheckCookie(typeof(SessionIdCookie), nameof(SessionIdCookie.IsAuthorize), true)]
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
            var cookies = new List<(bool, ICookieValue, TimeSpan)>();
            if (account is not null)
                cookies.Add((true, new SessionIdCookie { IsAuthorize = true, Id = account.Id }, TimeSpan.FromMinutes(3)));
            return new CookieResult(cookies);
        }
    }
}
