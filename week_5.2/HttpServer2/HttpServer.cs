using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpServer2;

public class HttpServer : IDisposable
{
    internal HttpListener _listener;
    public ServerStatus Status = ServerStatus.Stop;
    private ServerSettings _settings;

    public HttpServer()
    {
        _listener = new();
    }

    public void Start()
    {
        if (Status == ServerStatus.Start)
        {
            Console.WriteLine("Сервер уже запущен");
            return;
        }

        var settingsPath = "settings.json";
        if (File.Exists(settingsPath))
            _settings = JsonSerializer.Deserialize<ServerSettings>(File.ReadAllText(settingsPath));
        else
        {
            Console.WriteLine("Не найден файл настроек. Невозможно запустить сервер");
            return;
        }

        _listener.Prefixes.Clear();
        _listener.Prefixes.Add($"http://localhost:{_settings.Port}/");
        Console.WriteLine("Запуск сервера...");
        _listener.Start();
        Status = ServerStatus.Start;
        Console.WriteLine("Сервер запущен");

        ListeningAsync();
    }

    public void Stop()
    {
        if (Status == ServerStatus.Stop) return;

        Console.WriteLine("Остановка сервера...");
        _listener.Stop();
        Status = ServerStatus.Stop;
        Console.WriteLine("Сервер остановлен");
    }

    public void Restart()
    {
        if (Status == ServerStatus.Stop)
        {
            Console.WriteLine("Сервер уже остановлен. Для запуска используйте start вместо restart");
            return;
        }

        //Console.WriteLine("Перезапуск сервера...");
        Stop();
        Start();
        //Console.WriteLine("Сервер перезапущен");
    }

    private async Task ListeningAsync()
    {
        while (Status == ServerStatus.Start)
        {
            var context = await _listener.GetContextAsync();
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    new GetQueryHandler(_settings).HandleGetQuery(context);
                    break;
                case "POST":
                    PostQueryHandler.HandlePostQuery(context, _settings);
                    break;
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}
