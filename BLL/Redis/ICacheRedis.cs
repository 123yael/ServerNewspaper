using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Redis
{
    public interface ICacheRedis
    {
        public void SetItem(Item item);
        public Item? GetItem(string key);
        public IEnumerable<Item?>? GetAllItems();
    }
}
