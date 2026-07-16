# AGENTS.md

Repository-level operating instructions for AI agents. These instructions are subordinate to `docs/agentic/agentic-constitution.md` and are mandatory for work in this repository.

This file requires explicit human acceptance before it becomes authoritative.

---

## 1. Purpose and Authority

This file governs all AI coding agents working in this repository, regardless of model or orchestration tool.

This repository contains the Retailizer project — a historical proof-of-concept retail analytics system using facial recognition (circa 2016). It is a legacy codebase with obsolete toolchains, biometric data sensitivity, critical implementation defects that are historical evidence, and approved documentation that has undergone independent review.

**Constitution:** `docs/agentic/agentic-constitution.md` — highest repository-level authority.

## 2. Instruction Precedence

1. Explicit human instruction for the current task
2. The Agentic Constitution
3. This file (`AGENTS.md`)
4. Task-specific documentation
5. Existing technical and business documentation
6. Agent assumptions

A lower-priority instruction must not weaken a higher-priority constraint. Conflicts: follow the higher-priority rule, report the issue, avoid irreversible decisions.

## 3. Mandatory Reading

**Every task involving material repository work:** This file, the constitution, and the current task prompt.

**When relevant to the task:**

| Area | Required reading |
| --- | --- |
| Business behavior | `docs/business/business-description.md` |
| Technical changes | `docs/technical/technical-description.md` |
| Component work | `docs/technical/component-catalog.md` |
| Integrations | `docs/technical/integration-catalog.md`, relevant sequence diagrams |
| Data and persistence | `docs/technical/data-model.md` |
| Reliability or failures | `docs/technical/failure-modes.md` |
| Unresolved verification | `docs/technical/verification-backlog.md` |
| Security or privacy | Business description §17, technical description §22, failure modes, integration catalog |
| Deployment or infrastructure | ARM template, `parameters.json`, `deploy.ps1`, deployment-view diagram, initializer evidence |
| Governance | All applicable governance documents under `docs/agentic/` |

Purely mechanical, non-material work does not require reading unrelated documents.

Task-specific prompts may add required reading but must not weaken these baseline requirements.

## 4. Human Ownership

The human operator owns priorities, scope, version control, architectural decisions, business decisions, risk acceptance, release decisions, external-system access, and destructive actions.

Silence, missing instructions, or tool access is not approval. Agent output is advisory until explicitly accepted.

## 5. Before Modifying or Executing

Before any file modification, command with side effects, or external action, state a concise declaration:

- **Objective** — what the task requires
- **Allowed scope** — files and directories permitted
- **Prohibited operations** — what is forbidden
- **Permission level** — authorized action categories
- **Review level** — A, B, or C
- **Intended validation** — what will be verified and how
- **Expected outputs** — deliverables

This declaration is not required for pure reading, static inspection, or simple informational responses.

## 6. Default-Deny Permissions

The default is deny. Actions not listed as normally allowed require explicit task authorization.

**Normally allowed:** reading files, searching content, static inspection, read-only version-control inspection (status, diff, log, blame, branch/tag listing), reporting findings.

**Requires explicit authorization:** creating files; modifying files; moving files; deleting files; changing code, tests, documentation, or configuration; restoring dependencies; building; testing; running applications; executing scripts; changing dependencies; version-control writes; external calls; deployment; infrastructure changes; data changes; cleanup.

Repository access or tool availability is not authorization.

## 7. Version-Control Rules

Read-only inspection is allowed by default. All operations that modify version-control state require explicit task authorization. The human operator manages all versioning.

## 8. Scope, Least Change, and Task Decomposition

Make the smallest change that fully satisfies the objective. Do not perform opportunistic refactoring, formatting, renaming, dependency upgrades, cleanup, architecture changes, or unrelated fixes. Out-of-scope findings must be documented, not fixed.

Split a task when it contains more than one independently material concern with different decision boundaries, risk profiles, validation methods, or required reviewers.

When scope is ambiguous, choose the narrower safe interpretation and report the ambiguity.

## 9. Permission Non-Cascading

Permissions do not cascade. Each action category requires its own explicit authorization:

