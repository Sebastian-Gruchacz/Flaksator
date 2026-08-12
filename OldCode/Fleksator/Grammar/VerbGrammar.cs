namespace SharpDevs.Fleksator.Grammar
{
    public static class VerbGrammar
    {
        public static VerbTense GetTense(char code)
        {
            switch (code)
            {
                case 'T': return VerbTense.Present;
                case 'P': return VerbTense.Past;
                case 'F': return VerbTense.Future;
                default: return VerbTense._Unknown;
            }
        }

        public static char GetTenseCode(VerbTense tense)
        {
            switch (tense)
            {
                case VerbTense.Present: return 'T';
                case VerbTense.Past: return 'P';
                case VerbTense.Future: return 'F';
                default: return '?';
            }
        }

        public static GrammaticalPerson GetPerson(char code)
        {
            switch (code)
            {
                case '1': return GrammaticalPerson.First;
                case '2': return GrammaticalPerson.Second;
                case '3': return GrammaticalPerson.Third;
                default: return GrammaticalPerson._Unknown;
            }
        }

        public static char GetPersonCode(GrammaticalPerson person)
        {
            switch (person)
            {
                case GrammaticalPerson.First: return '1';
                case GrammaticalPerson.Second: return '2';
                case GrammaticalPerson.Third: return '3';
                default: return '?';
            }
        }
    }
}
