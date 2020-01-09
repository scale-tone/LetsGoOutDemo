using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LetsGoOutDemo.AspNetCore
{
    static class Utils
    {
        /// <summary>
        /// Asp.Net Core can't map plain text to method parameters, so need to handcraft request body parsing
        /// </summary>
        public static async Task<string> ReadAsStringAsync(this HttpRequest request)
        {
            using(var reader = new StreamReader(request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /// <summary>
        /// Converts string[] to RedisValue[]
        /// </summary>
        public static RedisValue[] ToRedisValues(this IEnumerable<string> values)
        {
            return values.Select(v => (RedisValue)v).ToArray();
        }

        /// <summary>
        /// Converts RedisValue[] to string[]
        /// </summary>
        public static HashSet<string> FromRedisValues(this RedisValue[] values)
        {
            return new HashSet<string>(values.Select(v => (string)v));
        }

        /// <summary>
        /// Converts JSON string to dynamic object
        /// </summary>
        public static dynamic FromJson(this string json)
        {
            return JsonConvert.DeserializeObject(json);
        }
    }
}
