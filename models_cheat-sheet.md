
Today 5:47 PM
Jak používat SKILL.md A playboks v Devinovi. Nauč mě to. Mám Devin Desktop v3.4.27

Worked for 17s
V Devinovi jsou SKILL.md a Playbook dvě různé vrstvy:

SKILL.md = znovupoužitelný pracovní postup uložený v repozitáři

Playbook = znovupoužitelný startovní prompt pro novou Devin session

AGENTS.md = trvalá pravidla a kontext repozitáře

Nejdůležitější je nepoužívat všechny tři na totéž.

1. Mentální model
Představ si práci vývojáře:

Mechanismus	Přirovnání	Typický obsah
AGENTS.md	firemní nebo projektová příručka	architektura, pravidla, zakázané operace, testovací standard
SKILL.md	standardní operační postup	„jak udělat release“, „jak ověřit migraci“, „jak vytvořit API endpoint“
Playbook	šablona zadání úkolu	„proveď security review“, „implementuj ticket“, „analyzuj bug“
Běžný prompt	konkrétní pracovní požadavek	„oprav issue #142“
Prakticky:

AGENTS.md říká, jak se v tomto repozitáři chovat.
SKILL.md říká, jak provést konkrétní opakovaný proces.
Playbook říká, jak má začít určitý typ Devin session.

2. Co přesně je SKILL.md
Skill je složka obsahující soubor SKILL.md a případně další podpůrné soubory:

.agents/
└── skills/
    └── review-pull-request/
        ├── SKILL.md
        ├── checklist.md
        └── scripts/
            └── collect-diff.sh
Pro cloudového Devina je doporučené umístění:

.agents/skills/<skill-name>/SKILL.md
Devin podporuje také například .github/skills, .claude/skills, .cursor/skills, .codex/skills, .cognition/skills a .windsurf/skills. Doporučil bych ale standardizovat na .agents/skills, protože je nástrojově neutrální. Devin Skills odpovídají otevřenému Agent Skills standardu. 

Skill se nenačítá celý neustále. Agent nejprve vidí hlavně jeho name a description; celý obsah a podpůrné soubory načte, až když skill vyhodnotí jako relevantní nebo jej explicitně vyvoláš. Tím se šetří kontextové okno. 

Minimální SKILL.md
---
name: review-pull-request
description: Review a pull request for correctness, regressions, security risks, missing tests, and violations of repository conventions. Use when asked to review a PR, branch, diff, or proposed implementation.
---

# Pull Request Review

## Procedure

1. Read the root AGENTS.md and any applicable nested AGENTS.md files.
2. Identify the base branch and changed files.
3. Inspect the complete diff.
4. Understand the affected execution paths before evaluating the changes.
5. Run relevant tests and static analysis where practical.
6. Check for:
   - correctness defects,
   - regressions,
   - security vulnerabilities,
   - concurrency or transaction issues,
   - error-handling gaps,
   - missing or weak tests,
   - violations of repository conventions.
7. Report findings by severity:
   - Critical
   - Major
   - Moderate
   - Minor
8. For every finding provide:
   - file and line,
   - concrete failure scenario,
   - impact,
   - recommended correction.
9. Do not modify the implementation unless explicitly asked.
10. If no material defect is found, state that explicitly and list residual risks.
Proč je důležitý description
description není jen dokumentace pro člověka. Je to hlavní signál, podle kterého agent rozhoduje, zda má skill použít. Oficiální dokumentace výslovně doporučuje popsat co skill dělá a kdy má být použit. 

Slabé:

description: Reviews code.
Dobré:

description: Review a pull request, branch, or git diff for correctness,
security vulnerabilities, regressions, architectural violations, and missing
tests. Use whenever the user asks for a code review or implementation audit.
3. Jak skill použít v Devin Desktop
V Devin Desktop 3.4.27 můžeš pracovat minimálně se dvěma relevantními agenty:

Cascade

Devin Local, pokud jej máš v účtu povolený

Jejich práce se skills je podobná, ale konfigurace není úplně stejná.

Varianta A: Workspace skill přes UI v Cascade
V panelu Cascade:

Otevři panel Cascade.

Klikni na tři tečky vpravo nahoře.

Otevři Skills.

Zvol + Workspace.

Zadej název, například:

review-pull-request
Název má používat malá písmena, čísla a pomlčky. Devin Desktop potom vytvoří workspace skill v projektu. Oficiální dokumentace pro Cascade používá cestu:

.windsurf/skills/<skill-name>/SKILL.md

Varianta B: Ruční vytvoření, které bych doporučil tobě
Protože používáš více agentických nástrojů, vytvoř:

.agents/skills/review-pull-request/SKILL.md
Toto je lepší pro přenositelnost mezi Devinem, Codexem a dalšími nástroji.

Pokud by tvoje konkrétní sestavení Cascade skill v .agents/skills nenabídlo v UI, můžeš mít kompatibilní kopii nebo odkaz také zde:

.windsurf/skills/review-pull-request/SKILL.md
Za hlavní zdroj pravdy bych ale stále považoval .agents/skills.

Vyvolání skillu
Automaticky
Napiš:

Review the current branch against main. Do not modify the code.
Pokud je description správně napsaný, Devin by měl skill aktivovat sám.

Explicitně
Použij zmínku:

@skills:review-pull-request Review the current branch against main.
V Cascade může být skill dostupný také přes běžné @mention. Oficiální dokumentace uvádí automatické modelové vyvolání i explicitní zmínku. 

Pro důležité procesy doporučuji explicitní vyvolání. Nespoléháš tak na to, zda agent správně rozpozná záměr.

4. Co může být uvnitř skillu
Skill není omezený na samotný markdown.

Může obsahovat:

.agents/skills/create-release/
├── SKILL.md
├── release-checklist.md
├── templates/
│   └── release-notes.md
└── scripts/
    ├── verify-version.py
    └── collect-changes.py
V SKILL.md pak uvedeš:

## Supporting resources

- Use `release-checklist.md` as the mandatory checklist.
- Use `templates/release-notes.md` for the release notes structure.
- Run `scripts/verify-version.py` before modifying version files.
- Run `scripts/collect-changes.py` to prepare the changelog input.
To je jedna z hlavních výhod skillu proti dlouhému promptu: může sdružovat instrukce, skripty, checklisty a šablony. 

5. Dynamické argumenty ve skillu
Devin Skills podporují argumenty.

Například:

---
name: release
description: Prepare and validate a repository release for a specified version and target branch.
---

# Release

Target version: `$0`  
Target branch: `$1`

## Procedure

1. Verify that the current branch is `$1`.
2. Verify that version `$0` does not already exist.
3. Run the full release test suite.
4. Update version metadata to `$0`.
5. Generate release notes.
6. Stop before publishing or pushing unless explicitly authorized.
Vyvolání:

@skills:release 1.4.0 main
Potom:

$0 = 1.4.0

$1 = main

$ARGUMENTS = 1.4.0 main

Devin podporuje také vložení výstupu shellového příkazu pomocí konstrukce !command``. Skill tak může při aktivaci získat například aktuální branch nebo diff. 

Příklad:

Current branch:

!`git branch --show-current`

Current changes:

!`git status --short`
Dynamické příkazy používej střídmě. Neměly by mít vedlejší účinky.

6. Co je Playbook
Playbook je opakovaně použitelný prompt pro celý typ session. Je uložený v Devin platformě a zpravidla jej připojíš při vytváření nové cloudové Devin session.

Oficiální dokumentace jej popisuje jako něco podobného vlastnímu systémovému promptu pro opakovaný typ úkolu. Je sdílitelný v organizaci a používá se, když se stejné zadání opakuje ve více sessions nebo repozitářích. 

Typický Playbook:

# Implement GitHub Issue

## Objective

Implement the supplied GitHub issue in the selected repository.

## Procedure

1. Read the issue, linked discussions, and applicable repository instructions.
2. Inspect the relevant implementation and tests.
3. Restate the acceptance criteria.
4. Produce an implementation plan before editing.
5. Implement the smallest complete solution.
6. Add or update tests covering the acceptance criteria.
7. Run relevant tests, linting, and type checking.
8. Inspect the final diff for unrelated changes.
9. Create a pull request.

## Constraints

- Do not weaken tests to make the implementation pass.
- Do not introduce unrelated refactoring.
- Do not change public APIs unless required by the issue.
- Do not merge the pull request.
- Escalate ambiguous product decisions instead of guessing.

## Completion report

Provide:

- summary of implementation,
- changed files,
- tests executed,
- known limitations,
- pull request link.
Potom při spuštění session doplníš pouze konkrétní hodnoty, například:

Repository: pospanet/simpleforge
Issue: #184
Target branch: main
7. Jak použít Playbook
V nové Devin session:

Otevři výběr Playbooků.

Vyber Playbook z Team nebo Community knihovny.

Zkontroluj, že se u promptu objevil modrý štítek neboli „pill“.

Před spuštěním můžeš jeho obsah pro danou session upravit.

Spusť session.

Playbook lze alternativně přiložit jako soubor:

implement-issue.devin.md
Oficiální dokumentace podporuje jak výběr z knihovny, tak připojení <filename>.devin.md. Playbook může mít také makro, například !implement-issue, kterým jej rychle připojíš. 

V červnu 2026 navíc Devin přidal možnost nastavit přímo v Playbooku režim Devina, například Normal nebo Fast. 

Kde jej vytvořit
V Devin webové aplikaci je správa typicky v:

Settings → Playbooks
nebo v knihovně Playbooků.

V Devin Desktop můžeš cloudovou Devin session otevřít a řídit, ale Playbook chápej primárně jako součást Devin platformy a cloudových sessions, nikoliv jako lokální workspace soubor typu SKILL.md.

8. Skill versus Playbook na konkrétním příkladu
Řekněme, že chceš pravidelně implementovat GitHub issues.

Playbook
Definuje celý pracovní kontrakt:

Implementuj issue, vytvoř plán, implementuj změny,
spusť testy, vytvoř PR a napiš completion report.
Je použitelný pro:

SimpleForge,

