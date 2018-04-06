namespace Obscureware.Flaksator.Data
{
    using System.Collections.Generic;

    public interface IStringListsRepository
    {
        IEnumerable<string> GetTitles();
        IEnumerable<string> GetTitleExtenders();


        void SaveTitles(IEnumerable<string> titles);
        void SaveTitleExtenders(IEnumerable<string> titleExtenders);

    }
}