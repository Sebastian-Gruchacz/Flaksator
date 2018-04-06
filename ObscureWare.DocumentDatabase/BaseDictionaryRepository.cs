using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    public class BaseDictionaryRepository<TKey, TValue>
    {
        private const string DICTIONARIES_IDENTIFIER = @"DICTIONARIES";
        
        private readonly LiteDatabase _db;

        public BaseDictionaryRepository(LiteDatabase db)
        {
            this._db = db;
        }

        public Dictionary<TKey, TValue> GetDictionary(string collectionKey)
        {
            var wrapper = this._db.GetCollection<DictionaryWrapper<TKey, TValue>>(DICTIONARIES_IDENTIFIER)
                .Find(w => w.Key == collectionKey).SingleOrDefault();
            if (wrapper != null)
            {
                return wrapper.Items.ToDictionary(i => i.Key, i => i.Value);
            }

            return new Dictionary<TKey, TValue>();
        }

        public void SaveDictionary(string collectionKey, Dictionary<TKey, TValue> dictionary)
        {
            var collection = this._db.GetCollection<DictionaryWrapper<TKey, TValue>>(DICTIONARIES_IDENTIFIER);
            var wrapper = collection.Find(w => w.Key == collectionKey).SingleOrDefault();
            if (wrapper == null)
            {
                wrapper = new DictionaryWrapper<TKey, TValue>(collectionKey, dictionary);
                collection.Insert(wrapper);
            }
            else
            {
                wrapper.Items = dictionary.ToArray();
                collection.Update(wrapper);
            }
        }
    }
}