AccessGW,

StreamHub,

jiný repozitář.

Skills v jednotlivých repozitářích
SimpleForge může mít:

@skills:test-simpleforge
@skills:add-provider
@skills:create-rfc
AccessGW může mít:

@skills:test-accessgw
@skills:update-runtime-manifest
@skills:verify-security-baseline
Playbook tedy řídí celou session, zatímco skill dodává repo-specifický postup uvnitř session.

Příklad startu:

[Playbook: Implement GitHub Issue]

Implement issue #184.

Use @skills:add-provider for the provider implementation workflow.
Use @skills:test-simpleforge before creating the PR.
9. Kdy použít AGENTS.md místo skillu
Do AGENTS.md dej pravidla platná obecně:

# Repository Instructions

- Python 3.14 is the supported runtime.
- Use pytest for all tests.
- Public functions require type annotations.
- Never modify migration history.
- Do not add dependencies without documenting the rationale.
- Run `pytest -q` before reporting completion.
Do skillu dej konkrétní proces:

# Add Provider

1. Add the provider adapter.
2. Register capabilities.
3. Update readiness checks.
4. Add contract tests.
5. Update provider documentation.
Jednoduchý test:

Platí to vždy? → AGENTS.md

Platí to jen při určité operaci? → SKILL.md

Je to typ zadání celé session? → Playbook

10. Doporučená struktura pro tvoje projekty
Pro SimpleForge bych začal takto:

SimpleForge/
├── AGENTS.md
├── .agents/
│   └── skills/
│       ├── implement-issue/
│       │   └── SKILL.md
│       ├── review-change/
│       │   └── SKILL.md
│       ├── add-provider/
│       │   ├── SKILL.md
│       │   └── provider-checklist.md
│       ├── create-rfc/
│       │   ├── SKILL.md
│       │   └── rfc-template.md
│       └── verify-release/
│           ├── SKILL.md
│           └── scripts/
│               └── verify_release.py
└── ...
A v Devin Playbooks:

Implement GitHub Issue
Review Existing Implementation
Investigate Bug
Prepare Release
Architecture Research
Documentation Audit
Nepřeháněl bych počet. Na začátku vytvoř:

review-change

implement-issue

run-project-validation

create-rfc

Teprve když se nějaký postup skutečně opakuje, udělej z něj další skill.

11. První skill, který bych ti doporučil
---
name: run-project-validation
description: Run the repository's required validation before reporting task completion or creating a pull request. Use after modifying code, configuration, tests, or build files.
---

# Project Validation

## Preparation

1. Read the root `AGENTS.md`.
2. Inspect the changed files using `git status` and `git diff`.
3. Determine which components are affected.
4. Identify the authoritative test, lint, type-check, and build commands from:
   - `AGENTS.md`,
   - project documentation,
   - CI configuration,
   - package metadata.

Do not invent commands when authoritative project commands exist.

## Validation procedure

1. Run tests directly related to the changed components.
2. Run the repository-required static analysis.
3. Run type checking where configured.
4. Run formatting checks without silently reformatting unrelated files.
5. Run the relevant build or packaging validation.
6. Inspect the final diff for:
   - unrelated changes,
   - generated files,
   - secrets,
   - debug code,
   - disabled tests,
   - weakened assertions.

## Failure handling

When a validation step fails:

1. Determine whether the failure is caused by the current changes.
2. Fix failures caused by the current changes when within task scope.
3. Do not conceal, skip, or weaken a failing check.
4. Report pre-existing or environment-related failures separately.
5. Include the command and the material error output in the final report.

## Completion report

Report:

- validation commands executed,
- passed checks,
- failed checks,
- skipped checks and exact reasons,
- residual risks.

Do not claim that validation passed unless the relevant commands were actually executed successfully.
Použití:

@skills:run-project-validation Validate the current implementation before creating the PR.
12. Nejčastější chyby
Skill je jen obecná rada
Špatně:

Write clean code and test your changes.
To patří spíše do AGENTS.md.

Skill musí být proveditelná procedura:

1. Run command X.
2. Inspect output Y.
3. If condition Z occurs, stop and report.
Skill je obrovský
Skill o 1 000 řádcích bývá špatně. Rozděl jej například na:

add-provider
test-provider
review-provider
release-provider
Playbook obsahuje repo-specifické příkazy
Playbook:

Run uv run pytest tests/providers/openai
To je příliš specifické. Patří to do SimpleForge skillu.

Playbook by měl říct:

Invoke the repository's validation skill and execute all checks applicable to the changed component.
Duplicitní pravidla
Stejnou instrukci neopakuj v:

AGENTS.md,

třech skills,

Playbooku,

každém promptu.

Urči jedno autoritativní místo.

Neověřitelný completion report
Místo:

Ensure everything works.
použij:

List every command actually executed and its result.
Do not claim tests passed when they were not run.
Doporučený pracovní režim pro tebe
Pro každou práci používej tuto skladbu:

1. AGENTS.md
   Trvalé mantinely repozitáře.

2. Playbook
   Typ celé cloudové Devin session.

3. SKILL.md
   Ověřené repo-specifické postupy.

4. Konkrétní prompt
   Ticket, branch, cíl a výjimky.
Příklad kompletního zadání:

[Playbook: Implement GitHub Issue]

Implement GitHub issue #184 in pospanet/simpleforge.

Use @skills:add-provider when implementing the provider integration.
Use @skills:run-project-validation before creating the pull request.

Constraints:
- Keep the existing capability contract.
- Do not introduce a second abstraction for provider readiness.
- Do not modify unrelated routing behavior.
- Stop and report if the issue conflicts with an accepted RFC.

Create a PR, but do not merge it.
Tím získáš rozdělení odpovědností:

Playbook řídí session.

AGENTS.md chrání projekt.

Skills zajišťují opakovatelnost.

Prompt obsahuje pouze konkrétní úkol.


14ea730f-5451-43bb-9c5e-d13ee5bae4cc.png
kde to je? 

Na téhle obrazovce to není. Jsi v přehledu existujících sessions v režimu Agent.

A hlavně oprava k tomu, co jsem psal předtím: v Devin Desktop 3.4.27 nemáš na této stránce samostatnou globální nabídku „Skills“. SKILL.md se typicky používá až uvnitř otevřeného projektu/workspace, zatímco Playbook se vybírá při zakládání cloudové Devin session.

Kde najdeš Playbook
Klikni vlevo nahoře na:

+ New session

V novém dialogu pro cloudovou session by měla být možnost:

vybrat repozitář,

napsat zadání,

připojit Playbook z Team nebo Community knihovny.

Po připojení se u promptu objeví modrý štítek s názvem Playbooku. Playbook lze také přiložit jako soubor něco.devin.md. 

Správa a vytváření Playbooků nemusí být přímo v této Desktop obrazovce. Otevři webovou aplikaci Devin:

app.devin.ai → Settings / Playbooks

Podle současné dokumentace Devin podporuje vytváření, editaci a mazání Playbooků ve správě platformy. 

Kde najdeš SKILL.md
SKILL.md není centrální položka v tomto seznamu sessions. Musíš nejprve otevřít konkrétní lokální projekt:

Přepni nahoře vlevo z Agent na Editor.

Otevři složku svého repozitáře.

V levém stromu souborů vytvoř například:

.agents/
└── skills/
    └── review-change/
        └── SKILL.md
Tedy výsledná cesta:

.agents/skills/review-change/SKILL.md
Pak jej můžeš v agentním chatu vyvolat například:

Use the review-change skill to review the current branch.
nebo, podporuje-li konkrétní agent explicitní skill mention:

@skills:review-change Review the current branch.
Co máš udělat teď
Na tvém screenshotu:

Playbook: klikni na + New session.

SKILL.md: klikni nahoře na Editor, otevři projekt a vytvoř .agents/skills/.../SKILL.md.

Správa Playbooků: pravděpodobně přes webové app.devin.ai, nikoliv přes tento seznam sessions.

Tlačítko Editor je na screenshotu úplně vlevo nahoře, hned vedle aktivního Agent.


e52d46e3-eae0-4532-84e6-35a62bed20de.png

Teď už je vidět, proč Playbook nemůžeš najít: máš zvolenou lokální session.

Dole pod promptem je:

Local → TechCaseStudy_BlueDynamic

To znamená, že spouštíš lokálního agenta nad složkou v počítači. Na této obrazovce proto nevidíš nabídku týmových ani komunitních Playbooků.

Co můžeš použít právě zde
SKILL.md
Pro lokální session je to správný mechanismus. V otevřeném projektu TechCaseStudy_BlueDynamic vytvoř:

TechCaseStudy_BlueDynamic/
└── .agents/
    └── skills/
        └── review-project/
            └── SKILL.md
Devin Desktop/Cascade oficiálně vyhledává skills také v:

.agents/skills/
a uživatelské globální skills v:

~/.agents/skills/

Globální skill je vhodný, když jej chceš používat ve všech lokálních projektech:

C:\Users\<uživatel>\.agents\skills\review-project\SKILL.md
Repozitářový skill je vhodný, když patří pouze k danému projektu:

TechCaseStudy_BlueDynamic\.agents\skills\review-project\SKILL.md
Jak se dostat k Playbooku
Klikni dole na tlačítko:

Local

Tím by se měl otevřít výběr prostředí nebo typu session. Potřebuješ přepnout z lokální práce na cloudovou / managed Devin session, pokud ji tvoje licence a workspace nabízejí.

Případně klikni vpravo dole na:

Go to agent manager ↗

Agent Manager je vhodnější místo pro vytvoření spravované Devin session. U cloudové session lze Playbook vybrat z týmové nebo komunitní knihovny; správné připojení se projeví modrým štítkem u promptu. Alternativně lze při startu přiložit soubor s příponou .devin.md. 

Co znamenají prvky na screenshotu
Local – agent pracuje přímo v tvém lokálním adresáři.

TechCaseStudy_BlueDynamic – aktuálně připojená složka projektu.

Code – zvolený pracovní režim.

Claude Opus 4.6 – zvolený model.

