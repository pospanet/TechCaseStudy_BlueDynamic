# Mission: Independent Review of the Retailizer Technical Analysis

## Role

Act as an independent senior software architect, distributed-systems reviewer, cloud-integration specialist, reliability engineer, security reviewer, and evidence auditor.

You are reviewing an evidence-based technical analysis of Retailizer, a real historical software project.

The analysis was created by another model. Do not assume that its conclusions are correct.

Your responsibility is to test the analysis independently against the actual project files and identify:

* factual errors;
* missing components or code paths;
* unsupported architectural claims;
* incorrect evidence classifications;
* incorrect runtime assumptions;
* inconsistencies among the analytical documents;
* misinterpretation of framework or cloud behavior;
* overlooked reliability, concurrency, persistence, security, or operational concerns;
* infrastructure resources incorrectly presented as actively used;
* intended behavior incorrectly presented as observed behavior;
* static conclusions that actually require build or runtime verification.

This is the second stage of a controlled technical-documentation workflow:

1. evidence-based technical analysis;
2. independent technical review by a different model;
3. reviewed final technical description produced later by the original author.

Your output is a review document.

You must not rewrite, correct, or replace the original technical-analysis files.

## Critical operating restriction: no Git operations

Do not perform any Git operation.

You must not:

* create or switch branches;
* create commits;
* inspect commit history;
* inspect tags;
* run Git status or Git diff;
* stage files;
* reset, restore, merge, rebase, cherry-pick, or stash;
* modify `.git` data;
* make any version-control decision.

The human operator manages all versioning manually.

Analyse only the files currently present in the workspace.

## Static-review restriction

This is a static independent review.

Do not:

* build the project;
* restore packages;
* run the applications;
* execute tests;
* create a compatibility environment;
* install historical SDKs;
* deploy infrastructure;
* call live cloud services;
* invoke initialization utilities;
* change configuration;
* create test harnesses;
* modify source code.

Where the truth cannot be established statically, require the appropriate later verification step.

## Required review inputs

Review all files under:

`docs/technical-analysis/`

Expected inputs include:

* `technical-analysis.md`
* `component-catalog.md`
* `integration-catalog.md`
* `data-model-catalog.md`
* `failure-mode-catalog.md`
* `technical-open-questions.md`
* `technical-evidence-index.md`
* all Mermaid diagrams under `docs/technical-analysis/diagrams/`

Also independently inspect the full Retailizer workspace, including:

* root `README.md`;
* existing documentation;
* solution files;
* project files;
* package and dependency manifests;
* source code;
* executable entry points;
* background tasks;
* controllers and API endpoints;
* DTOs and persistence entities;
* service implementations;
* configuration;
* infrastructure and deployment definitions;
* scripts;
* initialization utilities;
* comments and TODOs;
* sample payloads;
* technically relevant sample assets.

The project artifacts are the evidence base.

The technical-analysis documents are a claim set to be tested.

Do not rely only on the files, symbols, or interpretations cited by the original author.

Verify important conclusions directly.

## Historical-project principle

Retailizer is a real historical project.

Review the technical choices in the context of the period when the project was created.

Do not describe a technology, SDK, project format, or cloud service as an original design defect merely because it is obsolete today.

Distinguish:

* historically reasonable technical choices;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete implementation;
* unused or experimental components;
* external platform obsolescence;
* current build or operational limitations;
* current security or privacy implications.

Do not propose modernization or target architecture in this mission.

## Evidence classifications to verify

The original analysis should use these classifications:

### Observed

Directly supported by implementation artifacts.

### Documented

Explicitly stated in project documentation.

### Inferred

A reasoned interpretation supported by multiple artifacts but not explicitly confirmed.

### Unknown

Cannot be determined from the current workspace.

### Conflicting evidence

Different project artifacts support incompatible conclusions.

### Build verification required

Restoring or compiling is necessary to establish the conclusion.

### Runtime verification required

Static analysis cannot establish actual runtime behavior.

### External-service verification required

The conclusion depends on historical or live behavior of an external service.

### Stakeholder confirmation required

The original design intent cannot be safely reconstructed from technical artifacts.

Identify cases where the author:

* used the wrong classification;
* presented an inference as observed behavior;
* treated documentation as proof of implementation;
* treated declared infrastructure as proof of usage;
* treated a package reference as proof of runtime behavior;
* failed to require build or runtime verification;
* marked something unknown despite available evidence;
* failed to expose conflicting evidence;
* inferred framework defaults without sufficient support.

## Core review principles

### Independence

