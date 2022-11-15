using RazorLight;
using System.IO;
using System.Net;
using System.Reflection;

namespace HttpServer2.Routing
{
    public class MyContext
    {
        public HttpListenerContext Context;
        public ServerSettings Settings;
        public RazorLightEngine Razor;
        public MyContext(HttpListenerContext context, ServerSettings settings)
        {
            Context = context;
            Settings = settings;

            Razor = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), Settings.TemplatesFolder))
            .UseMemoryCachingProvider()
            .Build();
        }
    }
}