+ vlevo v promptu – přidávání kontextu nebo příloh; není to přímo tlačítko pro správu skills.

Go to agent manager – přechod ke správě agentních/cloudových sessions.

Důležitý závěr
Pro tuto konkrétní obrazovku používej:

AGENTS.md
+
.agents/skills/<název>/SKILL.md
+
konkrétní prompt
Playbook zde nepotřebuješ a pravděpodobně se zde ani nenabízí, protože jde o Local session.

První praktický krok: otevři Editor, v projektu vytvoř .agents/skills/review-project/SKILL.md a potom v nové lokální session napiš:

Use the review-project skill to perform an independent review of this repository.
Do not modify files. Report findings by severity.

Today 7:58 PM
pojďme tedy vytvořit pár základních skills

Worked for 10s
Začneme čtyřmi univerzálními skills, které využiješ skoro v každém repozitáři:

explore-repository – bezpečné zmapování neznámého projektu

implement-task – řízená implementace změny

review-change – nezávislá revize kódu nebo dokumentace

validate-project – skutečné ověření před dokončením práce

Devin doporučuje ukládat je do:

.agents/skills/<skill-name>/SKILL.md
Skills pak automaticky objeví v připojeném repozitáři. Název a zejména description rozhodují o tom, kdy Devin skill automaticky použije. 

Kam je teď vytvořit
V projektu TechCaseStudy_BlueDynamic vytvoř tuto strukturu:

TechCaseStudy_BlueDynamic/
├── AGENTS.md
└── .agents/
    └── skills/
        ├── explore-repository/
        │   └── SKILL.md
        ├── implement-task/
        │   └── SKILL.md
        ├── review-change/
        │   └── SKILL.md
        └── validate-project/
            └── SKILL.md
Zatím je dáme do projektu, ne globálně. Můžeš je tak verzovat, upravovat podle projektu a sdílet přes Git.

1. explore-repository
Cesta:

.agents/skills/explore-repository/SKILL.md
Obsah:

---
name: explore-repository
description: Systematically inspect and explain an unfamiliar repository before planning or modifying it. Use when asked to understand, analyze, map, onboard to, or explain a repository, project, subsystem, architecture, or implementation.
---

# Explore Repository

## Objective

Build an evidence-based understanding of the repository without modifying it.

## Procedure

1. Read the root `AGENTS.md`.
2. Locate and read any nested `AGENTS.md` files applicable to inspected directories.
3. Inspect the repository structure.
4. Identify:
   - primary languages and frameworks,
   - application entry points,
   - major modules and boundaries,
   - build and dependency configuration,
   - test structure,
   - CI/CD configuration,
   - deployment configuration,
   - important documentation.
5. Read authoritative configuration files before drawing conclusions.
6. Trace the execution paths relevant to the user's request.
7. Distinguish:
   - verified facts,
   - reasonable inferences,
   - unresolved questions.
8. Do not modify files unless explicitly requested.

## Investigation rules

- Do not infer architecture solely from directory names.
- Do not describe components that were not inspected.
- Prefer source code and active configuration over potentially outdated documentation.
- Check whether documentation matches the current implementation.
- When multiple implementations exist, determine which one is active.
- Cite concrete files and symbols in the findings.
- State when a conclusion remains uncertain.

## Output

Report:

1. repository purpose,
2. technology stack,
3. major components,
4. execution or data flow,
5. test and build approach,
6. important architectural constraints,
7. risks and unclear areas,
8. recommended files to inspect next.

Do not claim comprehensive understanding unless all material parts were inspected.
Použití:

Use @skills:explore-repository to analyze this repository.

Focus on:
- its purpose,
- architecture,
- main execution flow,
- testing approach,
- current risks.

Do not modify any files.
2. implement-task
Cesta:

.agents/skills/implement-task/SKILL.md
Obsah:

---
name: implement-task
description: Plan and implement a bounded software or documentation task with minimal unrelated changes, appropriate tests, and explicit validation. Use when asked to add, change, fix, refactor, or complete functionality or documentation.
---

# Implement Task

## Objective

Implement the requested task completely while minimizing unintended scope and regression risk.

## Phase 1: Understand

1. Read the applicable `AGENTS.md` files.
2. Restate:
   - requested outcome,
   - acceptance criteria,
   - explicit constraints,
   - excluded scope.
3. Inspect the current implementation and relevant tests.
4. Trace affected execution paths and dependencies.
5. Identify ambiguities or conflicts before editing.
6. Do not assume that the requested solution is technically correct merely because it was suggested.

## Phase 2: Plan

Create a concise implementation plan covering:

- files expected to change,
- behavioral changes,
- test strategy,
- compatibility or migration implications,
- significant risks.

For a small and unambiguous task, proceed after presenting a brief plan.

Stop and ask for clarification only when an unresolved ambiguity would materially change the implementation or create significant risk.

## Phase 3: Implement

1. Make the smallest complete change satisfying the acceptance criteria.
2. Follow existing repository architecture and conventions.
3. Avoid unrelated refactoring, renaming, formatting, or dependency upgrades.
4. Preserve backward compatibility unless the task explicitly requires otherwise.
5. Add or update tests for changed behavior.
6. Handle relevant failure and boundary cases.
7. Do not weaken existing tests or checks.
8. Do not silently remove functionality.

## Phase 4: Review

Inspect the complete diff and verify:

- every change is necessary,
- no unrelated files changed,
- no debugging artifacts remain,
- no secret or sensitive data was introduced,
- error handling is appropriate,
- documentation reflects changed behavior,
- tests prove the intended outcome.

## Phase 5: Validate

Invoke the `validate-project` skill when available.

Do not claim that tests, builds, linting, or type checks passed unless they were actually executed successfully.

## Completion report

Provide:

- implementation summary,
- files changed,
- tests added or changed,
- validation commands and results,
- unresolved issues,
- residual risks.

Do not create, push, merge, or publish a commit or pull request unless explicitly requested.
Použití:

Use @skills:implement-task.

Task:
Add validation of the configuration input.

Acceptance criteria:
- invalid input is rejected,
- the error explains the invalid field,
- existing valid input remains compatible,
- tests cover valid and invalid cases.

Do not create a commit or PR.
3. review-change
Cesta:

.agents/skills/review-change/SKILL.md
Obsah:

---
name: review-change
description: Independently review a branch, diff, pull request, implementation, or documentation change for defects, regressions, security risks, missing validation, and requirement violations. Use when asked to review, audit, critique, verify, assess, or challenge completed work.
---

# Review Change

## Objective

Perform an independent, evidence-based review. Do not validate the author's conclusions by default.

## Preparation

1. Read the applicable `AGENTS.md` files.
2. Determine the review target:
   - working-tree changes,
   - current branch against its base,
   - specified commit range,
   - pull request,
   - named files or documents.
3. Identify the intended requirements and acceptance criteria.
4. Inspect the complete relevant diff.
5. Read surrounding code or documentation needed to understand the change.
6. Inspect tests and configuration affected by the change.

## Review dimensions

Evaluate:

- functional correctness,
- requirement coverage,
- regressions,
- boundary and failure cases,
- error handling,
- security and data exposure,
- concurrency and transaction behavior,
- compatibility,
- architectural consistency,
- maintainability,
- test adequacy,
- documentation accuracy,
- operational and deployment impact.

For documentation changes also evaluate:

- internal consistency,
- factual support,
- separation of facts and assumptions,
- traceability to source material,
- ambiguous or unverifiable claims,
- missing decisions and ownership.

## Finding requirements

Report only concrete findings supported by evidence.

Each finding must include:

- severity,
- location,
- problem,
- failure scenario or consequence,
- recommended correction.

Severity levels:

- **Critical** — likely catastrophic impact, security compromise, data loss, or unusable result.
- **Major** — material functional failure or substantial requirement violation.
- **Moderate** — meaningful defect with limited scope or a significant maintainability risk.
- **Minor** — localized issue with low immediate impact.
- **Observation** — relevant note that is not a confirmed defect.

## Rules

- Prioritize correctness over style preferences.
- Do not invent hypothetical findings without a realistic failure path.
- Do not modify reviewed files unless explicitly requested.
- Do not treat passing tests as proof that the implementation is correct.
- Check whether tests actually exercise the claimed behavior.
- Identify unsupported claims separately from confirmed defects.
- Explicitly state when no material defect was found.
- Include residual risks and areas that could not be verified.

## Output

1. Review scope
2. Overall assessment
3. Findings ordered by severity
4. Validation performed
5. Residual risks
6. Recommended disposition:
   - accept,
   - accept with minor corrections,
   - revise,
   - reject.
Použití:

Use @skills:review-change to perform an independent review of the current branch against main.

Do not modify files.

Focus especially on:
- requirement coverage,
- unsupported assumptions,
- architectural inconsistencies,
- missing tests.

Report findings by severity.
4. validate-project
Cesta:

.agents/skills/validate-project/SKILL.md
Obsah:

---
name: validate-project
description: Determine and execute the repository's applicable tests, builds, linting, type checks, and final diff inspection before reporting task completion. Use after changes and before completion, delivery, commit, or pull request creation.
---

# Validate Project

## Objective

Produce verifiable evidence that the changed project remains correct and deliverable.

## Discover authoritative commands

1. Read applicable `AGENTS.md` files.
2. Inspect:
   - project README and contributor documentation,
   - package and dependency configuration,
   - test configuration,
   - build scripts,
   - task runners,
   - CI workflows.
3. Determine the authoritative validation commands.
4. Prefer repository-defined commands over improvised equivalents.
5. Do not install or upgrade dependencies without justification and authorization.

## Select validation scope

Determine which checks apply based on the changed files.

Run, where configured and relevant:

- focused tests for changed components,
- broader regression tests,
- linting,
- formatting checks,
- type checking,
- build or compilation,
- packaging validation,
- documentation validation,
- security or dependency checks required by the repository.

Start with focused checks where this provides faster diagnostic feedback, then execute the required broader suite.

## Failure handling

When a command fails:

1. capture the command and material error,
2. determine whether the failure was:
   - caused by the current change,
   - pre-existing,
   - environment-related,
   - inconclusive,
