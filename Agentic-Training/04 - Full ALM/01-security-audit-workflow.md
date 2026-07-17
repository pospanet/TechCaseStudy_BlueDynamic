# Workflow 1 — Security audit a náprava nálezů

## Modely a skills

| Fáze | Model | Skill | Chat |
|---|---|---|---|
| Business zadání | Claude Opus 4.6 | `explore-repository` | nový |
| Review business zadání | Claude Sonnet 4.6 | `review-change` nebo bez skillu | nový nezávislý |
| Zapracování připomínek | Claude Opus 4.6 | `explore-repository` | původní |
| Implementační plán | Claude Opus 4.6 | `explore-repository` | nový nebo navazující analytický |
| Review plánu | Claude Sonnet 4.6 | `review-change` | nový nezávislý |
| Zapracování připomínek | Claude Opus 4.6 | `explore-repository` | původní |
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

Security audit and remediation

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

- ochranu zákaznických, zařízení, tenantních a biometrických dat,
- prevenci neoprávněného přístupu a eskalace oprávnění,
- omezení úniku přihlašovacích údajů a příliš širokých oprávnění,
- jasné podmínky, za kterých lze systém demonstrovat, znovu použít nebo nasadit.

Acceptance criteria must be testable and unambiguous. Use Given/When/Then where practical.

Minimum acceptance-criteria coverage:

- neautorizovaný uživatel nemůže vyvolat chráněnou operaci,
- klientské identifikátory jsou validované a autorizované,
- citlivé credentials nejsou zbytečně vraceny nebo vloženy do klienta,
- tenantní a device isolation je ověřitelná,
- pro opravené exploit paths existují regresní testy,
- žádný potvrzený Critical nebo High nález nezůstane bez schválené výjimky.

Avoid implementation design unless it is a necessary business constraint.

Save as:
`docs/work-items/security-audit-business-requirement.md`

Do not modify any other file.
```

# 2. Review business zadání

```text
Use @skills:review-change.

Independently review:
`docs/work-items/security-audit-business-requirement.md`

The reviewed document is read-only. Do not modify it and do not modify production code.

Evaluate clarity of the problem, objective, scope, exclusions, assumptions, measurable success criteria, feasibility and testability of acceptance criteria.

Find:
- blockers,
- major issues,
- minor issues,
- missing acceptance criteria,
- ambiguous wording,
- conflicting requirements,
- hidden implementation assumptions,
- unjustified assumptions,
- missing business or operational risks.

For every issue cite the exact section and recommend a precise correction.

Save the complete review as:
`docs/work-items/security-audit-business-requirement.review.md`

The review file must contain:
1. reviewed document path and reviewed revision or commit when available,
2. overall verdict: Approved, Approved with changes, or Rework required,
3. numbered findings with severity,
4. exact source section for every finding,
5. recommended resolution,
6. missing or revised acceptance criteria,
7. a machine-readable resolution checklist using stable finding IDs such as `BR-001`.

Modify no file other than the review file.
```

# 3. Zapracování review business zadání

```text
Use @skills:explore-repository.

Reconcile and finalize:
`docs/work-items/security-audit-business-requirement.md`

Use the review findings from:
`docs/work-items/security-audit-business-requirement.review.md`

The review file is the authoritative input for this reconciliation step. Do not require findings to be copied into the chat.

Procedure:
1. Read both files.
2. Classify every finding ID as accepted, partially accepted or rejected.
3. Briefly justify every partial acceptance or rejection.
4. Incorporate all accepted changes into the business requirement.
5. Make mandatory acceptance criteria measurable and testable.
6. Remove ambiguity, contradictions and unnecessary implementation design.
7. Verify that every review finding has a recorded resolution.
8. Save the final consolidated document back to:
   `docs/work-items/security-audit-business-requirement.md`
9. After the final document is saved and verified, delete:
   `docs/work-items/security-audit-business-requirement.review.md`

Add a concise `Review Resolution` section to the final document containing finding ID, disposition and remaining risk. Do not duplicate the full review text.

Postconditions:
- exactly one business-requirement artifact remains,
- the temporary review file no longer exists,
- no production-code file is modified,
- the final document is internally consistent and self-contained.

If the review file is missing, stop and report the missing input. Do not infer review findings from chat history.
```

# 4. Implementační plán

```text
Use @skills:explore-repository.

Prepare a high-level implementation plan based on:
`docs/work-items/security-audit-business-requirement.md`

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
- trust boundaries, autentizaci a autorizaci,
- tenantní a device isolation,
- secrets a credentials,
- Azure oprávnění,
- zpracování biometrických dat,
- validaci vstupů,
- logování citlivých dat,
- dependency a deployment risk.

Every mandatory acceptance criterion must map to implementation steps, validation steps and expected evidence.

Save as:
`docs/work-items/security-audit-implementation-plan.md`

