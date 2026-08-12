using System.Collections.Generic;

namespace Flaksator
{
    internal static class WordFormatterParser
    {
        public static bool TryParse(string formatter, out string grammarCode, out List<int> categoryIds)
        {
            grammarCode = formatter;
            categoryIds = null;

            if (string.IsNullOrEmpty(formatter))
                return false;

            int separatorIndex = formatter.IndexOf('$');
            if (separatorIndex < 0)
                return true;

            grammarCode = formatter.Substring(0, separatorIndex);
            string categoryCode = formatter.Substring(separatorIndex + 1);
            if (grammarCode.Length == 0 || categoryCode.Length == 0)
                return false;

            categoryIds = new List<int>();
            foreach (string token in categoryCode.Split(','))
            {
                int categoryId;
                if (!int.TryParse(token, out categoryId) || categoryId <= 0)
                    return false;

                if (!categoryIds.Contains(categoryId))
                    categoryIds.Add(categoryId);
            }

            return categoryIds.Count > 0;
        }

        public static bool MatchesCategories(ICollection<int> wordCategories, IList<int> requestedCategories)
        {
            if (requestedCategories == null || requestedCategories.Count == 0)
                return true;

            if (wordCategories == null)
                return false;

            foreach (int categoryId in requestedCategories)
                if (wordCategories.Contains(categoryId))
                    return true;

            return false;
        }
    }
}
