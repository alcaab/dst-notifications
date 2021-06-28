using System.Collections.Generic;
using Desyco.Notification.Templating;
using Desyco.T5Templating.TextTemplating;

namespace Desyco.Notification.T5Templating
{
    public class T5CompilerEngine : ITemplateEngine
    {
        private readonly TemplateGenerator _engine;

        public T5CompilerEngine()
        {
            _engine = new TemplateGenerator();
            ReferencePaths = new List<string>();
        }

        public ICompiledTemplateRef CompileTemplate(string content)
        {
            _engine.ReferencePaths.AddRange(ReferencePaths);
            return new T5CompiledTemplateRef(_engine.CompileTemplate(content));
        }

        public List<string> ReferencePaths { get; }
    }
}