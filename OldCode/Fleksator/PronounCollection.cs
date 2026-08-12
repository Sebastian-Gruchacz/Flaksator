using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Fleksator.IO;

namespace SharpDevs.Fleksator
{
    public sealed class PronounCollection
    {
        private static readonly PronounCollection collection = new PronounCollection();
        private readonly List<Pronoun> pronouns = new List<Pronoun>();
        private readonly IGrammaticalWordSerializer<Pronoun> serializer;
        private readonly Random random = new Random();

        public static PronounCollection Collection
        {
            get { return collection; }
        }

        public List<Pronoun> Pronouns
        {
            get { return this.pronouns; }
        }

        private PronounCollection()
        {
            this.serializer = new GrammarSerializersFactory().GetOldSerializers().PronounSerializer;
        }

        public void LoadFromFile(string filePath)
        {
            this.pronouns.Clear();
            using (TextReader reader = new StreamReader(filePath, Encoding.Unicode, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimmed = line.Trim();
                    if (trimmed.Length == 0 || trimmed.StartsWith(";") || trimmed.StartsWith("EOF"))
                        continue;

                    Pronoun pronoun = this.serializer.Load(line);
                    if (pronoun != null)
                        this.pronouns.Add(pronoun);
                }
            }
        }

        public Pronoun GetPronoun(Predicate<Pronoun> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            List<Pronoun> matches = this.pronouns.FindAll(predicate);
            return matches.Count == 0 ? null : matches[this.random.Next(matches.Count)];
        }
    }
}
