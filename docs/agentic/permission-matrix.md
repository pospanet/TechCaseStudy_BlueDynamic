# Repository Permission Matrix

## 1. Purpose

This policy defines reusable permission profiles for AI-agent tasks in this repository.

It translates the principles in:

* `AGENTS.md`
* `docs/agentic/agentic-constitution.md`

into explicit operational permissions.

This policy determines which categories of actions may be performed. It does not define the task objective or grant access to unspecified repository areas.

Every task that may modify files, execute commands with side effects, or perform an external action must identify:

* the applicable permission profile;
* any explicitly granted additional permissions;
* allowed files and directories;
* prohibited operations;
* review level;
* permitted validation;
* expected outputs.

Repository or tool access does not constitute permission.

---

## 2. Authority and Precedence

The order of authority is:

1. explicit human instruction for the current task;
2. `docs/agentic/agentic-constitution.md`;
3. root `AGENTS.md`;
4. this permission matrix;
5. other repository policies;
6. task-specific supporting documentation;
7. agent assumptions.

A task may grant a narrowly scoped permission that is denied by default.

A task must not weaken:

* constitutional human ownership;
* secret and sensitive-data protections;
* review requirements;
* governance-change protections;
* explicit human-approval requirements;
* instruction precedence.

When instructions conflict, follow the higher-authority rule and report the conflict.

---

## 3. Default Permission State

Unless explicitly authorized, an agent may perform only:

* repository file reading;
* repository content searching;
* static analysis;
* approved documentation reading;
* read-only version-control inspection;
* reporting and planning without modifying files.

All other actions are denied by default.

---

## 4. Permission Non-Cascading

Permission profiles do not inherit from one another.

A higher-risk profile does not automatically include the permissions of lower-numbered profiles.

Examples:

* implementation permission does not authorize dependency restore;
* build permission does not authorize source-code modification;
* test permission does not authorize application runtime execution;
* runtime permission does not authorize deployment;
* file-modification permission does not authorize file deletion;
* cleanup permission does not authorize unrelated repository restructuring;
* Git read permission does not authorize Git writes;
* external-call permission does not authorize external data modification;
* infrastructure-edit permission does not authorize infrastructure deployment.

Every required action category must be explicitly authorized.

---

## 5. Common Action Categories

The matrix uses the following action categories.

### Repository inspection

* read files;
* search content;
* inspect project structure;
* inspect configuration without exposing secret values;
* inspect approved documentation.

### Version-control inspection

* status;
* diff;
* log;
* blame;
* branch listing;
* tag listing.

### Documentation changes

* create documentation;
* modify documentation;
* move documentation;
* delete temporary documentation.

### Source changes

* create or modify production source code;
* create or modify tests;
* modify scripts;
* modify project or solution files.

### Dependency operations

* restore existing declared dependencies;
* add a dependency;
* upgrade a dependency;
* remove a dependency;
* change package sources;
* change framework or SDK versions.

### Execution

* compile;
* run unit tests;
* run integration tests;
* run characterization tests;
* run applications;
* run scripts;
* run concurrency or load experiments.

### Configuration and data

* modify local configuration;
* modify shared configuration;
* modify schemas;
* modify stored data;
* execute migrations;
* access sensitive data.

### External actions

* call external services;
* provision resources;
* deploy;
* change external service configuration;
* send messages;
* publish artifacts;
* perform destructive actions.

### Version-control writes

* create or switch branches;
* stage files;
* commit;
* merge;
* rebase;
* cherry-pick;
* reset;
* restore;
* stash;
* pull;
* fetch;
* push;
* create or delete tags.

---

# 6. Permission Profiles

## P0 — Read-Only Discovery

### Purpose

Use for:

* repository exploration;
* static analysis;
* evidence collection;
* architecture investigation;
* business investigation;
* planning;
* review without file output.

### Normally permitted

* read repository files;
* search repository content;
* inspect approved documentation;
* inspect configuration while redacting secret values;
* inspect read-only version-control information;
* produce a completion response without repository changes.

### Explicitly prohibited

* creating, modifying, moving, or deleting files;
* dependency restore;
* build;
* tests;
* runtime execution;
* script execution;
* external calls;
* version-control writes;
* cleanup.

### Default review level

Level A when producing only a non-authoritative response.

Level B when the analytical output will become an authoritative input to future engineering decisions.

### Typical examples

* identify repository projects;
* locate a code path;
* analyse a defect statically;
* inspect an existing document;
* prepare a proposed task plan.

---

## P1 — Documentation Write

### Purpose

Use for creating or changing documentation without changing application behavior.

### Normally permitted

