using System.Collections.Generic;

namespace Desyco.Notification.Templating
{

    public interface ICompiledTemplateRef
    {
        string Content { get; set; }
        object TextTransformation { get; }
        string Process();
    }

    public class CompiledTemplateRef : ICompiledTemplateRef
    {
        public string Content { get; set; }
        public object TextTransformation => new object();

        public string Process()
        {
            return Content;
        }
    }

    public class DefaultTemplateEngine : ITemplateEngine
    {
        public ICompiledTemplateRef CompileTemplate(string content)
        {
            return new CompiledTemplateRef
            {
                Content = content
            };
        }

        public List<string> ReferencePaths { get; } = new List<string>();
    }
}