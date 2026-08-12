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

## Present-tense verbs

A finite verb formatter contains `V`, tense, person and number:

- `VT31` - present tense, third person, singular;
- `VT12` - present tense, first person, plural;
- `{subject:NN2F} ... {subject:VT3?}` - the verb inherits plural number from the named noun subject;
- `?` in the person position defaults to third person when a noun anchors the group.

`T` (present) is implemented in the first verb slice. `P` (past) and `F` (future) are reserved for later expansion, including gender agreement.

`Verbs.txt` keeps the infinitive first and stores explicit forms after it. This avoids guessing Polish conjugation rules and gives a future editor a lossless representation:

```text
zabijać|T11=zabijam|T21=zabijasz|T31=zabija|T12=zabijamy|T22=zabijacie|T32=zabijają
```
