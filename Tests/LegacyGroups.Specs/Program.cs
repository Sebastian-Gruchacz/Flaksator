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
            Run("incomplete verb formatter returns diagnostics", IncompleteVerbFormatterReturnsDiagnostics);
            Run("standalone present verb decodes", StandalonePresentVerbDecodes);
            Run("named group verb inherits subject number", NamedGroupVerbInheritsSubjectNumber);
            Run("past verb forms distinguish agreement classes", PastVerbFormsDistinguishAgreementClasses);
            Run("named group past verb inherits subject agreement", NamedGroupPastVerbInheritsSubjectAgreement);
            Run("reflexive particle may be distant from its verb", ReflexiveParticleMayBeDistantFromItsVerb);
            Run("reflexive group rejects incompatible definitions", ReflexiveGroupRejectsIncompatibleDefinitions);
            Run("standalone verb excludes reflexive entries", StandaloneVerbExcludesReflexiveEntries);
            Run("third-person pronoun paradigm declines", ThirdPersonPronounParadigmDeclines);
            Run("named pronoun inherits anchor agreement", NamedPronounInheritsAnchorAgreement);
            Run("prepositional pronoun variants decline", PrepositionalPronounVariantsDecline);
            Run("pronoun definitions return diagnostics", PronounDefinitionsReturnDiagnostics);
            Run("legacy dictionaries load", LegacyDictionariesLoad);
            Run("legacy present verb paradigms conjugate", LegacyPresentVerbParadigmsConjugate);
            Run("legacy future verb paradigms conjugate", LegacyFutureVerbParadigmsConjugate);
            Run("legacy compound future paradigms conjugate", LegacyCompoundFutureParadigmsConjugate);
            Run("expanded adjectives inflect directly", ExpandedAdjectivesInflectDirectly);
            Run("legacy category references are defined", LegacyCategoryReferencesAreDefined);
            Run("legacy dictionaries are fully categorized", LegacyDictionariesAreFullyCategorized);
            Run("legacy dictionary roots are unique", LegacyDictionaryRootsAreUnique);
            Run("dictionary display uses nominative singular", DictionaryDisplayUsesNominativeSingular);
            Run("legacy dictionaries generate a noun phrase", LegacyDictionariesGenerateNounPhrase);
            Run("legacy category formatters decode", LegacyCategoryFormattersDecode);
            Run("extended adjective categories decode", ExtendedAdjectiveCategoriesDecode);
            Run("extended noun categories decode", ExtendedNounCategoriesDecode);
            Run("grouped legacy templates decode", GroupedLegacyTemplatesDecode);
            Run("verb editor validation rejects duplicate forms", VerbEditorValidationRejectsDuplicateForms);
            Run("verb editor validation enforces tense gender", VerbEditorValidationEnforcesTenseGender);
            Run("verb editor validation enforces aspect tense", VerbEditorValidationEnforcesAspectTense);
            Run("verb paradigms with finite forms are complete", VerbFiniteParadigmsAreComplete);
            Run("compound future prefiller preserves manual forms", CompoundFuturePrefillerPreservesManualForms);
            Run("imperfective present paradigms include future", ImperfectivePresentParadigmsIncludeFuture);
            Run("explicit verb forms have classified aspect", ExplicitVerbFormsHaveClassifiedAspect);
            Run("verb editing status identifies actionable gaps", VerbEditingStatusIdentifiesActionableGaps);
            Run("modernized verse samples decode", ModernizedVerseSamplesDecode);
            Run("verse editor save preserves source structure", VerseEditorSavePreservesSourceStructure);
            Run("verse inspector maps named groups", VerseInspectorMapsNamedGroups);
            Run("verse inspector reports positioned diagnostics", VerseInspectorReportsPositionedDiagnostics);
            Run("verse formatter builder composes grammar codes", VerseFormatterBuilderComposesGrammarCodes);
            Run("verse formatter builder rejects incomplete standalone codes", VerseFormatterBuilderRejectsIncompleteStandaloneCodes);
            Run("verse formatter builder round-trips existing tokens", VerseFormatterBuilderRoundTripsExistingTokens);
            Run("generation exclusions filter noun and adjective categories", GenerationExclusionsFilterCategories);
            Run("generation exclusions override verse category requests", GenerationExclusionsOverrideVerseRequests);
            Run("verb metadata round-trips", VerbMetadataRoundTrips);
            Run("legacy verbs remain backward compatible", LegacyVerbsRemainBackwardCompatible);
            Run("verb serializer rejects inconsistent aspect", VerbSerializerRejectsInconsistentAspect);
            Run("verb categories are complete and filter generation", VerbCategoriesAreCompleteAndFilterGeneration);
            Run("verb formatter respects category and voice parameters", VerbFormatterRespectsCategoryAndVoiceParameters);

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

        private static void IncompleteVerbFormatterReturnsDiagnostics()
        {
            SetSingleFeminineNounAndAdjective();

            GroupInfo group = new GroupInfo(1);
            group.AddCode(0, "V");
            group.AddCode(1, "NN1F");

            AssertFalse(group.Analyze(), "an incomplete verb formatter should invalidate the group");
            AssertEqual("{INVALID_GROUP_FORMATTER}", group.Results[0], "an incomplete verb should be explicit");
            AssertEqual("kotka", group.Results[1], "supported members should still be generated");
        }

        private static void StandalonePresentVerbDecodes()
        {
            SetSinglePresentVerb("zabija", DecliantionNumber.Singular);

            TestDecoder decoder = new TestDecoder();
            AssertEqual("zabija", decoder.Decode("{VT31}"),
                "a standalone verb formatter should generate the requested present form");
        }

        private static void NamedGroupVerbInheritsSubjectNumber()
        {
            SetSingleFeminineNounAndAdjective("kotki", false, true);
            SetSinglePresentVerb("atakują", DecliantionNumber.Plural);

            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{subject:NN2F} z oddali {subject:VT3?}");

            AssertEqual("kotki z oddali atakują", result,
                "a named group verb should inherit plural agreement from its distant subject");
        }

        private static void PastVerbFormsDistinguishAgreementClasses()
        {
            LoadLegacyData();
            Verb verb = VerbCollection.Collection.Verbs.Find(item => item.Infinitive == "opętać");

            AssertTrue(verb != null, "the first past-tense paradigm should load");
            AssertPastForm(verb, DecliantionNumber.Singular, GrammaticalGender.MasculineThing, "opętał");
            AssertPastForm(verb, DecliantionNumber.Singular, GrammaticalGender.MasculineLife, "opętał");
            AssertPastForm(verb, DecliantionNumber.Singular, GrammaticalGender.MasculinePerson, "opętał");
            AssertPastForm(verb, DecliantionNumber.Singular, GrammaticalGender.Feminine, "opętała");
            AssertPastForm(verb, DecliantionNumber.Singular, GrammaticalGender.Neuter, "opętało");
            AssertPastForm(verb, DecliantionNumber.Plural, GrammaticalGender.MasculinePerson, "opętali");
            AssertPastForm(verb, DecliantionNumber.Plural, GrammaticalGender.MasculineThing, "opętały");
            AssertPastForm(verb, DecliantionNumber.Plural, GrammaticalGender.MasculineLife, "opętały");
            AssertPastForm(verb, DecliantionNumber.Plural, GrammaticalGender.Feminine, "opętały");
            AssertPastForm(verb, DecliantionNumber.Plural, GrammaticalGender.Neuter, "opętały");

            TestDecoder decoder = new TestDecoder();
            AssertEqual("opętała", decoder.Decode("{VP31F}"),
                "a standalone past formatter should accept explicit feminine agreement");
            AssertEqual("opętali", decoder.Decode("{VP32P}"),
                "a standalone past formatter should accept explicit masculine-personal agreement");
            AssertEqual("{VERB_DEFINITION_INCOMPLETE}", decoder.Decode("{VP31}"),
                "a standalone past formatter should require an agreement gender");
        }

        private static void NamedGroupPastVerbInheritsSubjectAgreement()
        {
            LoadLegacyData();

            AssertNamedPastAgreement("topór", GrammaticalGender.MasculineThing,
                DecliantionNumber.Singular, "NN1T", "opętał");
            AssertNamedPastAgreement("bestia", GrammaticalGender.Feminine,
                DecliantionNumber.Singular, "NN1F", "opętała");
            AssertNamedPastAgreement("ciało", GrammaticalGender.Neuter,
                DecliantionNumber.Singular, "NN1N", "opętało");
            AssertNamedPastAgreement("przodkowie", GrammaticalGender.MasculinePerson,
                DecliantionNumber.Plural, "NN2P", "opętali");
            AssertNamedPastAgreement("bestie", GrammaticalGender.Feminine,
                DecliantionNumber.Plural, "NN2F", "opętały");
        }

        private static void AssertPastForm(Verb verb, DecliantionNumber number,
            GrammaticalGender gender, string expected)
        {
            string actual = VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Past,
                GrammaticalPerson.Third, number, gender);
            AssertEqual(expected, actual, "the past form should match its agreement class");
        }

        private static void AssertNamedPastAgreement(string nounRoot, GrammaticalGender gender,
            DecliantionNumber number, string nounFormatter, string expectedVerb)
        {
            SetSingleNoun(nounRoot, gender, number == DecliantionNumber.Singular,
                number == DecliantionNumber.Plural);
            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{subject:" + nounFormatter + "} daleko {subject:VP3?}");
            AssertEqual(nounRoot + " daleko " + expectedVerb, result,
                "a distant past verb should inherit number and gender from its named subject");
        }

        private static void ReflexiveParticleMayBeDistantFromItsVerb()
        {
            SetSingleFeminineNounAndAdjective();
            SetSinglePresentVerb("delektuje", DecliantionNumber.Singular, true);

            TestDecoder decoder = new TestDecoder();
            AssertEqual("kotka delektuje spokojnie się ciszą",
                decoder.Decode("{subject:NN1F} {subject:VT3?} spokojnie {subject:R} ciszą"),
                "a reflexive particle should be placeable independently from its verb");
            AssertEqual("kotka się delektuje ciszą",
                decoder.Decode("{subject:NN1F} {subject:R} {subject:VT3?} ciszą"),
                "the same reflexive verb should support a particle before the verb");
        }

        private static void ReflexiveGroupRejectsIncompatibleDefinitions()
        {
            SetSingleFeminineNounAndAdjective();
            SetSinglePresentVerb("zabija", DecliantionNumber.Singular, false);
            TestDecoder decoder = new TestDecoder();

            AssertEqual("kotka {NO_VALID_VERB} się",
                decoder.Decode("{subject:NN1F} {subject:VT3?} {subject:R}"),
                "a reflexive slot must not bind a non-reflexive verb");
            AssertEqual("kotka {GROUP_REFLEXIVE_REQUIRES_VERB}",
                decoder.Decode("{subject:NN1F} {subject:R}"),
                "a reflexive slot without a verb should be diagnosed");
            AssertEqual("kotka {GROUP_REFLEXIVE_AMBIGUOUS} {GROUP_REFLEXIVE_AMBIGUOUS} {GROUP_REFLEXIVE_REQUIRES_SINGLE_VERB}",
                decoder.Decode("{subject:NN1F} {subject:VT3?} {subject:VT3?} {subject:R}"),
                "one reflexive slot must not ambiguously bind multiple verbs");
        }

        private static void StandaloneVerbExcludesReflexiveEntries()
        {
            SetSinglePresentVerb("delektuje", DecliantionNumber.Singular, true);

            TestDecoder decoder = new TestDecoder();
            AssertEqual("{NO_VALID_VERB}", decoder.Decode("{VT31}"),
                "a standalone formatter must not omit a required reflexive particle");
        }

        private static void ThirdPersonPronounParadigmDeclines()
        {
            LoadLegacyData();
            Pronoun pronoun = PronounCollection.Collection.Pronouns.Find(item => item.Root == "third-person");
            AssertTrue(pronoun != null, "the third-person pronoun paradigm should load");
            AssertTrue(pronoun != null && pronoun.Forms.Count == 55,
                "the third-person paradigm should contain 30 neutral and 25 prepositional forms");
            if (pronoun == null)
                return;

            string serialized = new SharpDevs.Fleksator.IO.GrammarSerializersFactory()
                .GetOldSerializers().PronounSerializer.Write(pronoun);
            Pronoun roundTripped = new SharpDevs.Fleksator.IO.GrammarSerializersFactory()
                .GetOldSerializers().PronounSerializer.Load(serialized);
            AssertTrue(roundTripped != null && roundTripped.Forms.Count == 55,
                "the pronoun paradigm should round-trip for a future editor");

            string[][] expectedForms =
            {
                new[] { "N31T", "on" }, new[] { "G31T", "go" }, new[] { "D31T", "mu" },
                new[] { "C31T", "go" }, new[] { "A31T", "nim" }, new[] { "L31T", "nim" },
                new[] { "N31F", "ona" }, new[] { "G31F", "jej" }, new[] { "D31F", "jej" },
                new[] { "C31F", "ją" }, new[] { "A31F", "nią" }, new[] { "L31F", "niej" },
                new[] { "N31N", "ono" }, new[] { "G31N", "go" }, new[] { "D31N", "mu" },
                new[] { "C31N", "je" }, new[] { "A31N", "nim" }, new[] { "L31N", "nim" },
                new[] { "N32P", "oni" }, new[] { "G32P", "ich" }, new[] { "D32P", "im" },
                new[] { "C32P", "ich" }, new[] { "A32P", "nimi" }, new[] { "L32P", "nich" },
                new[] { "N32F", "one" }, new[] { "G32F", "ich" }, new[] { "D32F", "im" },
                new[] { "C32F", "je" }, new[] { "A32F", "nimi" }, new[] { "L32F", "nich" }
            };

            foreach (string[] expected in expectedForms)
            {
                string code = expected[0];
                string actual = PronounDecliner.Decliner.MakeWord(pronoun,
                    EnumHelper.GetWordCase(code[0]), VerbGrammar.GetPerson(code[1]),
                    EnumHelper.GetWordAmount(code[2]), EnumHelper.GetWordGenre(code[3]));
                AssertEqual(expected[1], actual, "the stored pronoun form should round-trip: " + code);
            }

            AssertEqual("go", PronounDecliner.Decliner.MakeWord(pronoun, InflectionCase.Accusative,
                GrammaticalPerson.Third, DecliantionNumber.Singular, GrammaticalGender.MasculineLife),
                "singular masculine-life agreement should use the masculine pronoun class");
            AssertEqual("je", PronounDecliner.Decliner.MakeWord(pronoun, InflectionCase.Accusative,
                GrammaticalPerson.Third, DecliantionNumber.Plural, GrammaticalGender.Neuter),
                "non-masculine-personal plural agreement should use the common pronoun class");

            TestDecoder decoder = new TestDecoder();
            AssertEqual("ją", decoder.Decode("{PC31F}"),
                "a standalone pronoun formatter should accept complete agreement features");
        }

        private static void NamedPronounInheritsAnchorAgreement()
        {
            LoadLegacyData();
            AssertNamedPronoun("topór", GrammaticalGender.MasculineThing,
                DecliantionNumber.Singular, "NN1T", "PC???", "go");
            AssertNamedPronoun("bestia", GrammaticalGender.Feminine,
                DecliantionNumber.Singular, "NN1F", "PC???", "ją");
            AssertNamedPronoun("ciało", GrammaticalGender.Neuter,
                DecliantionNumber.Singular, "NN1N", "PC???", "je");
            AssertNamedPronoun("przodkowie", GrammaticalGender.MasculinePerson,
                DecliantionNumber.Plural, "NN2P", "PN???", "oni");
            AssertNamedPronoun("bestie", GrammaticalGender.Feminine,
                DecliantionNumber.Plural, "NN2F", "PA???", "nimi");
        }

        private static void PrepositionalPronounVariantsDecline()
        {
            LoadLegacyData();
            Pronoun pronoun = PronounCollection.Collection.Pronouns.Find(item => item.Root == "third-person");
            AssertTrue(pronoun != null, "the prepositional pronoun paradigm should load");
            if (pronoun == null)
                return;

            string[][] expectedForms =
            {
                new[] { "G31T", "niego" }, new[] { "D31T", "niemu" }, new[] { "C31T", "niego" },
                new[] { "A31T", "nim" }, new[] { "L31T", "nim" },
                new[] { "G31F", "niej" }, new[] { "D31F", "niej" }, new[] { "C31F", "nią" },
                new[] { "A31F", "nią" }, new[] { "L31F", "niej" },
                new[] { "G31N", "niego" }, new[] { "D31N", "niemu" }, new[] { "C31N", "nie" },
                new[] { "A31N", "nim" }, new[] { "L31N", "nim" },
                new[] { "G32P", "nich" }, new[] { "D32P", "nim" }, new[] { "C32P", "nich" },
                new[] { "A32P", "nimi" }, new[] { "L32P", "nich" },
                new[] { "G32F", "nich" }, new[] { "D32F", "nim" }, new[] { "C32F", "nie" },
                new[] { "A32F", "nimi" }, new[] { "L32F", "nich" }
            };

            foreach (string[] expected in expectedForms)
            {
                string code = expected[0];
                string actual = PronounDecliner.Decliner.MakeWord(pronoun,
                    EnumHelper.GetWordCase(code[0]), VerbGrammar.GetPerson(code[1]),
                    EnumHelper.GetWordAmount(code[2]), EnumHelper.GetWordGenre(code[3]),
                    PronounVariant.Prepositional);
                AssertEqual(expected[1], actual,
                    "the stored prepositional pronoun should round-trip: " + code);
            }

            TestDecoder decoder = new TestDecoder();
            AssertEqual("niego", decoder.Decode("{PG31TP}"),
                "a complete standalone formatter should select the prepositional variant");

            SetSingleNoun("bestia", GrammaticalGender.Feminine, true, false);
            AssertEqual("bestia bez niej", decoder.Decode("{victim:NN1F} bez {victim:PG???P}"),
                "a grouped prepositional pronoun should inherit feminine agreement");
            AssertEqual("bestia widzę ją", decoder.Decode("{victim:NN1F} widzę {victim:PC???}"),
                "the same anchor should still select the neutral accusative without the suffix");

            AssertEqual("{NO_VALID_PRONOUN}", decoder.Decode("{PN31FP}"),
                "a prepositional nominative should be rejected because that form does not exist");
            AssertEqual("{INVALID_FORMATTER}", decoder.Decode("{PG31FX}"),
                "unknown pronoun variant suffixes should be rejected");
        }

        private static void AssertNamedPronoun(string nounRoot, GrammaticalGender gender,
            DecliantionNumber number, string nounFormatter, string pronounFormatter, string expectedPronoun)
        {
            SetSingleNoun(nounRoot, gender, number == DecliantionNumber.Singular,
                number == DecliantionNumber.Plural);
            TestDecoder decoder = new TestDecoder();
            string result = decoder.Decode("{victim:" + nounFormatter + "} widzę {victim:" +
                pronounFormatter + "}");
            AssertEqual(nounRoot + " widzę " + expectedPronoun, result,
                "the distant pronoun should inherit anchor agreement while retaining its own case");
        }

        private static void PronounDefinitionsReturnDiagnostics()
        {
            LoadLegacyData();
            SetSingleNoun("bestia", GrammaticalGender.Feminine, true, false);
            TestDecoder decoder = new TestDecoder();

            AssertEqual("bestia {PRONOUN_DEFINITION_INCOMPLETE}",
                decoder.Decode("{victim:NN1F} {victim:P????}"),
                "a grouped pronoun should require an explicit case");
            AssertEqual("{PRONOUN_DEFINITION_INCOMPLETE}", decoder.Decode("{PC???}"),
                "a standalone pronoun should require complete agreement features");

            PronounCollection.Collection.Pronouns.Clear();
            AssertEqual("bestia {NO_VALID_PRONOUN}",
                decoder.Decode("{victim:NN1F} {victim:PC???}"),
                "an empty pronoun dictionary should return a diagnostic marker");
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
            AssertFalse(WordFormatterParser.TryParse("NN1F$!3", out grammarCode, out categoryIds),
                "generation exclusions must not leak into persistent verse syntax");
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
            AssertTrue(AdjectiveCollection.Collection.Adjectives.Count >= 312,
                "expanded adjective dictionary should contain the historical entries and the new category set");
            AssertTrue(VerbCollection.Collection.Verbs.Count >= 100,
                "the expanded legacy verb dictionary should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(verb => verb.Forms.Count > 0).Count >= 58,
                "the explicit present and past paradigms should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(
                verb => verb.Aspect == VerbAspect.Perfective).Count >= 10,
                "the first aspect-qualified future paradigms should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(
                verb => verb.Aspect == VerbAspect.Imperfective).Count >= 54,
                "the classified imperfective paradigms should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(
                verb => verb.Aspect == VerbAspect.Biaspectual).Count >= 1,
                "the first explicitly biaspectual paradigm should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(verb => verb.IsReflexive).Count >= 2,
                "the explicit reflexive verbs should load");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(
                verb => VerbEditingStatus.Inspect(verb).NeedsWork).Count == 42,
                "the editor work queue should expose the remaining uninflected proposals");
            AssertTrue(PronounCollection.Collection.Pronouns.Count >= 1,
                "the first pronoun paradigm should load");
            AssertTrue(WordCategories.Categories.NounCategories.Count >= 23, "complete noun categories should load");
            AssertTrue(WordCategories.Categories.AdjectiveCategories.Count >= 22, "complete adjective categories should load");
        }

        private static void LegacyPresentVerbParadigmsConjugate()
        {
            LoadLegacyData();
            string[][] paradigms =
            {
                new[] { "zabijać", "zabijam", "zabijasz", "zabija", "zabijamy", "zabijacie", "zabijają" },
                new[] { "krwawić", "krwawię", "krwawisz", "krwawi", "krwawimy", "krwawicie", "krwawią" },
                new[] { "atakować", "atakuję", "atakujesz", "atakuje", "atakujemy", "atakujecie", "atakują" },
                new[] { "delektować", "delektuję", "delektujesz", "delektuje", "delektujemy", "delektujecie", "delektują" },
                new[] { "krzyczeć", "krzyczę", "krzyczysz", "krzyczy", "krzyczymy", "krzyczycie", "krzyczą" },
                new[] { "wbijać", "wbijam", "wbijasz", "wbija", "wbijamy", "wbijacie", "wbijają" },
                new[] { "rzucać", "rzucam", "rzucasz", "rzuca", "rzucamy", "rzucacie", "rzucają" },
                new[] { "wyrywać", "wyrywam", "wyrywasz", "wyrywa", "wyrywamy", "wyrywacie", "wyrywają" },
                new[] { "kopać", "kopię", "kopiesz", "kopie", "kopiemy", "kopiecie", "kopią" },
                new[] { "uderzać", "uderzam", "uderzasz", "uderza", "uderzamy", "uderzacie", "uderzają" },
                new[] { "trafiać", "trafiam", "trafiasz", "trafia", "trafiamy", "trafiacie", "trafiają" },
                new[] { "czuć", "czuję", "czujesz", "czuje", "czujemy", "czujecie", "czują" },
                new[] { "widzieć", "widzę", "widzisz", "widzi", "widzimy", "widzicie", "widzą" },
                new[] { "biec", "biegnę", "biegniesz", "biegnie", "biegniemy", "biegniecie", "biegną" },
                new[] { "padać", "padam", "padasz", "pada", "padamy", "padacie", "padają" },
                new[] { "pożerać", "pożeram", "pożerasz", "pożera", "pożeramy", "pożeracie", "pożerają" },
                new[] { "podążać", "podążam", "podążasz", "podąża", "podążamy", "podążacie", "podążają" },
                new[] { "być", "jestem", "jesteś", "jest", "jesteśmy", "jesteście", "są" },
                new[] { "mieć", "mam", "masz", "ma", "mamy", "macie", "mają" },
                new[] { "leżeć", "leżę", "leżysz", "leży", "leżymy", "leżycie", "leżą" },
                new[] { "siedzieć", "siedzę", "siedzisz", "siedzi", "siedzimy", "siedzicie", "siedzą" },
                new[] { "umierać", "umieram", "umierasz", "umiera", "umieramy", "umieracie", "umierają" },
                new[] { "lecieć", "lecę", "lecisz", "leci", "lecimy", "lecicie", "lecą" },
                new[] { "wylatywać", "wylatuję", "wylatujesz", "wylatuje", "wylatujemy", "wylatujecie", "wylatują" },
                new[] { "odpadać", "odpadam", "odpadasz", "odpada", "odpadamy", "odpadacie", "odpadają" },
                new[] { "przewracać", "przewracam", "przewracasz", "przewraca", "przewracamy", "przewracacie", "przewracają" },
                new[] { "poniewierać", "poniewieram", "poniewierasz", "poniewiera", "poniewieramy", "poniewieracie", "poniewierają" },
                new[] { "wiedzieć", "wiem", "wiesz", "wie", "wiemy", "wiecie", "wiedzą" },
                new[] { "myśleć", "myślę", "myślisz", "myśli", "myślimy", "myślicie", "myślą" },
                new[] { "chcieć", "chcę", "chcesz", "chce", "chcemy", "chcecie", "chcą" },
                new[] { "musieć", "muszę", "musisz", "musi", "musimy", "musicie", "muszą" },
                new[] { "próbować", "próbuję", "próbujesz", "próbuje", "próbujemy", "próbujecie", "próbują" },
                new[] { "słyszeć", "słyszę", "słyszysz", "słyszy", "słyszymy", "słyszycie", "słyszą" },
                new[] { "błagać", "błagam", "błagasz", "błaga", "błagamy", "błagacie", "błagają" },
                new[] { "kazać", "każę", "każesz", "każe", "każemy", "każecie", "każą" },
                new[] { "robić", "robię", "robisz", "robi", "robimy", "robicie", "robią" },
                new[] { "mierzyć", "mierzę", "mierzysz", "mierzy", "mierzymy", "mierzycie", "mierzą" },
                new[] { "wyjmować", "wyjmuję", "wyjmujesz", "wyjmuje", "wyjmujemy", "wyjmujecie", "wyjmują" },
                new[] { "parzyć", "parzę", "parzysz", "parzy", "parzymy", "parzycie", "parzą" },
                new[] { "przecinać", "przecinam", "przecinasz", "przecina", "przecinamy", "przecinacie", "przecinają" },
                new[] { "zanurzać", "zanurzam", "zanurzasz", "zanurza", "zanurzamy", "zanurzacie", "zanurzają" },
                new[] { "dusić", "duszę", "dusisz", "dusi", "dusimy", "dusicie", "duszą" },
                new[] { "mówić", "mówię", "mówisz", "mówi", "mówimy", "mówicie", "mówią" },
                new[] { "obcinać", "obcinam", "obcinasz", "obcina", "obcinamy", "obcinacie", "obcinają" },
                new[] { "wydłubywać", "wydłubuję", "wydłubujesz", "wydłubuje", "wydłubujemy", "wydłubujecie", "wydłubują" },
                new[] { "chwytać", "chwytam", "chwytasz", "chwyta", "chwytamy", "chwytacie", "chwytają" },
                new[] { "gryźć", "gryzę", "gryziesz", "gryzie", "gryziemy", "gryziecie", "gryzą" }
            };
            GrammaticalPerson[] persons =
            {
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third,
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third
            };
            DecliantionNumber[] numbers =
            {
                DecliantionNumber.Singular, DecliantionNumber.Singular, DecliantionNumber.Singular,
                DecliantionNumber.Plural, DecliantionNumber.Plural, DecliantionNumber.Plural
            };

            foreach (string[] expected in paradigms)
            {
                Verb verb = VerbCollection.Collection.Verbs.Find(item => item.Infinitive == expected[0]);
                AssertTrue(verb != null, "the explicit verb paradigm should load: " + expected[0]);
                if (verb == null)
                    continue;

                bool expectedReflexive = expected[0] == "delektować" || expected[0] == "poniewierać";
                AssertTrue(verb.IsReflexive == expectedReflexive,
                    "only explicitly marked paradigms should load as reflexive: " + expected[0]);

                for (int i = 0; i < persons.Length; i++)
                {
                    string actual = VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Present,
                        persons[i], numbers[i], GrammaticalGender._Unknown);
                    AssertEqual(expected[i + 1], actual,
                        "the explicit present form should round-trip: " + expected[0]);
                }
            }
        }

        private static void LegacyFutureVerbParadigmsConjugate()
        {
            LoadLegacyData();
            string[][] paradigms =
            {
                new[] { "zabić", "zabiję", "zabijesz", "zabije", "zabijemy", "zabijecie", "zabiją" },
                new[] { "obciąć", "obetnę", "obetniesz", "obetnie", "obetniemy", "obetniecie", "obetną" },
                new[] { "wydłubać", "wydłubię", "wydłubiesz", "wydłubie", "wydłubiemy", "wydłubiecie", "wydłubią" },
                new[] { "pochłonąć", "pochłonę", "pochłoniesz", "pochłonie", "pochłoniemy", "pochłoniecie", "pochłoną" },
                new[] { "nadejść", "nadejdę", "nadejdziesz", "nadejdzie", "nadejdziemy", "nadejdziecie", "nadejdą" },
                new[] { "dopaść", "dopadnę", "dopadniesz", "dopadnie", "dopadniemy", "dopadniecie", "dopadną" },
                new[] { "znaleźć", "znajdę", "znajdziesz", "znajdzie", "znajdziemy", "znajdziecie", "znajdą" },
                new[] { "wbić", "wbiję", "wbijesz", "wbije", "wbijemy", "wbijecie", "wbiją" },
                new[] { "rozpocząć", "rozpocznę", "rozpoczniesz", "rozpocznie", "rozpoczniemy", "rozpoczniecie", "rozpoczną" },
                new[] { "pociąć", "potnę", "potniesz", "potnie", "potniemy", "potniecie", "potną" }
            };
            GrammaticalPerson[] persons =
            {
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third,
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third
            };
            DecliantionNumber[] numbers =
            {
                DecliantionNumber.Singular, DecliantionNumber.Singular, DecliantionNumber.Singular,
                DecliantionNumber.Plural, DecliantionNumber.Plural, DecliantionNumber.Plural
            };

            foreach (string[] expected in paradigms)
            {
                Verb verb = VerbCollection.Collection.Verbs.Find(item => item.Infinitive == expected[0]);
                AssertTrue(verb != null, "the explicit future paradigm should load: " + expected[0]);
                if (verb == null)
                    continue;
                AssertTrue(verb.Aspect == VerbAspect.Perfective,
                    "a simple future paradigm should retain perfective aspect: " + expected[0]);
                for (int i = 0; i < persons.Length; i++)
                {
                    string actual = VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Future,
                        persons[i], numbers[i], GrammaticalGender._Unknown);
                    AssertEqual(expected[i + 1], actual,
                        "the explicit future form should round-trip: " + expected[0]);
                }
            }

            Verb selected = VerbCollection.Collection.Verbs.Find(item => item.Infinitive == "zabić");
            VerbCollection.Collection.Verbs.Clear();
            VerbCollection.Collection.Verbs.Add(selected);
            TestDecoder decoder = new TestDecoder();
            AssertEqual("zabijesz", decoder.Decode("{VF21$1}"),
                "a standalone future formatter should decode a categorized form");
            SetSingleNoun("bestie", GrammaticalGender.Feminine, false, true);
            AssertEqual("bestie zabiją", decoder.Decode("{subject:NN2F} {subject:VF3?}"),
                "a grouped future formatter should inherit plural number");
        }

        private static void LegacyCompoundFutureParadigmsConjugate()
        {
            LoadLegacyData();
            string[][] paradigms =
            {
                new[] { "zabijać", "będę zabijać", "będziesz zabijać", "będzie zabijać", "będziemy zabijać", "będziecie zabijać", "będą zabijać" },
                new[] { "być", "będę", "będziesz", "będzie", "będziemy", "będziecie", "będą" },
                new[] { "mieć", "będę mieć", "będziesz mieć", "będzie mieć", "będziemy mieć", "będziecie mieć", "będą mieć" },
                new[] { "widzieć", "będę widzieć", "będziesz widzieć", "będzie widzieć", "będziemy widzieć", "będziecie widzieć", "będą widzieć" },
                new[] { "czuć", "będę czuć", "będziesz czuć", "będzie czuć", "będziemy czuć", "będziecie czuć", "będą czuć" },
                new[] { "biec", "będę biec", "będziesz biec", "będzie biec", "będziemy biec", "będziecie biec", "będą biec" },
                new[] { "myśleć", "będę myśleć", "będziesz myśleć", "będzie myśleć", "będziemy myśleć", "będziecie myśleć", "będą myśleć" },
                new[] { "mówić", "będę mówić", "będziesz mówić", "będzie mówić", "będziemy mówić", "będziecie mówić", "będą mówić" },
                new[] { "robić", "będę robić", "będziesz robić", "będzie robić", "będziemy robić", "będziecie robić", "będą robić" },
                new[] { "chcieć", "będę chcieć", "będziesz chcieć", "będzie chcieć", "będziemy chcieć", "będziecie chcieć", "będą chcieć" }
            };
            GrammaticalPerson[] persons =
            {
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third,
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third
            };
            DecliantionNumber[] numbers =
            {
                DecliantionNumber.Singular, DecliantionNumber.Singular, DecliantionNumber.Singular,
                DecliantionNumber.Plural, DecliantionNumber.Plural, DecliantionNumber.Plural
            };

            foreach (string[] expected in paradigms)
            {
                Verb verb = VerbCollection.Collection.Verbs.Find(item => item.Infinitive == expected[0]);
                AssertTrue(verb != null, "the compound future paradigm should load: " + expected[0]);
                if (verb == null)
                    continue;
                AssertTrue(verb.Aspect == VerbAspect.Imperfective,
                    "a compound future paradigm should retain imperfective aspect: " + expected[0]);
                for (int i = 0; i < persons.Length; i++)
                {
                    string actual = VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Future,
                        persons[i], numbers[i], GrammaticalGender._Unknown);
                    AssertEqual(expected[i + 1], actual,
                        "the compound future form should round-trip: " + expected[0]);
                }
            }
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
                List<Adjective> matches = AdjectiveCollection.Collection.Adjectives.FindAll(item => item.Root == root);
                AssertTrue(matches.Count == 1, "expanded adjective should load exactly once: " + root);
                Adjective adjective = matches.Count == 1 ? matches[0] : null;
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

        private static void LegacyDictionaryRootsAreUnique()
        {
            LoadLegacyData();
            HashSet<string> nounRoots = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> adjectiveRoots = new HashSet<string>(StringComparer.Ordinal);
            List<string> duplicates = new List<string>();

            foreach (Noun noun in NounCollection.Collection.Nouns)
            {
                if (!nounRoots.Add(noun.Root))
                    duplicates.Add("noun: " + noun.Root);
            }

            foreach (Adjective adjective in AdjectiveCollection.Collection.Adjectives)
            {
                if (!adjectiveRoots.Add(adjective.Root))
                    duplicates.Add("adjective: " + adjective.Root);
            }

            AssertTrue(duplicates.Count == 0, "dictionary roots should be unique within each word class:" +
                Environment.NewLine + string.Join(Environment.NewLine, duplicates));
        }

        private static void DictionaryDisplayUsesNominativeSingular()
        {
            LoadLegacyData();
            Noun noun = NounCollection.Collection.Nouns.Find(item => item.Root == "jelit");
            Adjective adjective = AdjectiveCollection.Collection.Adjectives.Find(item => item.Root == "gładka");

            AssertTrue(noun != null, "the stem-based noun fixture should load");
            AssertTrue(adjective != null, "the adjective fixture should load");
            AssertEqual("jelito", DictionaryDisplayText.ForNoun(noun),
                "the noun list should display the generated nominative singular instead of the stored stem");
            AssertEqual("gładka", DictionaryDisplayText.ForAdjective(adjective),
                "the adjective list should display its generated feminine nominative singular");

            Noun singularUnavailable = new Noun { Root = "tylko-mnogie", CanBeSingular = false };
            AssertEqual("tylko-mnogie", DictionaryDisplayText.ForNoun(singularUnavailable),
                "the display should fall back to the stored root when singular is unavailable");
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
            VerbCollection.Collection.Verbs.Clear();
            PronounCollection.Collection.Pronouns.Clear();

            NounCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Nouns.txt"));
            AdjectiveCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Adjectives.txt"));
            VerbCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Verbs.txt"));
            PronounCollection.Collection.LoadFromFile(Path.Combine(dataPath, "Pronouns.txt"));
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
            SetSingleNoun(root, GrammaticalGender.Feminine, canBeSingular, canBePlural);

            AdjectiveCollection.Collection.Adjectives.Clear();
            AdjectiveCollection.Collection.Adjectives.Add(new Adjective { Root = "czarny" });
        }

        private static void SetSingleNoun(string root, GrammaticalGender gender,
            bool canBeSingular, bool canBePlural)
        {
            Noun noun = new Noun
            {
                Root = root,
                Genre = gender,
                IsConstant = true,
                CanBeSingular = canBeSingular,
                CanBePlural = canBePlural
            };

            NounCollection.Collection.Nouns.Clear();
            NounCollection.Collection.SortedNouns.Clear();
            NounCollection.Collection.Nouns.Add(noun);
            NounCollection.Collection.SortedNouns.Add(gender, new List<Noun> { noun });
        }

        private static void SetSinglePresentVerb(string form, DecliantionNumber number)
        {
            SetSinglePresentVerb(form, number, false);
        }

        private static void VerbEditorValidationRejectsDuplicateForms()
        {
            List<VerbForm> forms = new List<VerbForm>
            {
                CreateVerbForm(VerbTense.Present, GrammaticalGender._Unknown, "robi"),
                CreateVerbForm(VerbTense.Present, GrammaticalGender._Unknown, "czyni")
            };
            string error;
            AssertFalse(VerbFormValidator.TryValidate(forms, out error), "duplicate form keys should be rejected");
            AssertTrue(!string.IsNullOrEmpty(error), "duplicate form validation should explain the problem");
        }

        private static void VerbEditorValidationEnforcesTenseGender()
        {
            string error;
            AssertFalse(VerbFormValidator.TryValidate(new[] {
                CreateVerbForm(VerbTense.Past, GrammaticalGender._Unknown, "robił")
            }, out error), "past form without gender should be rejected");
            AssertFalse(VerbFormValidator.TryValidate(new[] {
                CreateVerbForm(VerbTense.Present, GrammaticalGender.Feminine, "robi")
            }, out error), "present form with gender should be rejected");
            AssertTrue(VerbFormValidator.TryValidate(new[] {
                CreateVerbForm(VerbTense.Past, GrammaticalGender.Feminine, "robiła")
            }, out error), "past form with gender should be accepted");
        }

        private static void VerbEditorValidationEnforcesAspectTense()
        {
            string error;
            AssertFalse(VerbFormValidator.TryValidate(VerbAspect.Perfective, new[] {
                CreateVerbForm(VerbTense.Present, GrammaticalGender._Unknown, "robi")
            }, out error), "a perfective verb must reject a present-tense form");
            AssertTrue(VerbFormValidator.TryValidate(VerbAspect.Perfective, new[] {
                CreateVerbForm(VerbTense.Future, GrammaticalGender._Unknown, "zrobi")
            }, out error), "a perfective verb should accept a future form");
            AssertTrue(VerbFormValidator.TryValidate(VerbAspect.Imperfective, new[] {
                CreateVerbForm(VerbTense.Present, GrammaticalGender._Unknown, "robi")
            }, out error), "an imperfective verb should accept a present form");
        }

        private static void VerbFiniteParadigmsAreComplete()
        {
            LoadLegacyData();
            foreach (Verb verb in VerbCollection.Collection.Verbs)
            {
                string error;
                AssertTrue(VerbFormValidator.TryValidateCompleteFiniteParadigms(verb, out error),
                    error ?? ("finite paradigms should be complete: " + verb.Infinitive));
            }

            Verb incomplete = new Verb { Infinitive = "testować", Aspect = VerbAspect.Imperfective };
            incomplete.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "testuję"));
            string incompleteError;
            AssertFalse(VerbFormValidator.TryValidateCompleteFiniteParadigms(
                incomplete, out incompleteError), "a partial present paradigm should be diagnosed");
            AssertTrue(!string.IsNullOrEmpty(incompleteError),
                "an incomplete paradigm should identify the validation problem");
        }

        private static void CompoundFuturePrefillerPreservesManualForms()
        {
            Verb verb = new Verb { Infinitive = "mówić", Aspect = VerbAspect.Imperfective };
            verb.Forms.Add(new VerbForm
            {
                Tense = VerbTense.Future,
                Person = GrammaticalPerson.First,
                Number = DecliantionNumber.Singular,
                Gender = GrammaticalGender._Unknown,
                Text = "będę mówił"
            });
            int added;
            string error;
            AssertTrue(CompoundFuturePrefiller.TryAddMissingForms(verb, out added, out error),
                "an imperfective verb should support compound-future prefilling");
            AssertTrue(added == 5, "only five missing future forms should be added");
            AssertEqual("będę mówił", VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Future,
                GrammaticalPerson.First, DecliantionNumber.Singular, GrammaticalGender._Unknown),
                "a manually entered alternative should never be overwritten");
            AssertEqual("będą mówić", VerbConjurer.Conjurer.MakeWord(verb, VerbTense.Future,
                GrammaticalPerson.Third, DecliantionNumber.Plural, GrammaticalGender._Unknown),
                "a missing compound form should be generated from the infinitive");

            Verb byc = new Verb { Infinitive = "być", Aspect = VerbAspect.Imperfective };
            AssertTrue(CompoundFuturePrefiller.TryAddMissingForms(byc, out added, out error),
                "the irregular auxiliary verb should support prefilling");
            AssertTrue(added == 6, "all six forms of an empty paradigm should be added");
            AssertEqual("będziesz", VerbConjurer.Conjurer.MakeWord(byc, VerbTense.Future,
                GrammaticalPerson.Second, DecliantionNumber.Singular, GrammaticalGender._Unknown),
                "być should use its one-word irregular future");

            Verb perfective = new Verb { Infinitive = "zabić", Aspect = VerbAspect.Perfective };
            AssertFalse(CompoundFuturePrefiller.TryAddMissingForms(perfective,
                out added, out error), "a perfective verb should not receive compound future forms");
        }

        private static void ImperfectivePresentParadigmsIncludeFuture()
        {
            LoadLegacyData();
            foreach (Verb verb in VerbCollection.Collection.Verbs)
            {
                if (verb.Aspect != VerbAspect.Imperfective ||
                    !verb.Forms.Exists(form => form.Tense == VerbTense.Present))
                    continue;
                foreach (GrammaticalPerson person in new[] {
                    GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third })
                {
                    foreach (DecliantionNumber number in new[] {
                        DecliantionNumber.Singular, DecliantionNumber.Plural })
                    {
                        AssertTrue(verb.Forms.Exists(form => form.Matches(VerbTense.Future,
                            person, number, GrammaticalGender._Unknown)),
                            "an imperfective present paradigm should include future: " +
                            verb.Infinitive);
                    }
                }
            }
        }

        private static void ExplicitVerbFormsHaveClassifiedAspect()
        {
            LoadLegacyData();
            foreach (Verb verb in VerbCollection.Collection.Verbs)
            {
                if (verb.Forms.Count == 0)
                    continue;
                AssertTrue(verb.Aspect != VerbAspect.Unknown,
                    "a verb with explicit forms should have classified aspect: " + verb.Infinitive);
            }
        }

        private static void VerbEditingStatusIdentifiesActionableGaps()
        {
            Verb proposal = new Verb { Infinitive = "tłuc", CanBeActive = true };
            proposal.Categories.Add(1);
            VerbEditingStatus proposalStatus = VerbEditingStatus.Inspect(proposal);
            AssertTrue(proposalStatus.NeedsWork, "an uninflected proposal should need work");
            AssertTrue(proposalStatus.Summary.Contains("brak aspektu") &&
                proposalStatus.Summary.Contains("brak form"),
                "an uninflected proposal should expose aspect and form gaps");

            Verb ready = new Verb { Infinitive = "zabić", Aspect = VerbAspect.Perfective,
                CanBeActive = true };
            ready.Categories.Add(1);
            foreach (GrammaticalPerson person in new[] {
                GrammaticalPerson.First, GrammaticalPerson.Second, GrammaticalPerson.Third })
                foreach (DecliantionNumber number in new[] {
                    DecliantionNumber.Singular, DecliantionNumber.Plural })
                    ready.Forms.Add(new VerbForm { Tense = VerbTense.Future, Person = person,
                        Number = number, Gender = GrammaticalGender._Unknown, Text = "forma" });
            AssertFalse(VerbEditingStatus.Inspect(ready).NeedsWork,
                "a categorized, classified and complete record should be ready");

            Verb incomplete = new Verb { Infinitive = "mówić",
                Aspect = VerbAspect.Imperfective, CanBeActive = true };
            incomplete.Categories.Add(5);
            incomplete.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "mówi"));
            AssertTrue(VerbEditingStatus.Inspect(incomplete).Summary.Contains("Niepełny paradygmat"),
                "a partial finite paradigm should show its validation issue");
        }

        private static VerbForm CreateVerbForm(VerbTense tense, GrammaticalGender gender, string text)
        {
            return new VerbForm
            {
                Tense = tense,
                Person = GrammaticalPerson.Third,
                Number = DecliantionNumber.Singular,
                Gender = gender,
                Text = text
            };
        }

        private static void ModernizedVerseSamplesDecode()
        {
            string versesPath = Path.Combine(LoadLegacyData(), "Verses.txt");
            VersesCreator.Creator.LoadFromFile(versesPath);
            string[] markers =
            {
                "{subject:VP3?}", "{subject:R}", "{victim:PG???P}", "{VT11}",
                "{VT11$1}", "{VT11$9}", "{VT31$4}"
            };

            foreach (string marker in markers)
            {
                VerseEntry entry = VersesCreator.Creator.Verses.Find(item => item.Text.Contains(marker));
                AssertTrue(entry != null, "the verse dictionary should contain a sample for " + marker);
                if (entry == null)
                    continue;

                string decoded = VersesCreator.Creator.DecodeVerse(entry.Text);
                AssertFalse(decoded.Contains("{"), "the sample should decode without diagnostics: " + marker);
                AssertFalse(decoded.Contains("}"), "the sample should not leave an unfinished formatter: " + marker);
                AssertTrue(VerseDefinitionInspector.Inspect(entry.Text).Diagnostics.Count == 0,
                    "the sample should be structurally valid in the editor: " + marker);
            }
        }

        private static void VerseInspectorMapsNamedGroups()
        {
            string source = "{subject:NN1F} {subject:A???E} {subject:VT3?} spokojnie {subject:R} {VT11}";
            VerseDefinitionInspection inspection = VerseDefinitionInspector.Inspect(source);

            AssertTrue(inspection.Groups.Count == 1, "the inspector should find one named group");
            AssertEqual("subject", inspection.Groups[0].Name, "the inspector should retain the group name");
            AssertTrue(inspection.Groups[0].Tokens.Count == 4,
                "the named group should contain noun, adjective, verb and reflexive particle");
            AssertTrue(inspection.Tokens.FindAll(item => string.IsNullOrEmpty(item.GroupName)).Count == 1,
                "the inspector should keep standalone formatters outside the group");
            AssertTrue(inspection.Diagnostics.Count == 0, "the complete group should have no structural diagnostics");
            AssertTrue(inspection.Tokens[0].StartIndex == 0 && inspection.Tokens[0].Length == 14,
                "token positions should select the complete formatter in the editor");
        }

        private static void VerseInspectorReportsPositionedDiagnostics()
        {
            string source = "tekst {bad-name:R} {subject:A???E} }";
            VerseDefinitionInspection inspection = VerseDefinitionInspector.Inspect(source);

            AssertTrue(inspection.Diagnostics.Count >= 4,
                "invalid group names, missing nouns, incomplete reflexive groups and stray braces should be reported");
            foreach (VerseDefinitionDiagnostic diagnostic in inspection.Diagnostics)
            {
                AssertTrue(diagnostic.StartIndex >= 0 && diagnostic.StartIndex < source.Length,
                    "every structural diagnostic should point into the source text");
                AssertTrue(diagnostic.Length > 0, "every structural diagnostic should have a selectable range");
            }
        }

        private static void VerseFormatterBuilderComposesGrammarCodes()
        {
            string formatter;
            string error;
            VerseFormatterBuildRequest noun = new VerseFormatterBuildRequest
            {
                Kind = VerseFormatterBuildKind.Noun,
                GroupName = "subject",
                CaseCode = 'N',
                NumberCode = '1',
                GenderCode = 'F'
            };
            noun.CategoryIds.Add(3);
            noun.CategoryIds.Add(11);
            AssertTrue(VerseFormatterBuilder.TryBuild(noun, out formatter, out error),
                "a categorized noun formatter should build");
            AssertEqual("{subject:NN1F$3,11}", formatter, "the noun builder should retain group and categories");

            VerseFormatterBuildRequest pastVerb = new VerseFormatterBuildRequest
            {
                Kind = VerseFormatterBuildKind.Verb,
                GroupName = "subject",
                TenseCode = 'P',
                PersonCode = '3',
                NumberCode = '?',
                GenderCode = '?'
            };
            AssertTrue(VerseFormatterBuilder.TryBuild(pastVerb, out formatter, out error),
                "a grouped past verb should inherit agreement");
            AssertEqual("{subject:VP3?}", formatter, "the past verb builder should omit inherited gender");

            VerseFormatterBuildRequest pronoun = new VerseFormatterBuildRequest
            {
                Kind = VerseFormatterBuildKind.Pronoun,
                GroupName = "victim",
                CaseCode = 'G',
                PersonCode = '?',
                NumberCode = '?',
                GenderCode = '?',
                PrepositionalPronoun = true
            };
            AssertTrue(VerseFormatterBuilder.TryBuild(pronoun, out formatter, out error),
                "a grouped prepositional pronoun should build");
            AssertEqual("{victim:PG???P}", formatter,
                "the pronoun builder should retain inherited agreement and variant suffix");
        }

        private static void VerseFormatterBuilderRejectsIncompleteStandaloneCodes()
        {
            string formatter;
            string error;
            VerseFormatterBuildRequest adjective = new VerseFormatterBuildRequest
            {
                Kind = VerseFormatterBuildKind.Adjective,
                GroupName = string.Empty,
                CaseCode = '?',
                NumberCode = '?',
                GenderCode = '?',
                LevelCode = 'E'
            };
            AssertFalse(VerseFormatterBuilder.TryBuild(adjective, out formatter, out error),
                "a standalone adjective must not contain inherited features");

            VerseFormatterBuildRequest reflexive = new VerseFormatterBuildRequest
            {
                Kind = VerseFormatterBuildKind.ReflexiveParticle,
                GroupName = string.Empty
            };
            AssertFalse(VerseFormatterBuilder.TryBuild(reflexive, out formatter, out error),
                "a reflexive particle must belong to a group");

            List<int> categories = new List<int>();
            AssertFalse(VerseFormatterBuilder.TryParseCategories("3,x", categories, out error),
                "invalid category text should be rejected before building");
        }

        private static void VerseFormatterBuilderRoundTripsExistingTokens()
        {
            string[] formatters =
            {
                "{subject:NN1F$3,11}", "{subject:A???E$2}", "{subject:VP3?}",
                "{victim:PG???P}", "{subject:R}", "{VT11}", "{VF21$1}"
            };
            foreach (string original in formatters)
            {
                VerseFormatterBuildRequest request;
                string error;
                AssertTrue(VerseFormatterBuilder.TryParse(original, out request, out error),
                    "the builder should parse an existing token: " + original);
                if (request == null)
                    continue;
                string rebuilt;
                AssertTrue(VerseFormatterBuilder.TryBuild(request, out rebuilt, out error),
                    "the parsed token should build again: " + original);
                AssertEqual(original, rebuilt, "editing without changes should preserve the formatter");
            }
        }

        private static void GenerationExclusionsFilterCategories()
        {
            GenerationCategoryFilter.Clear();
            try
            {
                SetCategorizedWords();
                GenerationCategoryFilter.ExcludedNounCategories.Add(3);
                Noun noun = NounCollection.Collection.GetNounEx(GrammaticalGender.Feminine,
                    null, true, false);
                AssertTrue(noun != null && noun.Root == "siekiera",
                    "excluding Animal should leave the Weapon noun available");

                GenerationCategoryFilter.ExcludedAdjectiveCategories.Add(1);
                Adjective adjective = AdjectiveCollection.Collection.GetAdjective(candidate => true);
                AssertTrue(adjective != null && adjective.Root == "dobry",
                    "excluding Color should leave the Positive adjective available");
            }
            finally
            {
                GenerationCategoryFilter.Clear();
            }
        }

        private static void GenerationExclusionsOverrideVerseRequests()
        {
            GenerationCategoryFilter.Clear();
            try
            {
                SetCategorizedWords();
                GenerationCategoryFilter.ExcludedNounCategories.Add(3);
                TestDecoder decoder = new TestDecoder();
                AssertEqual("{NO_VALID_NOUN}", decoder.Decode("{NN1F$3}"),
                    "a session exclusion should override a verse requesting the excluded category");
                AssertEqual("siekiera", decoder.Decode("{NN1F$11}"),
                    "an unrelated requested category should remain available");
            }
            finally
            {
                GenerationCategoryFilter.Clear();
            }
        }

        private static void VerbMetadataRoundTrips()
        {
            SharpDevs.Fleksator.IO.IGrammaticalWordSerializer<Verb> serializer =
                new SharpDevs.Fleksator.IO.GrammarSerializersFactory()
                    .GetOldSerializers().VerbSerializer;
            Verb source = new Verb
            {
                Infinitive = "testować",
                DisplayName = "testować",
                ConjugationNumber = 4,
                Aspect = VerbAspect.Biaspectual,
                CanBeActive = true,
                CanBePassive = true,
                IsReflexive = true
            };
            source.Categories.Add(1);
            source.Categories.Add(7);
            source.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "testuje"));

            string serialized = serializer.Write(source);
            AssertTrue(serialized.Contains("|R|CAT=1,7|CONJ=4|ASPECT=B|VOICE=AP"),
                "verb metadata should use explicit stable tokens");
            Verb loaded = serializer.Load(serialized);
            AssertTrue(loaded != null, "a verb with metadata should deserialize");
            if (loaded == null)
                return;
            AssertTrue(loaded.IsReflexive && loaded.ConjugationNumber == 4 &&
                loaded.Aspect == VerbAspect.Biaspectual && loaded.CanBeActive && loaded.CanBePassive,
                "voice, reflexivity, conjugation and aspect should round-trip");
            AssertTrue(loaded.Categories.Count == 2 && loaded.Categories.Contains(1) &&
                loaded.Categories.Contains(7), "verb categories should round-trip");
        }

        private static void LegacyVerbsRemainBackwardCompatible()
        {
            SharpDevs.Fleksator.IO.IGrammaticalWordSerializer<Verb> serializer =
                new SharpDevs.Fleksator.IO.GrammarSerializersFactory()
                    .GetOldSerializers().VerbSerializer;
            Verb verb = serializer.Load("zabijać|T31=zabija");
            AssertTrue(verb != null && verb.CanBeActive && !verb.CanBePassive,
                "legacy records should retain active-only defaults");
            AssertTrue(verb != null && verb.ConjugationNumber == 0 &&
                verb.Aspect == VerbAspect.Unknown && verb.Categories.Count == 0,
                "legacy records should not invent conjugation, aspect or categories");
            AssertEqual("zabijać|T31=zabija", serializer.Write(verb),
                "writing an untouched legacy record should not add default metadata");
        }

        private static void VerbSerializerRejectsInconsistentAspect()
        {
            SharpDevs.Fleksator.IO.IGrammaticalWordSerializer<Verb> serializer =
                new SharpDevs.Fleksator.IO.GrammarSerializersFactory()
                    .GetOldSerializers().VerbSerializer;
            AssertTrue(serializer.Load("zabić|ASPECT=P|T31=zabija") == null,
                "a hand-edited perfective record with a present form should be rejected");
            AssertTrue(serializer.Load("zabijać|ASPECT=I|T31=zabija|T31=morduje") == null,
                "a hand-edited record with duplicate form keys should be rejected");

            Verb invalid = new Verb { Infinitive = "zabić", Aspect = VerbAspect.Perfective };
            invalid.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "zabija"));
            bool threw = false;
            try
            {
                serializer.Write(invalid);
            }
            catch (InvalidOperationException)
            {
                threw = true;
            }
            AssertTrue(threw, "saving an inconsistent aspect record should fail explicitly");
        }

        private static void VerbCategoriesAreCompleteAndFilterGeneration()
        {
            LoadLegacyData();
            AssertTrue(WordCategories.Categories.VerbCategories.Count == 12,
                "the extended stable verb taxonomy should contain twelve categories");
            AssertTrue(WordCategories.Categories.VerbCategories.ContainsKey(11),
                "state and existence should have a stable category identifier");
            AssertTrue(WordCategories.Categories.VerbCategories.ContainsKey(12),
                "thought and intent should have a stable category identifier");
            AssertTrue(VerbCollection.Collection.Verbs.FindAll(
                verb => verb.Categories.Count == 0).Count == 0,
                "every existing verb should be categorized");
            foreach (Verb verb in VerbCollection.Collection.Verbs)
                foreach (int categoryId in verb.Categories)
                    AssertTrue(WordCategories.Categories.VerbCategories.ContainsKey(categoryId),
                        "verb " + verb.Infinitive + " should reference a defined category");

            GenerationCategoryFilter.Clear();
            try
            {
                VerbCollection.Collection.Verbs.Clear();
                Verb violent = new Verb { Infinitive = "atakować", CanBeActive = true };
                violent.Categories.Add(1);
            Verb moving = new Verb { Infinitive = "uciekać", CanBeActive = true };
            moving.Categories.Add(4);
            Verb state = new Verb { Infinitive = "być", CanBeActive = true };
            state.Categories.Add(11);
            Verb thought = new Verb { Infinitive = "myśleć", CanBeActive = true };
            thought.Categories.Add(12);
            VerbCollection.Collection.Verbs.Add(violent);
            VerbCollection.Collection.Verbs.Add(moving);
            VerbCollection.Collection.Verbs.Add(state);
            VerbCollection.Collection.Verbs.Add(thought);
            GenerationCategoryFilter.ExcludedVerbCategories.Add(1);
            GenerationCategoryFilter.ExcludedVerbCategories.Add(4);
            GenerationCategoryFilter.ExcludedVerbCategories.Add(11);
            Verb selected = VerbCollection.Collection.GetVerb(candidate => true);
            AssertTrue(selected == thought,
                "the session filter should leave the non-excluded thought verb available");
            }
            finally
            {
                GenerationCategoryFilter.Clear();
            }
        }

        private static void VerbFormatterRespectsCategoryAndVoiceParameters()
        {
            Verb violent = new Verb { Infinitive = "atakować", CanBeActive = true };
            violent.Categories.Add(1);
            violent.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "atakuje"));
            Verb passiveOnly = new Verb { Infinitive = "ścigać", CanBePassive = true };
            passiveOnly.Categories.Add(4);
            passiveOnly.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "ściga"));
            Verb state = new Verb { Infinitive = "być", CanBeActive = true };
            state.Categories.Add(11);
            state.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "jest"));
            Verb thought = new Verb { Infinitive = "myśleć", CanBeActive = true };
            thought.Categories.Add(12);
            thought.Forms.Add(CreateVerbForm(VerbTense.Present,
                GrammaticalGender._Unknown, "myśli"));
            VerbCollection.Collection.Verbs.Clear();
            VerbCollection.Collection.Verbs.Add(violent);
            VerbCollection.Collection.Verbs.Add(passiveOnly);
            VerbCollection.Collection.Verbs.Add(state);
            VerbCollection.Collection.Verbs.Add(thought);

            TestDecoder decoder = new TestDecoder();
            AssertEqual("atakuje", decoder.Decode("{VT31$1}"),
                "a positive category suffix should filter active verbs");
            AssertEqual("{NO_VALID_VERB}", decoder.Decode("{VT31$4}"),
                "the active formatter should not select a passive-only verb");
            AssertEqual("jest", decoder.Decode("{VT31$11}"),
                "a positive category suffix should select a state verb");
            AssertEqual("myśli", decoder.Decode("{VT31$12}"),
                "a positive category suffix should select a thought verb");
        }

        private static void VerseEditorSavePreservesSourceStructure()
        {
            string sourcePath = Path.Combine(LoadLegacyData(), "Verses.txt");
            string temporaryPath = Path.Combine(Path.GetTempPath(), "Flaksator-Verses-" + Guid.NewGuid() + ".txt");
            try
            {
                File.Copy(sourcePath, temporaryPath);
                string[] before = File.ReadAllLines(temporaryPath, Encoding.Unicode);
                int commentCount = Array.FindAll(before, item => item.TrimStart().StartsWith(";")).Length;
                int eofCount = Array.FindAll(before, item => item.Trim().StartsWith("EOF")).Length;

                VersesCreator.Creator.LoadFromFile(temporaryPath);
                VerseEntry first = VersesCreator.Creator.Verses[0];
                first.Text += " [test]";
                VersesCreator.Creator.SaveToFile(temporaryPath);

                byte[] bytes = File.ReadAllBytes(temporaryPath);
                AssertTrue(bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE,
                    "the verse editor should retain the UTF-16 LE BOM");
                string[] after = File.ReadAllLines(temporaryPath, Encoding.Unicode);
                AssertTrue(after.Length == before.Length, "editing one verse should preserve the source line count");
                AssertTrue(Array.FindAll(after, item => item.TrimStart().StartsWith(";")).Length == commentCount,
                    "editing one verse should preserve comments");
                AssertTrue(Array.FindAll(after, item => item.Trim().StartsWith("EOF")).Length == eofCount,
                    "editing one verse should preserve EOF markers");
                AssertTrue(Array.Exists(after, item => item.EndsWith("[test]")),
                    "the edited verse should be written back to its source line");
            }
            finally
            {
                if (File.Exists(temporaryPath))
                    File.Delete(temporaryPath);
            }
        }

        private static void SetSinglePresentVerb(string form, DecliantionNumber number, bool isReflexive)
        {
            Verb verb = new Verb
            {
                Infinitive = "testować",
                IsReflexive = isReflexive,
                CanBeActive = true
            };
            verb.Forms.Add(new VerbForm
            {
                Tense = VerbTense.Present,
                Person = GrammaticalPerson.Third,
                Number = number,
                Gender = GrammaticalGender._Unknown,
                Text = form
            });

            VerbCollection.Collection.Verbs.Clear();
            VerbCollection.Collection.Verbs.Add(verb);
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
