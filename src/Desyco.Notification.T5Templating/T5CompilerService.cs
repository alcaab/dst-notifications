using System.Collections.Generic;
using Desyco.Notification.Templating;

namespace Desyco.Notification.T5Templating
{
    public class T5CompilerService : TemplateCompilerService
    {
        private readonly ITemplateEngine _templateEngine;

        public T5CompilerService(ITemplateEngine templateEngine) : base(templateEngine)
        {
            _templateEngine = templateEngine;
        }


        public override string Process(CompiledTemplateResult result, Dictionary<string, object> data)
        {

            var transformType = result.CompiledTemplate.TextTransformation.GetType();
            var sessionProp = transformType
                .GetProperty("Session", typeof(IDictionary<string, object>));

            if (sessionProp != null)
                sessionProp
                    .SetValue(result.CompiledTemplate.TextTransformation, data, null);

            return result.CompiledTemplate.Process();
        }
    }
}