using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HttpServer2
{
    class GetQueryHandler
    {
        ServerSettings _settings;
        public GetQueryHandler(ServerSettings settings)
        {
            _settings = settings;
        }

        public void HandleGetQuery(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            var filePath = GetFilePath(request.UrlReferrer?.LocalPath, request.RawUrl!);

            byte[] buffer;
            if (File.Exists(filePath))
            {
                buffer = File.ReadAllBytes(filePath);
                response.ContentType = ContentTypeIdentifier.GetContentType(filePath);
            } else
            {
                buffer = Encoding.UTF8.GetBytes(Constants.NotFoundPage);
                response.ContentType = "text/html; charset=UTF-8";
                response.StatusCode = 404;
            }
            response.ContentLength64 = buffer.Length;

            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }

        public string GetFilePath(string? referrerLocalPath, string rawUrl)
        {
            var filePath = "";
            if (string.IsNullOrEmpty(referrerLocalPath))
            {
                if (rawUrl == _settings.Google.RawUrl)
                    filePath = Path.Combine(_settings.Google.MainFolder, _settings.Google.MainHTML);
                else if (rawUrl == _settings.Steam.RawUrl)
                    filePath = Path.Combine(_settings.Steam.MainFolder, _settings.Steam.MainHTML);
                else if (rawUrl == _settings.MainPage.RawUrl)
                    filePath = Path.Combine(_settings.MainPage.MainFolder, _settings.Steam.MainHTML);

            }
            else if (referrerLocalPath == _settings.Google.RawUrl)
            {
                filePath = Path.Combine(_settings.Google.MainFolder, rawUrl.Substring(1));
            }
            else if (referrerLocalPath == _settings.Steam.RawUrl)
            {
                filePath = Path.Combine(_settings.Steam.MainFolder, rawUrl.Substring(1));
            }
            return filePath;
        }
    }
}
