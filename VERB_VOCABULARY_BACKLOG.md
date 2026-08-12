# Verb vocabulary backlog

The first extraction batch comes directly from literal forms already present in `Verses.txt`. Each promoted lemma has a complete present-tense paradigm and at least one generator category. This is a curated migration, not automatic lemmatization.

## Implemented from the legacy verses

- `wbijać`, `rzucać`, `wyrywać`, `kopać`
- `uderzać`, `trafiać`, `pożerać`
- `czuć`, `widzieć`
- `biec`, `padać`, `podążać`
- completed the previously listed `krzyczeć` paradigm

Two representative literal verse fragments now exercise category-selected present forms (`VT11` and `VT31`) while keeping the original text structure.

The second extraction batch adds `być`, `mieć`, `leżeć`, `siedzieć`, `umierać`, `lecieć`, `wylatywać`, `odpadać`, `przewracać` and reflexive `poniewierać się`. Category 11 (`State & Existence`) was introduced for the state and position verbs; the remaining additions reuse movement, injury, transformation, violence and control where appropriate.

Literal occurrences from this batch stay unchanged in the verse source. A category selects a semantic family rather than a lemma, so replacing `jesteś` with `{VT21$11}` could also generate `masz`, `leżysz` or `umierasz` and damage the sentence. Deterministic specifications exercise these new forms without weakening authored text.

The third extraction batch adds `wiedzieć`, `myśleć`, `chcieć`, `musieć`, `próbować`, `słyszeć`, `błagać`, `kazać`, `robić` and `mierzyć`. Category 12 (`Thought & Intent`) separates cognition and volition from physical sensation; speech, control and creation remain additional categories where the verb genuinely spans those meanings.

The fourth extraction batch adds `wyjmować`, `parzyć`, `przecinać`, `zanurzać`, `dusić`, `mówić`, `obcinać`, `wydłubywać`, `chwytać` and `gryźć`. Perfective source forms such as `obetnę`, `wydłubię` and `chwyciłem` are evidence for a lexical family only; their imperfective lemmas supply the actual present-tense paradigms. Future or past forms are not mislabeled as present forms.

The first future-tense batch activates the reserved `F` slots and adds perfective `zabić`, `obciąć`, `wydłubać`, `pochłonąć`, `nadejść`, `dopaść`, `znaleźć`, `wbić`, `rozpocząć` and `pociąć`. Each record carries `ASPECT=P`; legacy records without `ASPECT` remain valid and load as unknown. The verb editor exposes unknown, imperfective, perfective and biaspectual aspect values.

The first compound-future batch marks ten common records `ASPECT=I` and stores all six explicit `będę/będziesz/będzie/będziemy/będziecie/będą + infinitive` forms. `być` keeps its irregular one-word future. Validation prevents explicitly perfective records from acquiring misleading `T...` forms while leaving old unknown-aspect records readable.

The next aspect pass classifies ten unambiguous historical imperfectives without inventing missing forms: `zażynać`, `krwawić`, `tłamsić`, `masakrować`, `atakować`, `klonować`, `wypruwać`, `męczyć`, `przebijać` and `wybebeszać`. Serializer validation now also protects direct text edits, while a separate completeness check ensures that every stored `T` or `F` paradigm has all six person/number slots.

The second compound-future pass completes `krwawić`, `atakować`, `krzyczeć`, `wbijać`, `rzucać`, `wyrywać`, `kopać`, `uderzać`, `trafiać`, `padać`, `pożerać` and `podążać`. A dictionary invariant now requires every explicitly imperfective record with a present paradigm to carry the corresponding complete future paradigm.

The final explicit-form aspect pass classifies every remaining inflected record. Twenty-four further present paradigms are imperfective and receive complete compound futures, `kazać` is explicitly biaspectual, and past-only `opętać` is perfective. No record with stored forms may now remain at unknown aspect.

## Next extraction candidates

- movement and position: `stać`, `iść`, `chodzić`, `pełzać`
- perception and thought: `dostrzegać`, `patrzeć`, `pamiętać`, `zapominać`
- speech and intention: `wołać`, `odpowiadać`, `obiecywać`
- handling and impact: `uderzać narzędziem` variants, `rozrywać`, `zgniatać`
- bodily and state changes: `gnić`, `drżeć`, `wymiotować`

The state/existence category now covers stable state and position verbs. Perfective `zostać` remains deferred until the grammar distinguishes future forms from the present-tense slots used by imperfective `zostawać`.