Perform your own code and artifact inspection.

Do not merely comment on the author’s prose.

### Static evidence discipline

Do not claim runtime behavior that cannot be established statically.

### End-to-end completeness

Do not verify components only in isolation.

Check whether the described end-to-end flows are actually connected by code, configuration, deployment artifacts, and contracts.

### Historical accuracy

Do not evaluate the system as though it were designed for current platforms.

### No defect invention

Be critical, but do not invent findings merely to produce a severe review.

## Mandatory review areas

### 1. Repository and project inventory

Verify that the analysis identified:

* every solution;
* every project;
* every executable;
* every library;
* initialization and deployment utilities;
* relevant scripts;
* device-side and cloud-side components;
* reporting or analytics artifacts.

Check for:

* omitted projects;
* incorrect project types;
* incorrect target frameworks;
* incorrect executable status;
* incorrect component ownership;
* missing references among projects.

### 2. Technology and dependency inventory

Verify:

* target frameworks;
* project formats;
* package versions;
* SDK usage;
* Azure dependencies;
* Windows IoT or UWP dependencies;
* web framework dependencies;
* persistence, messaging, storage, and image-analysis libraries.

Look for:

* referenced but unused dependencies;
* dependencies used transitively but not described;
* incorrect version claims;
* framework behavior assumed without evidence;
* availability or build issues presented as certain without verification.

### 3. Executable entry points

Independently identify all startup mechanisms.

Verify the analysis of:

* device background startup;
* backend API startup;
* queue or WebJob startup;
* initializer startup;
* deployment entry points;
* framework-discovered handlers.

Check whether any component could be invoked through framework conventions rather than direct references.

### 4. Component architecture

Review every item in `component-catalog.md`.

For each component verify:

* responsibility;
* actual location;
* inputs and outputs;
* state ownership;
* direct dependencies;
* deployment target;
* trust boundary;
* failure behavior;
* verification status.

Identify:

* missing components;
* components split or combined incorrectly;
* helper classes presented as standalone runtime components;
* runtime components presented as libraries;
* infrastructure resources presented as application components without qualification.

### 5. End-to-end runtime flow

Independently trace the primary flow:

1. camera enumeration;
2. media initialization;
3. frame capture;
4. local face detection;
5. face crop generation;
6. image upload;
7. message construction;
8. IoT Hub submission;
9. routing or stream processing;
10. Service Bus delivery;
11. queue-triggered processing;
12. image retrieval;
13. face analysis;
14. person identification or creation;
15. event creation;
16. visit correlation;
17. persistence;
18. reporting or analytics consumption.

For every hop verify:

* producer;
* consumer;
* payload;
* resource name or endpoint;
* protocol or SDK;
* authentication;
* configuration;
* retry behavior;
* missing links;
* evidence classification.

Identify any point where the original analysis created an apparently complete flow by joining artifacts that are not demonstrably connected.

### 6. Device-registration flow

Trace the registration process independently.

Verify claims about:

* device identifier creation;
* request format;
* API endpoint;
* request validation;
* authorization;
* IoT Hub registry interaction;
* key return;
* local credential storage;
* duplicate registration;
* later use of credentials.

Pay particular attention to claims that registration is unauthenticated.

A valid finding may state:

* no authentication or authorization mechanism was found in the inspected workspace;
* protection may have existed outside the repository;
* deployment confirmation is required.

Do not convert absence of evidence into definitive proof of public exposure.

### 7. Device capture and local processing

Inspect:

* capture-loop behavior;
* delay or throttling;
* cancellation;
* asynchronous invocation;
* exception propagation;
* stream and image disposal;
* concurrency;
* memory pressure;
* upload scheduling;
* camera lifecycle.

Verify every reliability concern in the original analysis.

Look specifically for:

* unawaited tasks;
* `async void`;
* fire-and-forget calls;
* infinite loops;
* missing cancellation;
* simultaneous frame processing;
* resource ownership ambiguity;
* error paths that stop capture;
* error paths that silently continue.

Do not report a risk if language or framework semantics contradict it.

### 8. Messaging and integration contracts

Review all entries in `integration-catalog.md`.

Verify:

* actual producers and consumers;
* payload fields;
* serialization;
* configuration source;
* authentication;
* message routing;
* delivery semantics;
* correlation support;
* duplicate detection;
* tenant and store propagation;
* retry and failure behavior.

Check whether framework defaults have been presented as configured behavior.

Check whether DTO reuse has been presented as an explicit versioned contract.

### 9. Event-processing flow

Independently trace:

