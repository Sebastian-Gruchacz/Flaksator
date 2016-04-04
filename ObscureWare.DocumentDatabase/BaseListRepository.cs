using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    public class BaseListRepository
    {
        private const string LISTS_IDENTIFIER = @"LISTS";
        private const string DICTIONARIES_IDENTIFIER = @"DICTIONARIES";

        private readonly LiteDatabase _db;

        protected BaseListRepository(LiteDatabase db)
        {
            _db = db;
        }

        protected IEnumerable<string> GetStrings(string collectionKey)
        {
            var wrapper = _db.GetCollection<StringCollectionWrapper>(LISTS_IDENTIFIER)
                .Find(w => w.Key == collectionKey).SingleOrDefault();
            if (wrapper != null)
            {
                return wrapper.Strings;
            }

            return new string[0];
        }

        protected Dictionary<TKey, TValue> GetDictionary<TKey, TValue>(string collectionKey)
        {
            var wrapper = _db.GetCollection<DictionaryWrapper<TKey, TValue>>(DICTIONARIES_IDENTIFIER)
                .Find(w => w.Key == collectionKey).SingleOrDefault();
            if (wrapper != null)
            {
                return wrapper.Items.ToDictionary(i => i.Key, i => i.Value);
            }

            return new Dictionary<TKey, TValue>();
        }

        protected void SaveStrings(string collectionKey, IEnumerable<string> items)
        {
            var collection = _db.GetCollection<StringCollectionWrapper>(LISTS_IDENTIFIER);
            var wrapper = _db.GetCollection<StringCollectionWrapper>(LISTS_IDENTIFIER)
                .Find(w => w.Key == collectionKey).SingleOrDefault();
            if (wrapper == null)
            {
                wrapper = new StringCollectionWrapper(collectionKey, items);
                collection.Insert(wrapper);
            }
            else
            {
                wrapper.Strings = items.ToArray();
                collection.Update(wrapper);
            }
        }
    }
}
