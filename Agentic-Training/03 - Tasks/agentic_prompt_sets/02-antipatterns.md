# Prompt Set 2 — Antipattern Detection and Refactoring

## Recommended workflow

| Phase | Model | Skill | Chat |
|---|---|---|---|
| Analysis | Claude Opus 4.6 | `explore-repository` | New chat |
| Independent review | GPT-5.4 | `explore-repository` | New chat |
| Implementation | Claude Sonnet 4.6 | `implement-task` + `validate-project` | New chat |
| Change review | GPT-5.4 | `review-change` | New chat |

## A. Antipattern analysis

```text
Use @skills:explore-repository.

Perform a read-only analysis of the current repository focused on architectural, design, code-quality and maintainability antipatterns.

Follow all applicable AGENTS.md files. Do not modify files.

First map the solution structure, component responsibilities, dependency directions, execution flows, external integrations, persistence model and framework boundaries.

Identify concrete repository-supported examples of:

- god classes or overly broad responsibilities
- high coupling and low cohesion
- duplicated business or integration logic
- hidden dependencies and service locator patterns
- inappropriate static or global mutable state
- business logic mixed with UI, transport, persistence or cloud integration
- leaky abstractions
- primitive obsession and weak domain modeling
- excessive parameter lists
- temporal coupling and fragile initialization order
- feature envy and misplaced responsibilities
- shotgun surgery
- inconsistent error handling
- synchronous blocking of asynchronous work
- fire-and-forget operations
- missing cancellation, timeout or retry boundaries
- resource lifecycle problems
- chatty service integrations
- repeated database or remote-service queries
- unbounded collections or queues
- testability barriers
- dead code and obsolete abstractions
- configuration embedded in implementation
- framework-specific concerns spread across the codebase
- unsupported or obsolete technology dependencies

Do not label normal legacy constraints as antipatterns without explaining their concrete negative effect in this repository.

For every finding provide:

1. antipattern name
2. severity
3. confidence
4. exact files, classes and methods
5. concrete evidence
6. why it is harmful here
7. observed or likely impact
8. recommended refactoring
9. scope: local, component-wide or architectural
10. migration and regression risk
11. tests or measurements needed before change

Produce a prioritized matrix using impact, frequency, change cost and regression risk. End with the five best refactoring candidates by value-to-effort ratio and clearly identify antipatterns that should not be addressed during a short modernization exercise.
```

## B. Independent antipattern review

```text
Use @skills:explore-repository.

Perform an independent second-pass review of the repository's architecture and code-quality risks. Do not modify files.

Challenge common overdiagnosis. Distinguish true design defects from intentional proof-of-concept shortcuts, framework constraints and harmless style preferences.

Focus on:

- whether reported duplication represents the same business rule or merely similar syntax
- whether proposed abstractions would reduce or increase complexity
- dependency direction and testability
- state ownership and lifecycle
- async and concurrency design
- service boundaries and integration wrappers
- obsolete code that is still reachable
- refactorings likely to cause broad regression
- simpler local alternatives to architectural rewrites

Return:

1. confirmed high-value antipatterns
2. missed antipatterns
3. likely false positives
4. refactorings whose cost exceeds their benefit
5. a recommended bounded refactoring slice suitable for implementation now
6. acceptance criteria for that slice

Do not modify the repository.
```

## C. Implement a bounded refactoring

```text
Use @skills:implement-task.

Implement the highest-value bounded antipattern remediation that can be completed safely without changing externally visible behavior.

Inspect the repository first and select a coherent refactoring slice with strong evidence, limited blast radius and clear validation. Prefer removing duplicated logic, clarifying responsibility, isolating an external integration, fixing unsafe async structure or improving resource lifecycle over broad architectural rewrites.

Before editing, state:

- the selected antipattern
- evidence and affected execution paths
- intended internal design improvement
- explicit behavior that must remain unchanged
- files expected to change
- tests and validation to use
- excluded cleanup

Implementation rules:

- preserve observable behavior and public contracts
- keep the diff small and coherent
- avoid unrelated formatting, renaming and dependency upgrades
- do not introduce speculative abstractions
- add or update tests proving behavior preservation where practical
- improve error handling only where directly related
- document any behavior that cannot be validated in the local environment

After implementation, use @skills:validate-project. Report the final diff, behavior-preservation evidence, validation results and residual architectural debt.
```

## D. Review the refactoring

```text
Use @skills:review-change.

Independently review the current uncommitted refactoring.

Evaluate:

- behavior preservation
- whether the targeted antipattern was actually reduced
- whether coupling or complexity moved elsewhere
- unnecessary abstraction
- regressions in initialization, state, async behavior or error handling
- public-contract changes
- missing or weak tests
- excessive scope and unrelated churn

Inspect the complete diff and relevant surrounding code. Do not modify files.

Report concrete findings by severity and conclude whether the refactoring improves maintainability at an acceptable regression risk.
```