3. fix failures caused by the current change when within scope,
4. rerun the affected validation,
5. do not skip, suppress, or weaken a check merely to obtain a passing result,
6. report unresolved failures explicitly.

## Final diff inspection

Inspect:

- `git status`,
- the complete relevant diff,
- new and untracked files.

Check for:

- unrelated changes,
- accidental formatting churn,
- generated artifacts,
- temporary or debug files,
- credentials and secrets,
- disabled checks or tests,
- weakened assertions,
- undocumented behavioral changes,
- changes outside the requested scope.

## Completion report

Use a table containing:

| Check | Command | Result | Notes |
|---|---|---|---|

Then report:

- checks not run and exact reasons,
- failures determined to be pre-existing,
- environment limitations,
- final diff assessment,
- residual risk.

Never use statements such as “all tests pass” unless the relevant test commands were actually executed and succeeded.
Použití:

Use @skills:validate-project to validate the current working-tree changes.

Do not change application behavior merely to make validation pass.
Fix only defects introduced by the current task.
Jak je nechat vytvořit přímo Devinem
V aktuálním projektu spusť novou Local session a vlož tento prompt:

Create the following repository skills under `.agents/skills/`:

1. `explore-repository`
2. `implement-task`
3. `review-change`
4. `validate-project`

Each skill must have its own directory and a `SKILL.md` file.

Use the exact contents supplied below without shortening, paraphrasing, or combining the skills.

Before writing:
- verify whether any of these files already exist,
- do not overwrite existing content without reporting it,
- preserve the existing root `AGENTS.md`,
- do not modify any other files.

After writing:
- list all created files,
- validate the YAML frontmatter,
- validate that each directory name matches the skill `name`,
- show `git diff --stat`,
- do not commit or push.
Pod něj vlož čtyři obsahy výše.

Doporučený režim v Devin Desktop
Pro pouhé vytvoření těchto markdownových souborů:

Session: nová Local session

Mode: Code

Model: levnější model než Opus, například dostupný Sonnet

Worktree: není nutný, protože změna je malá a snadno kontrolovatelná

Commit: zatím nevytvářet

Jak ověřit, že Devin skill vidí
Po vytvoření založ novou session. Skills nemusí být spolehlivě znovu načteny do již běžící session.

Do nového promptu napiš @ a zkontroluj, zda se objeví sekce Skills. Devin oficiálně podporuje referenci skills přes @Skills; automaticky je také vybírá podle názvu a popisu. 

První test:

Use @skills:explore-repository.

Analyze this repository and explain:
- its purpose,
- architecture,
- important files,
- test strategy,
- unresolved risks.

Do not modify files.
Druhý test:

Use @skills:review-change.

Review the current uncommitted changes.
Do not modify them.
Za touto základní sadou bych pro tvoji práci doplnil ještě dva specializované skills: review-documentation a create-authoritative-document. Ty dávají smysl vzhledem k tomu, že Devinem často vytváříš architektonické, governance a business dokumenty.


Já je vytvořil ručně. Mám i tak spustit ten prompt na jejich vytvoření?

Ne. Když jsi je vytvořil ručně, prompt na vytvoření už nespouštěj. Riskoval bys zbytečné přepsání nebo duplicitní změny.

Spusť jen ověřovací prompt:

Validate the repository skills under `.agents/skills/`.

Check:

1. Each skill has its own directory.
2. Each directory contains `SKILL.md`.
3. Each `SKILL.md` has valid YAML frontmatter.
4. The frontmatter contains:
   - `name`
   - `description`
5. The `name` exactly matches the directory name.
6. There are no duplicate skill names.
7. The descriptions clearly state:
   - what the skill does,
   - when it should be used.
8. Markdown structure is valid and readable.
9. References to other skills or files point to existing paths.
10. No unrelated files were modified.

Do not modify any files.

Report:
- discovered skills,
- validation results per skill,
- warnings,
- recommended corrections,
- whether the skills appear ready for use in a new Devin session.
Potom vytvoř novou Local session a otestuj jeden skill explicitně, například:

Use @skills:explore-repository.

Analyze this repository and explain:
- its purpose,
- architecture,
- main components,
- test strategy,
- unresolved risks.

Do not modify files.
Když se @skills:explore-repository v autocomplete neobjeví, ještě to samo o sobě nemusí znamenat problém. Zkus přímý text:

Use the `explore-repository` skill from `.agents/skills/explore-repository/SKILL.md`.
Důležité je testovat v nově založené session, protože již otevřená session nemusela nově přidané skills načíst.


Připrav mi tabulku kde budou následující modely: Claude opus 4.6, Claude sonnet 4.6, Claude haiku 4.6, DeepSeek V4 Pro, Gemini 3.5 Flash, Gemini 3.1 Pro, GLM-5.2, GPT-5.4, GPT-5.1-codex, Kimi K2.6, Kimi 2.7, Swe-1.6   Ohodnoť je a doporuč na co se hodí. Takový cheet sheet

Níže je praktický cheat sheet pro volbu modelu v Devin Desktop, zaměřený hlavně na programování, review, analýzu architektury a práci s dokumentací.

Hodnocení je relativní v rámci této skupiny:

5/5 = špička

1/5 = slabé nebo nevhodné

„Efektivita“ znamená kombinaci rychlosti, spotřeby kreditů/tokenů a dosažené kvality.

Výsledek výrazně ovlivňuje Devinův agentní harness, kvalita zadání, AGENTS.md a skills; nejde o absolutní benchmark.

Modelový cheat sheet
Model	Kód	Analýza / architektura	Rychlost	Efektivita	Nejlepší použití	Hlavní slabina
Claude Opus 4.6	5	5	2	2	Nejtěžší review, architektura, hledání skrytých vad, rozsáhlé refaktory, kritická rozhodnutí	Pomalý a drahý; u jednoduchých úloh zbytečný
Claude Sonnet 4.6	5	4.5	4	4	Výchozí model pro většinu implementací, review, debugging a dokumentaci	U extrémně složitých a nejasných úloh může být méně důsledný než Opus
Claude Haiku 4.6*	3	2.5	5	5	Drobné změny, mechanické úpravy, hledání v repu, jednoduché testy a dokumentace	Slabší hluboké uvažování a dlouhé autonomní úlohy
DeepSeek V4 Pro	4.5	4.5	3	5	Cenově efektivní hlubší coding, rozsáhlý kontext, agentní úlohy, druhý názor	Méně předvídatelný styl a dodržení jemných instrukcí než Claude/GPT
Gemini 3.5 Flash	4	4	5	4.5	Rychlý průzkum repa, velké množství souborů, agentní coding, iterace a multimodální vstupy	U jemných architektonických rozhodnutí může být příliš rychlý a sebejistý
Gemini 3.1 Pro	4.5	4.5	3	3.5	Velký kontext, komplexní analýza, software engineering, přesná práce s nástroji	Pomalejší než Flash; jde o preview model
GLM-5.2	4.5	4.5	2.5	4.5	Dlouhodobé agentní úlohy, rozsáhlé systémy, open-source alternativní review	Nižší provozní stabilita a předvídatelnost než hlavní západní modely
GPT-5.4	5	5	3	3.5	Komplexní coding, precizní analýza, business a technické dokumenty, tool use	Pro běžnou implementaci může být pomalejší a dražší než Sonnet
GPT-5.1-Codex	4	3.5	3.5	3.5	Agentní implementace, opravy a práce v terminálu	Starší a dnes již překonaný model; OpenAI jej označuje jako deprecated
Kimi K2.6	4.5	4	3	5	Coding, dlouhé autonomní běhy, frontend, multimodalita, multi-agentní úlohy	Výstupy mohou potřebovat silnější finální review
Kimi K2.7 Code	5	4	4	5	Coding-first úlohy, rozsáhlé změny, dlouhý kontext, implementace issue end-to-end	Je specializovaný na kód; není první volbou pro business analýzu
SWE-1.6	3.5	2.5	5	5	Rychlé lokální úpravy, repo navigace, rutinní implementace a testy	Předchozí generace; slabší strategické uvažování a kritické review
* Claude Haiku 4.6: v aktuálních veřejných materiálech Anthropicu jsem nenašel oficiální model s tímto názvem. Anthropic dokumentuje Claude Haiku 4.5. Jestli Devin zobrazuje „Haiku 4.6“, může jít o vlastní alias, rollout nebo označení v uživatelském rozhraní; zacházel bych s ním jako s rychlým malým modelem, nikoli jako s ekvivalentem Sonnetu 4.6. 

Moje doporučení pro Devin Desktop
Výchozí model: Claude Sonnet 4.6
Pro přibližně 70 % tvých úloh bych volil:

Claude Sonnet 4.6

Anthropic jej pozicuje jako univerzální model pro coding, agenty, computer use a profesionální workflow. Nabízí nejlepší praktický kompromis mezi kvalitou, rychlostí a cenou. 

Používej ho na:

implementaci issue,

opravy bugů,

středně velké refaktory,

vytváření a úpravu AGENTS.md,

tvorbu skills,

dokumentaci,

standardní code review,

návrh testů,

práci s několika desítkami souborů.

Nejtěžší úlohy: Claude Opus 4.6 nebo GPT-5.4
Claude Opus 4.6
Vyber jej, když je nejdůležitější:

kritické nezávislé review,

hledání skrytých systémových problémů,

nejasná architektura,

rozsáhlý legacy systém,

security review,

posouzení práce jiného modelu,

rozhodnutí s vysokými následky.

Opus 4.6 byl přímo navržen pro komplexní coding, agentní práci a hluboké dlouhodobé uvažování. Anthropic uváděl výrazné zlepšení například při komplexních kyberbezpečnostních vyšetřováních. 

GPT-5.4
Vyber jej, když se kombinuje:

složitý technický problém,

práce s nástroji,

coding,

dokumentace,

business nebo produktové rozhodování,

potřeba velmi strukturovaného výsledku.

