using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Services
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public Task DeleteFile(string path, string container)
        {
            if (path != null)
            {
                var fileName = Path.GetFileName(path);
                string directory = Path.Combine(_env.WebRootPath, container, fileName);

                if (File.Exists(directory))
                    File.Delete(directory);
            }

            return Task.FromResult(0);
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string path, string contentType)
        {
            await DeleteFile(path, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(_env.WebRootPath, container);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = Path.Combine(folder, fileName);

            await File.WriteAllBytesAsync(path, content);

            var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var urlDB = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");

            return urlDB;
        }
    }
}
