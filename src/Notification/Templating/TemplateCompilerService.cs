using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Desyco.Notification.Templating
{
    public abstract class TemplateCompilerService : ITemplateCompilerService
    {
        private readonly ITemplateEngine _templateEngine;

        protected TemplateCompilerService(ITemplateEngine templateEngine)
        {
            _templateEngine = templateEngine;
            CompiledTemplates = new Dictionary<string, CompiledTemplateResult>();
        }

        public Dictionary<string, CompiledTemplateResult> CompiledTemplates { get; }

        public string Compile(CompilerOptions options)
        {
            return Compile(options.TemplateKey, options.Content, options.Version, options.Data);
        }

        public string Compile(string templateKey, string content, TimeSpan version, Dictionary<string, object> data)
        {
            //agrega de forma automatica los tipos de objectos enviados como parametros
            foreach (var item in data)
            {
                var t = item.Value.GetType().Name;
                var path = Path.GetDirectoryName(item.Value.GetType().Assembly.Location);
                if (!_templateEngine.ReferencePaths.Contains(path))
                    _templateEngine.ReferencePaths.Add(path);
            }

            var tcr = Compile(templateKey, content, version);
            return Process(tcr, data);
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            CompiledTemplates.Clear();
            return Task.CompletedTask;
        }

        public virtual string Process(CompiledTemplateResult result, Dictionary<string, object> data)
        {
            return result.CompiledTemplate.Process();
        }

        private CompiledTemplateResult Compile(string templateKey, string content, TimeSpan version)
        {
            if (CompiledTemplates.ContainsKey(templateKey))
            {
                //obliga a recompilar porque la version guardad no concuerda con la enviada
                if (CompiledTemplates[templateKey].Version.CompareTo(version) == 0)
                    return CompiledTemplates[templateKey];

                CompiledTemplates[templateKey].Version = version;
                CompiledTemplates[templateKey].CompiledTemplate = _templateEngine.CompileTemplate(content);

                return CompiledTemplates[templateKey];
            }

            CompiledTemplates.Add(templateKey, new CompiledTemplateResult
            {
                TemplateKey = templateKey,
                Version = version,
                CompiledTemplate = _templateEngine.CompileTemplate(content)
            });

            return CompiledTemplates[templateKey];
        }
    }

    public class DefaultTemplateCompilerService : TemplateCompilerService
    {
        public DefaultTemplateCompilerService(ITemplateEngine templateEngine) : base(templateEngine)
        {
        }
    }
}