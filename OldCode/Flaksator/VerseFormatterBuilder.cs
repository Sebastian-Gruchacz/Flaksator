using System;
using System.Collections.Generic;
using System.Text;

namespace Flaksator
{
    public enum VerseFormatterBuildKind
    {
        Noun,
        Adjective,
        Verb,
        Pronoun,
        ReflexiveParticle
    }

    public sealed class VerseFormatterBuildRequest
    {
        private readonly List<int> categoryIds = new List<int>();

        public VerseFormatterBuildRequest()
        {
            this.CaseCode = 'N';
            this.NumberCode = '1';
            this.GenderCode = 'F';
            this.LevelCode = 'E';
            this.TenseCode = 'T';
            this.PersonCode = '3';
        }

        public VerseFormatterBuildKind Kind { get; set; }

        public string GroupName { get; set; }

        public char CaseCode { get; set; }

        public char NumberCode { get; set; }

        public char GenderCode { get; set; }

        public char LevelCode { get; set; }

        public char TenseCode { get; set; }

        public char PersonCode { get; set; }

        public bool PrepositionalPronoun { get; set; }

        public List<int> CategoryIds
        {
            get { return this.categoryIds; }
        }

    }

    public static class VerseFormatterBuilder
    {
        public static bool TryParse(string formatter, out VerseFormatterBuildRequest request,
            out string error)
        {
            request = null;
            error = null;
            if (string.IsNullOrWhiteSpace(formatter))
            {
                error = "Formatter nie może być pusty.";
                return false;
            }

            string descriptor = formatter.Trim();
            if (descriptor[0] == '{' || descriptor[descriptor.Length - 1] == '}')
            {
                if (descriptor.Length < 3 || descriptor[0] != '{' ||
                    descriptor[descriptor.Length - 1] != '}')
                {
                    error = "Formatter ma nieparzyste nawiasy klamrowe.";
                    return false;
                }
                descriptor = descriptor.Substring(1, descriptor.Length - 2);
            }

            string groupName = string.Empty;
            string formatterCode = descriptor;
            int groupSeparator = descriptor.IndexOf(':');
            if (groupSeparator >= 0)
            {
                groupName = descriptor.Substring(0, groupSeparator);
                formatterCode = descriptor.Substring(groupSeparator + 1);
                if (!WordFormatterParser.IsValidGroupName(groupName))
                {
                    error = "Formatter ma niepoprawną nazwę grupy.";
                    return false;
                }
            }

            string code;
            List<int> categories;
            if (!WordFormatterParser.TryParse(formatterCode, out code, out categories))
            {
                error = "Formatter ma niepoprawną listę kategorii.";
                return false;
            }

            VerseFormatterBuildRequest parsed = new VerseFormatterBuildRequest
            {
                GroupName = groupName
            };
            if (categories != null)
                parsed.CategoryIds.AddRange(categories);

            if (code == "R")
            {
                parsed.Kind = VerseFormatterBuildKind.ReflexiveParticle;
            }
            else if (code.Length == 4 && code[0] == 'N')
            {
                parsed.Kind = VerseFormatterBuildKind.Noun;
                parsed.CaseCode = code[1];
                parsed.NumberCode = code[2];
                parsed.GenderCode = code[3];
            }
            else if (code.Length == 5 && code[0] == 'A')
            {
                parsed.Kind = VerseFormatterBuildKind.Adjective;
                parsed.CaseCode = code[1];
                parsed.NumberCode = code[2];
                parsed.GenderCode = code[3];
                parsed.LevelCode = code[4];
            }
            else if ((code.Length == 4 || code.Length == 5) && code[0] == 'V')
            {
                parsed.Kind = VerseFormatterBuildKind.Verb;
                parsed.TenseCode = code[1];
                parsed.PersonCode = code[2];
                parsed.NumberCode = code[3];
                parsed.GenderCode = code.Length == 5 ? code[4] : '?';
            }
            else if ((code.Length == 5 || code.Length == 6) && code[0] == 'P')
            {
                parsed.Kind = VerseFormatterBuildKind.Pronoun;
                parsed.CaseCode = code[1];
                parsed.PersonCode = code[2];
                parsed.NumberCode = code[3];
                parsed.GenderCode = code[4];
                parsed.PrepositionalPronoun = code.Length == 6 && code[5] == 'P';
                if (code.Length == 6 && !parsed.PrepositionalPronoun)
                {
                    error = "Zaimek ma niepoprawny wariant.";
                    return false;
                }
            }
            else
            {
                error = "Nieobsługiwany kod formattera „" + code + "”.";
                return false;
            }

            string normalized;
            if (!TryBuild(parsed, out normalized, out error))
                return false;
            request = parsed;
            return true;
        }

        public static bool TryBuild(VerseFormatterBuildRequest request, out string formatter,
            out string error)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            formatter = null;
            error = null;
            string group = (request.GroupName ?? string.Empty).Trim();
            if (group.Length > 0 && !WordFormatterParser.IsValidGroupName(group))
            {
                error = "Nazwa grupy może zawierać tylko litery, cyfry i znak _.";
                return false;
            }