Only within explicitly listed documentation paths:

* create documentation files;
* modify documentation files;
* create text-based diagrams;
* update documentation indexes;
* remove explicitly listed temporary documentation during authorized cleanup.

### Not automatically permitted

* source-code changes;
* project-file changes;
* configuration changes;
* build;
* dependency restore;
* tests;
* runtime execution;
* script execution;
* external calls;
* modification of protected historical artifacts;
* deletion outside explicit cleanup scope;
* version-control writes.

### Required task scope

The task must identify:

* writable documentation paths;
* final output paths;
* whether intermediate analysis or review artifacts may be created;
* exact cleanup scope;
* whether authoritative documentation is being changed.

### Default review level

Level A for non-semantic formatting or link repair.

Level B for authoritative business or technical documentation.

Level C for:

* governance documentation;
* security or privacy policy;
* permission boundaries;
* agent instructions;
* documents changing approved business semantics.

### Typical examples

* create a technical analysis draft;
* update a component catalog;
* prepare a reviewed business description;
* fix a broken internal documentation link.

---

## P2 — Build and Reproduction Experiment

### Purpose

Use for determining whether the historical project can be restored, compiled, or packaged without intentionally changing production behavior.

### Normally permitted when explicitly included

* restore already declared dependencies;
* compile specified projects;
* inspect compiler and restore output;
* create temporary local build outputs;
* document toolchain requirements;
* create explicitly authorized build-environment files.

### Not automatically permitted

* changing production source code;
* adding, upgrading, replacing, or removing dependencies;
* changing target frameworks;
* converting project formats;
* modifying package sources;
* running applications;
* invoking external services;
* deployment;
* committing generated outputs;
* version-control writes.

### Required safeguards

The task must define:

* projects to build;
* permitted restore operation;
* permitted toolchain;
* allowed environment changes;
* generated-output locations;
* cleanup expectations;
* whether failed-build diagnosis may create documentation.

### Default review level

Level B.

Level C when the experiment requires:

* obsolete or untrusted tooling;
* external package sources;
* privileged system changes;
* changes to build configuration;
* sensitive credentials.

### Typical examples

* determine whether the original solution restores;
* capture compiler failures;
* document missing SDK requirements;
* test a containerized legacy toolchain.

---

## P3 — Test and Characterization Baseline

### Purpose

Use for creating or executing tests that establish current legacy behavior.

### Normally permitted when explicitly scoped

* create or modify tests;
* create test fixtures using non-sensitive synthetic data;
* run specified tests;
* create mocks, fakes, or local test doubles;
* create characterization outputs;
* document observed behavior.

### Not automatically permitted

* changing production behavior;
* changing public contracts;
* using production data;
* calling live external services;
* changing dependencies;
* modifying deployment;
* changing shared configuration;
* version-control writes.

### Required safeguards

The task must define:

* test type;
* permitted test projects and paths;
* allowed production-code seams, if any;
* permitted dependency restore;
* external-service isolation;
* fixture-data restrictions;
* expected validation output.

### Default review level

Level B.

Level C when tests involve:

* security behavior;
* identity or biometric behavior;
* business-rule interpretation;
* persistent data;
* concurrency;
* live external systems.

### Typical examples

* add characterization tests for visit correlation;
* create contract tests for a message schema;
* capture current behavior for duplicate events;
* verify serialization of legacy DTOs.

---

## P4 — Controlled Implementation

### Purpose

Use for contained changes to production code with a clearly defined behavior and verification strategy.

### Normally permitted only within explicitly listed paths

* create or modify production source code;
* create or modify relevant tests;
* update directly affected non-authoritative documentation;
* run explicitly authorized build and test commands.

### Not automatically permitted

* dependency changes;
* framework changes;
* public contract changes;
* database-schema changes;
* business-semantic changes;
* configuration-contract changes;
* infrastructure changes;
* deployment;
* external-service calls;
* data migration;
* deletion of unrelated files;
* version-control writes.

### Required task definition

The task must specify:

* affected component;
* intended behavior;
* protected behavior;
* writable paths;
* required tests;
* permitted build commands;
* permitted runtime execution;
* compatibility expectations;
* rollback or reversal approach.

### Default review level

Level B.

Level C when the implementation affects any protected high-risk area.

### Typical examples

* fix a local null-handling defect;
* improve resource disposal without changing contracts;
* add local input validation;
* implement a contained internal helper.

---

## P5 — Protected Behavior and Contract Change

### Purpose

Use for changes affecting behavior or contracts whose correctness is essential to system meaning or compatibility.

This includes:

