using System.Collections.Generic;
using LiteDB;
using ObscureWare.DocumentDatabase;

namespace Obscureware.Flaksator
{
    internal class LiteDictionariesRepository : BaseListRepository, IDictionariesRepository
    {
        private const string TITLES = @"TITLES";
        private const string TITLE_EXTENDERS = @"TITLE_EXTENDERS";

        public LiteDictionariesRepository(LiteDatabase db) : base(db)
        {

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

        
    }
}