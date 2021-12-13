using System.Collections;
using System.Text;
using System.Text.Json;

namespace OpsMain.Client.Extensions
{
    public static class LogEx
    {
        public static string ToLogString(this IEnumerable list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item.ToString() + " ");
            }
            return sb.ToString();
        }
        public static string ToJsonString(this object obj)
        {
            return JsonSerializer.Serialize(obj,new JsonSerializerOptions { ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve });
        }

    }
}