- Implementation permission does not authorize dependency changes
- Build permission does not authorize package upgrades
- Test permission does not authorize external calls
- Runtime permission does not authorize deployment
- File-modification permission does not authorize deletion
- Cleanup permission does not authorize unrelated restructuring
- Git read permission does not authorize Git writes

## 10. Materiality and Review Levels

### Level A — Self-review

Non-material changes only: no change to behavior, meaning, authority, or protected areas; mechanically verifiable; low and reversible impact.

### Level B — Independent review

Material changes with contained impact. Requires: independent review by a different reviewer → evidence-based disposition of material findings → human acceptance.

### Level C — Independent review and explicit human approval

High-risk changes: business rules, security, privacy, identity, persistence, external contracts, messaging, concurrency, dependencies, platform migration, infrastructure, deployment, destructive operations, biometric data, governance.

Requires: explicit scope, impact analysis, independent specialist review, disposition of every material finding, explicit human approval.

**Reviewer duties:** inspect primary evidence; verify claimed validation; challenge unsupported claims; record disposition for every material finding; keep unresolved findings visible.

### When uncertain

If a reasonable reviewer could disagree about whether the change affects behavior, authority, safety, contracts, or future decisions, treat it as material.

## 11. Mixed-Risk Tasks

Inseparable mixed-risk tasks inherit the highest applicable review level. A mixed task must never be downgraded because most changes are low risk.

If concerns are independently separable, split them into separate tasks with separate classifications.

## 12. Protected Areas

| Area | Protection | Category |
| --- | --- | --- |
| `README.md` (root) | Read-only | Historical artifact |
| `LICENSE.md` | Read-only | Legal artifact |
| `images/` | Read-only | Historical artifact |
| `Retailizer.Azure.Initializer/satya.jpg` | Read-only; no identity speculation | Sensitive data |
| `Retailizer.Azure.Initializer/` | Read-only; Level C for any execution | High-risk provisioning utility |
| `docs/agentic/agentic-constitution.md` | Read-only; §26 change process | Governance authority |
| `AGENTS.md` (root) | Level C governance change | Governance authority |
| `docs/agentic/*.md` (current and planned) | Level C governance change | Governance policy |
| `docs/business/*.md` | Read-only unless explicit task | Authoritative documentation |
| `docs/technical/*.md`, `docs/technical/diagrams/` | Read-only unless explicit task | Authoritative documentation |
| Production source (`Backend/`, `Common/`, `Reailizer.Job/`, `Retailizer.UWP/`) | Explicit task + Level B/C | Business logic and contracts |
| Configuration (`*.config`, `appsettings*.json`, `*.csproj`, `*.xproj`, `project.json`) | Explicit task required | Configuration contracts |
| `Retailizer.Azure/Templates/` | Read-only unless explicit deployment task | Infrastructure definitions |
| `Retailizer.sln` | Explicit task required | Solution structure |
| `.git/` | Never modify | Version-control internals |
| `.gitignore`, `.gitattributes` | Read-only | Version-control metadata |
| Files containing or referencing secrets | Reference by key name only; never expose values | Secret protection |

Full inventory: `docs/agentic/protected-assets.md` [planned]

## 13. Build, Test, Runtime, Scripts, and External Actions

This project uses an obsolete toolchain (ASP.NET Core 1.0 xproj, .NET Framework 4.5.2, UWP IoT, Project Oxford SDKs). Building requires a historical environment.

Each category requires its own explicit task permission:

- **Build/compile** — does not authorize modifying project files, upgrading toolchains, or changing dependencies
- **Package restore** — separate from build permission
- **Tests** — does not authorize package upgrades, destructive setup, or live external calls
- **Runtime execution** — does not authorize configuration changes or deployment
- **Scripts** — does not authorize infrastructure changes
- **External-service calls** — must identify target, action, environment, safeguards, and rollback
- **Deployment/infrastructure** — preparing manifests does not authorize execution

When validation is not permitted or possible, preserve explicit verification requirements for future tasks.

## 14. Evidence and Uncertainty

Material claims must be supported by evidence using these classifications:

- **Observed** — directly supported by project artifacts or executed validation
- **Documented** — explicitly stated in approved documentation
- **Inferred** — supported by multiple sources but not directly confirmed
- **Unknown** — cannot be determined from available evidence
- **Conflicting Evidence** — sources support incompatible conclusions
- **Verification Required** — needs build, runtime, deployment, external-service, or stakeholder verification

