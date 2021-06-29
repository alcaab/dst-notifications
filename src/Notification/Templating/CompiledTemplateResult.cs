using System;


namespace Desyco.Notification.Templating
{
    public class CompiledTemplateResult
    {
        public string TemplateKey { get; set; }
        public ICompiledTemplateRef CompiledTemplate { get; set; }
        public TimeSpan Version { get; set; }
    }
}