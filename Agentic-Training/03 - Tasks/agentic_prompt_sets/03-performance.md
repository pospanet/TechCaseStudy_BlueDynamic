# Prompt Set 3 — Performance Analysis and Optimization

## Recommended workflow

| Phase | Model | Skill | Chat |
|---|---|---|---|
| Analysis | Claude Opus 4.6 | `explore-repository` | New chat |
| Independent review | GPT-5.4 | `explore-repository` | New chat |
| Implementation | GPT-5.1-codex | `implement-task` + `validate-project` | New chat |
| Change review | Claude Sonnet 4.6 | `review-change` | New chat |

## A. Performance analysis

```text
Use @skills:explore-repository.

Perform a read-only performance and scalability analysis of the current repository.

Follow all applicable AGENTS.md files. Do not modify files and do not claim measured performance results unless benchmark, profiling or telemetry evidence exists in the repository.

Trace the important end-to-end execution paths and inspect likely costs involving:

- Azure and other network round trips
- database and document-store access
- repeated or sequential remote calls
- large image and payload handling
- serialization and deserialization
- unnecessary allocations and copies
- synchronous blocking and sync-over-async
- incorrect async usage
- operations that could safely execute concurrently
- excessive polling
- duplicate or repeated processing
- client and connection creation inside hot paths
- missing batching or caching
- inefficient collection operations
- unbounded queues, collections or retries
- lock contention and shared mutable state
- retry storms and missing backoff
- UI-thread work
- startup and initialization cost
- external API quotas and rate limits
- inefficient logging in hot paths

Separate findings into:

- confirmed code-level inefficiencies
- highly probable bottlenecks
- scalability risks
- hypotheses requiring profiling
- optimizations unlikely to matter at the expected workload

For every finding provide:

1. affected execution path
2. exact files and methods
3. reason the operation may be expensive
4. expected symptom
5. workload conditions under which it matters
6. proposed optimization
7. correctness and reliability trade-offs
8. required measurement
9. recommended benchmark, profiler or telemetry signal
10. implementation risk

End with:

- top five optimization candidates
- quick wins
- architectural improvements
- a measurement plan
- optimizations that should not be attempted without profiling

Do not modify the repository.
```

## B. Independent performance review

```text
Use @skills:explore-repository.

Perform an independent second-pass performance review of the current repository. Do not modify files.

Challenge speculative optimization claims and search for overlooked bottlenecks, especially across component boundaries.

Evaluate:

- complete request and event-processing paths
- number and ordering of external calls
- whether concurrency would be safe or harmful
- repeated client construction
- payload size and image transformations
- database query shape and result materialization
- retry, timeout and backpressure behavior
- queue and message-processing throughput
- duplicate work and idempotency
- startup and one-time initialization versus steady-state cost
- optimizations that risk data corruption, throttling or semantic change

Return:

1. confirmed high-confidence bottlenecks
2. missed bottlenecks
3. likely false positives
4. measurements required before implementation
5. one bounded optimization suitable for implementation now
6. explicit success metrics and regression guardrails

Do not modify the repository.
```

## C. Implement a measured optimization

```text
Use @skills:implement-task.

Implement one bounded, high-confidence performance optimization in the current repository.

First inspect the repository and select an optimization with a clear expensive path, limited blast radius and preserved semantics. Prefer eliminating redundant remote calls, reusing expensive clients, removing unnecessary blocking, batching safe operations or avoiding repeated serialization over broad rewrites.

Before editing, provide:

- selected bottleneck and evidence
- expected performance effect without inventing numeric gains
- behavior and correctness invariants
- files expected to change
- measurement or validation strategy
- risks and rollback conditions

Implementation rules:

- preserve externally visible behavior and data semantics
- do not weaken validation, durability, security or error handling for speed
- avoid unrelated refactoring and dependency upgrades
- add focused tests for correctness and concurrency where supported
- add lightweight measurement hooks only when appropriate and non-invasive
- do not claim improvement unless measured; otherwise describe it as expected

After implementation, use @skills:validate-project. Report commands, results, observed measurements if available, unmeasured assumptions and residual performance risks.
```

## D. Review the performance change

```text
Use @skills:review-change.

Independently review the current uncommitted performance optimization.

Focus on:

- semantic and data-consistency regressions
- unsafe concurrency
- changed ordering or timing assumptions
- resource leaks and client lifetime
- retry, timeout and throttling behavior
- security or isolation regressions
- invalid performance claims
- missing measurement or regression tests
- optimizations that merely move the bottleneck
- excessive scope

Do not modify files. Report concrete findings by severity and state whether the change is safe to retain even if the expected performance gain remains unmeasured.
```
