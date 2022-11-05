using HttpServer2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2
{
    public interface IAccountsDAO
    {
        IEnumerable<Account> GetAll();
        int Update(Account obj);
        Account? GetById(int id);
        int Delete(Account obj);
        int Create(Account obj);
    }
}
