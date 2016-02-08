using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SharpDevs.Helpers;
using SharpDevs.Helpers.Ini;
using SharpDevs.Fleksator;

namespace Flaksator
{
    class TitleCreator : DecodeBase
    {
        #region Singleton Implementation

        private static volatile TitleCreator _decliner = null;
        private static object _lockObject = new object();

        public static TitleCreator Creator
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new TitleCreator();
                    }
                }

                return _decliner;
            }
        }

        private TitleCreator()
        { }

        #endregion

        Random rnd = new Random();

        private List<string> titles = new List<string>();
        private List<string> subTitles = new List<string>();

        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.titles.Clear();

            IniFile iFile = new IniFile();
            FileInfo info = new FileInfo(filePath);
            if (!info.Exists)
                throw new FileNotFoundException();

            StreamReader sr = new StreamReader(info.FullName, Encoding.Unicode, true);
            try
            {
                // Load Titles
                iFile.Load(sr);
                IniSection iTitles = iFile["MainPart"];
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
                        titles.Add(line);
                }

                // Load subtitles
                this.subTitles.Clear();
                IniSection iSubTitles = iFile["SubTitle"];
                foreach (IniKey iKey in iSubTitles.Keys)
                {
                    if (iKey.IsComment)
                        continue; // entry is commented out

                    string line = iKey.Name;
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes

#endif

                    if (!string.IsNullOrEmpty(line))
                        subTitles.Add(line);
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

        public string GetRandomTitle()
        {
            string verse = this.titles[rnd.Next(0, this.titles.Count)];
            string title = DecodeLine(verse);

            if (rnd.Next(100) > 80)
            {
                verse = this.subTitles[rnd.Next(0, this.subTitles.Count)];
                string subTitle = DecodeLine(verse);
                if (subTitle != null)
                {
                    title = string.Format("{0} ({1})", title, subTitle);
                }
            }

            return title;
        }

    }
}
