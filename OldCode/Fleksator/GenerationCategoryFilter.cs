using System.Collections.Generic;

namespace SharpDevs.Fleksator
{
    public static class GenerationCategoryFilter
    {
        private static readonly HashSet<int> excludedNounCategories = new HashSet<int>();
        private static readonly HashSet<int> excludedAdjectiveCategories = new HashSet<int>();
        private static readonly HashSet<int> excludedVerbCategories = new HashSet<int>();

        public static HashSet<int> ExcludedNounCategories
        {
            get { return excludedNounCategories; }
        }

        public static HashSet<int> ExcludedAdjectiveCategories
        {
            get { return excludedAdjectiveCategories; }
        }

        public static HashSet<int> ExcludedVerbCategories
        {
            get { return excludedVerbCategories; }
        }

        public static bool AllowsNoun(ICollection<int> categories)
        {
            return Allows(categories, excludedNounCategories);
        }

        public static bool AllowsAdjective(ICollection<int> categories)
        {
            return Allows(categories, excludedAdjectiveCategories);
        }

        public static bool AllowsVerb(ICollection<int> categories)
        {
            return Allows(categories, excludedVerbCategories);
        }

        public static void Clear()
        {
            excludedNounCategories.Clear();
            excludedAdjectiveCategories.Clear();
            excludedVerbCategories.Clear();
        }

        private static bool Allows(ICollection<int> categories, ICollection<int> exclusions)
        {
            if (exclusions.Count == 0 || categories == null || categories.Count == 0)
                return true;
            foreach (int category in categories)
                if (exclusions.Contains(category))
                    return false;
            return true;
        }
    }
}
