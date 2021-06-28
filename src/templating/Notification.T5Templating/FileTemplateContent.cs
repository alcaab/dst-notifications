using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Desyco.Notification.Templating;
using Microsoft.Extensions.Logging;

namespace Desyco.Notification.T5Templating
{
    public class FileTemplateContentService : ITemplateContentProvider
    {
        private readonly ITemplateCompilerService _compiler;
        private readonly ILoggerFactory _loggerFactory;
        private readonly string _templateDirectory;

        public FileTemplateContentService(
            ITemplateCompilerService compiler,
            ILoggerFactory loggerFactory,
            NotificationOptions options)
        {
            _compiler = compiler;
            _loggerFactory = loggerFactory;
            _templateDirectory =
                options.GetExternalOptionsProvider<string>(NotificationConst.TemplateProviderType);

        }

        public string TemplateDirectory { get; set; }


        public async Task<string> GetTemplateContent(string templateKey, Dictionary<string, object> data)
        {
            var fileName = Path.Combine(_templateDirectory, templateKey);

            if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);

            var fileVesion = TimeSpan.FromTicks(File.GetLastWriteTime(fileName).Ticks);

            using (var reader = File.OpenText(fileName))
            {
                var content = await reader.ReadToEndAsync();
                return _compiler.Compile(templateKey, content, fileVesion, data);
            }
        }
    }
}