* business rules;
* event semantics;
* visit correlation;
* identity behavior;
* tenant or store scope;
* APIs;
* message schemas;
* serialized formats;
* configuration contracts;
* persistence behavior;
* database schemas;
* migration logic;
* concurrency;
* ordering;
* retries;
* idempotence;
* security controls.

### Required permissions

Every allowed action must be listed explicitly.

No action is implied merely by selecting P5.

### Required preparation

Before implementation, the task must include:

* current-state analysis;
* affected contracts or rules;
* compatibility impact;
* data impact;
* failure scenarios;
* verification plan;
* rollback or recovery considerations;
* required reviewer expertise.

### Review level

Always Level C.

### Human approval

Explicit human approval is required:

* before implementation when the task changes approved semantics or data;
* after independent review before acceptance;
* before executing migrations or external changes.

### Typical examples

* change visit-completion rules;
* add message-version fields;
* modify tenant filtering;
* change persistence identity;
* implement idempotent queue processing;
* change authentication or authorization.

---

## P6 — Dependency, Framework, and Platform Change

### Purpose

Use for:

* dependency additions;
* dependency upgrades or removals;
* SDK changes;
* target-framework changes;
* project-format conversion;
* runtime-platform migration;
* replacement of obsolete managed services.

### Required preparation

The task must include:

* dependency or platform inventory;
* compatibility analysis;
* affected projects;
* transitive impact;
* contract impact;
* build and test strategy;
* migration sequence;
* rollback strategy;
* licensing and source considerations;
* security implications.

### Not automatically permitted

* application redesign;
* business-rule changes;
* data migration;
* infrastructure deployment;
* removal of historical artifacts;
* live external-service changes;
* version-control writes.

### Review level

Always Level C.

### Human approval

Explicit human approval is required before implementation and before acceptance.

### Typical examples

* migrate from an obsolete project format;
* upgrade target framework;
* replace a retired SDK;
* change package-management infrastructure.

---

## P7 — Infrastructure, Deployment, and External Action

### Purpose

Use for operations that affect systems outside the local repository workspace.

This includes:

* infrastructure provisioning;
* deployment;
* service configuration;
* database migration execution;
* external API calls that mutate state;
* credential rotation;
* publishing;
* sending messages;
* data deletion;
* shared-environment modification.

### Default state

Denied.

Selecting P7 alone does not authorize an external action.

### Required explicit authorization

The task must identify:

* exact target;
* environment;
* operation;
* credentials or identity mechanism;
* data involved;
* preconditions;
* validation;
* rollback;
* expected external effect;
* whether dry-run or plan-only mode is required;
* human approval checkpoint.

### Review level

Always Level C.

### Human approval

Explicit human approval is required immediately before any external, destructive, billed, or irreversible action.

Approval of a plan does not automatically approve execution.

### Typical examples

* deploy an application;
* provision Azure resources;
* execute a database migration;
* rotate an API key;
* modify a Service Bus queue;
* call a live Face API;
* delete cloud data.

---

## P8 — Governance Change

### Purpose

Use for creating or modifying repository governance.

Governance artifacts include:

* root `AGENTS.md`;
* `docs/agentic/agentic-constitution.md`;
* this permission matrix;
* task protocol;
* verification policy;
* protected-assets policy;
* governance task templates;
* other documents defining agent authority or required behavior.

### Required workflow

1. explicit human scope;
2. analysis;
3. draft;
4. independent review by a different model;
5. evidence-based disposition of findings;
6. corrected final output;
7. explicit human acceptance;
8. cleanup only when explicitly authorized.

### Not permitted

An agent must not modify governance:

* because a rule is inconvenient;
* as an incidental part of another task;
* to retroactively authorize an action;
* to lower the review level of current work;
* without explicit human scope.

### Review level

Always Level C.

### Human approval

Explicit human acceptance is required before the change becomes authoritative.

---

# 7. Action Matrix

Legend:

* **A** — normally allowed by the profile, within explicit task scope;
* **E** — requires an additional explicit permission;
* **D** — denied unless the task is reclassified or separately authorized;
* **H** — explicit human approval required immediately before action;
* **N/A** — not applicable to the profile.

