using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desyco.Notification.Extensions
{
    public static class Extensions
    {

        public static async Task CreateMessageBody(this ITemplateContentProvider  source, NotificationMessage m)
        {

            var data = m.Data/*.JsonCopy()*/;
            data.Add("message", m);
            m.Body = await source.GetTemplateContent(m.TemplateKey, data);

        }

        public static T JsonClone<T>(this T source) where T : class
        {
            if (source == null) return default;
            var data = JsonConvert.SerializeObject(source, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });

            return JsonConvert.DeserializeObject<T>(data);

        }
    }
}