OpenAI jej popisuje jako frontier model pro komplexní profesionální práci, s kontextovým oknem přes jeden milion tokenů a nastavitelnou úrovní reasoning effort. 

Praktický rozdíl
Potřeba	Doporučený model
Najít skryté vady a zpochybnit návrh	Opus 4.6
Vyřešit komplexní úlohu a vytvořit precizní výstup	GPT-5.4
Dlouhé nezávislé review dokumentů a architektury	Opus 4.6
Kombinace kódu, tabulek, analýzy a dokumentace	GPT-5.4
Nejlepší levnější coding modely
Kimi K2.7 Code
Velmi zajímavá volba pro:

implementaci většího issue,

dlouhodobé agentní coding úlohy,

frontend,

rozsáhlé změny v repozitáři,

samostatné dokončení implementace a testů.

Moonshot jej označuje jako svůj nejsilnější coding model. Má 256k kontext a proti K2.6 údajně zlepšuje dlouhodobé agentní coding úlohy a snižuje spotřebu reasoning tokenů přibližně o 30 %. 

Verdikt: pravděpodobně nejlepší kandidát na levnější alternativu k Sonnetu pro čisté programování.

DeepSeek V4 Pro
Dobrá volba pro:

dlouhý kontext,

komplexní reasoning,

agentní práci,

rozsáhlou analýzu kódu,

nezávislý druhý názor.

DeepSeek V4 Pro podporuje thinking i non-thinking režim, tool calls, JSON output a kontext až 1 milion tokenů. 

Verdikt: velmi dobrá hodnota, ale finální změny bych ještě nechal zkontrolovat Sonnetem, Opusem nebo GPT.

GLM-5.2
Určen zejména pro:

dlouhodobé autonomní úlohy,

rozsáhlé systémy,

agentic engineering,

větší refaktory,

otevřenější experimentální workflow.

Z.ai jej výslovně staví jako model pro long-horizon tasks a agentic engineering. 

Verdikt: silný experimentální model, ale zatím bych ho nepoužíval bez závěrečného review na kritické změny.

Rychlé a rutinní práce
SWE-1.6
Vhodný pro:

navigaci v repozitáři,

jednoduché úpravy,

vytvoření test skeletonu,

přejmenování symbolů,

mechanické refaktory,

opravu lint chyb,

menší změny dokumentace.

Cognition jej označuje jako předchozí generaci svého software-engineering modelu; současnou generací už je SWE-1.7. 

Verdikt: použij tam, kde je zadání přesné a snadno ověřitelné testy.

Gemini 3.5 Flash
Výborný pro:

rychlé čtení velkého repozitáře,

analýzu mnoha souborů,

první implementační pokus,

generování variant,

frontend a multimodální vstupy,

rychlé iterace.

Google jej označuje jako svůj nejinteligentnější Flash model a model určený mimo jiné pro agentní a coding úlohy. 

Verdikt: velmi dobrý rychlý pracovní model, ale u high-stakes review bych použil hlubší model.

Modely, které bych dnes volil méně často
GPT-5.1-Codex
Byl navržen přímo pro agentní coding a má 400k kontext. OpenAI jej však nyní uvádí jako deprecated; novější generace jej překonaly. 

Použij ho jen když:

se v konkrétním Devin workflow chová stabilněji než novější model,

má výrazně nižší kreditní cenu,

řešíš standardní coding bez potřeby nejvyšší kvality.

Jinak preferuj:

Sonnet 4.6 → Kimi K2.7 Code → GPT-5.4

Kimi K2.6
Je stále silný univerzální multimodální a coding model, podporuje thinking i non-thinking práci a agentní scénáře. K2.7 Code je však pro čisté programování novější a specializovanější. 

Používej K2.6 místo K2.7, když potřebuješ:

obrazový vstup,

obecnou analýzu mimo kód,

kombinaci dokumentace, výzkumu a implementace.

Praktická rozhodovací tabulka
Typ úlohy	První volba	Levnější / rychlejší alternativa	Finální kontrola
Malá oprava s jasným zadáním	SWE-1.6	Haiku / Gemini Flash	Sonnet jen podle rizika
Standardní implementace	Sonnet 4.6	Kimi K2.7 Code	Sonnet 4.6
Velký refaktor	Sonnet 4.6	Kimi K2.7 / DeepSeek V4 Pro	Opus 4.6
Neznámý rozsáhlý repozitář	Gemini 3.5 Flash	DeepSeek V4 Pro	Sonnet 4.6
Architektonický návrh	Opus 4.6	GPT-5.4 / Gemini 3.1 Pro	Druhý nezávislý model
Kritický code review	Opus 4.6	GPT-5.4	Případně oba
Security review	Opus 4.6	GPT-5.4	Ruční ověření
Business + technická analýza	GPT-5.4	Sonnet 4.6	Opus při sporných závěrech
Dokumentace / AGENTS.md	Sonnet 4.6	GPT-5.4	Opus pro governance review
Frontend a UI	Kimi K2.7 Code	Gemini 3.5 Flash	Sonnet 4.6
Dlouhá autonomní agentní úloha	Kimi K2.7 / GLM-5.2	DeepSeek V4 Pro	Opus nebo Sonnet
Mechanická změna ve stovkách souborů	SWE-1.6	Gemini Flash	Sonnet nad výsledným diffem
Moje jednoduché pravidlo pro tebe
Používej tento výběr:

Běžná práce:
Claude Sonnet 4.6

Nejtěžší analýza nebo nezávislé review:
Claude Opus 4.6

Komplexní kombinace kódu, analýzy a dokumentace:
GPT-5.4

Levnější rozsáhlá implementace:
Kimi K2.7 Code

Rychlý průzkum a jednoduchá práce:
Gemini 3.5 Flash nebo SWE-1.6

Druhý názor:
DeepSeek V4 Pro nebo GLM-5.2
Pro tvoje současné vytváření a ověřování SKILL.md bych použil:

vytvoření nebo úprava skillu: Sonnet 4.6,

rychlá formální validace: SWE-1.6,

kritické posouzení celé skill architektury: Opus 4.6,

běžné používání skillu při implementaci: Sonnet 4.6 nebo Kimi K2.7 Code.


Ano, je to haiku 4.5 uprav cheet sheet a dej mi ho jako MD5

A pod to ještě jeden cheat sheet. kde bude uloha a pak jaké modely a i jestli vícero průchodů. Jako třeba Analýcha: prvotní, review, komp=életace. a Atakle pro všechny běžné práce nad kódem. Budou to dva separátní md files


Analyzed
from pathlib import Path

out_dir = Path("/mnt/data")
out_dir.mkdir(parents=True, exist_ok=True)

models_md = """# Devin Desktop – Model Cheat Sheet

> Praktické hodnocení pro práci nad repozitářem v Devin Desktop.  
> Škála: **5 = špička**, **1 = slabé**.  
> Hodnocení je relativní a orientační; výsledek ovlivňuje kvalita zadání, `AGENTS.md`, skills, velikost repozitáře a možnost ověřit výsledek testy.

## Rychlá volba

- **Výchozí model pro většinu práce:** Claude Sonnet 4.6
- **Nejtěžší analýza a kritické review:** Claude Opus 4.6
- **Komplexní kombinace kódu, nástrojů a dokumentace:** GPT-5.4
- **Rychlé a levné rutinní úlohy:** Claude Haiku 4.5, Gemini 3.5 Flash, SWE-1.6
- **Levnější dlouhá implementace:** Kimi K2.7, DeepSeek V4 Pro
- **Velký kontext a alternativní druhý názor:** Gemini 3.1 Pro, GLM-5.2

## Srovnávací tabulka

| Model | Kód | Debugging | Architektura | Review | Dokumentace | Rychlost | Efektivita | Nejvhodnější použití | Hlavní omezení |
|---|---:|---:|---:|---:|---:|---:|---:|---|---|
| **Claude Opus 4.6** | 5 | 5 | 5 | 5 | 5 | 2 | 2 | Kritické review, složitá architektura, bezpečnost, rozsáhlé legacy systémy, hledání skrytých vad | Pomalý a drahý; pro rutinní práci zbytečný |
| **Claude Sonnet 4.6** | 5 | 4.5 | 4.5 | 4.5 | 4.5 | 4 | 4.5 | Výchozí model pro implementaci, debugging, běžné review, refaktoring a technickou dokumentaci | U nejasných high-stakes úloh nemusí být tak důsledný jako Opus |
| **Claude Haiku 4.5** | 3.5 | 3 | 2.5 | 2.5 | 3.5 | 5 | 5 | Malé opravy, mechanické úpravy, navigace v repu, jednoduché testy, rychlé shrnutí | Slabší hluboké uvažování a dlouhé autonomní úlohy |
| **DeepSeek V4 Pro** | 4.5 | 4.5 | 4.5 | 4 | 3.5 | 3 | 4.5 | Cenově efektivní implementace, dlouhý kontext, komplexní debugging, druhý názor | Méně konzistentní dodržení jemných instrukcí; vhodné finální review |
| **Gemini 3.5 Flash** | 4 | 4 | 3.5 | 3.5 | 4 | 5 | 5 | Rychlý průzkum velkého repa, klasifikace, první implementační průchod, multimodální vstupy | U jemných návrhových rozhodnutí může být příliš sebejistý |
| **Gemini 3.1 Pro** | 4.5 | 4.5 | 4.5 | 4.5 | 4.5 | 3 | 3.5 | Velký kontext, komplexní analýza, návrh systému, práce s mnoha soubory | Pomalejší a nákladnější než Flash |
| **GLM-5.2** | 4.5 | 4 | 4.5 | 4 | 3.5 | 3 | 4.5 | Dlouhé agentní úlohy, větší systémy, experimentální alternativní implementace a review | Nižší předvídatelnost než Claude/GPT; požaduje silnou validaci |
| **GPT-5.4** | 5 | 5 | 5 | 5 | 5 | 3 | 3.5 | Komplexní profesionální práce, coding + analýza + dokumentace, tool use, přesná syntéza | Pro běžnou práci dražší a pomalejší než Sonnet |
| **GPT-5.1-Codex** | 4 | 4 | 3.5 | 3.5 | 3 | 3.5 | 3 | Agentní coding, práce v terminálu, opravy a implementace s jasnými testy | Starší/deprecated generace; používat jen pokud je v Devinovi výhodný nebo stabilní |
| **Kimi K2.6** | 4.5 | 4 | 4 | 3.5 | 4 | 3.5 | 4.5 | Dlouhé autonomní běhy, coding, multimodalita, kombinace kódu a obecné analýzy | Finální výsledek je vhodné nezávisle zkontrolovat |
| **Kimi K2.7** | 5 | 4.5 | 4 | 4 | 3.5 | 4 | 5 | Coding-first úlohy, větší implementace, frontend, dlouhé změny napříč repem | Méně vhodný pro business a governance dokumenty |
| **SWE-1.6** | 3.5 | 3.5 | 2.5 | 2.5 | 3 | 5 | 5 | Rychlé lokální úpravy, repo navigace, rutinní implementace, test skeletony, lint opravy | Slabší strategické uvažování, architektura a kritické review |

## Doporučení podle typu práce

| Typ práce | První volba | Levnější / rychlejší alternativa | Doporučené finální review |
|---|---|---|---|
| Malá oprava s jasným zadáním | Claude Haiku 4.5 | SWE-1.6 | Sonnet podle rizika |
| Standardní implementace | Claude Sonnet 4.6 | Kimi K2.7 | Sonnet 4.6 |
| Složitý debugging | Claude Sonnet 4.6 | DeepSeek V4 Pro | Opus 4.6 |
| Velký refaktor | Claude Sonnet 4.6 | Kimi K2.7 | Opus 4.6 |
| Architektonický návrh | Claude Opus 4.6 | GPT-5.4 / Gemini 3.1 Pro | Jiný frontier model |
| Kritický code review | Claude Opus 4.6 | GPT-5.4 | Případně oba nezávisle |
| Security review | Claude Opus 4.6 | GPT-5.4 | Ruční ověření a nástroje |
| Rychlý průzkum repozitáře | Gemini 3.5 Flash | SWE-1.6 | Sonnet 4.6 |
| Technická dokumentace | Claude Sonnet 4.6 | GPT-5.4 | Opus pro governance |
| Dlouhá autonomní implementace | Kimi K2.7 | DeepSeek V4 Pro / GLM-5.2 | Sonnet nebo Opus |
| Mechanická změna ve velkém počtu souborů | SWE-1.6 | Haiku 4.5 | Sonnet nad diffem |
| Business + technická syntéza | GPT-5.4 | Sonnet 4.6 | Opus při sporných závěrech |

## Praktické pravidlo

```text
Rutinní a levná práce:
Haiku 4.5 / Gemini 3.5 Flash / SWE-1.6

Běžná produkční práce:
Claude Sonnet 4.6

Nejtěžší analýza nebo nezávislé review:
Claude Opus 4.6

Komplexní kombinace kódu, nástrojů a dokumentace:
GPT-5.4

Levnější dlouhá implementace:
Kimi K2.7 / DeepSeek V4 Pro

Alternativní druhý názor:
Gemini 3.1 Pro / GLM-5.2
Poznámky
GPT-5.1-Codex je starší model; nepoužívej jej jako automatickou první volbu, pokud jsou dostupné novější alternativy.

U modelů s nižší předvídatelností odděluj implementaci a review do různých sessions.

Kritické změny neposuzuj pouze podle toho, že testy prošly. Review musí ověřit také požadavky, rizika a správnost testů.
"""