| Action                                   | P0 | P1 | P2 | P3 | P4 | P5 | P6 | P7 | P8 |
| ---------------------------------------- | -: | -: | -: | -: | -: | -: | -: | -: | -: |
| Read repository files                    |  A |  A |  A |  A |  A |  A |  A |  A |  A |
| Search repository content                |  A |  A |  A |  A |  A |  A |  A |  A |  A |
| Read-only Git inspection                 |  A |  A |  A |  A |  A |  A |  A |  A |  A |
| Create documentation                     |  D |  A |  E |  E |  E |  E |  E |  E |  A |
| Modify authoritative documentation       |  D |  E |  D |  D |  E |  E |  E |  E |  A |
| Modify production code                   |  D |  D |  D |  E |  A |  E |  E |  E |  D |
| Modify tests                             |  D |  D |  D |  A |  E |  E |  E |  E |  D |
| Restore declared dependencies            |  D |  D |  A |  E |  E |  E |  A |  E |  D |
| Add or change dependencies               |  D |  D |  D |  D |  D |  E |  A |  E |  D |
| Compile                                  |  D |  D |  A |  E |  E |  E |  A |  E |  D |
| Run unit tests                           |  D |  D |  D |  A |  E |  E |  E |  E |  D |
| Run integration tests                    |  D |  D |  D |  E |  E |  E |  E |  E |  D |
| Run application locally                  |  D |  D |  D |  E |  E |  E |  E |  E |  D |
| Execute repository scripts               |  D |  D |  D |  E |  E |  E |  E |  E |  D |
| Modify local configuration               |  D |  D |  E |  E |  E |  E |  E |  E |  D |
| Modify shared configuration              |  D |  D |  D |  D |  D |  E |  E |  H |  D |
| Modify contracts or schemas              |  D |  D |  D |  D |  D |  E |  E |  E |  D |
| Modify business rules                    |  D |  D |  D |  D |  D |  E |  D |  D |  D |
| Modify security controls                 |  D |  D |  D |  D |  D |  E |  E |  E |  D |
| Access sensitive test data               |  D |  D |  D |  E |  E |  E |  E |  E |  D |
| Access production data                   |  D |  D |  D |  D |  D |  D |  D |  H |  D |
| Call read-only external service          |  D |  D |  D |  D |  D |  E |  E |  H |  D |
| Mutate external service                  |  D |  D |  D |  D |  D |  D |  D |  H |  D |
| Provision infrastructure                 |  D |  D |  D |  D |  D |  D |  E |  H |  D |
| Deploy                                   |  D |  D |  D |  D |  D |  D |  D |  H |  D |
| Execute data migration                   |  D |  D |  D |  D |  D |  E |  E |  H |  D |
| Delete explicitly listed temporary files |  D |  E |  E |  E |  E |  E |  E |  E |  E |
| Delete non-generated repository files    |  D |  D |  D |  D |  D |  E |  E |  H |  E |
| Git write operations                     |  D |  D |  D |  D |  D |  D |  D |  E |  D |

An `E` entry does not mean the agent may grant the permission to itself. The current task or human operator must grant it explicitly.

---

# 8. Review-Level Mapping

| Profile | Normal minimum | Escalate to Level C when                                                                                          |
| ------- | -------------- | ----------------------------------------------------------------------------------------------------------------- |
| P0      | Level A or B   | Output becomes authoritative governance, security, privacy, business, or architecture guidance                    |
| P1      | Level B        | Governance, protected semantics, security/privacy policy, or sensitive-data handling                              |
| P2      | Level B        | Privileged tools, untrusted packages, configuration changes, external sources, or system-wide environment changes |
| P3      | Level B        | Identity, security, persistence, concurrency, business behavior, or sensitive data                                |
| P4      | Level B        | Any protected behavior, contract, security, data, dependency, or deployment impact                                |
| P5      | Level C        | Always                                                                                                            |
| P6      | Level C        | Always                                                                                                            |
| P7      | Level C        | Always                                                                                                            |
| P8      | Level C        | Always                                                                                                            |

When a task spans multiple inseparable profiles, use the highest applicable review level.

When concerns are separable, split them into independently scoped tasks.

---

# 9. Explicit Additional Permissions

A task may extend a profile using narrowly scoped additional permissions.

Example:

```text
Primary permission profile:
P4 — Controlled Implementation

Additional permissions:
- Restore already declared NuGet dependencies.
- Compile `Common` and `Reailizer.Job`.
- Run unit tests under `Tests/VisitCorrelation/`.
- Update `docs/technical/verification-backlog.md`.

Not permitted:
- Dependency version changes.
- External-service calls.
- Configuration changes.
- Git write operations.
- File deletion.
```

Additional permissions must identify:

* exact action;
* exact target or path;
* applicable environment;
* relevant limits;
* whether the permission includes cleanup;
* whether human approval is required before execution.

Broad statements such as these are insufficient:

* “do whatever is needed”;
* “fix all related issues”;
* “you have full access”;
* “use best judgement”;
* “make it work.”

---

# 10. File-Scope Rules

A permission profile does not grant repository-wide write access.

Every write-enabled task must define one or more:

