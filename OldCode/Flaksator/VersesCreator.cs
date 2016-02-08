using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Flaksator
{
    class VersesCreator : DecodeBase
    {
        #region Singleton Implementation

        private static volatile VersesCreator _decliner = null;
        private static object _lockObject = new object();

        public static VersesCreator Creator
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new VersesCreator();
                    }
                }

                return _decliner;
            }
        }

        private VersesCreator()
        { }

        #endregion

        Random rnd = new Random();

        private List<string> verses = new List<string>();

        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.verses.Clear();

            FileInfo fi = new FileInfo(filePath);
            TextReader tr = new StreamReader(fi.FullName, Encoding.Default, true);
            string line = null;
            try
            {
                while ((line = tr.ReadLine()) != null)
                {
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes

#endif
                    if (line.Trim().StartsWith(";"))
                        continue; // entry is commented out

                    if (!string.IsNullOrEmpty(line))
                        verses.Add(line);
                }
            }
            finally
            {
                tr.Close();
            }
        }

        public void SaveToFile(string filePath)
        {

        }

        #endregion

        public string GetRandomVerse()
        {
            string verse = this.verses[rnd.Next(0, this.verses.Count)];

            return DecodeLine(verse);
        }


    }
}
