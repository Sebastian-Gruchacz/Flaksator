using System;
using System.Collections.Generic;
using System.Text;
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

            return verb;
        }

        public string Write(Verb word)
        {
            if (word == null)
                throw new ArgumentNullException("word");

            StringBuilder result = new StringBuilder(word.Infinitive);
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