Do not present inference as fact, documented intention as implemented behavior, or package presence as proof of execution. Retain unknowns explicitly. A correct incomplete result is preferable to a complete fictional one.

## 15. Documentation and Implementation Conflicts

Approved business and technical documentation are the authoritative current reconstruction for baseline understanding. However:

- Agents must still inspect relevant implementation evidence
- Contradictions between documentation and source code must be reported as Conflicting Evidence or Verification Required
- Documentation must not be treated as infallible runtime proof
- Source code must not silently override approved documentation without reporting the conflict

## 16. Validation Honesty

Do not claim a change works unless verification was actually performed and passed. The completion report must distinguish: validation executed successfully; executed with failures; not executed (with reasons); prohibited by scope; impossible in current environment.

Static inspection alone is not a substitute for runtime verification when runtime behavior is material.

## 17. Security Findings Outside Scope

An agent must not automatically fix an out-of-scope security issue. When a security concern is discovered during unrelated work:

1. Stop only work made unsafe by the issue
2. Complete safe independent work
3. Report the finding without exposing secrets or unnecessary exploit detail
4. Provide a preliminary severity assessment
5. Recommend a separate Level C task

Immediate risk of data loss or external harm requires escalation and suspension of affected work.

## 18. Working Artifacts and Cleanup

Temporary analysis and review artifacts use separated paths: `docs/<topic>-analysis/` and `docs/<topic>-review/`. Final artifacts belong in authoritative locations.

Cleanup is never automatically authorized. Cleanup requires:

- Explicit task scope specifying what may be removed
- Verification that unique information is preserved in final artifacts
- Verification that no unresolved finding exists only in a file being removed
- Confirmation that no protected or historical file is affected

Do not delete files merely because they appear unused, old, duplicated, or obsolete.

## 19. Stop Conditions

Stop the unsafe or unauthorized part of the task and report when:

1. Required action exceeds assigned scope
2. A protected artifact requires unauthorized modification
3. A secret or external credential is required
4. Business behavior is ambiguous and requires a human decision
5. A destructive or irreversible action is required but not authorized
6. An external action is required but not authorized
7. Required validation cannot be performed or is prohibited
8. Unique information may be lost
9. Project artifacts materially conflict and safe resolution is not possible
10. A dependency, architecture, platform, or contract decision is required
11. Version-control modification is required but not authorized
12. A requested result would require presenting inference as fact
13. Sensitive data may be mishandled or exposed
14. A security finding creates immediate risk of harm or data loss
15. Concurrent or unexplained workspace changes make ownership unclear

Complete any clearly safe, independent portion of the task. Report each stop condition with: affected area, evidence, risk, completed safe work, and decision required.

## 20. Completion Report

Every task must conclude with a factual completion report:

1. Work performed
2. Files created, modified, moved, or removed
3. Validation performed and results
4. Validation not performed and reasons
5. Assumptions
6. Unresolved issues (including any Conflicting Evidence)
7. Out-of-scope findings
8. Review level applied and justification
9. Human decisions required
10. Compliance with task restrictions

Do not claim success more broadly than evidence supports.

## 21. Future Detailed Policies

The following planned policies may extend this file with granular rules. They are subordinate to the constitution and this file — they may extend but must not weaken these instructions.

- `docs/agentic/permission-matrix.md` [planned]
- `docs/agentic/task-protocol.md` [planned]
- `docs/agentic/verification-policy.md` [planned]
- `docs/agentic/protected-assets.md` [planned]
- `docs/agentic/task-template.md` [planned]

Until these exist: the constitution and this file remain controlling; absence of a detailed policy does not grant permission; task instructions must explicitly provide missing operational details.

## 22. Governance Changes

Changes to governance artifacts are Level C. This includes: this file, the Agentic Constitution, and all current or planned policy files under `docs/agentic/`.

Governance changes require: explicit scope, independent review by a different model or reviewer, evidence-based disposition of every material finding, and explicit human acceptance.

An agent must not change governance because a rule is inconvenient. Temporary task exceptions must be explicit, narrow, non-persistent, and unable to weaken the constitution.
