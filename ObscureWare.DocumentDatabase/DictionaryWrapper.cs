using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    public class DictionaryWrapper<TKey, TValue>
    {
        public DictionaryWrapper()
        {

        }

        public DictionaryWrapper(string key, IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            this.Key = key;
            this.Items = items.ToArray();
        }

        [BsonId]
        public string Key { get; set; }

        public KeyValuePair<TKey, TValue>[] Items { get; set; }
    }
}