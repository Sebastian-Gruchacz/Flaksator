using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Flaksator;
using SharpDevs.Fleksator;
using SharpDevs.Fleksator.Grammar;

namespace LegacyGroups.Specs
{
    internal static class Program
    {
        private static int failures;

        private static int Main()
        {
            Run("adjective inherits noun agreement", AdjectiveInheritsNounAgreement);
            Run("explicit adjective features override inheritance", ExplicitAdjectiveFeaturesOverrideInheritance);
            Run("group without noun returns diagnostics", GroupWithoutNounReturnsDiagnostics);
            Run("duplicate noun returns diagnostics", DuplicateNounReturnsDiagnostics);
            Run("group preserves requested noun number", GroupPreservesRequestedNounNumber);
            Run("unavailable noun number returns diagnostics", UnavailableNounNumberReturnsDiagnostics);
            Run("invalid member does not crash decoding", InvalidMemberDoesNotCrashDecoding);
            Run("empty adjective dictionary returns diagnostics", EmptyAdjectiveDictionaryReturnsDiagnostics);
            Run("group selects an adjective that supports the requested level", GroupSelectsAdjectiveSupportingRequestedLevel);
            Run("categories filter ungrouped words", CategoriesFilterUngroupedWords);
            Run("categories filter grouped words", CategoriesFilterGroupedWords);
            Run("invalid category suffix is rejected", InvalidCategorySuffixIsRejected);
            Run("unsupported verb returns diagnostics", UnsupportedVerbReturnsDiagnostics);
            Run("legacy dictionaries load", LegacyDictionariesLoad);
            Run("expanded adjectives inflect directly", ExpandedAdjectivesInflectDirectly);
            Run("legacy category references are defined", LegacyCategoryReferencesAreDefined);
            Run("legacy dictionaries are fully categorized", LegacyDictionariesAreFullyCategorized);
            Run("legacy dictionaries generate a noun phrase", LegacyDictionariesGenerateNounPhrase);
            Run("legacy category formatters decode", LegacyCategoryFormattersDecode);
            Run("extended adjective categories decode", ExtendedAdjectiveCategoriesDecode);
            Run("extended noun categories decode", ExtendedNounCategoriesDecode);
            Run("grouped legacy templates decode", GroupedLegacyTemplatesDecode);

            Console.WriteLine(failures == 0
                ? "All legacy group specifications passed."
                : failures + " legacy group specification(s) failed.");
            return failures == 0 ? 0 : 1;
        }

        private static void AdjectiveInheritsNounAgreement()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???E");
            group.AddCode(1, "NN1F");

            AssertTrue(group.Analyze(), "valid noun phrase should be analyzed");
            AssertEqual("czarna", group.Results[0], "adjective should inherit feminine nominative singular");
            AssertEqual("kotka", group.Results[1], "noun should be emitted");
        }