Do not implement the plan.
```

# 5. Review implementačního plánu

```text
Use @skills:review-change.

Independently review:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.md`

Treat both source documents as read-only. Do not modify production code.

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

For every finding provide a stable ID such as `IP-001`, severity, exact plan section, affected requirement or acceptance criterion, impact and recommended exact change.

Save the complete review as:
`docs/work-items/security-audit-implementation-plan.review.md`

The review file must end with an approval verdict and a resolution checklist covering every finding ID.

Modify no file other than the review file.
```

# 6. Zapracování review implementačního plánu

```text
Use @skills:explore-repository.

Reconcile and finalize:
`docs/work-items/security-audit-implementation-plan.md`

Use:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.review.md`

Do not require review findings to be copied into the chat.

Procedure:
1. Read the requirement, plan and review files.
2. Classify every review finding ID as accepted, partially accepted, rejected or explicitly deferred.
3. Close all accepted blockers.
4. Resolve accepted major findings or record a justified deferral with owner and follow-up condition.
5. Preserve full requirement and acceptance-criterion traceability.
6. Make ordering, checkpoints, rollback and validation evidence explicit.
7. Remove speculative and unrelated work.
8. Save the final consolidated plan back to:
   `docs/work-items/security-audit-implementation-plan.md`
9. Verify that every finding ID has a resolution.
10. After successful verification, delete:
    `docs/work-items/security-audit-implementation-plan.review.md`

Add a concise `Plan Review Resolution` section to the final plan containing finding ID, disposition, affected section and remaining risk. Do not copy the complete review into the plan.

Postconditions:
- exactly one implementation-plan artifact remains,
- the temporary review file no longer exists,
- the final plan is self-contained and consistent with the business requirement,
- no production code is modified.

If the review file is missing, stop and report the missing input.
```

# 7. Atomické tasky

```text
Use @skills:implement-task.

Create atomic tasks from:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.md`

Do not modify production code.

Save as:
`docs/work-items/security-audit-tasks.md`

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
`docs/work-items/security-audit-tasks.md`

Authoritative inputs:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.md`
- `docs/work-items/security-audit-tasks.md`

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

Authoritative inputs:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.md`
- `docs/work-items/security-audit-tasks.md`

Review the actual diff and current repository state. Do not modify implementation files.

Verify correctness, scope, acceptance criteria, error handling, security, privacy, performance, compatibility, tests, validation evidence, documentation and configuration.

Classify findings:
- Blocker
- Major
- Minor
- Nit
- Informational

For every finding provide:
- stable finding ID such as `CR-TASK-ID-001`,
- exact file and location,
- violated requirement or task criterion,
- concrete impact,
- recommended fix,
- required regression test,
- merge-blocking status.

Save the complete review as:
`docs/work-items/security-audit-TASK-ID-code-review.md`

For a reviewed task range, use:
`docs/work-items/security-audit-TASK-ID-to-TASK-ID-code-review.md`

The review file must include approval verdict, acceptance-criteria status, required fixes, optional follow-ups and residual risk.

Modify no file other than the code-review file.
```

# 10. Zapracování code-review nálezů

```text
Use @skills:implement-task.

Address review findings for:
`TASK-ID` or `TASK-ID..TASK-ID`

Read the corresponding review file:
- single task: `docs/work-items/security-audit-TASK-ID-code-review.md`
- task range: `docs/work-items/security-audit-TASK-ID-to-TASK-ID-code-review.md`

Use the business requirement, implementation plan and task list as authoritative scope. Do not require findings to be copied into the chat.

Procedure:
1. Read the review file and classify every finding ID as accepted, partially accepted, rejected or deferred.
2. Justify rejections and deferrals.
3. Implement all accepted Blocker and Major findings.
4. Implement directly relevant, low-risk Minor findings.
5. Add or update regression tests where required.
6. Use @skills:validate-project for safe validation.
7. Update the affected task entries in `docs/work-items/security-audit-tasks.md` with final status, validation evidence and any approved residual risk.
8. Verify that every code-review finding ID has a resolution.
9. After implementation and validation succeed, delete the corresponding code-review file.

Do not append the full review to the task list. Retain only a concise final resolution/status and remaining risk.

Postconditions:
- implementation contains all accepted fixes,
- tests and validation evidence are current,
- the temporary code-review file no longer exists,
- only the canonical task document remains as the workflow record,
- no unrelated work is introduced.

If validation fails, keep the review file in place and report the failure. Delete it only after successful reconciliation and validation.
```

# 11. Finální validace oblasti

```text
Use @skills:validate-project.

Perform final validation of:
Security audit and remediation

Authoritative inputs:
- `docs/work-items/security-audit-business-requirement.md`
- `docs/work-items/security-audit-implementation-plan.md`
- `docs/work-items/security-audit-tasks.md`

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
