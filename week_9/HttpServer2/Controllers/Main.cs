using HttpServer2.Attributes;
using HttpServer2.ServerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.Controllers
{
    [ApiController("/")]
    internal class Main
    {
        [HttpGET("/")]
        public IControllerResult GetMainPage()
        {
            return new View("index");
        }
    }
}
