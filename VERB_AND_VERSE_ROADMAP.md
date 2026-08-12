# Verb and verse editor roadmap

## Repository findings

- `Verbs.txt` contained 47 infinitive proposals but no grammatical data.
- `VerbConjurer` was empty and `OldVerbSerializer` threw `NotImplementedException`.
- no verb collection was loaded by the application;
- formatter `V` was disabled in both independent decoding and groups;
- existing numeric groups already connect distant placeholders, but their identifiers are difficult to understand in authored verses;
- `Verses.txt` historically used nouns and adjectives only; representative lines now exercise named anchors, present and past verbs, movable reflexive particles and inherited pronouns.

## First implemented slice

- explicit present-tense forms are losslessly stored in `Verbs.txt`;
- explicit simple-future forms use `F` slots and retain perfective `ASPECT=P` metadata;
- explicit compound-future forms retain spaces and imperfective `ASPECT=I` metadata;
- the verb editor can prefill missing compound-future slots without overwriting manual alternatives;
- the verb editor can filter an actionable work queue and shows missing aspect, forms or incomplete paradigms beside each lemma;
- standalone verb formatters use `V<tense><person><number>`;
- noun groups may contain a present-tense verb and provide its number;
- groups accept readable names such as `subject` while retaining numeric identifiers;
- three complete paradigms exercise different endings: `zabijać`, `krwawić`, `atakować`.
- the first past-tense paradigm (`opętać`) exercises all five Polish agreement classes and inherits number and gender from a named subject.
- reflexive verbs use one persisted `|R` marker and a movable group slot, so `się` is not duplicated in every inflected form.
- a data-driven third-person pronoun paradigm can inherit person, number and gender from a distant named noun while retaining its own case.
- an explicit formatter suffix selects prepositional pronoun variants (`niego`, `niej`, `nich`) without inspecting or coupling to surrounding text.

Explicit forms are intentional. Polish conjugation patterns can later prefill an editor, but the persisted representation must retain irregular and manually corrected forms without depending on a heuristic.

## Next vertical slices

1. Continue expanding explicit paradigms from verbs found in the unprocessed historical verses. The taxonomy now has twelve stable categories after separating state and existence from movement, and thought and intent from sensation and emotion.
2. Add category support for pronouns when their dictionary gains category metadata, and offer deliberate group-wide rename/refactoring.

The verse editor preserves comments, blank lines, section markers and `EOF` records when saving an edited source line. It lists active verses only, filters without rebuilding on selection, and renders a randomized preview through the real decoder. Its structured layer inserts common formatters, visualizes named groups and standalone tokens, navigates from positioned diagnostics back to the source, and offers a full builder for case, number, gender, person, tense, level, pronoun variant and named categories. Double-clicking an existing token reopens it in the builder and replaces its exact source range without changing an untouched formatter.

The source text format remains the compatibility boundary. Future editors should round-trip it rather than replacing it with an opaque serialized document.

Generator category exclusions are a separate session-level concern. They live in the main UI, override positive category requests during random selection, and never alter verse definitions.
