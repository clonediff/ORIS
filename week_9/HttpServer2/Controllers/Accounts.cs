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
        [CheckCookie(typeof(SessionIdCookie))]
        public IEnumerable<Account> GetAccounts()
        {
            return orm.Select<Account>();
        }

        [HttpGET("/info")]
        [CheckCookie(typeof(SessionIdCookie))]
        public Account? GetAccountInfo(
            [FromCookie(typeof(SessionIdCookie), nameof(SessionIdCookie.SessionId))] Guid sessionId)
        {
            var sessionManager = SessionManager.Inst;
            var accountId = sessionManager.GetSessionInfo(sessionId).AccountId;
            return orm
                .Select<Account>()
                .Where(x => x.Id == accountId)
                .FirstOrDefault();
        }

        [HttpGET("/{id}")]
        [CheckCookie(typeof(SessionIdCookie))]
        public Account? GetAccountById(int id)
        {
            return orm
                .Select<Account>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        [HttpPOST("/")]
        public IControllerResult Login(string login, string password)
        {
            var account = orm.Select<Account>().Where(x => x.Login == login && x.Password == password).FirstOrDefault();
            var cookies = new List<(ICookieValue, TimeSpan)>();
            if (account is not null)
            {
                var sessionManager = SessionManager.Inst;
                var session = sessionManager.CreateSession(account.Id, login);
                cookies.Add((new SessionIdCookie { SessionId = session.Id }, default));
            }
            return new CookieResult(cookies);
        }
    }
}
