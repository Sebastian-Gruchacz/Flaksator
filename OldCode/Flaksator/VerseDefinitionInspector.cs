using System;
using System.Collections.Generic;

namespace Flaksator
{
    public enum VerseFormatterKind
    {
        Unknown,
        Noun,
        Adjective,
        Verb,
        Pronoun,
        ReflexiveParticle
    }

    public sealed class VerseFormatterToken
    {
        public int StartIndex { get; internal set; }

        public int Length { get; internal set; }

        public string Descriptor { get; internal set; }

        public string GroupName { get; internal set; }

        public string Code { get; internal set; }

        public VerseFormatterKind Kind { get; internal set; }

        public override string ToString()
        {
            return this.Kind + ": {" + this.Descriptor + "}";
        }
    }

    public sealed class VerseDefinitionDiagnostic
    {
        public int StartIndex { get; internal set; }

        public int Length { get; internal set; }

        public string Message { get; internal set; }

        public override string ToString()
        {
            return this.Message;
        }
    }

    public sealed class VerseGroupSummary
    {
        private readonly List<VerseFormatterToken> tokens = new List<VerseFormatterToken>();

        internal VerseGroupSummary(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public List<VerseFormatterToken> Tokens
        {
            get { return this.tokens; }
        }
    }

    public sealed class VerseDefinitionInspection
    {
        private readonly List<VerseFormatterToken> tokens = new List<VerseFormatterToken>();
        private readonly List<VerseGroupSummary> groups = new List<VerseGroupSummary>();
        private readonly List<VerseDefinitionDiagnostic> diagnostics = new List<VerseDefinitionDiagnostic>();

        public List<VerseFormatterToken> Tokens
        {
            get { return this.tokens; }
        }

        public List<VerseGroupSummary> Groups
        {
            get { return this.groups; }
        }

        public List<VerseDefinitionDiagnostic> Diagnostics
        {
            get { return this.diagnostics; }
        }
    }

    public static class VerseDefinitionInspector
    {
        public static VerseDefinitionInspection Inspect(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            VerseDefinitionInspection inspection = new VerseDefinitionInspection();
            Dictionary<string, VerseGroupSummary> groups =
                new Dictionary<string, VerseGroupSummary>(StringComparer.Ordinal);
            int position = 0;
            while (position < source.Length)
            {
                int open = source.IndexOf('{', position);
                int strayClose = source.IndexOf('}', position);
                if (strayClose >= 0 && (open < 0 || strayClose < open))
                {
                    AddDiagnostic(inspection, strayClose, 1, "Zamykający nawias } nie ma pary.");
                    position = strayClose + 1;
                    continue;
                }
                if (open < 0)
                    break;

                int close = source.IndexOf('}', open + 1);
                int nestedOpen = source.IndexOf('{', open + 1);
                if (close < 0 || (nestedOpen >= 0 && nestedOpen < close))
                {
                    AddDiagnostic(inspection, open, 1, "Otwierający nawias { nie ma poprawnej pary.");
                    position = open + 1;
                    continue;
                }

                string descriptor = source.Substring(open + 1, close - open - 1);
                VerseFormatterToken token = ParseToken(descriptor, open, close - open + 1, inspection);
                if (token != null)
                {
                    inspection.Tokens.Add(token);
                    if (!string.IsNullOrEmpty(token.GroupName))
                    {
                        VerseGroupSummary group;
                        if (!groups.TryGetValue(token.GroupName, out group))
                        {
                            group = new VerseGroupSummary(token.GroupName);
                            groups.Add(token.GroupName, group);
                            inspection.Groups.Add(group);
                        }
                        group.Tokens.Add(token);
                    }
                }
                position = close + 1;
            }

            foreach (VerseGroupSummary group in inspection.Groups)
                ValidateGroup(group, inspection);
            return inspection;
        }

