using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Redis
{
    public class CacheRedis : ICacheRedis
    {
        private readonly IConnectionMultiplexer _redis;

        public CacheRedis(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void SetItem(Item item)
        {
            var db = _redis.GetDatabase();
            var serialItem = JsonSerializer.Serialize(item);

            db.HashSet("hashEmail", new HashEntry[]
            {
                new HashEntry(item.Email, serialItem)
            });
        }

        public Item? GetItem(string key)
        {
            var db = _redis.GetDatabase();

            var item = db.HashGet("hashEmail", key);

            if (string.IsNullOrEmpty(item))
                return null;
            
            return JsonSerializer.Deserialize<Item>(item!);
        }

        public IEnumerable<Item?>? GetAllItems()
        {
            var db = _redis.GetDatabase();

            var complataHash = db.HashGetAll("hashEmail");

            if(complataHash.Length > 0)
            {
                var obj = Array.ConvertAll(complataHash, val => JsonSerializer.Deserialize<Item>(val.Value!)).ToList();
                return obj;
            }

            return null;
        }


    }
}
