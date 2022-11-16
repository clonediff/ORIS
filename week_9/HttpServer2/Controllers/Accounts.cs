using HttpServer2.Attributes;
using HttpServer2.Models;
using HttpServer2.ServerResponse;
using System.Collections.Generic;
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
        public bool PostAccounts(string login, string password)
        {
            return orm.Select<Account>().Where(x => x.Login == login && x.Password == password).Any();
        }
    }
}
