# Dictionary category coverage

Snapshot after full categorization and the first vocabulary expansion:

| Dictionary | Entries | Initial categorized | Current categorized | Uncategorized |
| --- | ---: | ---: | ---: | ---: |
| Nouns | 276 | 182/276 (65.9%) | 276/276 (100%) | 0 |
| Adjectives | 317 | 40/273 (14.7%) | 317/317 (100%) | 0 |

The initial conservative pass classified entries with an unambiguous match in the original taxonomy. Later passes categorized the remaining historical entries and extended the taxonomy without changing identifiers 1-6.

Current coverage of the extended adjective categories, counted directly from `Adjectives.txt`:

| ID | Category | Entries |
| ---: | --- | ---: |
| 7 | Material | 8 |
| 8 | Shape | 10 |
| 9 | Temperature | 12 |
| 10 | Place & Origin | 16 |
| 11 | Physical Condition | 81 |
| 12 | Time | 12 |
| 13 | Size | 9 |
| 14 | Motion & Dynamics | 12 |
| 15 | Texture & Surface | 13 |
| 16 | Technology | 10 |
| 17 | Abstract & Conceptual | 20 |
| 18 | Value & Cost | 12 |
| 19 | Style & Theme | 13 |
| 20 | Behavior & Attitude | 25 |
| 21 | Intensity & Impact | 18 |
| 22 | Sound | 10 |

The first vocabulary expansion added 44 non-duplicate adjectives. It deliberately strengthened the smallest practical groups: sound, technology, temperature, texture and surface, time, and value and cost. Semantically non-gradable entries use `!`, so the generator does not produce artificial comparative forms such as `bardziej kwantowa`.

The final noun categorization pass added three stable categories:

| ID | Category | Entries |
| ---: | --- | ---: |
| 21 | Time | 4 |
| 22 | Nature | 10 |
| 23 | Event | 3 |

Existing category identifiers must remain stable because dictionary records and formatter suffixes persist their numeric values.

The executable specifications enforce these safeguards:

- every category identifier referenced by a dictionary entry must exist in `Categories.ini`;
- every noun and adjective must remain categorized;
- every extended adjective category must generate at least one valid inflected form;
- every new noun category must generate at least one valid inflected form;
- every adjective from the expansion must load and generate its exact expected feminine nominative, feminine genitive and non-personal plural nominative.
