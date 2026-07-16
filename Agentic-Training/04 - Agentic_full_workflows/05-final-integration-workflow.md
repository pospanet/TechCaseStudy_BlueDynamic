# Workflow 5 — Závěrečná integrace všech změn

## Modely a skills

| Fáze | Model | Skill | Chat |
|---|---|---|---|
| Business zadání | GPT-5.4 | `explore-repository` | nový |
| Review business zadání | Claude Sonnet 4.6 | `review-change` nebo bez skillu | nový nezávislý |
| Zapracování připomínek | GPT-5.4 | `explore-repository` | původní |
| Implementační plán | GPT-5.4 | `explore-repository` | nový nebo navazující analytický |
| Review plánu | Claude Sonnet 4.6 | `review-change` | nový nezávislý |
| Zapracování připomínek | GPT-5.4 | `explore-repository` | původní |
| Atomické tasky | GPT-5.1-codex | `implement-task` | nový |
| Implementace tasku | GPT-5.1-codex | `implement-task` | samostatný pro task |
| Code review | Claude Sonnet 4.6 | `review-change` | nový nezávislý |
| Oprava nálezů | GPT-5.1-codex | `implement-task` | původní implementační |
| Finální validace | Claude Sonnet 4.6 | `validate-project` | nový |

---

# 1. Business zadání

```text
Use @skills:explore-repository.

Prepare a business requirement document for this initiative:

Final integration, stabilization and release readiness

This is a read-only analysis. Do not modify production code.

Inspect the repository, architecture, current behavior, documentation, constraints and business context. Use repository evidence and clearly label assumptions.

Create a business-oriented specification containing:

1. Business problem
2. Current state
3. Target state
4. Explicit business objective
5. Scope
6. Out of scope
7. Stakeholders and affected users or systems
8. Business value
9. Risks of doing nothing
10. Constraints and dependencies
11. Assumptions
12. Non-functional expectations
13. Mandatory acceptance criteria
14. Desirable acceptance criteria
15. Success metrics
16. Required validation evidence
17. Rollout and operational considerations
18. Open questions
19. Decision log

Focus on:

- ověření, že všechny předchozí iniciativy fungují společně,
- prevenci regresí vzniklých interakcí security, refaktoringu, výkonu a framework změn,
- release nebo demo readiness,
- explicitní akceptaci residual risk.

Acceptance criteria must be testable and unambiguous. Use Given/When/Then where practical.

Minimum acceptance-criteria coverage:

- všechny čtyři předchozí iniciativy splní mandatory acceptance criteria,
- kritické end-to-end flows projdou,
- nezůstane unresolved Blocker nebo Major finding,
- build, testy a smoke testy projdou v definovaném prostředí,
- deployment a rollback postup je zdokumentován,
- residual risks jsou explicitně schváleny.

Avoid implementation design unless it is a necessary business constraint.

Save as:
`docs/work-items/final-integration-business-requirement.md`

Do not modify any other file.
```

# 2. Review business zadání

```text
Independently review:
`docs/work-items/final-integration-business-requirement.md`

Do not modify files.

Evaluate clarity of the problem, objective, scope, exclusions, assumptions, measurable success criteria, feasibility and testability of acceptance criteria.

Pay special attention to:
Předpoklad, že jednotlivě správné změny jsou správné i společně, neúplná konfigurace, skryté pořadí migrací, security regresní dopady a release approval bez evidence.

Find:
- blockers,
- major issues,
- minor issues,
- missing acceptance criteria,
- ambiguous wording,
- conflicting requirements,
- hidden implementation assumptions,
- unjustified assumptions,
- missing business or operational risk.

For every issue cite the exact section and recommend a precise correction.

Return verdict: Approved, Approved with changes, or Rework required.
Do not rewrite the document.
```

# 3. Zapracování review business zadání

```text
Update:
`docs/work-items/final-integration-business-requirement.md`

Incorporate all valid review findings.

First classify each finding as accepted, partially accepted or rejected. Briefly justify rejections. Preserve the business objective unless the review proves it invalid.

Make every mandatory acceptance criterion measurable and testable. Remove ambiguity and contradictions. Do not introduce unnecessary implementation design.

Append a `Review Resolution` table with:
- Finding
- Resolution
- Change made
- Remaining risk

Modify only this document.
```

# 4. Implementační plán

```text
Use @skills:explore-repository.

Prepare a high-level implementation plan based on:
`docs/work-items/final-integration-business-requirement.md`

Inspect the current repository and trace all affected components. Do not modify production code.

The plan must include:
1. Requirement summary
2. Current architecture and affected components
3. Proposed target behavior
4. Technical approach
5. Data-flow changes
6. API and contract changes
7. Configuration changes
8. Dependency changes
9. Migration needs
10. Compatibility considerations
11. Security and privacy implications
12. Performance implications
13. Error handling and observability
14. Rollout strategy
15. Backout strategy
16. Testing strategy
17. Validation strategy
18. Documentation changes
19. Risks and mitigations
20. Ordered implementation sequence
21. Requirement-to-plan traceability
22. Open technical decisions

Analysis focus:
- cross-change interactions,
- end-to-end flows,
- configuration consistency,
- dependency a framework compatibility,
- security a performance regressions,
- neúplné migrace,
- deployment a rollback readiness.

Every mandatory acceptance criterion must map to implementation steps, validation steps and expected evidence.

Save as:
`docs/work-items/final-integration-implementation-plan.md`

Do not implement the plan.
```

