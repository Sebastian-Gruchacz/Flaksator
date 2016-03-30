using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace ObscureWare.DocumentDatabase
{
    internal class LiteDictionariesRepository : IDictionariesRepository
    {
        private const string TITLES = @"TITLES";
        private const string TITLE_EXTENDERS = @"TITLE_EXTENDERS";

        private readonly LiteDatabase _db;

        public LiteDictionariesRepository(LiteDatabase db)
        {
            _db = db;
        }

        public IEnumerable<string> GetTitles()
        {
            return GetStrings(TITLES);
        }

       

        public IEnumerable<string> GetTitleExtenders()
        {
            return GetStrings(TITLE_EXTENDERS);
        }

        public void SaveTitles(IEnumerable<string> titles)
        {
            SaveStrings(TITLES, titles);
        }

        

        public void SaveTitleExtenders(IEnumerable<string> titleExtenders)
        {
            SaveStrings(TITLE_EXTENDERS, titleExtenders);
        }

        // TODO: maybe do collections with one object with array property instead? replacement will work safer way

        private IEnumerable<string> GetStrings(string collectionKey)
        {
            return _db.GetCollection<StringWrapper>(collectionKey).FindAll().Select(w => w.Value);
        }

        private void SaveStrings(string collectionKey, IEnumerable<string> items)
        {
            var collection = _db.GetCollection<StringWrapper>(collectionKey);
            collection.Delete(wrapper => true); // delete all
            collection.Insert(items.Select(s => new StringWrapper(s)));
        }
    }
}