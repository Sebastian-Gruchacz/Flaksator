using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public static class CompoundFuturePrefiller
    {
        public static bool TryAddMissingForms(Verb verb, out int added, out string error)
        {
            added = 0;
            if (verb == null)
            {
                error = "Nie wybrano czasownika.";
                return false;
            }
            if (verb.Aspect != VerbAspect.Imperfective)
            {
                error = "Przyszłość złożoną można uzupełnić tylko dla czasownika niedokonanego.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(verb.Infinitive))
            {
                error = "Czasownik musi mieć bezokolicznik.";
                return false;
            }

            GrammaticalPerson[] persons =
            {
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third,
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third
            };
            DecliantionNumber[] numbers =
            {
                DecliantionNumber.Singular, DecliantionNumber.Singular, DecliantionNumber.Singular,
                DecliantionNumber.Plural, DecliantionNumber.Plural, DecliantionNumber.Plural
            };
            string[] auxiliaries = { "będę", "będziesz", "będzie", "będziemy", "będziecie", "będą" };

            for (int i = 0; i < persons.Length; i++)
            {
                if (verb.Forms.Exists(form => form.Matches(VerbTense.Future, persons[i],
                    numbers[i], GrammaticalGender._Unknown)))
                    continue;
                verb.Forms.Add(new VerbForm
                {
                    Tense = VerbTense.Future,
                    Person = persons[i],
                    Number = numbers[i],
                    Gender = GrammaticalGender._Unknown,
                    Text = verb.Infinitive == "być"
                        ? auxiliaries[i]
                        : auxiliaries[i] + " " + verb.Infinitive
                });
                added++;
            }

            error = null;
            return true;
        }
    }
}