# 5. Review implementačního plánu

```text
Use @skills:review-change.

Independently review:
- `docs/work-items/final-integration-business-requirement.md`
- `docs/work-items/final-integration-implementation-plan.md`

Do not modify files.

Verify requirement coverage, acceptance-criterion traceability, consistency with the actual repository, implementation ordering, dependencies, migration, rollback, testing, validation and absence of unrelated work.

Identify:
- blocking findings,
- major findings,
- minor findings,
- missing traceability,
- invalid repository assumptions,
- hidden coupling,
- ordering problems,
- steps too large for safe implementation,
- acceptance criteria that cannot be proven.

Recommend exact plan changes and provide an approval verdict.
```

# 6. Zapracování review implementačního plánu

```text
Update:
`docs/work-items/final-integration-implementation-plan.md`

Incorporate all valid findings from the plan review.

Close all blockers. Resolve or explicitly defer major findings. Preserve traceability to the approved business requirement. Make ordering, checkpoints, rollback and validation evidence explicit.

Append a `Plan Review Resolution` table with:
- Finding
- Resolution
- Plan section changed
- Remaining risk
- Deferred follow-up

Modify only this document.
```

# 7. Atomické tasky

```text
Use @skills:implement-task.

Create atomic tasks from:
- `docs/work-items/final-integration-business-requirement.md`
- `docs/work-items/final-integration-implementation-plan.md`

Do not modify production code.

Save as:
`docs/work-items/final-integration-tasks.md`

For every task include:
1. Task ID
2. Title
3. Objective
4. Requirement references
5. Acceptance-criterion references
6. Exact scope
7. Out of scope
8. Expected files or components
9. Dependencies
10. Preconditions
11. Implementation instructions
12. Required tests
13. Validation commands
14. Expected evidence
15. Rollback considerations
16. Risks
17. Definition of done

Rules:
- one primary objective per task,
- keep tasks independently reviewable,
- separate preparatory refactoring from behavior changes,
- separate dependency upgrades from unrelated cleanup,
- identify dependencies and parallelizable tasks,
- add checkpoints after risky changes,
- include documentation, migration and final integration tasks.

Order tasks into waves:
- Wave 0: prerequisites and evidence
- Wave 1: enabling changes
- Wave 2: primary implementation
- Wave 3: integration and migration
- Wave 4: validation and documentation
```

# 8. Implementace jednoho tasku

```text
Use @skills:implement-task.

Implement `TASK-ID` from:
`docs/work-items/final-integration-tasks.md`

Authoritative inputs:
- `docs/work-items/final-integration-business-requirement.md`
- `docs/work-items/final-integration-implementation-plan.md`
- `docs/work-items/final-integration-tasks.md`

Implement only this task and strictly necessary dependencies.

Before editing, restate the objective, acceptance criteria and expected scope. Inspect the current code and report if the task is no longer valid.

Rules:
- keep the change atomic,
- no unrelated refactoring,
- preserve behavior outside scope,
- add required tests,
- update only required docs and configuration,
- follow repository governance,
- use @skills:validate-project for safe checks.

After implementation report changed files, behavior, tests, commands, results, acceptance-criteria status, risks and readiness for code review.

Do not start another task.
```

# 9. Code review tasku

```text
Use @skills:review-change.

Review implementation of:
`TASK-ID` or `TASK-ID..TASK-ID`

Use the business requirement, implementation plan and task list as authoritative scope. Review the actual diff. Do not modify files.

Verify correctness, scope, acceptance criteria, error handling, security, privacy, performance, compatibility, tests, validation evidence, docs and configuration.

Classify findings:
- Blocker
- Major
- Minor
- Nit
- Informational

For every finding provide exact file and location, violated requirement, impact, recommended fix and whether a regression test is required.

End with approval verdict, acceptance-criteria status, required fixes, optional follow-ups and residual risk.
```

# 10. Zapracování code-review nálezů

```text
Use @skills:implement-task.

Address valid review findings for:
`TASK-ID` or `TASK-ID..TASK-ID`

First classify findings as accepted, partially accepted or rejected and justify rejections. Implement all accepted Blocker and Major findings. Add regression tests where required. Implement relevant low-risk Minor findings.

Then use @skills:validate-project.

Report each finding, resolution, changed files, test changes, validation result, remaining risk and deferred follow-up.

Do not start unrelated work.
```

# 11. Finální validace oblasti

```text
Use @skills:validate-project.

Perform final validation of:
Final integration, stabilization and release readiness

Authoritative inputs:
- `docs/work-items/final-integration-business-requirement.md`
- `docs/work-items/final-integration-implementation-plan.md`
- `docs/work-items/final-integration-tasks.md`

Verify all mandatory acceptance criteria, all tasks, build and tests, documentation and configuration consistency, unresolved review findings, rollout readiness, rollback readiness and residual risks.

Produce:
- acceptance-criteria matrix,
- task completion matrix,
- validation commands and results,
- open defects,
- deferred items,
- residual risks,
- final verdict: Accepted, Conditionally accepted, or Not accepted.

Do not claim success without evidence.
```
