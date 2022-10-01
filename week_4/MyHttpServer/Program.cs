using System.Net;
using System.Net.Http.Headers;

namespace MyHttpServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var server = new MyHttpServer("http://localhost:8888/");

            server.Start();

            while (true)
            {
                await server.WaitContextAsync().ContinueWith(t => server.CreateResponse());
                if (!server.IsActive)
                    if (server.WaitCommands())
                        return;
            }
        }
    }
}