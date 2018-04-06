namespace Obscureware.Flaksator.Data
{
    using System.Collections.Generic;

    using LiteDB;

    using ObscureWare.DocumentDatabase;

    internal class LightStringListsRepository : BaseListRepository,
        IStringListsRepository
    {
        private const string TITLES = @"TITLES";
        private const string TITLE_EXTENDERS = @"TITLE_EXTENDERS";

        public LightStringListsRepository(LiteDatabase db) : base(db)
        {

        }

        public IEnumerable<string> GetTitles()
        {
            return this.GetStrings(TITLES);
        }



        public IEnumerable<string> GetTitleExtenders()
        {
            return this.GetStrings(TITLE_EXTENDERS);
        }

        public void SaveTitles(IEnumerable<string> titles)
        {
            this.SaveStrings(TITLES, titles);
        }

        public void SaveTitleExtenders(IEnumerable<string> titleExtenders)
        {
            this.SaveStrings(TITLE_EXTENDERS, titleExtenders);
        }

        
    }
}