workflow_md = """# Devin Desktop – Workflow Cheat Sheet pro práci nad kódem

Cíl: zvolit model pro jednotlivé průchody, nikoli jeden model pro celý problém.
Pro důležitější práci používej oddělené sessions, aby reviewer nebyl ovlivněn vlastním předchozím řešením.

Základní princip
Doporučený víceprůchodový vzor:

1. Průzkum / analýza
2. Plán
3. Implementace
4. Validace
5. Nezávislé review
6. Opravy
7. Finální kontrola a kompletace
Ne každá úloha potřebuje všech sedm kroků. Čím vyšší je riziko změny, tím více kroků odděluj.

Přehled běžných workflow
Úloha	Průchod	Doporučený model	Alternativa	Samostatná session?	Poznámka
Analýza neznámého repozitáře	1. Rychlé zmapování	Gemini 3.5 Flash	SWE-1.6	Ano	Struktura, technologie, entry points, testy
2. Hluboká analýza	Claude Sonnet 4.6	Gemini 3.1 Pro	Ano	Ověřit skutečné execution paths
3. Kritické review závěrů	Claude Opus 4.6	GPT-5.4	Ano	Hledat nepodložené závěry
4. Kompletace reportu	GPT-5.4	Sonnet 4.6	Volitelně	Sloučit jen ověřené závěry
Návrh nové funkce	1. Analýza dopadu	Claude Sonnet 4.6	Gemini 3.1 Pro	Ano	Rozhraní, data, kompatibilita, testy
2. Návrh variant	GPT-5.4	Claude Opus 4.6	Ano	Porovnat minimálně 2 realistické varianty
3. Review návrhu	Claude Opus 4.6	GPT-5.4	Ano	Nechat jiný model návrh zpochybnit
4. Finalizace plánu	Claude Sonnet 4.6	GPT-5.4	Volitelně	Jasná acceptance criteria a scope
Standardní implementace	1. Průzkum a plán	Claude Sonnet 4.6	Gemini 3.5 Flash	Může být stejná	U středních úloh plán před editací
2. Implementace	Claude Sonnet 4.6	Kimi K2.7	Může být stejná	Nejmenší úplná změna
3. Validace	SWE-1.6	Haiku 4.5	Spíše stejná	Spustit repo-defined testy a kontrolu diffu
4. Nezávislé review	Claude Opus 4.6	GPT-5.4	Ano	Reviewer nesmí opravovat během prvního průchodu
5. Opravy	Claude Sonnet 4.6	Kimi K2.7	Nová nebo původní	Opravit pouze potvrzená zjištění
Malá, jasně ohraničená oprava	1. Implementace	Claude Haiku 4.5	SWE-1.6	Ne	Vhodné jen při jasném failure scenario
2. Test a diff	SWE-1.6	Gemini 3.5 Flash	Ne	Povinné cílené testy
3. Review podle rizika	Claude Sonnet 4.6	—	Jen vyšší riziko	U triviality lze vynechat
Složitý bug	1. Reprodukce a evidence	Claude Sonnet 4.6	DeepSeek V4 Pro	Ano	Nejprve potvrdit bug, nehádat příčinu
2. Root-cause analýza	Claude Opus 4.6	GPT-5.4	Ano	Trace execution/data flow
3. Návrh opravy	Claude Sonnet 4.6	GPT-5.4	Může navázat	Minimalizovat regresní plochu
4. Implementace	Claude Sonnet 4.6	Kimi K2.7	Ano	Přidat regresní test
5. Nezávislé review	Claude Opus 4.6	GPT-5.4	Ano	Ověřit root cause i test
Velký refaktor	1. Baseline a invarianty	Claude Opus 4.6	GPT-5.4	Ano	Co se nesmí změnit
2. Migrační plán	Claude Sonnet 4.6	Gemini 3.1 Pro	Ano	Fáze, rollback, compatibility
3. Implementace po částech	Claude Sonnet 4.6	Kimi K2.7	Více sessions	Každá část musí být samostatně validovatelná
4. Průběžná validace	SWE-1.6	Haiku 4.5	V každé části	Testy, typecheck, build, diff
5. Architektonické review	Claude Opus 4.6	GPT-5.4	Ano	Ověřit, že nevznikla druhá abstrakce
6. Finální regresní review	GPT-5.4	Opus 4.6	Ano	Celý výsledný diff
Code review / PR review	1. Zmapování změny	Gemini 3.5 Flash	SWE-1.6	Ano	Diff, požadavky, dotčené cesty
2. Hluboké review	Claude Opus 4.6	GPT-5.4	Ano	Konkrétní failure scenarios
3. Druhý názor u high-risk PR	GPT-5.4	Claude Sonnet 4.6	Ano	Nezávisle, bez znalosti prvního verdiktu
4. Konsolidace findings	Claude Sonnet 4.6	GPT-5.4	Ano	Odstranit duplicity a hypotetické vady
Security review	1. Threat model	Claude Opus 4.6	GPT-5.4	Ano	Assets, boundaries, attacker paths
2. Statická kontrola změn	GPT-5.4	Claude Opus 4.6	Ano	Auth, input, secrets, injection, permissions
3. Nástrojová validace	SWE-1.6	Haiku 4.5	Ano	Spustit existující security tooling
4. Nezávislá revize	Claude Opus 4.6	GPT-5.4	Ano	Povinná u citlivých změn
Testování existující implementace	1. Test-gap analýza	Claude Sonnet 4.6	GPT-5.4	Ano	Co testy skutečně dokazují
2. Tvorba testů	Claude Sonnet 4.6	Kimi K2.7	Může navázat	Boundary, failure, regression
3. Mutace / adversarial review	Claude Opus 4.6	DeepSeek V4 Pro	Ano	Hledat falešně pozitivní testy
4. Validace	SWE-1.6	Haiku 4.5	Ano	Spustit focused i širší suite
Performance optimalizace	1. Baseline měření	Claude Sonnet 4.6	SWE-1.6	Ano	Bez baseline neoptimalizovat
2. Bottleneck analýza	Claude Opus 4.6	GPT-5.4	Ano	Profiling evidence, ne intuice
3. Implementace	Claude Sonnet 4.6	Kimi K2.7	Ano	Jedna změna po druhé
4. Benchmark a regresní testy	SWE-1.6	Gemini 3.5 Flash	Ano	Porovnat před/po
5. Review trade-offů	GPT-5.4	Opus 4.6	Ano	Latence, paměť, složitost, náklady
API změna	1. Contract analýza	Claude Opus 4.6	GPT-5.4	Ano	Callers, compatibility, versioning
2. Implementace	Claude Sonnet 4.6	Kimi K2.7	Ano	Zachovat kontrakt nebo řídit migraci
3. Contract testy	Claude Sonnet 4.6	DeepSeek V4 Pro	Ano	Valid/invalid/boundary
4. Review kompatibility	GPT-5.4	Opus 4.6	Ano	Povinné u public API
Databázová migrace	1. Dopad a data model	Claude Opus 4.6	GPT-5.4	Ano	Locking, rollback, objem dat
2. Migrační plán	GPT-5.4	Claude Sonnet 4.6	Ano	Expand/migrate/contract
3. Implementace	Claude Sonnet 4.6	Kimi K2.7	Ano	Žádné destruktivní kroky bez schválení
4. Review a rollback test	Claude Opus 4.6	GPT-5.4	Ano	High-risk workflow
Dependency upgrade	1. Release-impact analýza	Gemini 3.1 Pro	Claude Sonnet 4.6	Ano	Breaking changes, security, transitive deps
2. Upgrade	Claude Sonnet 4.6	Kimi K2.7	Ano	Minimalizovat současné změny
3. Build/test validace	SWE-1.6	Gemini 3.5 Flash	Ano	Celá relevantní pipeline
4. Review regresí	Claude Opus 4.6	GPT-5.4	Ano	Zejména runtime změny
CI/CD změna	1. Analýza pipeline	Claude Sonnet 4.6	Gemini 3.1 Pro	Ano	Permissions, secrets, triggers, artifacts
2. Implementace	Claude Sonnet 4.6	Kimi K2.7	Ano	Co nejmenší změna
3. Security review	Claude Opus 4.6	GPT-5.4	Ano	Supply chain a permission scope
4. Validace	SWE-1.6	Haiku 4.5	Ano	Syntax + skutečný běh, je-li dostupný
Dokumentace kódu / ADR / RFC	1. Extrakce faktů	Gemini 3.5 Flash	SWE-1.6	Ano	Číst implementaci a aktivní konfiguraci
2. Návrh dokumentu	Claude Sonnet 4.6	GPT-5.4	Ano	Oddělit fakta, rozhodnutí a návrhy
3. Governance review	Claude Opus 4.6	GPT-5.4	Ano	Konzistence a vymahatelnost
4. Finalizace	GPT-5.4	Sonnet 4.6	Ano	Odstranit duplicity a rozpory
Tvorba nebo úprava AGENTS.md / skills	1. Audit repozitáře	Claude Sonnet 4.6	Gemini 3.5 Flash	Ano	Odvodit pravidla z reality repa
2. Návrh	Claude Sonnet 4.6	GPT-5.4	Ano	Konkrétní, ověřitelné instrukce
3. Kritické review	Claude Opus 4.6	GPT-5.4	Ano	Konflikty, duplicity, nebezpečná oprávnění
4. Finalizace	Claude Sonnet 4.6	GPT-5.4	Ano	Jedno autoritativní místo pro pravidlo
Kolik průchodů použít podle rizika
Riziko	Typická změna	Doporučené průchody
Nízké	Typo, malé README, lokalizovaná mechanická změna	Implementace → validace
Střední	Běžný bugfix, menší feature, interní API	Analýza → implementace → validace → review
Vysoké	Public API, auth, data, migrace, concurrency, větší refaktor	Analýza → návrh → review návrhu → implementace → validace → nezávislé review → opravy → finální review
Kritické	Security, produkční data, finanční logika, přístupová práva, nevratná migrace	Minimálně dva nezávislé frontier modely + nástrojová a lidská kontrola
Doporučené kombinace modelů
Úsporná
Průzkum: Gemini 3.5 Flash
Implementace: Kimi K2.7 nebo Sonnet 4.6
Validace: SWE-1.6
Review: Sonnet 4.6
Vyvážená
Analýza a plán: Sonnet 4.6
Implementace: Sonnet 4.6
Validace: SWE-1.6
Nezávislé review: Opus 4.6
Opravy: Sonnet 4.6
High-stakes
První analýza: Opus 4.6
Alternativní návrh / druhý názor: GPT-5.4
Implementace: Sonnet 4.6
Nástrojová validace: SWE-1.6
Nezávislé review: Opus 4.6 nebo GPT-5.4, jiný než autor návrhu
Finální kompletace: GPT-5.4
Pravidla pro více průchodů
Review prováděj v nové session.

Reviewerovi dej požadavky, diff a relevantní kontext, ale nevnucuj mu závěry implementátora.

Nepoužívej stejný model jako jediného autora i jediného reviewera u high-risk změn.

Levný model používej na mechanickou práci a validaci, nikoli jako jediný zdroj architektonického rozhodnutí.

Finální report musí rozlišovat:

co bylo skutečně ověřeno,

co je inference,

co nebylo možné ověřit.

Procházej celý výsledný diff, ne pouze poslední opravené soubory.

Testy jsou důkazem pouze toho, co skutečně pokrývají.
"""

