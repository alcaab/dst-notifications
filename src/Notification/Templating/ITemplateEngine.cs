using System.Collections.Generic;

namespace Desyco.Notification.Templating
{
    public interface ITemplateEngine
    {
        List<string> ReferencePaths { get; }
        ICompiledTemplateRef CompileTemplate(string content);
    }
}