* trigger method;
* message parsing;
* storage retrieval;
* external analysis call;
* face identification;
* person creation;
* event creation;
* event typing;
* persistence;
* visit lookup;
* visit creation or update;
* failure propagation.

Identify:

* omitted external calls;
* inconsistent sequence;
* persistence before or after correlation;
* partial-write risks;
* retry duplication risks;
* incorrect assumptions about Face API results;
* ambiguous error behavior.

### 10. Visit-correlation algorithm

Review the implemented logic in detail.

Verify claims concerning:

* active-visit lookup;
* matching criteria;
* person matching;
* tenant or store constraints;
* event ordering;
* new-visit creation;
* visit completion;
* `enter`, `payment`, and `leave`;
* duplicate events;
* missing events;
* concurrent processing;
* race conditions;
* consistency;
* idempotence.

This is a high-risk review area.

Identify where the original analysis:

* described a business rule not explicitly implemented;
* missed a possible race;
* inferred deterministic ordering from queue delivery;
* assumed uniqueness without enforcement;
* assumed transactionality across multiple writes;
* overlooked null, empty, or low-confidence results.

### 11. Data and message model

Review every entry in `data-model-catalog.md`.

Verify:

* identifiers;
* relationships;
* timestamps;
* persistence annotations or conventions;
* serialization;
* mutability;
* tenant and store scope;
* default values;
* invariants;
* ambiguity.

Check whether the analysis inferred domain guarantees from DTO shape alone.

Identify fields that appear declared but unused or populated inconsistently.

### 12. Persistence

Independently inspect persistence behavior.

Verify:

* database and collection names;
* initialization;
* reads and writes;
* query filters;
* IDs;
* partitioning evidence;
* uniqueness;
* updates;
* consistency;
* concurrency handling;
* deletion;
* retention;
* tenant filtering.

Look for:

* lost-update risks;
* read-modify-write races;
* non-atomic multi-document behavior;
* unrestricted queries;
* cross-tenant lookup risk;
* insert-versus-update ambiguity;
* collection provisioning not matched by actual usage.

Do not assume current Cosmos DB behavior applies to historical DocumentDB SDK behavior without qualification.

### 13. Infrastructure and deployment topology

Compare the written analysis and Mermaid diagrams against deployment artifacts.

Verify:

* every deployed resource;
* connections among resources;
* application settings;
* outputs;
* queue or topic names;
* Stream Analytics inputs and outputs;
* hosting relationships;
* external API resources;
* Power BI or reporting paths.

Identify:

* provisioned but unused resources;
* code-referenced but unprovisioned resources;
* inconsistent names;
* missing configuration bindings;
* topology links shown in diagrams without evidence.

### 14. Configuration and secrets

Review claims concerning:

* app settings;
* connection strings;
* API keys;
* endpoints;
* device credential persistence;
* environment separation;
* placeholders;
* hard-coded values;
* privilege scope;
* validation;
* secret rotation.

Do not copy secret values into the review.

If a secret-looking value exists, refer only to:

* file;
* key name;
* handling pattern;
* risk.

Check whether placeholders were incorrectly treated as real credentials or vice versa.

### 15. Error handling and failure modes

Review `failure-mode-catalog.md`.

Verify every material failure mode against code.

Look for missing failure modes involving:

* camera initialization;
* image capture;
* face detection;
* image upload;
* IoT message submission;
* stream routing;
* queue deserialization;
* missing blob;
* external API throttling;
* failed identity creation;
* failed persistence;
* partial visit update;
* malformed configuration;
* null or empty external-service responses.

Check:

* whether exceptions are caught;
* whether retries exist;
* whether state is partially changed;
* whether messages will be retried;
* whether failures are visible operationally.

### 16. Concurrency, retries, idempotence, and backpressure

This is a mandatory deep-review area.

Independently inspect:

* all async methods;
* unawaited operations;
* retry policies;
* retry limits;
* exponential backoff;
* jitter;
* framework-trigger concurrency;
* duplicate message behavior;
* idempotency mechanisms;
* message IDs;
* visit-update races;
* queue accumulation;
* external API throttling;
* cancellation.

Identify where the original analysis:

* overstates a risk;
* misses a risk;
* assumes single-threaded processing;
* assumes ordered delivery;
* assumes exactly-once delivery;
* assumes atomicity;
* relies on undocumented framework defaults.

### 17. Observability and operations

Verify claims about:

* logging;
* debug output;
* structured logs;
* correlation IDs;
* metrics;
* tracing;
* health checks;
* queue monitoring;
* poison-message handling;
* dead-letter behavior;
* alerts;
* operational dashboards.

