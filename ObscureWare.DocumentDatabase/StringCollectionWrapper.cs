using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    public class StringCollectionWrapper
    {
        public StringCollectionWrapper()
        {
            
        }

        public StringCollectionWrapper(string key, IEnumerable<string> strings)
        {
            this.Key = key;
            this.Strings = strings.ToArray();
        }

        [BsonId]
        public string Key { get; set; }

        public string[] Strings { get; set; }
    }
}