        private static void ExplicitAdjectiveFeaturesOverrideInheritance()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "AG??E");
            group.AddCode(1, "NN1F");

            AssertTrue(group.Analyze(), "valid noun phrase with an override should be analyzed");
            AssertEqual("czarnej", group.Results[0], "explicit genitive should override the noun case");
            AssertEqual("kotka", group.Results[1], "noun should retain its own nominative case");
        }

        private static void GroupWithoutNounReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???E");

            AssertFalse(group.Analyze(), "a group without a noun cannot be analyzed");
            AssertEqual("{GROUP_REQUIRES_NOUN}", group.Results[0], "missing noun should not produce blank output");
        }

        private static void DuplicateNounReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "NN1F");
            group.AddCode(1, "NN1F");

            AssertFalse(group.Analyze(), "a second noun should invalidate the group");
            AssertEqual("kotka", group.Results[0], "the first noun remains the group anchor");
            AssertEqual("{GROUP_HAS_MULTIPLE_NOUNS}", group.Results[1], "the duplicate noun should be diagnosed");
        }

        private static void InvalidMemberDoesNotCrashDecoding()
        {
            SetSingleFeminineNounAndAdjective();

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{1:} {1:NN1F}");

            AssertEqual("{INVALID_GROUP_FORMATTER} kotka", result, "invalid group member should survive as a diagnostic marker");
        }

        private static void GroupPreservesRequestedNounNumber()
        {
            SetSingleFeminineNounAndAdjective("kotki", false, true);

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???E");
            group.AddCode(1, "NN2F");

            AssertTrue(group.Analyze(), "a plural group should select a plural-capable noun");
            AssertEqual("czarne", group.Results[0], "adjective should inherit plural agreement");
            AssertEqual("kotki", group.Results[1], "plural noun should be emitted");
        }

        private static void UnavailableNounNumberReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective("kotka", true, false);

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???E");
            group.AddCode(1, "NN2F");

            AssertFalse(group.Analyze(), "a group must not silently change the requested number");
            AssertEqual("{GROUP_NOUN_UNAVAILABLE}", group.Results[0], "dependent members should explain the missing anchor");
            AssertEqual("{NO_VALID_NOUN}", group.Results[1], "noun should report that no matching number is available");
        }

        private static void UnsupportedVerbReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "V");
            group.AddCode(1, "NN1F");

            AssertFalse(group.Analyze(), "verbs are not supported by noun groups yet");
            AssertEqual("{UNSUPPORTED_GROUP_VERB}", group.Results[0], "unsupported feature should be explicit");
            AssertEqual("kotka", group.Results[1], "supported members should still be generated");
        }

        private static void EmptyAdjectiveDictionaryReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective();
            AdjectiveCollection.Collection.Adjectives.Clear();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???E");
            group.AddCode(1, "NN1F");

            AssertFalse(group.Analyze(), "an empty adjective dictionary should invalidate the group");
            AssertEqual("{NO_VALID_ADJECTIVE}", group.Results[0], "empty dictionary should not throw");
            AssertEqual("kotka", group.Results[1], "noun should still be emitted");
        }

        private static void GroupSelectsAdjectiveSupportingRequestedLevel()
        {
            SetSingleFeminineNounAndAdjective();
            AdjectiveCollection.Collection.Adjectives.Clear();
            for (int i = 0; i < 100; i++)
            {
                AdjectiveCollection.Collection.Adjectives.Add(new Adjective
                {
                    Root = "martwy",
                    CanBeLevelled = false
                });
            }
            AdjectiveCollection.Collection.Adjectives.Add(new Adjective { Root = "jasny" });

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "A???H");
            group.AddCode(1, "NN1F");

            AssertTrue(group.Analyze(), "a valid adjective must not be missed among incompatible entries");
            AssertEqual("bardziej jasna", group.Results[0], "the selected adjective should support the requested level");
            AssertEqual("kotka", group.Results[1], "noun should still be emitted");
        }

        private static void CategoriesFilterUngroupedWords()
        {
            SetCategorizedWords();

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{NN1F$11} {AN1FE$1}");

            AssertEqual("siekiera czarna", result, "category suffixes should filter independent words");
        }

        private static void CategoriesFilterGroupedWords()
        {
            SetCategorizedWords();

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{1:A???E$1} {1:NN1F$11}");

            AssertEqual("czarna siekiera", result, "category suffixes should filter every grouped member");
        }

        private static void InvalidCategorySuffixIsRejected()
        {
            string grammarCode;
            List<int> categoryIds;

            AssertFalse(WordFormatterParser.TryParse("NN1F$", out grammarCode, out categoryIds),
                "an empty category suffix should be invalid");
            AssertFalse(WordFormatterParser.TryParse("NN1F$weapon", out grammarCode, out categoryIds),
                "a non-numeric category should be invalid");
            AssertTrue(WordFormatterParser.TryParse("NN1F$3,11", out grammarCode, out categoryIds),
                "multiple numeric categories should be accepted");
            AssertEqual("NN1F", grammarCode, "category parsing should preserve the grammar code");
            AssertTrue(categoryIds.Count == 2 && categoryIds.Contains(3) && categoryIds.Contains(11),
                "all requested categories should be retained");
        }

        private static void LegacyDictionariesLoad()
        {
            LoadLegacyData();

            AssertTrue(NounCollection.Collection.Nouns.Count > 100, "legacy noun dictionary should contain substantial data");
            AssertTrue(AdjectiveCollection.Collection.Adjectives.Count >= 317,
                "expanded adjective dictionary should contain the historical entries and the new category set");
            AssertTrue(WordCategories.Categories.NounCategories.Count >= 23, "complete noun categories should load");
            AssertTrue(WordCategories.Categories.AdjectiveCategories.Count >= 22, "complete adjective categories should load");
        }

        private static void ExpandedAdjectivesInflectDirectly()
        {
            LoadLegacyData();
            string[][] forms =
            {
                new[] { "melodyjna", "melodyjnej", "melodyjne" },
                new[] { "hałaśliwa", "hałaśliwej", "hałaśliwe" },
                new[] { "donośna", "donośnej", "donośne" },
                new[] { "piskliwa", "piskliwej", "piskliwe" },
                new[] { "chrapliwa", "chrapliwej", "chrapliwe" },
                new[] { "dudniąca", "dudniącej", "dudniące" },
                new[] { "ogłuszająca", "ogłuszającej", "ogłuszające" },
                new[] { "bezgłośna", "bezgłośnej", "bezgłośne" },
                new[] { "mechaniczna", "mechanicznej", "mechaniczne" },
                new[] { "laserowa", "laserowej", "laserowe" },
                new[] { "atomowa", "atomowej", "atomowe" },
                new[] { "robotyczna", "robotycznej", "robotyczne" },
                new[] { "cybernetyczna", "cybernetycznej", "cybernetyczne" },
                new[] { "analogowa", "analogowej", "analogowe" },
                new[] { "kwantowa", "kwantowej", "kwantowe" },
                new[] { "wrząca", "wrzącej", "wrzące" },
                new[] { "letnia", "letniej", "letnie" },
                new[] { "rozgrzana", "rozgrzanej", "rozgrzane" },
                new[] { "zamarznięta", "zamarzniętej", "zamarznięte" },
                new[] { "parząca", "parzącej", "parzące" },
                new[] { "mroźna", "mroźnej", "mroźne" },
                new[] { "chłodząca", "chłodzącej", "chłodzące" },
                new[] { "gładka", "gładkiej", "gładkie" },
                new[] { "szorstka", "szorstkiej", "szorstkie" },
                new[] { "lepka", "lepkiej", "lepkie" },
                new[] { "śliska", "śliskiej", "śliskie" },
                new[] { "ziarnista", "ziarnistej", "ziarniste" },
                new[] { "miękka", "miękkiej", "miękkie" },
                new[] { "twarda", "twardej", "twarde" },
                new[] { "aksamitna", "aksamitnej", "aksamitne" },
                new[] { "wieczna", "wiecznej", "wieczne" },
                new[] { "chwilowa", "chwilowej", "chwilowe" },
                new[] { "pradawna", "pradawnej", "pradawne" },
                new[] { "przyszła", "przyszłej", "przyszłe" },
                new[] { "miniona", "minionej", "minione" },
                new[] { "nocna", "nocnej", "nocne" },
                new[] { "dzienna", "dziennej", "dzienne" },
                new[] { "kosztowna", "kosztownej", "kosztowne" },
                new[] { "bezcenna", "bezcennej", "bezcenne" },
                new[] { "luksusowa", "luksusowej", "luksusowe" },
                new[] { "budżetowa", "budżetowej", "budżetowe" },
                new[] { "przeceniona", "przecenionej", "przecenione" },
                new[] { "opłacalna", "opłacalnej", "opłacalne" },
                new[] { "rozrzutna", "rozrzutnej", "rozrzutne" }
            };

            foreach (string[] expected in forms)
            {
                string root = expected[0];
                Adjective adjective = AdjectiveCollection.Collection.Adjectives.Find(item => item.Root == root);
                AssertTrue(adjective != null, "expanded adjective should load: " + root);
                if (adjective == null)
                    continue;

                string feminine = AdjectiveDecliner.Decliner.MakeWord(adjective, GrammaticalGender.Feminine,
                    InflectionCase.Nominative, DecliantionNumber.Singular, AdjectiveLevel.Equal);
                string genitive = AdjectiveDecliner.Decliner.MakeWord(adjective, GrammaticalGender.Feminine,
                    InflectionCase.Genitive, DecliantionNumber.Singular, AdjectiveLevel.Equal);
                string plural = AdjectiveDecliner.Decliner.MakeWord(adjective, GrammaticalGender.MasculineThing,
                    InflectionCase.Nominative, DecliantionNumber.Plural, AdjectiveLevel.Equal);

                AssertEqual(root, feminine, "expanded adjective should retain its dictionary form");
                AssertEqual(expected[1], genitive, "expanded adjective should have the expected feminine genitive: " + root);
                AssertEqual(expected[2], plural, "expanded adjective should have the expected nominative plural: " + root);
            }
        }

        private static void LegacyCategoryReferencesAreDefined()
        {
            LoadLegacyData();
            List<string> undefined = new List<string>();

            foreach (Noun noun in NounCollection.Collection.Nouns)
            {
                foreach (int categoryId in noun.Categories)
                {
                    string diagnostic = "noun " + noun.Root + " -> " + categoryId;
                    if (!WordCategories.Categories.NounCategories.ContainsKey(categoryId) && !undefined.Contains(diagnostic))
                        undefined.Add(diagnostic);
                }
            }

            foreach (Adjective adjective in AdjectiveCollection.Collection.Adjectives)
            {
                foreach (int categoryId in adjective.Categories)
                {
                    string diagnostic = "adjective " + adjective.Root + " -> " + categoryId;
                    if (!WordCategories.Categories.AdjectiveCategories.ContainsKey(categoryId) && !undefined.Contains(diagnostic))
                        undefined.Add(diagnostic);
                }
            }

            AssertTrue(undefined.Count == 0, "dictionary entries reference undefined categories:" +
                Environment.NewLine + string.Join(Environment.NewLine, undefined));
        }

        private static void LegacyDictionariesAreFullyCategorized()
        {
            LoadLegacyData();
            int categorizedNouns = NounCollection.Collection.Nouns.FindAll(noun => noun.Categories.Count > 0).Count;
            int categorizedAdjectives = AdjectiveCollection.Collection.Adjectives.FindAll(
                adjective => adjective.Categories.Count > 0).Count;

            AssertTrue(categorizedNouns == NounCollection.Collection.Nouns.Count,
                "every noun should remain categorized");
            AssertTrue(categorizedAdjectives == AdjectiveCollection.Collection.Adjectives.Count,
                "every adjective should remain categorized");
        }

        private static void LegacyDictionariesGenerateNounPhrase()
        {
            LoadLegacyData();

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{1:A???E} {1:NN1F}");

            AssertFalse(string.IsNullOrWhiteSpace(result), "real dictionaries should generate a phrase");
            AssertFalse(result.Contains("{"), "real dictionaries should not leave a diagnostic marker: " + result);
        }

        private static void LegacyCategoryFormattersDecode()
        {
            LoadLegacyData();

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{NN1T$11} {AN1FE$1}");

            AssertFalse(string.IsNullOrWhiteSpace(result), "categorized formatters should generate real words");
            AssertFalse(result.Contains("{"), "categorized formatters should not leave a diagnostic marker: " + result);
        }

        private static void ExtendedAdjectiveCategoriesDecode()
        {
            LoadLegacyData();
            TestDecoder decoder = new TestDecoder();

            for (int categoryId = 7; categoryId <= 22; categoryId++)
            {
                string result = decoder.Decode("{AN1FE$" + categoryId + "}");
                AssertFalse(string.IsNullOrWhiteSpace(result), "category " + categoryId + " should generate an adjective");
                AssertFalse(result.Contains("{"), "category " + categoryId + " should not leave a diagnostic marker: " + result);
            }
        }

        private static void ExtendedNounCategoriesDecode()
        {
            LoadLegacyData();
            TestDecoder decoder = new TestDecoder();

            for (int categoryId = 21; categoryId <= 23; categoryId++)
            {
                string result = decoder.Decode("{NN1T$" + categoryId + "}");
                AssertFalse(string.IsNullOrWhiteSpace(result), "category " + categoryId + " should generate a noun");
                AssertFalse(result.Contains("{"), "category " + categoryId + " should not leave a diagnostic marker: " + result);
            }
        }

        private static void GroupedLegacyTemplatesDecode()
        {
            string dataPath = LoadLegacyData();
            TestDecoder decoder = new TestDecoder();
            List<string> failures = new List<string>();
            int templates = 0;

            foreach (string fileName in new[] { "_Title.txt", "Verses.txt" })
            {
                int lineNumber = 0;
                foreach (string line in File.ReadLines(Path.Combine(dataPath, fileName), Encoding.Unicode))
                {
                    lineNumber++;
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith(";") || line.TrimStart().StartsWith("EOF"))
                        continue;
                    if (line.IndexOf("{1:", StringComparison.Ordinal) < 0 &&
                        line.IndexOf("{2:", StringComparison.Ordinal) < 0)
                        continue;

                    templates++;
                    string result = decoder.Decode(line);
                    if (string.IsNullOrEmpty(result) || result.Contains("{"))
                        failures.Add(fileName + ":" + lineNumber + " => " + (result ?? "<null>"));
                }
            }

            AssertTrue(templates > 50, "the smoke test should cover the legacy grouped template corpus");
            AssertTrue(failures.Count == 0, "grouped templates left diagnostics:" + Environment.NewLine + string.Join(Environment.NewLine, failures));
        }

        private static string LoadLegacyData()
        {
            string dataPath = Path.Combine(FindRepositoryRoot(), "OldCode", "Flaksator", "DataFiles");

            NounCollection.Collection.Nouns.Clear();
            NounCollection.Collection.SortedNouns.Clear();
            AdjectiveCollection.Collection.Adjectives.Clear();

            NounCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Nouns.txt"));
            AdjectiveCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Adjectives.txt"));
            NounDecliner.Decliner.LoadPostfixes(Path.Combine(dataPath, "NounPostfixes.txt"));
            WordCategories.Categories.LoadFromFile(Path.Combine(dataPath, "Categories.ini"));

            return dataPath;
        }

        private static string FindRepositoryRoot()
        {
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "Flaksator.sln")))
                    return directory.FullName;

                directory = directory.Parent;
            }

            throw new DirectoryNotFoundException("Could not locate the Flaksator repository root.");
        }

        private static void SetSingleFeminineNounAndAdjective()
        {
            SetSingleFeminineNounAndAdjective("kotka", true, true);
        }

        private static void SetSingleFeminineNounAndAdjective(string root, bool canBeSingular, bool canBePlural)
        {
            Noun noun = new Noun
            {
                Root = root,
                Genre = GrammaticalGender.Feminine,
                IsConstant = true,
                CanBeSingular = canBeSingular,
                CanBePlural = canBePlural
            };

            NounCollection.Collection.Nouns.Clear();
            NounCollection.Collection.SortedNouns.Clear();
            NounCollection.Collection.Nouns.Add(noun);
            NounCollection.Collection.SortedNouns.Add(
                GrammaticalGender.Feminine,
                new List<Noun> { noun });

            AdjectiveCollection.Collection.Adjectives.Clear();
            AdjectiveCollection.Collection.Adjectives.Add(new Adjective { Root = "czarny" });
        }

        private static void SetCategorizedWords()
        {
            Noun animal = new Noun
            {
                Root = "kotka",
                Genre = GrammaticalGender.Feminine,
                IsConstant = true
            };
            animal.Categories.Add(3);

            Noun weapon = new Noun
            {
                Root = "siekiera",
                Genre = GrammaticalGender.Feminine,
                IsConstant = true
            };
            weapon.Categories.Add(11);

            NounCollection.Collection.Nouns.Clear();
            NounCollection.Collection.SortedNouns.Clear();
            NounCollection.Collection.Nouns.Add(animal);
            NounCollection.Collection.Nouns.Add(weapon);
            NounCollection.Collection.SortedNouns.Add(
                GrammaticalGender.Feminine,
                new List<Noun> { animal, weapon });

            Adjective color = new Adjective { Root = "czarny" };
            color.Categories.Add(1);
            Adjective positive = new Adjective { Root = "dobry" };
            positive.Categories.Add(2);

            AdjectiveCollection.Collection.Adjectives.Clear();
            AdjectiveCollection.Collection.Adjectives.Add(color);
            AdjectiveCollection.Collection.Adjectives.Add(positive);
        }

        private static void Run(string name, Action specification)
        {
            try
            {
                specification();
                Console.WriteLine("PASS: " + name);
            }
            catch (Exception exception)
            {
                failures++;
                Console.WriteLine("FAIL: " + name);
                Console.WriteLine("      " + exception.Message);
            }
        }

        private static void AssertTrue(bool actual, string message)
        {
            if (!actual)
                throw new InvalidOperationException(message);
        }

        private static void AssertFalse(bool actual, string message)
        {
            if (actual)
                throw new InvalidOperationException(message);
        }

        private static void AssertEqual(string expected, string actual, string message)
        {
            if (!string.Equals(expected, actual, StringComparison.Ordinal))
                throw new InvalidOperationException(
                    message + ". Expected '" + expected + "', got '" + actual + "'.");
        }

        private sealed class TestDecoder : DecodeBase
        {
            public string Decode(string template)
            {
                return this.DecodeLine(template);
            }
        }
    }
}
