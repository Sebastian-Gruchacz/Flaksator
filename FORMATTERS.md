# Flaksator formatters

The legacy grammar code remains unchanged:

- `NN1T` - noun, nominative, singular, masculine thing;
- `A???E` - adjective inheriting case, number and gender from its group noun;
- `{1:A???E} {1:NN1T}` - adjective and noun agreement group.

An optional `$` suffix filters a word by category identifiers from `Categories.ini`:

- `NN1T$11` - a noun from category 11 (`Weapon`);
- `A???E$3,4` - an adjective from category 3 or 4;
- `{1:A???E$3} {1:NN1T$11}` - category filters combined with group agreement.

Multiple category identifiers use OR semantics. Existing formatters without a category suffix continue to select from the complete dictionary.
