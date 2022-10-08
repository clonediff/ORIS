namespace HttpServer2
{
    public static class Constants
    {
        public const string NotFoundPage =
@"<!DOCTYPE html> 
<html>
    <head>
        <meta charset=""utf-8"" />
        <title>404 Not Found</title>
    </head>
    <body style=""display:flex; justify-content:center;"">
        <h1>404 Not Found</h1>
    </body>
</html>";
    }

    public class ServerSettings
    {
        public int Port { get; init; }
        public PageSetting Google { get; init; }
        public PageSetting Steam { get; init; }
        public PageSetting MainPage { get; init;  }
    }

    public class PageSetting
    {
        public string RawUrl { get; init; }
        public string MainFolder { get; init; }
        public string MainHTML { get; init; }
    }
}
