using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obscureware.Flaksator.Parsers
{
    public class VerseParserVersion1 : IVerseParser
    {
        public int SupportedVersion => 1;

        public VerseBuilder Parse(string verseString)
        {
            throw new NotImplementedException();
        }


        protected string DecodeLine(string verse)
        {
            List<string> descriptors = new List<string>();

            string result = null;

            int bracketIndex = verse.IndexOf('{');
            int elementNumber = 0;
            int closingBracket = 0;
            if (bracketIndex >= 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(verse.Substring(closingBracket, bracketIndex - closingBracket));

                while (bracketIndex >= 0)
                {
                    closingBracket = verse.IndexOf('}', bracketIndex + 1);
                    if (closingBracket < 0)
                        return null; //syntax error

                    // try finding valid word
                    string code = verse.Substring(bracketIndex + 1, closingBracket - bracketIndex - 1);

                    descriptors.Add(code);
                    sb.Append("{" + elementNumber + "}");
                    elementNumber++;

                    bracketIndex = verse.IndexOf('{', closingBracket + 1);

                    if (bracketIndex >= 0)
                        sb.Append(verse.Substring(closingBracket + 1, bracketIndex - closingBracket - 1));
                    else
                        sb.Append(verse.Substring(closingBracket + 1));
                }

                //syntax error (note that there is no way to display brackets in text this way
                result = sb.ToString();
            }
            else
            {
                return verse; // no placeholders
            }

            // now decoding...
            string[] results = new string[descriptors.Count];
            Dictionary<int, GroupInfo> groups = new Dictionary<int, GroupInfo>();

            for (int i = 0; i < descriptors.Count; i++)
            {
                string code = descriptors[i];

                // check group
                int index = code.IndexOf(':');
                if (index >= 0)
                {
                    // this is group
                    string groupStr = code.Substring(0, index);
                    int groupNumber = -1;
                    if (!int.TryParse(groupStr, out groupNumber))
                    {
                        results[i] = "{INVALID_GROUP_FORMATTER}";
                    }
                    else
                    {
                        GroupInfo info = null;
                        if (groups.ContainsKey(groupNumber))
                            info = groups[groupNumber];
                        else
                        {
                            info = new GroupInfo(groupNumber);
                            groups.Add(groupNumber, info);
                        }

                        info.AddCode(i, code.Substring(index + 1));
                    }
                }
                else
                {
                    // this is normal word
                    results[i] = this.Decode(code);
                }
            }

            // decode groups
            foreach (GroupInfo group in groups.Values)
            {
                bool rzlt = group.Analyze();
                foreach (int ind in group.Results.Keys)
                {
                    results[ind] = group.Results[ind];
                }

            }

            return string.Format(result, results);
        }
    }

    public interface IVerseParser
    {
        int SupportedVersion { get; }

        VerseBuilder Parse(string verseString);
    }

    public class VerseBuilder
    {
    }
}