            string code;
            switch (request.Kind)
            {
                case VerseFormatterBuildKind.Noun:
                    if (!IsOneOf(request.CaseCode, "NACDGLV") ||
                        !IsOneOf(request.NumberCode, "12") || !IsOneOf(request.GenderCode, "LPTFN"))
                    {
                        error = "Rzeczownik wymaga jawnego przypadku, liczby i rodzaju.";
                        return false;
                    }
                    code = "N" + request.CaseCode + request.NumberCode + request.GenderCode;
                    break;

                case VerseFormatterBuildKind.Adjective:
                    if (!IsOneOf(request.CaseCode, "NACDGLV?") ||
                        !IsOneOf(request.NumberCode, "12?") ||
                        !IsOneOf(request.GenderCode, "LPTFN?") ||
                        !IsOneOf(request.LevelCode, "EHS"))
                    {
                        error = "Przymiotnik ma niepoprawne cechy gramatyczne.";
                        return false;
                    }
                    if (group.Length == 0 && (request.CaseCode == '?' || request.NumberCode == '?' ||
                        request.GenderCode == '?'))
                    {
                        error = "Przymiotnik bez grupy nie może dziedziczyć cech oznaczonych ?.";
                        return false;
                    }
                    code = "A" + request.CaseCode + request.NumberCode + request.GenderCode +
                        request.LevelCode;
                    break;

                case VerseFormatterBuildKind.Verb:
                    if (!IsOneOf(request.TenseCode, "TPF") ||
                        !IsOneOf(request.PersonCode, "123?") || !IsOneOf(request.NumberCode, "12?"))
                    {
                        error = "Czasownik ma niepoprawny czas, osobę lub liczbę.";
                        return false;
                    }
                    if (group.Length == 0 && (request.PersonCode == '?' || request.NumberCode == '?'))
                    {
                        error = "Czasownik bez grupy wymaga jawnej osoby i liczby.";
                        return false;
                    }
                    if (request.TenseCode == 'P' && group.Length == 0 &&
                        !IsOneOf(request.GenderCode, "LPTFN"))
                    {
                        error = "Czasownik przeszły bez grupy wymaga rodzaju.";
                        return false;
                    }
                    code = "V" + request.TenseCode + request.PersonCode + request.NumberCode;
                    if (request.TenseCode == 'P' && request.GenderCode != '?')
                        code += request.GenderCode;
                    break;

                case VerseFormatterBuildKind.Pronoun:
                    if (!IsOneOf(request.CaseCode, "NACDGLV") ||
                        !IsOneOf(request.PersonCode, "123?") ||
                        !IsOneOf(request.NumberCode, "12?") ||
                        !IsOneOf(request.GenderCode, "LPTFN?"))
                    {
                        error = "Zaimek ma niepoprawne cechy gramatyczne.";
                        return false;
                    }
                    if (group.Length == 0 && (request.PersonCode == '?' || request.NumberCode == '?' ||
                        request.GenderCode == '?'))
                    {
                        error = "Zaimek bez grupy nie może dziedziczyć cech oznaczonych ?.";
                        return false;
                    }
                    code = "P" + request.CaseCode + request.PersonCode + request.NumberCode +
                        request.GenderCode + (request.PrepositionalPronoun ? "P" : string.Empty);
                    break;

                case VerseFormatterBuildKind.ReflexiveParticle:
                    if (group.Length == 0)
                    {
                        error = "Partykuła zwrotna R wymaga nazwanej grupy.";
                        return false;
                    }
                    if (request.CategoryIds.Count > 0)
                    {
                        error = "Partykuła R nie obsługuje kategorii.";
                        return false;
                    }
                    code = "R";
                    break;

                default:
                    error = "Nieznany typ formattera.";
                    return false;
            }

            StringBuilder descriptor = new StringBuilder();
            if (group.Length > 0)
                descriptor.Append(group).Append(':');
            descriptor.Append(code);
            if (request.CategoryIds.Count > 0)
            {
                HashSet<int> unique = new HashSet<int>();
                descriptor.Append('$');
                for (int i = 0; i < request.CategoryIds.Count; i++)
                {
                    int categoryId = request.CategoryIds[i];
                    if (categoryId <= 0)
                    {
                        error = "Identyfikatory kategorii muszą być dodatnimi liczbami.";
                        return false;
                    }
                    if (!unique.Add(categoryId))
                        continue;
                    if (unique.Count > 1)
                        descriptor.Append(',');
                    descriptor.Append(categoryId);
                }
            }

            formatter = "{" + descriptor + "}";
            return true;
        }

        public static bool TryParseCategories(string text, IList<int> target, out string error)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            error = null;
            if (string.IsNullOrWhiteSpace(text))
                return true;

            foreach (string token in text.Split(','))
            {
                int categoryId;
                if (!int.TryParse(token.Trim(), out categoryId) || categoryId <= 0)
                {
                    error = "Kategorie podaj jako dodatnie liczby oddzielone przecinkami.";
                    return false;
                }
                if (!target.Contains(categoryId))
                    target.Add(categoryId);
            }
            return true;
        }

        private static bool IsOneOf(char value, string values)
        {
            return values.IndexOf(value) >= 0;
        }
    }
}