Distinguish:

* implemented telemetry;
* framework-default telemetry;
* TODOs;
* documented intention;
* infrastructure capability that was not configured.

### 18. Security and trust boundaries

Review technical security claims concerning:

* device trust;
* backend endpoint access;
* device credentials;
* storage credentials;
* Service Bus credentials;
* database credentials;
* external API credentials;
* authentication;
* authorization;
* input validation;
* tenant isolation;
* transport security;
* privilege boundaries;
* image and biometric-data exposure.

Check for:

* claims that exceed available evidence;
* missing trust boundaries;
* unvalidated external input;
* credentials returned or persisted insecurely;
* tenant identifiers accepted from clients;
* broad connection strings;
* absent authorization checks;
* deployment controls that may exist outside the repository.

This is not a full threat model, but significant omissions must be reported.

### 19. Build and execution assumptions

Verify the stated toolchain and runtime assumptions.

Check:

* target frameworks;
* expected Visual Studio generation;
* UWP or IoT SDK requirements;
* operating system;
* device hardware;
* Azure service availability;
* package availability;
* deployment assumptions;
* local configuration requirements.

Ensure the original analysis distinguishes:

* directly declared;
* documented;
* inferred;
* build verification required;
* runtime verification required.

### 20. Incomplete, unused, and contradictory elements

Review all findings about:

* unused projects;
* unused classes;
* unused service clients;
* unused infrastructure;
* incomplete methods;
* TODOs;
* naming inconsistencies;
* misspelled directories or projects;
* dead code;
* experimental assets;
* documentation divergence.

Do not accept a claim of unused code based only on lack of direct textual reference.

Consider:

* dependency injection;
* reflection;
* framework conventions;
* configuration-based discovery;
* deployment triggers;
* serialization.

### 21. Diagram review

Review every Mermaid diagram for:

* consistency with code;
* consistency with the written analysis;
* missing components;
* incorrect direction;
* unsupported links;
* incorrect trust boundaries;
* inaccurate sequence order;
* conflation of intended and implemented behavior;
* unclear uncertainty notation.

Also inspect Mermaid syntax manually.

Do not install rendering tools.

## Review finding severity

Classify each finding as one of the following.

### Critical

The analysis materially misrepresents:

* core architecture;
* end-to-end data flow;
* security boundary;
* persistence behavior;
* visit-correlation behavior;
* major runtime component;
* external-service integration;
* a severe reliability property.

### Major

An important conclusion is incorrect, unsupported, contradictory, or materially incomplete.

### Moderate

A meaningful ambiguity, missing code path, incomplete failure mode, weak evidence reference, or incorrect verification classification exists.

### Minor

A local precision, naming, diagram, formatting, or traceability issue.

### Observation

A useful caution or improvement that is not an actual defect.

## Finding identifiers

Use stable identifiers:

* `TREV-CRIT-001`
* `TREV-MAJ-001`
* `TREV-MOD-001`
* `TREV-MIN-001`
* `TREV-OBS-001`

Do not renumber findings later within this review.

## Required finding format

For every finding include:

* identifier;
* severity;
* technical area;
* affected analytical file;
* affected section, catalog item, or diagram;
* claim under review;
* reviewer assessment;
* original evidence classification;
* recommended evidence classification;
* independently verified evidence;
* technical impact;
* required correction;
* required verification step;
* stakeholder confirmation requirement.

Example:

```markdown
### TREV-MAJ-001 — Visit update is described as idempotent without evidence

**Area:** Reliability and persistence

**Affected artifact:** `technical-analysis.md`, section 21

**Claim under review:** Reprocessing a queue message does not create duplicate
events or duplicate visit state.

**Reviewer assessment:** No idempotency key, message-deduplication record, or
event uniqueness enforcement was found. The queue-triggered flow appears able
to repeat persistence operations after retry.

**Original evidence classification:** Observed

**Recommended evidence classification:** Unknown, with runtime verification
required; duplicate-processing risk is observed.

**Evidence:**
- `<repository-relative file and symbol>`
- `<repository-relative file and symbol>`

**Technical impact:** Retried or duplicated messages may distort visits and
analytics.

**Required correction:** Remove the idempotency claim and describe the
evidence-backed duplicate-processing risk.

**Required verification:** Characterization test with repeated identical
messages.

**Stakeholder confirmation required:** No
```

## Minimum verification sample

Independently verify at minimum:

