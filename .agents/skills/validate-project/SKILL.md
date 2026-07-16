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