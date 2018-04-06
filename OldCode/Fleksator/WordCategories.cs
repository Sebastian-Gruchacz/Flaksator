using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SharpDevs.Helpers;
using SharpDevs.Helpers.Ini;

namespace SharpDevs.Fleksator
{
    /// <summary>
    /// Contains list of language part categories
    /// </summary>
    public class WordCategories
    {
        #region Singleton Implementation

        private static volatile WordCategories _decliner = null;
        private static object _lockObject = new object();

        public static WordCategories Categories
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new WordCategories();
                    }
                }

                return _decliner;
            }
        }

        private WordCategories()
        { }

        #endregion

        private Dictionary<int, string> nounCategories = new Dictionary<int, string>();
        public Dictionary<int, string> NounCategories
        {
            get { return this.nounCategories; }
        }

        private Dictionary<int, string> adjCategories = new Dictionary<int, string>();
        public Dictionary<int, string> AdjectiveCategories
        {
            get { return this.adjCategories; }
        }

        private Dictionary<int, string> verbCategories = new Dictionary<int, string>();
        public Dictionary<int, string> VerbCategories
        {
            get { return this.verbCategories; }
        }

        #region Loading & Saving

        void LoadDictionary(Dictionary<int, string> dic, IniSection section)
        {
            dic.Clear();

            if (section == null)
                return;

            foreach (IniKey iKey in section.Keys)
            {
                if (iKey.IsComment)
                    continue; // entry is commented out

                string key = iKey.Name;
                string value = iKey.Value;
#if DEBUG
                if (key.Trim().StartsWith("EOF"))
                    break; // for testing purposes
#else
                if (key.Trim().StartsWith("EOF"))
                    continue; // ignore
#endif
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    dic.Add(int.Parse(key), value);
            }
        }

        public void LoadFromFile(string filePath)
        {
            IniFile iFile = new IniFile();
            FileInfo info = new FileInfo(filePath);
            if (!info.Exists)
                throw new FileNotFoundException();

            StreamReader sr = new StreamReader(info.FullName, Encoding.Unicode, true);

            try
            {
                // Load Titles
                iFile.Load(sr);

                this.LoadDictionary(this.nounCategories, iFile["Noun"]);
                this.LoadDictionary(this.adjCategories, iFile["Adjective"]);
                this.LoadDictionary(this.verbCategories, iFile["Verb"]);
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
    }
}
