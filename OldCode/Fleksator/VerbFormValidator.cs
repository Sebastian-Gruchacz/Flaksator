using System;
using System.Collections.Generic;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public static class VerbFormValidator
    {
        public static bool TryValidate(IEnumerable<VerbForm> forms, out string error)
        {
            return TryValidate(VerbAspect.Unknown, forms, out error);
        }

        public static bool TryValidate(VerbAspect aspect, IEnumerable<VerbForm> forms, out string error)
        {
            if (forms == null)
                throw new ArgumentNullException("forms");

            HashSet<string> keys = new HashSet<string>(StringComparer.Ordinal);
            foreach (VerbForm form in forms)
            {
                if (form == null)
                {
                    error = "Lista zawiera pustą formę czasownika.";
                    return false;
                }

                if (form.Tense == VerbTense._Unknown ||
                    form.Person == GrammaticalPerson._Unknown ||
                    form.Number == DecliantionNumber._Unknown)
                {
                    error = "Każda forma musi mieć czas, osobę i liczbę.";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(form.Text))
                {
                    error = "Tekst formy czasownika nie może być pusty.";
                    return false;
                }

                if (form.Tense == VerbTense.Past && form.Gender == GrammaticalGender._Unknown)
                {
                    error = "Forma czasu przeszłego musi mieć rodzaj.";
                    return false;
                }

                if (aspect == VerbAspect.Perfective && form.Tense == VerbTense.Present)
                {
                    error = "Czasownik dokonany nie może mieć form czasu teraźniejszego.";
                    return false;
                }

                if (form.Tense != VerbTense.Past && form.Gender != GrammaticalGender._Unknown)
                {
                    error = "Rodzaj można podać tylko dla czasu przeszłego.";
                    return false;
                }

                string key = ((int)form.Tense) + ":" + ((int)form.Person) + ":" +
                    ((int)form.Number) + ":" + ((int)form.Gender);
                if (!keys.Add(key))
                {
                    error = "Dwie formy mają ten sam czas, osobę, liczbę i rodzaj.";
                    return false;
                }
            }

            error = null;
            return true;
        }

        public static bool TryValidateCompleteFiniteParadigms(Verb verb, out string error)
        {
            if (verb == null)
                throw new ArgumentNullException("verb");
            if (!TryValidate(verb.Aspect, verb.Forms, out error))
                return false;

            foreach (VerbTense tense in new[] { VerbTense.Present, VerbTense.Future })
            {
                bool hasTense = verb.Forms.Exists(form => form.Tense == tense);
                if (!hasTense)
                    continue;

                foreach (GrammaticalPerson person in new[] {
                    GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third })
                {
                    foreach (DecliantionNumber number in new[] {
                        DecliantionNumber.Singular, DecliantionNumber.Plural })
                    {
                        if (verb.Forms.Exists(form => form.Matches(tense, person, number,
                            GrammaticalGender._Unknown)))
                            continue;
                        error = "Niepełny paradygmat " + VerbGrammar.GetTenseCode(tense) +
                            " czasownika „" + verb.Infinitive + "”.";
                        return false;
                    }
                }
            }

            error = null;
            return true;
        }
    }
}
