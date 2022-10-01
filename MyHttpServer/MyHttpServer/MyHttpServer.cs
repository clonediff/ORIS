using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;

namespace MyHttpServer
{
    public class MyHttpServer
    {
        HttpListener listener;
        public HttpListenerContext context;

        public bool IsActive => listener.IsListening;

        public MyHttpServer(string uri)
        {
            listener = new HttpListener();
            listener.Prefixes.Add(uri);
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine("Сервер запущен...");
        }

        public void Stop()
        {               
            listener.Stop();
            Console.WriteLine("Сервер остановлен");
        }

        public void Restart()
        {
            listener.Stop();
            listener.Start();
            Console.WriteLine("Сервер перезапущен...");
        }

        public async Task WaitContextAsync()
        {
            context = await listener.GetContextAsync();
        }

        internal void CreateResponse()
        {
            try
            {
                if (listener.IsListening)
                {
                    var request = context.Request;
                    var response = context.Response;

                    var rawUrl = request.RawUrl;
                    byte[] buffer;
                    switch (rawUrl)
                    {
                        case "/google":
                            string googlePage = File.ReadAllText(Path.Combine("Templates", "Google", "index.html"));
                            buffer = Encoding.UTF8.GetBytes(googlePage);
                            break;
                        default:
                            string defaultPage = "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
                            buffer = Encoding.UTF8.GetBytes(defaultPage);
                            break;
                    }

                    response.ContentLength64 = buffer.Length;

                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Stop();
            }
        }

        public bool WaitCommands()
        {
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
                return false;
            switch (input)
            {
                case "start":
                    if (!listener.IsListening)
                        Start();
                    break;
                case "stop":
                    Stop();
                    break;
                case "restart":
                    Restart();
                    break;
                case "exit":
                    return true;
                case "cls":
                    Console.Clear();
                    break;
            }
            return false;
        }
    }
}
