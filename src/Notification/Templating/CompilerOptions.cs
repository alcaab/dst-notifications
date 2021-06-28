using System;
using System.Collections.Generic;

namespace Desyco.Notification.Templating
{
    public class CompilerOptions
    {
        public string TemplateKey { get; set; }
        public string Content { get; set; }
        public TimeSpan Version { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}