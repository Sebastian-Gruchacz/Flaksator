using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Flaksator
{
    public sealed class VerseEntry
    {
        internal VerseEntry(string text, int sourceLineIndex)
        {
            this.Text = text;
            this.SourceLineIndex = sourceLineIndex;
        }

        public VerseEntry(string text)
            : this(text, -1)
        { }

        public string Text { get; set; }

        internal int SourceLineIndex { get; set; }

        public override string ToString()
        {
            return this.Text;
        }
    }

    public class VersesCreator : DecodeBase
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

        private readonly List<VerseEntry> verses = new List<VerseEntry>();
        private readonly List<string> sourceLines = new List<string>();
        private readonly HashSet<int> originalVerseLineIndexes = new HashSet<int>();

        public List<VerseEntry> Verses
        {
            get { return this.verses; }
        }

        #region Loading & Saving

        public void LoadFromFile(string filePath)
        {
            this.verses.Clear();
            this.sourceLines.Clear();
            this.originalVerseLineIndexes.Clear();

            FileInfo fi = new FileInfo(filePath);
            TextReader tr = new StreamReader(fi.FullName, Encoding.Default, true);
            string line = null;
            bool loadVerses = true;
            try
            {
                while ((line = tr.ReadLine()) != null)
                {
                    int sourceLineIndex = this.sourceLines.Count;
                    this.sourceLines.Add(line);
#if DEBUG
                    if (line.Trim().StartsWith("EOF"))
                    {
                        loadVerses = false;
                        continue; // preserve the rest of the source, but do not generate from it
                    }
#else
                    if (line.Trim().StartsWith("EOF"))
                        continue; // ignore
#endif
                    if (!loadVerses)
                        continue;
                    if (line.Trim().StartsWith(";"))
                        continue; // entry is commented out

                    if (!string.IsNullOrEmpty(line))
                    {
                        this.verses.Add(new VerseEntry(line, sourceLineIndex));
                        this.originalVerseLineIndexes.Add(sourceLineIndex);
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
            Dictionary<int, VerseEntry> existingEntries = new Dictionary<int, VerseEntry>();
            List<VerseEntry> newEntries = new List<VerseEntry>();
            foreach (VerseEntry entry in this.verses)
            {
                if (entry.SourceLineIndex >= 0)
                    existingEntries[entry.SourceLineIndex] = entry;
                else
                    newEntries.Add(entry);
            }

            int insertionIndex = this.sourceLines.FindIndex(
                item => item.Trim().StartsWith("EOF", StringComparison.Ordinal));
            if (insertionIndex < 0)
                insertionIndex = this.sourceLines.Count;

            using (TextWriter writer = new StreamWriter(filePath, false, Encoding.Unicode))
            {
                for (int i = 0; i <= this.sourceLines.Count; i++)
                {
                    if (i == insertionIndex)
                    {
                        foreach (VerseEntry entry in newEntries)
                            writer.WriteLine(entry.Text);
                    }

                    if (i == this.sourceLines.Count)
                        break;

                    VerseEntry existing;
                    if (this.originalVerseLineIndexes.Contains(i))
                    {
                        if (existingEntries.TryGetValue(i, out existing))
                            writer.WriteLine(existing.Text);
                    }
                    else
                    {
                        writer.WriteLine(this.sourceLines[i]);
                    }
                }
            }

            this.LoadFromFile(filePath);
        }

        #endregion

        public string GetRandomVerse()
        {
            string verse = this.verses[this.rnd.Next(0, this.verses.Count)].Text;

            return this.DecodeLine(verse);
        }

        public string DecodeVerse(string verse)
        {
            if (verse == null)
                throw new ArgumentNullException("verse");

            return this.DecodeLine(verse);
        }


    }
}
