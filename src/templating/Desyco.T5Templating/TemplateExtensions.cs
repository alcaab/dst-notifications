using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Desyco.T5Templating.TextTemplating;
using Desyco.T5Templating.TextTemplating.Microsoft.VisualStudio.TextTemplating;

namespace Desyco.T5Templating
{
    public static class TemplateExtensions
    {
        public static string ProcessTemplate(
            string context, 
            Dictionary<string,object> data
        )
        {
            var gen = new TemplateGenerator();
            var libs = new List<string>();
            var pars = new Dictionary<string,string>();
            var sb = new StringBuilder();

            foreach (var kv in data)
            {
                var location = Path.GetDirectoryName(kv.Value.GetType().Assembly.Location);
                if (!gen.ReferencePaths.Contains(location))
                    gen.ReferencePaths.Add(Path.GetDirectoryName(location));

                var name = Path.GetFileName(kv.Value.GetType().Assembly.Location);
                if (!libs.Contains(name))
                    libs.Add(name);

                pars.Add(kv.Key, kv.Value.GetType().FullName);
            }

            libs.ForEach(lib =>sb.AppendLine($"<#@ assembly name=\"{lib}\" #>"));
            pars.ToList().ForEach(par=> sb.AppendLine($"<#@ parameter name=\"{par.Key}\" type=\"{par.Value}\" #>"));
            sb.AppendLine("");
            sb.AppendLine(context);

            gen.Session = new TextTemplatingSession(data);

            var result = gen.ProcessFromString(sb.ToString());
            //var result = new TemplatingEngine()
            //   .CompileTemplate(sb.ToString(), gen)
            //   .Process(new TextTemplatingSession(data));


            return result;
        }

        public static string ProcessFromString(this TemplateGenerator gen, string content)
        {

            string outFile = null;

            gen.ProcessTemplate(null, content,ref outFile, out var outContent);

            return outContent;
        }

        //public static string Process(this CompiledTemplate source, ITextTemplatingSession session)
        //{

        //    var transformType = source.TextTransformation.GetType();

        //    var sessionProp = transformType
        //        .GetProperty("Session", typeof(IDictionary<string, object>));

        //    if (sessionProp != null)
        //        sessionProp
        //            .SetValue(source.TextTransformation, session, null);

        //    return source.Process();

        //}
    }
}
