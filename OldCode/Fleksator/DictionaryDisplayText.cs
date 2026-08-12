using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public static class DictionaryDisplayText
    {
        public static string ForNoun(Noun noun)
        {
            if (noun == null)
                return string.Empty;

            string form = NounDecliner.Decliner.MakeWord(
                noun, InflectionCase.Nominative, DecliantionNumber.Singular);
            return string.IsNullOrEmpty(form) ? noun.Root : form;
        }

        public static string ForAdjective(Adjective adjective)
        {
            if (adjective == null)
                return string.Empty;

            string form = AdjectiveDecliner.Decliner.MakeWord(adjective, GrammaticalGender.Feminine,
                InflectionCase.Nominative, DecliantionNumber.Singular, AdjectiveLevel.Equal);
            return string.IsNullOrEmpty(form) ? adjective.Root : form;
        }
    }
}
