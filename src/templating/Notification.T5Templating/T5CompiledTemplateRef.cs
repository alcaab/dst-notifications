using Desyco.Notification.Templating;
using Desyco.T5Templating.TextTemplating;

namespace Desyco.Notification.T5Templating
{
    public class T5CompiledTemplateRef : ICompiledTemplateRef
    {
        private readonly CompiledTemplate _compiledTemplate;

        public T5CompiledTemplateRef(CompiledTemplate compiledTemplate)
        {
            _compiledTemplate = compiledTemplate;
        }

        public string Content { get; set; }
        public object TextTransformation => _compiledTemplate.TextTransformation;

        public string Process()
        {
            return _compiledTemplate.Process();
        }
    }
}