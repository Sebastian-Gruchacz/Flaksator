using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SharpDevs.Helpers;
using SharpDevs.Helpers.Ini;
using SharpDevs.Fleksator;

namespace Flaksator
{
    class SongCreator : DecodeBase
    {
        #region Singleton Implementation

        private static volatile SongCreator _decliner = null;
        private static object _lockObject = new object();

        public static SongCreator Creator
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new SongCreator();
                    }
                }

                return _decliner;
            }
        }

        private SongCreator()
        { }

        #endregion

        Random rnd = new Random();

        private List<string> elements = new List<string>();
        Dictionary<string, string> parts = new Dictionary<string, string>();

        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.elements.Clear();
            this.parts.Clear();

            IniFile iFile = new IniFile();
            FileInfo info = new FileInfo(filePath);
            if (!info.Exists)
                throw new FileNotFoundException();

            StreamReader sr = new StreamReader(info.FullName, Encoding.Unicode, true);
            try
            {
                iFile.Load(sr);


                IniSection iTitles = iFile["Elements"];

                foreach (IniKey iKey in iTitles.Keys)
                {
                    if (iKey.IsComment)
                        continue; // entry is commented out

                    string line = iKey.Name;
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes

#endif

                    if (!string.IsNullOrEmpty(line))
                        elements.Add(line);
                }

                // static elements
                iTitles = iFile["StaticElements"];


                foreach (IniKey iKey in iTitles.Keys)
                {
                    if (iKey.IsComment)
                        continue; // entry is commented out

                    string line = iKey.Name;
                    string val = iKey.Value;
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes

#endif
                    if (!string.IsNullOrEmpty(line))
                        parts.Add(line, val);
                }
            }
            finally
            {
                sr.Close();
            }

        }

        public void SaveToFile(string filePath)
        {

        }

        #endregion


        public string GenerateRandomSong()
        {
            StringBuilder sb = new StringBuilder();

            // Write title
            sb.AppendLine(TitleCreator.Creator.GetRandomTitle().ToUpper());
            sb.AppendLine();

            if (rnd.Next(100) > 92)
            {
                // instrumental
                sb.AppendFormat("({0})", parts["INSTR"]);
            }
            else
            {
                #region Main Part

                int songElements = rnd.Next(3, 15); // max number of song parts
                bool variousRefrains = rnd.Next(100) > 70; // specify if refrains shall be random or not...

                string refrain = GetStanza(rnd.Next(2, 5));
                int stanzaLength = rnd.Next(2, 5);

                SongPart lastPart = SongPart.Refrain;

                for (int i = 1; i <= songElements; i++)
                {
                    // let there be no 2 refrains one after one
                    SongPart part = RandomizePart();
                    if (part == lastPart)
                        while (part == SongPart.Refrain)
                            part = RandomizePart();

                    lastPart = part;

                    // No Bridges or interludes on the begging
                    if (i == 1 &&
                        (part == SongPart.Bridge || part == SongPart.Interludium))
                        continue;

                    switch (part)
                    {
                        case SongPart.Refrain:
                            {
                                sb.AppendFormat("({0}){1}", parts["REF"], Environment.NewLine);

                                if (variousRefrains)
                                    sb.Append(GetStanza(rnd.Next(2, 5)));
                                else
                                    sb.Append(refrain);

                                sb.Append(Environment.NewLine);
                                break;
                            }

                        case SongPart.Stanza:
                            {
                                sb.Append(GetStanza(stanzaLength));
                                sb.Append(Environment.NewLine);
                                break;
                            }

                        case SongPart.Bridge:
                            {
                                sb.AppendFormat("({0}){1}", parts["BRIDGE"], Environment.NewLine);
                                sb.Append(Environment.NewLine);
                                break;
                            }

                        case SongPart.Interludium:
                            {
                                sb.AppendFormat("({0}){1}", parts["INTER"], Environment.NewLine);
                                sb.Append(Environment.NewLine);
                                break;
                            }

                        case SongPart.Element:
                            {
                                string ePart = this.elements[rnd.Next(0, this.elements.Count)];
                                ePart = CapitalizeMultiline(DecodeLine(ePart));
                                sb.AppendFormat("({0}){1}", ePart, Environment.NewLine);
                                sb.Append(Environment.NewLine);

                                // add stanza after each separator

                                sb.Append(GetStanza(stanzaLength));
                                sb.Append(Environment.NewLine);


                                break;
                            }
                    }
                }

                #endregion

                // There should not be special element as last item...
                if (lastPart == SongPart.Element || lastPart == SongPart.Interludium
                    || lastPart == SongPart.Bridge)
                {
                    // put one more stanza after
                    sb.Append(GetStanza(rnd.Next(2, 5)));
                    sb.Append(Environment.NewLine);
                }

                // Ending
                if (rnd.Next(100) > 90)
                {
                    sb.AppendFormat("({0}){1}", parts["END"], Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    sb.Append(GetStanza(rnd.Next(2, 5)));
                    sb.Append(Environment.NewLine);
                }

            }

            return sb.ToString();
        }

        public string GetStanza(int versesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= versesCount; i++)
            {
                sb.AppendLine(GetSongVerse());
            }

            return sb.ToString();
        }

        private SongPart RandomizePart()
        {
            int rand = rnd.Next(100);

            if (rand < 40)
                return SongPart.Stanza;
            else if (rand < 80)
                return SongPart.Refrain;
            else if (rand < 90)
                return SongPart.Element;
            else if (rand < 95)
                return SongPart.Bridge;
            else
                return SongPart.Interludium;
        }

        public string GetSongVerse()
        {
            string verse = VersesCreator.Creator.GetRandomVerse().Trim();
            if (verse == null)
                return null;

            // break lines
            verse = verse.Replace("<br>", Environment.NewLine);

            // Capiatalize first lines       

            // & return
            return CapitalizeMultiline(verse);

        }

        private string CapitalizeMultiline(string line)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(line[0].ToString().ToUpper());
            int index = 1;
            int nextIndex = -1;
            while ((nextIndex = line.IndexOf("\n", index)) >= 0)
            {
                sb.Append(line.Substring(index, nextIndex - index));
                sb.Append(Environment.NewLine);
                if (nextIndex < line.Length)
                    sb.Append(line[nextIndex + 1].ToString().ToUpper());
                index = nextIndex + 2; // skip big letter

            }
            sb.Append(line.Substring(index));
            return sb.ToString();
        }
    }


    public enum SongPart { Stanza, Refrain, Bridge, Element, Interludium };
}
