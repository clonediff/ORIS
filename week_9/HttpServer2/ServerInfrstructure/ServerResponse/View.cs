using HttpServer2.Routing;
using RazorLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2.ServerResponse
{
    public class View : IControllerResult
    {
        string _viewPath;
        object _model;

        public View(string viewPath) => _viewPath = viewPath;
        public View(string viewPath, object model) : this(viewPath)
            => _model = model;

        public async void ExecuteResult(MyContext context)
        {
            var result = await context.Razor.CompileRenderAsync(_viewPath, _model);
            context.Context.Response.WriteBody(
                context.Context.Request.ContentEncoding.GetBytes(result),
                "text/html");
        }
    }
}
