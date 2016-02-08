using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SharpDevs.Fleksator.Grammar;

namespace SharpDevs.Fleksator
{
    public class AdjectiveCollection
    {
        #region Singleton Implementation

        private static volatile AdjectiveCollection _collection = null;
        private static object _lockObject = new object();

        public static AdjectiveCollection Collection
        {
            get
            {
                if (_collection == null)
                {
                    lock (_lockObject)
                    {
                        if (_collection == null)
                            _collection = new AdjectiveCollection();
                    }
                }

                return _collection;
            }
        }

        private AdjectiveCollection()
        { }

        #endregion

        private List<Adjective> adjectives = new List<Adjective>();
        public List<Adjective> Adjectives
        {
            get { return adjectives; }
        }

        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.adjectives.Clear();

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

                    Adjective adjective = new Adjective();
                    if (adjective.AnalyzeLine(line))
                        this.adjectives.Add(adjective);
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

        Random rnd = new Random();

        public Adjective GetRandomAdjective()
        {
            return this.adjectives[rnd.Next(0, this.adjectives.Count)];
        }
    }
}
