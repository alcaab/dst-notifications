using Desyco.Notification.Templating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Desyco.Notification.T5Templating
{
    public static class T5TemplatingCollectionExtensions
    {
        public static TemplateOptions UseT5FileTemplating(this NotificationOptions options,string directoryTemplate)
        {

            options.ConfigureProviderOptions(NotificationConst.TemplateProviderType, directoryTemplate);
            
            options.UseTemplateCompiler(
                sp => new T5CompilerEngine(),
                sp => new T5CompilerService(sp.GetService<ITemplateEngine>()));
            
            options.UseTemplateContent(sp => new FileTemplateContentService(
                sp.GetService<ITemplateCompilerService>(), sp.GetService<ILoggerFactory>(),
                sp.GetService<NotificationOptions>()));

            return new TemplateOptions(options);
        }

        public static TemplateOptions UseT5Templating(this NotificationOptions options)
        {
            options.UseTemplateCompiler(
                sp => new T5CompilerEngine(),
                sp => new T5CompilerService(sp.GetService<ITemplateEngine>()));

            return new TemplateOptions(options);
        }

    }

    public class TemplateOptions
    {
        public TemplateOptions(NotificationOptions options)
        {
            Options = options;
        }

        public NotificationOptions Options { get; }

        public string TemplateDirectory { get; set; }




    }
}