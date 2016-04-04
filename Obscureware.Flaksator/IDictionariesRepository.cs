using System.Collections.Generic;

namespace Obscureware.Flaksator
{
    public interface IDictionariesRepository
    {
        IEnumerable<string> GetTitles();
        IEnumerable<string> GetTitleExtenders();


        void SaveTitles(IEnumerable<string> titles);
        void SaveTitleExtenders(IEnumerable<string> titleExtenders);

    }
}