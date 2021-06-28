using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Desyco.Notification.Templating
{
    public interface ITemplateCompilerService
    {
        Dictionary<string, CompiledTemplateResult> CompiledTemplates { get; }

        string Compile(string templateKey, string content, TimeSpan version, Dictionary<string, object> data);

        string Compile(CompilerOptions options);

        string Process(CompiledTemplateResult result, Dictionary<string, object> data);
        Task Start();
        Task Stop();
    }
}