file1 = out_dir / "devin-model-cheat-sheet.md"
file2 = out_dir / "devin-code-workflow-cheat-sheet.md"
file1.write_text(models_md, encoding="utf-8")
file2.write_text(workflow_md, encoding="utf-8")

print(file1)
print(file2)

STDOUT/STDERR
/mnt/data/devin-model-cheat-sheet.md
/mnt/data/devin-code-workflow-cheat-sheet.md
Hotovo. Opravil jsem Claude Haiku na 4.5 a rozdělil výstup do dvou samostatných Markdown souborů:


Srovnání všech modelů, bodové hodnocení, silné stránky, omezení a rychlá volba podle typu práce.


Úlohy nad kódem rozdělené na jednotlivé průchody: analýza, návrh, implementace, validace, nezávislé review, opravy a kompletace. Obsahuje i úsporný, vyvážený a high-stakes workflow.

Hodnocení je praktické a relativní pro použití v Devin Desktop; dostupnost modelů a jejich ceny se mohou v Devinovi měnit. Devin průběžně aktualizuje seznam dostupných modelů a Anthropic oficiálně uvádí model Claude Haiku 4.5. 
 


Library
/
devin-model-cheat-sheet.md


# Devin Desktop – Model Cheat Sheet

> Praktické hodnocení pro práci nad repozitářem v Devin Desktop.  
> Škála: **5 = špička**, **1 = slabé**.  
> Hodnocení je relativní a orientační; výsledek ovlivňuje kvalita zadání, `AGENTS.md`, skills, velikost repozitáře a možnost ověřit výsledek testy.

## Rychlá volba

- **Výchozí model pro většinu práce:** Claude Sonnet 4.6
- **Nejtěžší analýza a kritické review:** Claude Opus 4.6
- **Komplexní kombinace kódu, nástrojů a dokumentace:** GPT-5.4
- **Rychlé a levné rutinní úlohy:** Claude Haiku 4.5, Gemini 3.5 Flash, SWE-1.6
- **Levnější dlouhá implementace:** Kimi K2.7, DeepSeek V4 Pro
- **Velký kontext a alternativní druhý názor:** Gemini 3.1 Pro, GLM-5.2

## Srovnávací tabulka

| Model | Kód | Debugging | Architektura | Review | Dokumentace | Rychlost | Efektivita | Nejvhodnější použití | Hlavní omezení |
|---|---:|---:|---:|---:|---:|---:|---:|---|---|
| **Claude Opus 4.6** | 5 | 5 | 5 | 5 | 5 | 2 | 2 | Kritické review, složitá architektura, bezpečnost, rozsáhlé legacy systémy, hledání skrytých vad | Pomalý a drahý; pro rutinní práci zbytečný |
| **Claude Sonnet 4.6** | 5 | 4.5 | 4.5 | 4.5 | 4.5 | 4 | 4.5 | Výchozí model pro implementaci, debugging, běžné review, refaktoring a technickou dokumentaci | U nejasných high-stakes úloh nemusí být tak důsledný jako Opus |
| **Claude Haiku 4.5** | 3.5 | 3 | 2.5 | 2.5 | 3.5 | 5 | 5 | Malé opravy, mechanické úpravy, navigace v repu, jednoduché testy, rychlé shrnutí | Slabší hluboké uvažování a dlouhé autonomní úlohy |
| **DeepSeek V4 Pro** | 4.5 | 4.5 | 4.5 | 4 | 3.5 | 3 | 4.5 | Cenově efektivní implementace, dlouhý kontext, komplexní debugging, druhý názor | Méně konzistentní dodržení jemných instrukcí; vhodné finální review |
| **Gemini 3.5 Flash** | 4 | 4 | 3.5 | 3.5 | 4 | 5 | 5 | Rychlý průzkum velkého repa, klasifikace, první implementační průchod, multimodální vstupy | U jemných návrhových rozhodnutí může být příliš sebejistý |
| **Gemini 3.1 Pro** | 4.5 | 4.5 | 4.5 | 4.5 | 4.5 | 3 | 3.5 | Velký kontext, komplexní analýza, návrh systému, práce s mnoha soubory | Pomalejší a nákladnější než Flash |
| **GLM-5.2** | 4.5 | 4 | 4.5 | 4 | 3.5 | 3 | 4.5 | Dlouhé agentní úlohy, větší systémy, experimentální alternativní implementace a review | Nižší předvídatelnost než Claude/GPT; požaduje silnou validaci |
| **GPT-5.4** | 5 | 5 | 5 | 5 | 5 | 3 | 3.5 | Komplexní profesionální práce, coding + analýza + dokumentace, tool use, přesná syntéza | Pro běžnou práci dražší a pomalejší než Sonnet |
| **GPT-5.1-Codex** | 4 | 4 | 3.5 | 3.5 | 3 | 3.5 | 3 | Agentní coding, práce v terminálu, opravy a implementace s jasnými testy | Starší/deprecated generace; používat jen pokud je v Devinovi výhodný nebo stabilní |
| **Kimi K2.6** | 4.5 | 4 | 4 | 3.5 | 4 | 3.5 | 4.5 | Dlouhé autonomní běhy, coding, multimodalita, kombinace kódu a obecné analýzy | Finální výsledek je vhodné nezávisle zkontrolovat |
| **Kimi K2.7** | 5 | 4.5 | 4 | 4 | 3.5 | 4 | 5 | Coding-first úlohy, větší implementace, frontend, dlouhé změny napříč repem | Méně vhodný pro business a governance dokumenty |
| **SWE-1.6** | 3.5 | 3.5 | 2.5 | 2.5 | 3 | 5 | 5 | Rychlé lokální úpravy, repo navigace, rutinní implementace, test skeletony, lint opravy | Slabší strategické uvažování, architektura a kritické review |

