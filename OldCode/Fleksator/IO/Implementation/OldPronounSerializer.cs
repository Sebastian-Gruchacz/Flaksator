using System;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator.IO.Implementation
{
    internal sealed class OldPronounSerializer : IGrammaticalWordSerializer<Pronoun>
    {
        public Pronoun Load(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            string[] elements = input.Split('|');
            Pronoun pronoun = new Pronoun { Root = elements[0], DisplayName = elements[0] };
            for (int i = 1; i < elements.Length; i++)
            {
                string token = elements[i];
                int separator = token.IndexOf('=');
                if ((separator != 4 && separator != 5) || token.Length <= separator + 1)
                    return null;

                string code = token.Substring(0, separator);
                InflectionCase aCase = EnumHelper.GetWordCase(code[0]);
                GrammaticalPerson person = VerbGrammar.GetPerson(code[1]);
                DecliantionNumber number = EnumHelper.GetWordAmount(code[2]);
                GrammaticalGender gender = EnumHelper.GetWordGenre(code[3]);
                PronounVariant variant = code.Length == 5 && code[4] == 'P'
                    ? PronounVariant.Prepositional
                    : PronounVariant.Neutral;
                if (aCase == InflectionCase._Unknown || person == GrammaticalPerson._Unknown ||
                    number == DecliantionNumber._Unknown || gender == GrammaticalGender._Unknown)
                    return null;
                if (code.Length == 5 && code[4] != 'P')
                    return null;

                pronoun.Forms.Add(new PronounForm
                {
                    Case = aCase,
                    Person = person,
                    Number = number,
                    Gender = gender,
                    Variant = variant,
                    Text = token.Substring(separator + 1)
                });
            }

            return pronoun;
        }

        public string Write(Pronoun word)
        {
            if (word == null)
                throw new ArgumentNullException("word");

            StringBuilder result = new StringBuilder(word.Root);
            foreach (PronounForm form in word.Forms)
            {
                result.Append('|');
                result.Append(EnumHelper.GetWordCaseCode(form.Case));
                result.Append(VerbGrammar.GetPersonCode(form.Person));
                result.Append(EnumHelper.GetWordAmountCode(form.Number));
                result.Append(EnumHelper.GetWordGenreCode(form.Gender));
                if (form.Variant == PronounVariant.Prepositional)
                    result.Append('P');
                result.Append('=');
                result.Append(form.Text);
            }

            return result.ToString();
        }
    }
}
