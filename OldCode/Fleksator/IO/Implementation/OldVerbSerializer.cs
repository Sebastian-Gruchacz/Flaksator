using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO.Implementation
{
    internal class OldVerbSerializer : IGrammaticalWordSerializer<Verb>
    {
        public Verb Load(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string[] elements = input.Split('|');
            Verb verb = new Verb
            {
                Infinitive = elements[0],
                DisplayName = elements[0],
                CanBeActive = true
            };

            for (int i = 1; i < elements.Length; i++)
            {
                string token = elements[i];
                if (token == "R")
                {
                    verb.IsReflexive = true;
                    continue;
                }

                if (token.StartsWith("CAT="))
                {
                    foreach (string categoryToken in token.Substring(4).Split(','))
                    {
                        int categoryId;
                        if (!int.TryParse(categoryToken, out categoryId) || categoryId <= 0)
                            return null;
                        if (!verb.Categories.Contains(categoryId))
                            verb.Categories.Add(categoryId);
                    }
                    continue;
                }

                if (token.StartsWith("CONJ="))
                {
                    int conjugation;
                    if (!int.TryParse(token.Substring(5), out conjugation) || conjugation < 0)
                        return null;
                    verb.ConjugationNumber = conjugation;
                    continue;
                }

                if (token.StartsWith("ASPECT="))
                {
                    switch (token.Substring(7))
                    {
                        case "I": verb.Aspect = VerbAspect.Imperfective; break;
                        case "P": verb.Aspect = VerbAspect.Perfective; break;
                        case "B": verb.Aspect = VerbAspect.Biaspectual; break;
                        default: return null;
                    }
                    continue;
                }

                if (token.StartsWith("VOICE="))
                {
                    string voices = token.Substring(6);
                    if (voices != "A" && voices != "P" && voices != "AP")
                        return null;
                    verb.CanBeActive = voices.IndexOf('A') >= 0;
                    verb.CanBePassive = voices.IndexOf('P') >= 0;
                    continue;
                }

                int separator = token.IndexOf('=');
                if (separator < 0)
                    continue;

                string code = token.Substring(0, separator);
                string text = token.Substring(separator + 1);
                if ((code.Length != 3 && code.Length != 4) || text.Length == 0)
                    return null;

                VerbTense tense = VerbGrammar.GetTense(code[0]);
                GrammaticalPerson person = VerbGrammar.GetPerson(code[1]);
                DecliantionNumber number = EnumHelper.GetWordAmount(code[2]);
                GrammaticalGender gender = code.Length == 4
                    ? EnumHelper.GetWordGenre(code[3])
                    : GrammaticalGender._Unknown;
                if (tense == VerbTense._Unknown || person == GrammaticalPerson._Unknown ||
                    number == DecliantionNumber._Unknown ||
                    (code.Length == 4 && gender == GrammaticalGender._Unknown))
                    return null;

                verb.Forms.Add(new VerbForm
                {
                    Tense = tense,
                    Person = person,
                    Number = number,
                    Gender = gender,
                    Text = text
                });
            }

            string validationError;
            if (!VerbFormValidator.TryValidate(verb.Aspect, verb.Forms, out validationError))
                return null;

            verb.DisplayName = verb.Infinitive + (verb.IsReflexive ? " się" : string.Empty);
            return verb;
        }

        public string Write(Verb word)
        {
            if (word == null)
                throw new ArgumentNullException("word");
            string validationError;
            if (!VerbFormValidator.TryValidate(word.Aspect, word.Forms, out validationError))
                throw new InvalidOperationException(validationError);

            StringBuilder result = new StringBuilder(word.Infinitive);
            if (word.IsReflexive)
                result.Append("|R");
            if (word.Categories.Count > 0)
            {
                result.Append("|CAT=");
                for (int i = 0; i < word.Categories.Count; i++)
                {
                    if (i > 0)
                        result.Append(',');
                    result.Append(word.Categories[i]);
                }
            }
            if (word.ConjugationNumber > 0)
                result.Append("|CONJ=").Append(word.ConjugationNumber);
            switch (word.Aspect)
            {
                case VerbAspect.Imperfective: result.Append("|ASPECT=I"); break;
                case VerbAspect.Perfective: result.Append("|ASPECT=P"); break;
                case VerbAspect.Biaspectual: result.Append("|ASPECT=B"); break;
            }
            if (!word.CanBeActive || word.CanBePassive)
            {
                result.Append("|VOICE=");
                if (word.CanBeActive)
                    result.Append('A');
                if (word.CanBePassive)
                    result.Append('P');
            }

            foreach (VerbForm form in word.Forms)
            {
                result.Append('|');
                result.Append(VerbGrammar.GetTenseCode(form.Tense));
                result.Append(VerbGrammar.GetPersonCode(form.Person));
                result.Append(EnumHelper.GetWordAmountCode(form.Number));
                if (form.Gender != GrammaticalGender._Unknown)
                    result.Append(EnumHelper.GetWordGenreCode(form.Gender));
                result.Append('=');
                result.Append(form.Text);
            }

            return result.ToString();
        }
    }
}
