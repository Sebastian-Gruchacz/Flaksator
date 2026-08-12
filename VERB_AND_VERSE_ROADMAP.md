# Verb and verse editor roadmap

## Repository findings

- `Verbs.txt` contained 47 infinitive proposals but no grammatical data.
- `VerbConjurer` was empty and `OldVerbSerializer` threw `NotImplementedException`.
- no verb collection was loaded by the application;
- formatter `V` was disabled in both independent decoding and groups;
- existing numeric groups already connect distant placeholders, but their identifiers are difficult to understand in authored verses;
- `Verses.txt` currently uses nouns and adjectives only. Its comments describe the intended agreement behavior, but no verse editor exists.

## First implemented slice

- explicit present-tense forms are losslessly stored in `Verbs.txt`;
- standalone verb formatters use `V<tense><person><number>`;
- noun groups may contain a present-tense verb and provide its number;
- groups accept readable names such as `subject` while retaining numeric identifiers;
- three complete paradigms exercise different endings: `zabijać`, `krwawić`, `atakować`.

Explicit forms are intentional. Polish conjugation patterns can later prefill an editor, but the persisted representation must retain irregular and manually corrected forms without depending on a heuristic.

## Next vertical slices

1. Add past-tense forms with gender agreement and tests for masculine-personal versus non-masculine-personal plural.
2. Add reflexive verbs and a controlled `się` placement policy.
3. Introduce pronoun records and let them inherit person, number and gender from a named anchor while overriding case.
4. Build the verb editor on the tested serializer and preview every stored form.
5. Build the verse editor with text segments, formatter blocks, named-anchor visualization, diagnostics and repeated live previews.

The source text format remains the compatibility boundary. Future editors should round-trip it rather than replacing it with an opaque serialized document.
