using System;
using System.Collections.Generic;
using System.Text;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    /// <summary>
    /// Translates Enumerations into CHAR codes
    /// </summary>
    public static class EnumHelper
    {
        public static InflectionCase GetWordCase(char code)
        {
            switch (code)
            {
                case 'N':
                    return InflectionCase.Nominative;
                case 'A':
                    return InflectionCase.Ablative;
                case 'C':
                    return InflectionCase.Accusative;
                case 'D':
                    return InflectionCase.Dative;
                case 'G':
                    return InflectionCase.Genitive;
                case 'L':
                    return InflectionCase.Locative;
                case 'V':
                    return InflectionCase.Vocative;
            }

            return InflectionCase._Unknown;
        }

        public static char GetWordCaseCode(InflectionCase aCase)
        {
            switch (aCase)
            {
                case InflectionCase.Nominative:
                    return 'N';
                case InflectionCase.Ablative:
                    return 'A';
                case InflectionCase.Accusative:
                    return 'C';
                case InflectionCase.Dative:
                    return 'D';
                case InflectionCase.Genitive:
                    return 'G';
                case InflectionCase.Locative:
                    return 'L';
                case InflectionCase.Vocative:
                    return 'V';
            }

            return '?';
        }

        public static DecliantionNumber GetWordAmount(char code)
        {
            switch (code)
            {
                case '1':
                    return DecliantionNumber.Singular;
                case '2':
                    return DecliantionNumber.Plural;
            }

            return DecliantionNumber._Unknown;
        }

        public static char GetWordAmountCode(DecliantionNumber amount)
        {
            switch (amount)
            {
                case DecliantionNumber.Singular:
                    return '1';
                case DecliantionNumber.Plural:
                    return '2';
            }

            return '?';
        }


        public static char GetGrammarCode(GrammaticalPart part)
        {
            switch (part)
            {
                case GrammaticalPart.Adjective:
                    return 'A';
                case GrammaticalPart.Noun:
                    return 'N';
                case GrammaticalPart.Verb:
                    return 'V';
            }

            return '?';
        }

        public static GrammaticalGender GetWordGenre(char code)
        {
            switch (code)
            {
                case 'L':
                    return GrammaticalGender.MasculineLife;
                case 'P':
                    return GrammaticalGender.MasculinePerson;
                case 'T':
                    return GrammaticalGender.MasculineThing;
                case 'F':
                    return GrammaticalGender.Feminine;
                case 'N':
                    return GrammaticalGender.Neuter;
            }

            return GrammaticalGender._Unknown;
        }

        public static char GetWordGenreCode(GrammaticalGender genre)
        {
            switch (genre)
            {
                case GrammaticalGender.MasculineLife:
                    return 'L';
                case GrammaticalGender.MasculinePerson:
                    return 'P';
                case GrammaticalGender.MasculineThing:
                    return 'T';
                case GrammaticalGender.Feminine:
                    return 'F';
                case GrammaticalGender.Neuter:
                    return 'N';
            }

            return ' ';
        }

        public static AdjectiveLevel GetAdjectiveLevel(char code)
        {
            switch (code)
            {
                case 'E':
                    return AdjectiveLevel.Equal;
                case 'H':
                    return AdjectiveLevel.Higher;
                case 'S':
                    return AdjectiveLevel.Highest;
            }

            return AdjectiveLevel._Unknown;
        }
    }
}
