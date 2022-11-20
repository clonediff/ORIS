using HttpServer2.Routing;
using HttpServer2.Routing.Attributes;
using HttpServer2.ServerInfrstructure.CookiesAndSessions;
using HttpServer2.ServerResponse;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace HttpServer2.Attributes
{
    public class AttributeRequestsHandler
    {
        public static readonly RouteTree routes;

        static AttributeRequestsHandler()
        {
            routes = new();

            var controllerTypes = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => Attribute.IsDefined(x, typeof(ApiControllerAttribute)));

            foreach (var controller in controllerTypes)
            {
                var controllerRoute = controller.GetCustomAttribute<ApiControllerAttribute>()!.Uri?.Trim('/') ?? controller.Name;
                foreach (var method in controller.GetMethods().Where(x => Attribute.IsDefined(x, typeof(HttpMethodAttribute))))
                {
                    var methodRoute = method.GetCustomAttribute<HttpMethodAttribute>()!.Uri?.Trim('/') ?? method.Name;
                    string fullRoute = string.Join('/', controllerRoute, methodRoute);
                    if (string.IsNullOrEmpty(methodRoute))
                        fullRoute = controllerRoute;
                    var methodArgumentToOrderIndex = GetMethodArgumentToOrderIndex(method);
                    var methodTypeAttr = method.GetCustomAttribute<HttpMethodAttribute>()!.GetType();
                    var methodType = methodTypeAttr.Name.Replace("Http", "").Replace("Attribute", "");
                    routes.AddRoute(Enum.Parse<HttpMethod>(methodType), fullRoute, method, methodArgumentToOrderIndex);
                }
            }
        }

        static Dictionary<string, (int index, Type type)> GetMethodArgumentToOrderIndex(MethodInfo method)
            => method
                .GetParameters()
                .Select((x, i) => (Name: x.Name!, i, type: x.ParameterType))
                .ToDictionary(pair => pair.Name, pair => (pair.i, pair.type));

        public bool HandleController(MyContext context)
        {
            var request = context.Context.Request;
            var response = context.Context.Response;

            if (!routes.TryNavigate(Enum.Parse<HttpMethod>(request.HttpMethod),
                request.RawUrl!.Substring(1),
                request.InputStream, request.ContentEncoding,
                out var method, out var parameters))
                return false;

            if (!CheckCookies(context, method, out var notfound))
            {
                notfound.ExecuteResult(context);
                response.OutputStream.Close();
                return true;
            }

            var controller = Activator.CreateInstance(method.DeclaringType!);

            foreach (var x in method.DeclaringType!.GetFields(
                BindingFlags.NonPublic |
                BindingFlags.Instance).Where(x => x.FieldType == typeof(MyORM)))
                x.FieldType.GetField("connectionString")?.SetValue(x.GetValue(controller), context.Settings.DBConnectionString);


            var ret = method.Invoke(controller, parameters);

            if (ret is IControllerResult result)
                result.ExecuteResult(context);
            else
                new DefaultJsonResult(ret).ExecuteResult(context);

            response.OutputStream.Close();

            return true;
        }

        bool CheckCookies(MyContext context, MethodInfo method, out IControllerResult notFound)
        {
            var request = context.Context.Request;
            var response = context.Context.Response;

            var checkCookies = method
                .GetCustomAttributes<CheckCookie>();

            notFound = default!;

            foreach (var checkCookie in checkCookies)
            {
                var cookieType = checkCookie.Type;
                var cookieInst = Activator.CreateInstance(cookieType) as ICookieValue;
                if (cookieInst == null) throw new ArgumentException("CheckValue must contains only ICookieValue type for checking");
                var cookieName = cookieType.Name.Replace("Cookie", "");
                var foundCookie = request.Cookies[cookieName];
                if (foundCookie is null)
                {
                    notFound = cookieInst!.IfNotExists;
                    return false;
                }
                var cookieValue = CookieValueSerializer.Deserialize(foundCookie.Value, cookieType);
                var property = cookieValue!.GetType().GetProperty(checkCookie.PropertyName);
                if (!checkCookie.Value.Equals(property.GetValue(cookieValue)))
                    return false;
            }

            return true;
        }
    }
}
