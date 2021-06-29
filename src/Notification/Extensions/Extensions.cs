using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desyco.Notification.Extensions
{
    public static class Extensions
    {

        public static async Task CreateMessageBody(this ITemplateContentProvider  source, NotificationMessage m)
        {
            /*
             * Serialization is causing an issue because property Data which is a Dictionary<string,object> object
             * that is loosing its type and insted all objects types are turned to JObject.
             * 
             */
            var data = m.Data/*.JsonClone()*/;
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