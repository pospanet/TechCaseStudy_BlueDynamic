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