using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpDevs.Fleksator.Grammar;
using SharpDevs.Fleksator.IO;

namespace SharpDevs.Fleksator
{
    public sealed class VerbCollection
    {
        private static readonly VerbCollection collection = new VerbCollection();
        private readonly List<Verb> verbs = new List<Verb>();
        private readonly IGrammaticalWordSerializer<Verb> serializer;
        private readonly Random random = new Random();

        public static VerbCollection Collection
        {
            get { return collection; }
        }

        public List<Verb> Verbs
        {
            get { return this.verbs; }
        }

        private VerbCollection()
        {
            this.serializer = new GrammarSerializersFactory().GetOldSerializers().VerbSerializer;
        }

        public void LoadFromFile(string filePath)
        {
            this.verbs.Clear();
            using (TextReader reader = new StreamReader(filePath, Encoding.Unicode, true))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string trimmed = line.Trim();
                    if (trimmed.Length == 0 || trimmed.StartsWith(";") || trimmed.StartsWith("EOF"))
                        continue;

                    Verb verb = this.serializer.Load(line);
                    if (verb != null)
                        this.verbs.Add(verb);
                }
            }
        }

        public void SaveToFile(string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath, false, Encoding.Unicode))
            {
                foreach (Verb verb in this.verbs)
                    writer.WriteLine(this.serializer.Write(verb));
            }
        }

        public Verb GetVerb(Predicate<Verb> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            List<Verb> matches = this.verbs.FindAll(candidate =>
                GenerationCategoryFilter.AllowsVerb(candidate.Categories) && predicate(candidate));
            return matches.Count == 0 ? null : matches[this.random.Next(matches.Count)];
        }
    }
}
