using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface ITemplateContentProvider
    {
        Task<string> GetTemplateContent(string templateKey, Dictionary<string, object> data);
    }
}