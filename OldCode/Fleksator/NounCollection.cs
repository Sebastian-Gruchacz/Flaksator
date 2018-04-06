using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Fleksator.IO;

namespace SharpDevs.Fleksator
{
    public class NounCollection
    {
        #region Singleton Implementation

        private static volatile NounCollection _decliner = null;
        private static object _lockObject = new object();

        public static NounCollection Collection
        {
            get
            {
                if (_decliner == null)
                {
                    lock (_lockObject)
                    {
                        if (_decliner == null)
                            _decliner = new NounCollection();
                    }
                }

                return _decliner;
            }
        }

        private NounCollection()
        {
            // TODO: zmieniæ na lepszy inject, wywaliæ singletony
            this._grammarSerializers = new GrammarSerializersFactory().GetOldSerializers();
        }

        #endregion

        private List<Noun> nouns = new List<Noun>();
        public List<Noun> Nouns
        {
            get { return this.nouns; }
        }

        public Dictionary<GrammaticalGender, List<Noun>> sortedNouns = new Dictionary<GrammaticalGender, List<Noun>>();
        public Dictionary<GrammaticalGender, List<Noun>> SortedNouns
        {
            get { return this.sortedNouns; }
        }


        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.nouns.Clear();

            FileInfo fi = new FileInfo(filePath);
            TextReader tr = new StreamReader(fi.FullName, Encoding.Unicode, true);
            string line = null;
            try
            {
                while ((line = tr.ReadLine()) != null)
                {
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                        break; // for testing purposes
#else
                    if (line.Trim().StartsWith("EOF"))
                        continue; // ignore
#endif
                    if (line.Trim().StartsWith(";"))
                        continue; // entry is commented out


                    Noun noun = this._grammarSerializers.NounSerializer.Load(line);
                    if (noun != null)
                    {
                        List<Noun> subList = null;
                        if (this.sortedNouns.ContainsKey(noun.Genre))
                            subList = this.sortedNouns[noun.Genre];
                        else
                        {
                            subList = new List<Noun>();
                            this.sortedNouns.Add(noun.Genre, subList);
                        }
                        subList.Add(noun);
                        this.nouns.Add(noun);

                    }
                }
            }
            finally
            {
                tr.Close();
            }
        }

        public void SaveToFile(string filePath)
        {
            FileInfo fi = new FileInfo(filePath);
            TextWriter tw = new StreamWriter(fi.FullName, false, Encoding.Unicode);
            try
            {
                foreach (Noun noun in this.nouns)
                {
                    tw.WriteLine(this._grammarSerializers.NounSerializer.Write(noun));
                }
            }
            finally
            {
                tw.Close();
            }
        }

        #endregion

        Random rnd = new Random();

        private IGrammarSerializers _grammarSerializers;

        public Noun GetRandomNoun()
        {
            return this.nouns[this.rnd.Next(0, this.nouns.Count)];
        }

        public Noun GetRandomNoun(GrammaticalGender genre)
        {
            if (!this.sortedNouns.ContainsKey(genre))
                return null;

            List<Noun> subList = this.sortedNouns[genre];

            if (subList.Count > 0)
                return subList[this.rnd.Next(0, subList.Count)];
            else
                return null;
        }

        public Noun GetNoun(GrammaticalGender? genre, int? categoryId, bool shallSupportSingular, bool shallSupportPlural)
        {
            List<Noun> searchResults = this.nouns.FindAll(p => (genre == null || p.Genre == genre)
                && (categoryId == null || p.Categories.Contains(categoryId ?? -1))
                && (!shallSupportPlural || p.CanBePlural)
                && (!shallSupportSingular || p.CanBeSingular));

            if (searchResults == null || searchResults.Count < 1)
                return null;

            return searchResults[this.rnd.Next(0, searchResults.Count)];
        }

        public Noun GetNounEx(GrammaticalGender? genre, List<int> categoryIds, bool shallSupportSingular, bool shallSupportPlural)
        {
            List<Noun> searchResults = this.nouns.FindAll(p => (genre == null || p.Genre == genre)
                && (categoryIds == null || categoryIds.Count == 0 || p.Categories.Exists(d => categoryIds.Contains(d)))
                && (!shallSupportSingular || p.CanBeSingular));

            if (searchResults == null || searchResults.Count < 1)
                return null;

            return searchResults[this.rnd.Next(0, searchResults.Count)];
        }
    }
}
