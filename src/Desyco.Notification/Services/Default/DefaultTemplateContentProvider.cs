using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class DefaultTemplateContentProvider : ITemplateContentProvider
    {

        public Task<string> GetTemplateContent(string templateKey, Dictionary<string, object> data)
        {

            return Task.FromResult("");
        }
    }
}