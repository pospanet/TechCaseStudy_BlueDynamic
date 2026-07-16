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