* all projects and executable entry points;
* all components classified as runtime components;
* all integrations in the integration catalog;
* all Critical and High-impact failure modes;
* at least fifteen major architectural conclusions;
* at least ten data-model conclusions;
* all claims about authentication and authorization;
* all claims about tenant isolation;
* all claims about retries and idempotence;
* all visit-correlation claims;
* all diagram edges representing external integrations;
* all claims classified as conflicting evidence.

Record this coverage in the appendix.

## Required output directory

Create documentation only under:

`docs/technical-review/`

If the directory does not exist, create it.

Do not create or change files outside this directory.

## Required output file

Create:

`docs/technical-review/technical-analysis-review.md`

Use this structure:

1. Document status and purpose
2. Review scope
3. Review method
4. Inputs reviewed
5. Review limitations
6. Overall assessment
7. Summary of findings
8. Critical findings
9. Major findings
10. Moderate findings
11. Minor findings
12. Observations
13. Missing components or code paths
14. Unsupported or overconfident claims
15. Evidence-classification issues
16. Repository and dependency assessment
17. Runtime-flow assessment
18. Device-registration assessment
19. Capture and local-processing assessment
20. Messaging and integration assessment
21. Event-processing assessment
22. Visit-correlation assessment
23. Data-model and persistence assessment
24. Infrastructure and deployment assessment
25. Configuration and secret-handling assessment
26. Reliability and failure-mode assessment
27. Concurrency, retry, idempotence, and backpressure assessment
28. Observability and operations assessment
29. Security and trust-boundary assessment
30. Build and runtime verification assessment
31. Diagram assessment
32. Internal-consistency assessment
33. Required corrections before finalization
34. Required future verification experiments
35. Stakeholder questions introduced or changed by review
36. Review coverage appendix

Clearly mark the document as:

`Independent technical review — original analysis not modified`

## Required summary tables

Include a finding summary:

| ID | Severity | Area | Required action | Verification required |
| -- | -------- | ---- | --------------- | --------------------- |

Include a verification summary:

| Verification type | Required question | Priority       | Expected evidence |
| ----------------- | ----------------- | -------------- | ----------------- |
| Build             | ...               | Blocking       | ...               |
| Runtime           | ...               | Important      | ...               |
| External service  | ...               | Important      | ...               |
| Stakeholder       | ...               | Useful context | ...               |

## Reviewer restrictions

You may create or modify only:

`docs/technical-review/technical-analysis-review.md`

Do not modify:

* any file under `docs/technical-analysis/`;
* business documentation;
* source code;
* tests;
* project files;
* dependencies;
* configuration;
* deployment templates;
* scripts;
* root README;
* sample assets;
* existing historical artifacts;
* Git metadata.

Do not:

* rewrite the technical analysis;
* correct catalogs directly;
* correct diagrams directly;
* create the final technical description;
* propose modernization;
* propose target architecture;
* add tests;
* build or run the project;
* call live services;
* resolve ambiguity without evidence;
* perform Git operations.

## Review quality threshold

The review is insufficient if it mainly:

* praises the analysis;
* repeats its conclusions;
* suggests prose or formatting improvements;
* produces generic security or reliability advice;
* lists theoretical risks not tied to Retailizer code.

The review must independently test the analysis against the actual workspace.

If no Critical or Major defects are found, document the verification work in sufficient depth to justify that result.

Do not manufacture findings merely to appear rigorous.

## Self-review before completion

Before finishing:

1. verify all mandatory review areas were covered;
2. verify every finding includes project evidence;
3. verify severity is justified;
4. verify required corrections are actionable;
5. verify verification requirements are correctly classified;
6. verify summary tables match detailed findings;
7. verify all finding identifiers are unique;
8. verify the original technical analysis was not modified;
9. verify no file outside `docs/technical-review/` was modified;
10. verify no Git operation was performed;
11. verify no build, restore, runtime execution, deployment, or external call was performed;
12. identify any important uncertainty in the review itself.

## Completion report

Provide a concise completion report containing:

1. file created;
2. number of findings by severity;
3. three strongest parts of the original technical analysis;
4. five most important required corrections;
5. most significant missing component or code path;
6. most important reliability result;
7. most important security or trust-boundary result;
8. highest-priority build and runtime verification requirements;
9. new stakeholder questions;
10. review limitations;
11. confirmation that the original analysis was not modified;
12. confirmation that no Git operation was performed;
13. confirmation that no build, restore, runtime execution, deployment, or external-service call was performed;
14. confirmation that no file outside `docs/technical-review/` was modified.