        private static VerseFormatterToken ParseToken(string descriptor, int start, int length,
            VerseDefinitionInspection inspection)
        {
            if (descriptor.Length == 0)
            {
                AddDiagnostic(inspection, start, length, "Pusty znacznik {}.");
                return null;
            }

            string groupName = null;
            string formatter = descriptor;
            int separator = descriptor.IndexOf(':');
            if (separator >= 0)
            {
                groupName = descriptor.Substring(0, separator);
                formatter = descriptor.Substring(separator + 1);
                if (!WordFormatterParser.IsValidGroupName(groupName))
                    AddDiagnostic(inspection, start, length, "Niepoprawna nazwa grupy „" + groupName + "”.");
            }

            string code;
            List<int> categories;
            if (!WordFormatterParser.TryParse(formatter, out code, out categories))
            {
                AddDiagnostic(inspection, start, length, "Niepoprawny formatter lub lista kategorii.");
                code = formatter;
            }

            VerseFormatterKind kind = GetKind(code);
            if (kind == VerseFormatterKind.Unknown)
                AddDiagnostic(inspection, start, length, "Nieznany formatter „" + code + "”.");
            else if (!HasValidShape(code, kind))
                AddDiagnostic(inspection, start, length, "Formatter „" + code + "” ma niepoprawną długość.");
            if (kind == VerseFormatterKind.ReflexiveParticle && string.IsNullOrEmpty(groupName))
                AddDiagnostic(inspection, start, length, "Znacznik R musi należeć do nazwanej grupy.");

            return new VerseFormatterToken
            {
                StartIndex = start,
                Length = length,
                Descriptor = descriptor,
                GroupName = groupName,
                Code = code,
                Kind = kind
            };
        }

        private static VerseFormatterKind GetKind(string code)
        {
            if (code == "R")
                return VerseFormatterKind.ReflexiveParticle;
            if (string.IsNullOrEmpty(code))
                return VerseFormatterKind.Unknown;
            switch (code[0])
            {
                case 'N': return VerseFormatterKind.Noun;
                case 'A': return VerseFormatterKind.Adjective;
                case 'V': return VerseFormatterKind.Verb;
                case 'P': return VerseFormatterKind.Pronoun;
                default: return VerseFormatterKind.Unknown;
            }
        }

        private static bool HasValidShape(string code, VerseFormatterKind kind)
        {
            switch (kind)
            {
                case VerseFormatterKind.Noun: return code.Length == 4;
                case VerseFormatterKind.Adjective: return code.Length == 5;
                case VerseFormatterKind.Verb: return code.Length == 4 || code.Length == 5;
                case VerseFormatterKind.Pronoun: return code.Length == 5 || code.Length == 6;
                case VerseFormatterKind.ReflexiveParticle: return code == "R";
                default: return false;
            }
        }

        private static void ValidateGroup(VerseGroupSummary group, VerseDefinitionInspection inspection)
        {
            int nounCount = group.Tokens.FindAll(item => item.Kind == VerseFormatterKind.Noun).Count;
            int verbCount = group.Tokens.FindAll(item => item.Kind == VerseFormatterKind.Verb).Count;
            List<VerseFormatterToken> reflexive = group.Tokens.FindAll(
                item => item.Kind == VerseFormatterKind.ReflexiveParticle);
            if (nounCount == 0)
                AddDiagnostic(inspection, group.Tokens[0].StartIndex, group.Tokens[0].Length,
                    "Grupa „" + group.Name + "” nie ma kotwicy rzeczownikowej.");
            else if (nounCount > 1)
                AddDiagnostic(inspection, group.Tokens[0].StartIndex, group.Tokens[0].Length,
                    "Grupa „" + group.Name + "” ma więcej niż jeden rzeczownik.");
            if (reflexive.Count > 1)
                AddDiagnostic(inspection, reflexive[1].StartIndex, reflexive[1].Length,
                    "Grupa „" + group.Name + "” ma więcej niż jeden znacznik R.");
            if (reflexive.Count > 0 && verbCount != 1)
                AddDiagnostic(inspection, reflexive[0].StartIndex, reflexive[0].Length,
                    "Grupa „" + group.Name + "” z R musi mieć dokładnie jeden czasownik.");
        }

        private static void AddDiagnostic(VerseDefinitionInspection inspection, int start, int length,
            string message)
        {
            inspection.Diagnostics.Add(new VerseDefinitionDiagnostic
            {
                StartIndex = start,
                Length = length,
                Message = message
            });
        }
    }
}
