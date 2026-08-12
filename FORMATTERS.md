# Flaksator formatters

The legacy grammar code remains unchanged:

- `NN1T` - noun, nominative, singular, masculine thing;
- `A???E` - adjective inheriting case, number and gender from its group noun;
- `{1:A???E} {1:NN1T}` - adjective and noun agreement group.

Groups may also use readable names. Numeric groups remain fully compatible:

- `{subject:NN2F} ... {subject:A???E}` - a named agreement group;
- group names contain letters, digits or `_` and may connect distant placeholders in one template.

An optional `$` suffix filters a word by category identifiers from `Categories.ini`:

- `NN1T$11` - a noun from category 11 (`Weapon`);
- `A???E$3,4` - an adjective from category 3 or 4;
- `{1:A???E$3} {1:NN1T$11}` - category filters combined with group agreement.

Multiple category identifiers use OR semantics. Existing formatters without a category suffix continue to select from the complete dictionary.

Category exclusions are deliberately not stored in formatter definitions. The main generator UI maintains a session-wide exclusion set for nouns and adjectives. An excluded category takes precedence over a positive `$` request in a verse, allowing the same source corpus to be generated under different content filters without rewriting `Verses.txt`. Verb exclusions are already represented in the UI and become active once verb records gain category metadata.

## Verbs

A finite verb formatter contains `V`, tense, person and number:

- `VT31` - present tense, third person, singular;
- `VT12` - present tense, first person, plural;
- `VF21` - future tense, second person, singular;
- `{subject:NN2F} ... {subject:VT3?}` - the verb inherits plural number from the named noun subject;
- `?` in the person position defaults to third person when a noun anchors the group.

Past-tense formatters add an agreement gender when they are independent:

- `VP31F` - past, third person, singular feminine (`opętała`);
- `VP32P` - past, third person, plural masculine-personal (`opętali`);
- `{subject:NN1N} ... {subject:VP3?}` - number and gender are inherited from the named subject (`opętało`);
- `{subject:NN2F} ... {subject:VP3?}` - non-masculine-personal plural agreement (`opętały`).

For compact storage, past-tense agreement uses five form classes: singular masculine, feminine and neuter, plus plural masculine-personal and other. `T` (present), `P` (past) and `F` (future) are implemented. Simple perfective future forms use the same person/number shape as present forms but remain separate `F...` records, so `obetnę` is never mislabeled as present tense. Imperfective compound forms are also stored explicitly and may contain spaces, for example `F21=będziesz mówić`.

`Verbs.txt` keeps the infinitive first and stores explicit forms after it. This avoids guessing Polish conjugation rules and gives a future editor a lossless representation:

```text
zabijać|T11=zabijam|T21=zabijasz|T31=zabija|T12=zabijamy|T22=zabijacie|T32=zabijają
opętać|P31T=opętał|P31F=opętała|P31N=opętało|P32P=opętali|P32F=opętały
obciąć|ASPECT=P|F11=obetnę|F21=obetniesz|F31=obetnie|F12=obetniemy|F22=obetniecie|F32=obetną
zabijać|ASPECT=I|F11=będę zabijać|F21=będziesz zabijać|F31=będzie zabijać|F12=będziemy zabijać|F22=będziecie zabijać|F32=będą zabijać
```

Verb records may also carry backward-compatible metadata tokens before their forms:

- `CAT=1,2` - one or more verb categories from the `[Verb]` section of `Categories.ini`;
- `CONJ=4` - an optional conjugation-group number (`0`/absent means not classified yet);
- `ASPECT=I`, `ASPECT=P`, `ASPECT=B` - imperfective, perfective or biaspectual; absent means not classified yet;
- `VOICE=A`, `VOICE=P`, `VOICE=AP` - active, passive, or both; legacy records default to active-only;
- `R` - the existing reflexive marker.

Example: `testować|R|CAT=1,7|CONJ=4|VOICE=AP|T31=testuje`. Current `V...` formatters generate active voice and therefore exclude passive-only records. Passive voice metadata is persisted for a future dedicated formatter.

The editor rejects present-tense `T...` forms on a record explicitly marked `ASPECT=P`. Imperfective and biaspectual records may retain both present and explicit future forms; unknown aspect preserves backward compatibility for historical records.

The serializer applies the same structural validation when loading hand-edited data and before saving. Duplicate form keys and perfective records containing `T...` forms are rejected. Dictionary specifications additionally require every stored present or future paradigm to contain all six person/number combinations; past-tense agreement remains independently sparse because it uses gender classes.

The verb editor's `Uzupełnij przyszły` action fills only missing `F...` slots for `ASPECT=I`. It uses the neutral infinitive construction (`będę mówić`) and the irregular one-word future of `być`; existing alternatives such as `będę mówił` remain untouched for manual control.

All dictionary records that contain explicit forms also declare their aspect. Uninflected historical proposals may remain unknown until their paradigm is authored; this preserves honest uncertainty without weakening generation-ready data.

## Reflexive verbs

`R` is a movable reflexive-particle slot inside a group. The dictionary marks a reflexive verb once with `|R`; individual forms do not repeat `się`:

```text
delektować|R|T11=delektuję|T21=delektujesz|T31=delektuje|T12=delektujemy|T22=delektujecie|T32=delektują
```

The author controls the particle position independently:

- `{subject:NN1F} {subject:R} {subject:VT3?}` - `kotka się delektuje`;
- `{subject:NN1F} {subject:VT3?} spokojnie {subject:R}` - `kotka delektuje spokojnie się`.

A group with `R` must contain exactly one verb, and that verb must be marked reflexive. A standalone verb formatter deliberately excludes reflexive entries because it has no particle slot.

## Personal pronouns

A pronoun formatter contains `P`, case, person, number and gender:

- `PC31F` - third-person feminine singular accusative (`ją`);
- `PA32F` - third-person non-masculine-personal plural instrumental (`nimi`);
- `{victim:NN1F} ... {victim:PC???}` - accusative stays explicit while person defaults to third and number/gender come from `victim`;
- `{victim:NN2P} ... {victim:PN???}` - nominative masculine-personal plural (`oni`).

The first pronoun dictionary contains the third-person paradigm: six cases for five agreement classes. As with past verbs, singular masculine genders share one class, and plural distinguishes masculine-personal from all other genders.

`Pronouns.txt` stores every form explicitly so a later editor can round-trip corrections. Without a suffix, the formatter selects neutral, non-emphatic forms such as `go`, `mu`, `ją`, `ich` and `im`.

An optional final `P` explicitly requests the prepositional variant:

- `{victim:NN1F} widzę {victim:PC???}` - `bestia widzę ją`;
- `{victim:NN1F} bez {victim:PG???P}` - `bestia bez niej`;
- `PG31TP` - masculine singular genitive after a preposition (`niego`);
- `PC32FP` - non-masculine-personal plural accusative after a preposition (`nie`).

The formatter never guesses from surrounding text. The author or future verse editor selects the prepositional mode explicitly. There are 30 neutral forms and 25 prepositional forms; nominative deliberately has no prepositional variant.