## Doporučení podle typu práce

| Typ práce | První volba | Levnější / rychlejší alternativa | Doporučené finální review |
|---|---|---|---|
| Malá oprava s jasným zadáním | Claude Haiku 4.5 | SWE-1.6 | Sonnet podle rizika |
| Standardní implementace | Claude Sonnet 4.6 | Kimi K2.7 | Sonnet 4.6 |
| Složitý debugging | Claude Sonnet 4.6 | DeepSeek V4 Pro | Opus 4.6 |
| Velký refaktor | Claude Sonnet 4.6 | Kimi K2.7 | Opus 4.6 |
| Architektonický návrh | Claude Opus 4.6 | GPT-5.4 / Gemini 3.1 Pro | Jiný frontier model |
| Kritický code review | Claude Opus 4.6 | GPT-5.4 | Případně oba nezávisle |
| Security review | Claude Opus 4.6 | GPT-5.4 | Ruční ověření a nástroje |
| Rychlý průzkum repozitáře | Gemini 3.5 Flash | SWE-1.6 | Sonnet 4.6 |
| Technická dokumentace | Claude Sonnet 4.6 | GPT-5.4 | Opus pro governance |
| Dlouhá autonomní implementace | Kimi K2.7 | DeepSeek V4 Pro / GLM-5.2 | Sonnet nebo Opus |
| Mechanická změna ve velkém počtu souborů | SWE-1.6 | Haiku 4.5 | Sonnet nad diffem |
| Business + technická syntéza | GPT-5.4 | Sonnet 4.6 | Opus při sporných závěrech |

## Praktické pravidlo

```text
Rutinní a levná práce:
Haiku 4.5 / Gemini 3.5 Flash / SWE-1.6

Běžná produkční práce:
Claude Sonnet 4.6

Nejtěžší analýza nebo nezávislé review:
Claude Opus 4.6

Komplexní kombinace kódu, nástrojů a dokumentace:
GPT-5.4

Levnější dlouhá implementace:
Kimi K2.7 / DeepSeek V4 Pro

Alternativní druhý názor:
Gemini 3.1 Pro / GLM-5.2
```

## Poznámky

- **GPT-5.1-Codex** je starší model; nepoužívej jej jako automatickou první volbu, pokud jsou dostupné novější alternativy.
- U modelů s nižší předvídatelností odděluj **implementaci** a **review** do různých sessions.
- Kritické změny neposuzuj pouze podle toho, že testy prošly. Review musí ověřit také požadavky, rizika a správnost testů.
Library
/
devin-model-cheat-sheet.md


# Devin Desktop – Model Cheat Sheet

> Praktické hodnocení pro práci nad repozitářem v Devin Desktop.  
> Škála: **5 = špička**, **1 = slabé**.  
> Hodnocení je relativní a orientační; výsledek ovlivňuje kvalita zadání, `AGENTS.md`, skills, velikost repozitáře a možnost ověřit výsledek testy.

## Rychlá volba

- **Výchozí model pro většinu práce:** Claude Sonnet 4.6
- **Nejtěžší analýza a kritické review:** Claude Opus 4.6
- **Komplexní kombinace kódu, nástrojů a dokumentace:** GPT-5.4
- **Rychlé a levné rutinní úlohy:** Claude Haiku 4.5, Gemini 3.5 Flash, SWE-1.6
- **Levnější dlouhá implementace:** Kimi K2.7, DeepSeek V4 Pro
- **Velký kontext a alternativní druhý názor:** Gemini 3.1 Pro, GLM-5.2

## Srovnávací tabulka

| Model | Kód | Debugging | Architektura | Review | Dokumentace | Rychlost | Efektivita | Nejvhodnější použití | Hlavní omezení |
|---|---:|---:|---:|---:|---:|---:|---:|---|---|
| **Claude Opus 4.6** | 5 | 5 | 5 | 5 | 5 | 2 | 2 | Kritické review, složitá architektura, bezpečnost, rozsáhlé legacy systémy, hledání skrytých vad | Pomalý a drahý; pro rutinní práci zbytečný |
| **Claude Sonnet 4.6** | 5 | 4.5 | 4.5 | 4.5 | 4.5 | 4 | 4.5 | Výchozí model pro implementaci, debugging, běžné review, refaktoring a technickou dokumentaci | U nejasných high-stakes úloh nemusí být tak důsledný jako Opus |
| **Claude Haiku 4.5** | 3.5 | 3 | 2.5 | 2.5 | 3.5 | 5 | 5 | Malé opravy, mechanické úpravy, navigace v repu, jednoduché testy, rychlé shrnutí | Slabší hluboké uvažování a dlouhé autonomní úlohy |
| **DeepSeek V4 Pro** | 4.5 | 4.5 | 4.5 | 4 | 3.5 | 3 | 4.5 | Cenově efektivní implementace, dlouhý kontext, komplexní debugging, druhý názor | Méně konzistentní dodržení jemných instrukcí; vhodné finální review |
| **Gemini 3.5 Flash** | 4 | 4 | 3.5 | 3.5 | 4 | 5 | 5 | Rychlý průzkum velkého repa, klasifikace, první implementační průchod, multimodální vstupy | U jemných návrhových rozhodnutí může být příliš sebejistý |
| **Gemini 3.1 Pro** | 4.5 | 4.5 | 4.5 | 4.5 | 4.5 | 3 | 3.5 | Velký kontext, komplexní analýza, návrh systému, práce s mnoha soubory | Pomalejší a nákladnější než Flash |
| **GLM-5.2** | 4.5 | 4 | 4.5 | 4 | 3.5 | 3 | 4.5 | Dlouhé agentní úlohy, větší systémy, experimentální alternativní implementace a review | Nižší předvídatelnost než Claude/GPT; požaduje silnou validaci |
| **GPT-5.4** | 5 | 5 | 5 | 5 | 5 | 3 | 3.5 | Komplexní profesionální práce, coding + analýza + dokumentace, tool use, přesná syntéza | Pro běžnou práci dražší a pomalejší než Sonnet |
| **GPT-5.1-Codex** | 4 | 4 | 3.5 | 3.5 | 3 | 3.5 | 3 | Agentní coding, práce v terminálu, opravy a implementace s jasnými testy | Starší/deprecated generace; používat jen pokud je v Devinovi výhodný nebo stabilní |
| **Kimi K2.6** | 4.5 | 4 | 4 | 3.5 | 4 | 3.5 | 4.5 | Dlouhé autonomní běhy, coding, multimodalita, kombinace kódu a obecné analýzy | Finální výsledek je vhodné nezávisle zkontrolovat |
| **Kimi K2.7** | 5 | 4.5 | 4 | 4 | 3.5 | 4 | 5 | Coding-first úlohy, větší implementace, frontend, dlouhé změny napříč repem | Méně vhodný pro business a governance dokumenty |
| **SWE-1.6** | 3.5 | 3.5 | 2.5 | 2.5 | 3 | 5 | 5 | Rychlé lokální úpravy, repo navigace, rutinní implementace, test skeletony, lint opravy | Slabší strategické uvažování, architektura a kritické review |

## Doporučení podle typu práce

| Typ práce | První volba | Levnější / rychlejší alternativa | Doporučené finální review |
|---|---|---|---|
| Malá oprava s jasným zadáním | Claude Haiku 4.5 | SWE-1.6 | Sonnet podle rizika |
| Standardní implementace | Claude Sonnet 4.6 | Kimi K2.7 | Sonnet 4.6 |
| Složitý debugging | Claude Sonnet 4.6 | DeepSeek V4 Pro | Opus 4.6 |
| Velký refaktor | Claude Sonnet 4.6 | Kimi K2.7 | Opus 4.6 |
| Architektonický návrh | Claude Opus 4.6 | GPT-5.4 / Gemini 3.1 Pro | Jiný frontier model |
| Kritický code review | Claude Opus 4.6 | GPT-5.4 | Případně oba nezávisle |
| Security review | Claude Opus 4.6 | GPT-5.4 | Ruční ověření a nástroje |
| Rychlý průzkum repozitáře | Gemini 3.5 Flash | SWE-1.6 | Sonnet 4.6 |
| Technická dokumentace | Claude Sonnet 4.6 | GPT-5.4 | Opus pro governance |
| Dlouhá autonomní implementace | Kimi K2.7 | DeepSeek V4 Pro / GLM-5.2 | Sonnet nebo Opus |
| Mechanická změna ve velkém počtu souborů | SWE-1.6 | Haiku 4.5 | Sonnet nad diffem |
| Business + technická syntéza | GPT-5.4 | Sonnet 4.6 | Opus při sporných závěrech |

## Praktické pravidlo

```text
Rutinní a levná práce:
Haiku 4.5 / Gemini 3.5 Flash / SWE-1.6

Běžná produkční práce:
Claude Sonnet 4.6

Nejtěžší analýza nebo nezávislé review:
Claude Opus 4.6

Komplexní kombinace kódu, nástrojů a dokumentace:
GPT-5.4

Levnější dlouhá implementace:
Kimi K2.7 / DeepSeek V4 Pro

Alternativní druhý názor:
Gemini 3.1 Pro / GLM-5.2
```

## Poznámky

- **GPT-5.1-Codex** je starší model; nepoužívej jej jako automatickou první volbu, pokud jsou dostupné novější alternativy.
- U modelů s nižší předvídatelností odděluj **implementaci** a **review** do různých sessions.
- Kritické změny neposuzuj pouze podle toho, že testy prošly. Review musí ověřit také požadavky, rizika a správnost testů.