* writable files;
* writable directories;
* file-type constraints;
* generated-output directories;
* exact deletion paths.

The agent must not modify adjacent files merely because they are technically related.

Out-of-scope changes must be reported rather than applied.

If a required change falls outside the allowed file scope:

1. stop that part of the task;
2. complete safe in-scope work;
3. report the required extension;
4. wait for explicit authorization.

---

# 11. Generated and Temporary Artifacts

Creating generated or temporary artifacts requires explicit permission.

The task must specify:

* permitted location;
* purpose;
* retention or cleanup requirement;
* whether the artifact may contain sensitive information;
* whether it is authoritative or disposable.

Temporary artifacts must not be placed in historical or authoritative documentation paths.

Build outputs, restored packages, logs, caches, and generated binaries must not be treated as repository deliverables unless explicitly requested.

---

# 12. Sensitive Data

No profile automatically authorizes copying or repurposing sensitive data.

Sensitive data includes:

* credentials;
* tokens;
* private keys;
* connection-string values;
* biometric images;
* identity identifiers;
* personal data;
* production payloads;
* customer data.

When a task requires limited sensitive-data inspection:

* use the minimum necessary access;
* do not reproduce values;
* do not include values in prompts, documentation, logs, fixtures, or reports;
* prefer synthetic data for tests;
* report only file, key name, handling pattern, and risk.

Live or production sensitive-data access requires P7 and explicit human approval.

---

# 13. Security Findings Outside Scope

Discovering a security issue does not expand task permissions.

The agent must:

1. stop only work made unsafe by the finding;
2. complete safe independent work;
3. report the issue without secrets or unnecessary exploit detail;
4. provide a preliminary severity;
5. recommend a separate P5 or P7 Level C task as appropriate.

Immediate risk of external harm or data loss requires suspension and human escalation.

---

# 14. Cleanup Permissions

Cleanup is a separate permission category.

Permission to create or modify files does not authorize their deletion.

Every cleanup authorization must define:

* exact files or directories;
* safety gate;
* final authoritative outputs;
* unique-information preservation;
* unresolved-finding preservation;
* historical-artifact protection.

Before deletion, verify:

* final outputs exist;
* final outputs are self-contained;
* no unique evidence will be lost;
* no unresolved finding exists only in a deleted file;
* no remaining document links to the deleted artifact;
* deletion does not exceed the exact authorized scope.

---

# 15. Version-Control Permissions

Read-only version-control inspection is normally allowed.

All version-control writes require explicit, operation-specific authorization.

Authorization to perform one Git operation does not authorize another.

Examples:

* permission to create a branch does not authorize committing;
* permission to commit does not authorize pushing;
* permission to fetch does not authorize merging;
* permission to inspect diff does not authorize restoring files.

Unless explicitly authorized, the human operator manages all versioning.

---

# 16. External-Action Approval Gate

For P7 actions, the agent must present a final pre-execution declaration containing:

* exact external target;
* exact operation;
* environment;
* expected effect;
* data affected;
* credentials or identity mechanism;
* validation;
* rollback;
* destructive or billed consequences;
* current human approval status.

The agent must not proceed until explicit approval is provided for that specific action.

Earlier approval of analysis, planning, code preparation, or deployment artifacts does not count as execution approval.

---

# 17. Task-Start Declaration

Before modifying files, executing a side-effecting command, or performing an external action, the agent must state:

* objective;
* primary permission profile;
* additional permissions;
* allowed file scope;
* prohibited operations;
* review level;
* intended validation;
* expected outputs.

Pure reading and static inspection do not require this declaration.

---

# 18. Completion Report

Every write-enabled or side-effecting task must report:

* primary permission profile;
* additional permissions used;
* review classification;
* work completed;
* files created;
* files modified;
* files moved;
* files removed;
* commands executed;
* validation performed and results;
* validation not performed and reasons;
* external actions performed;
* assumptions;
* unresolved issues;
* out-of-scope findings;
* human decisions still required;
* compliance with restrictions.

The report must not claim permissions, validation, or success beyond what actually occurred.

---

# 19. Missing or Ambiguous Permission

When a required permission is absent or ambiguous:

1. do not assume permission;
2. choose the narrower safe interpretation;
3. continue only clearly permitted work;
4. report the missing permission;
5. request or await explicit authorization for the blocked action.

Absence of a detailed supporting policy does not grant permission.

---

# 20. Policy Changes

This permission matrix is a governance artifact.

Changes require:

* P8 — Governance Change;
* explicit human scope;
* independent review by a different model;
* disposition of material findings;
* explicit human acceptance.

An agent must not modify this matrix to authorize its current task.
