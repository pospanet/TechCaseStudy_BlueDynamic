# Prompt Set 5 — Final Integrated Review and Hardening

## Recommended workflow

| Phase | Model | Skill | Chat |
|---|---|---|---|
| Analysis | GPT-5.4 | `explore-repository` | New chat after all changes |
| Independent review | Claude Opus 4.6 | `review-change` | New chat |
| Implementation | GPT-5.1-codex | `implement-task` + `validate-project` | New chat |
| Final acceptance review | Claude Sonnet 4.6 | `review-change` | New chat |

## A. Integrated post-change analysis

```text
Use @skills:explore-repository.

Perform a read-only integrated assessment of the current repository after the security, antipattern, performance and framework-modernization work.

Follow all applicable AGENTS.md files. Inspect the current repository state, complete working-tree diff and affected execution paths. Do not modify files.

Determine:

- what materially changed
- whether the changes form a coherent system state
- whether security controls remain compatible with performance and framework changes
- whether refactoring preserved behavior
- whether framework changes altered runtime assumptions
- whether documentation and configuration match implementation
- whether new risks were introduced across component boundaries
- whether tests and validation adequately cover the combined changes

Evaluate:

- functional regressions
- security regressions and bypasses
- authentication, authorization and isolation
- async, concurrency and ordering behavior
- error handling and retries
- external-service compatibility
- configuration, binding and deployment consistency
- resource lifecycle and performance
- data-format and serialization compatibility
- unsupported environments and tooling gaps
- incomplete documentation or migration instructions

Produce:

# Change Inventory
# Integrated Architecture Assessment
# Cross-Cutting Regressions
# Security Posture
# Performance and Reliability Posture
# Framework and Dependency Compatibility
# Validation Coverage
# Remaining Blockers
# Recommended Release Decision

Rank all remaining work as release blocker, required follow-up, recommended improvement or accepted debt.
```

## B. Independent final change review

```text
Use @skills:review-change.

Perform an independent final review of all current uncommitted changes against the original repository baseline.

Do not modify files. Inspect the complete diff, not only the most recent changes.

Review for:

- functional defects and regressions
- security vulnerabilities and incomplete remediations
- framework and package incompatibility
- async, concurrency and resource-lifecycle defects
- incorrect performance assumptions
- behavior changes hidden inside refactoring
- configuration and deployment inconsistencies
- weak or missing tests
- unsupported claims in documentation
- accidental secrets, generated artifacts or unrelated churn

For every concrete finding provide severity, exact location, failure scenario, impact and correction. Explicitly identify residual risks that cannot be verified in the available environment.

End with one disposition: accept, accept with minor corrections, revise or reject.
```

## C. Implement final corrections

```text
Use @skills:implement-task.

Resolve the concrete release-blocking and high-confidence defects remaining in the current working tree after the integrated review.

Before editing, inspect the complete current diff and select only corrections required for correctness, security, compatibility or reliable validation. Do not add new features, broad refactoring or speculative optimization.

Present a concise correction plan listing:

- findings to fix
- files expected to change
- acceptance criteria
- validation commands
- findings intentionally deferred and why

Implementation rules:

- make the smallest complete corrections
- preserve the intended security, refactoring, performance and framework-upgrade outcomes
- avoid unrelated changes
- add or strengthen regression tests where practical
- do not suppress, weaken or delete failing checks merely to obtain a green result
- do not expose secrets

After implementation, use @skills:validate-project and inspect the complete final diff.

Report:

- corrections made
- files changed
- tests changed
- validation command table
- checks not run and exact reasons
- residual risks
- release readiness recommendation
```

## D. Final acceptance review

```text
Use @skills:review-change.

Perform the final acceptance review of the complete current working tree.

Do not modify files. Verify that previously identified release-blocking defects are resolved and that the correction pass did not introduce regressions.

Evaluate correctness, security, compatibility, performance safety, test adequacy, documentation accuracy, deployment readiness and diff cleanliness.

Report only concrete findings. If no material defect remains, state that explicitly and list residual environmental or runtime risks.

Conclude with exactly one recommendation:

- Accept
- Accept with minor corrections
- Revise
- Reject
```
