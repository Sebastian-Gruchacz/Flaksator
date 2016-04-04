using System.Collections.Generic;

namespace Obscureware.Flaksator
{
    public interface IStringListsRepository
    {
        IEnumerable<string> GetTitles();
        IEnumerable<string> GetTitleExtenders();


        void SaveTitles(IEnumerable<string> titles);
        void SaveTitleExtenders(IEnumerable<string> titleExtenders);

    }
}