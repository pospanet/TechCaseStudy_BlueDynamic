# Mission: Produce the Reviewed Final Technical Description and Clean Up Intermediate Artifacts

## Role

Continue as the senior software architect, repository archaeologist, distributed-systems engineer, cloud architect, and legacy-system analyst who performed the original technical analysis of Retailizer.

An independent reviewer has now assessed that analysis.

Your task is to:

1. evaluate every review finding independently;
2. verify each finding against the actual project files;
3. correct factual errors and unsupported conclusions;
4. correct evidence classifications and verification requirements;
5. preserve unresolved uncertainty where static evidence is insufficient;
6. produce the final reviewed technical documentation;
7. validate that the final documentation is complete and internally consistent;
8. remove intermediate analysis and review artifacts that are no longer needed.

This is the third and final stage of the technical-documentation workflow:

1. evidence-based technical analysis;
2. independent technical review by another model;
3. reviewed final technical documentation and cleanup.

The resulting repository should contain clean, maintainable technical documentation rather than the internal working artifacts of the analysis and review process.

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

You may inspect ordinary project files and use normal filesystem operations only within the explicitly allowed documentation scope.

## Static-analysis restriction

Do not:

* build the project;
* restore packages;
* run any application;
* execute tests;
* install historical SDKs;
* create a compatibility environment;
* deploy infrastructure;
* call live external services;
* invoke initialization utilities;
* modify configuration;
* add test harnesses;
* execute generated scripts.

This mission must remain based on static project evidence.

Where a conclusion cannot be established statically, preserve an explicit verification requirement.

## Required inputs

Read and evaluate all available files under:

* `docs/technical-analysis/`
* `docs/technical-review/`

Expected inputs include:

### Original technical-analysis artifacts

* `docs/technical-analysis/technical-analysis.md`
* `docs/technical-analysis/component-catalog.md`
* `docs/technical-analysis/integration-catalog.md`
* `docs/technical-analysis/data-model-catalog.md`
* `docs/technical-analysis/failure-mode-catalog.md`
* `docs/technical-analysis/technical-open-questions.md`
* `docs/technical-analysis/technical-evidence-index.md`
* all Mermaid diagrams under `docs/technical-analysis/diagrams/`

### Independent review

* `docs/technical-review/technical-analysis-review.md`

Also inspect the actual Retailizer project as needed to verify findings, including:

* root `README.md`;
* existing documentation;
* solution and project files;
* source code;
* executable entry points;
* background jobs;
* controllers and API endpoints;
* DTOs and persistence entities;
* service implementations;
* package manifests;
* configuration;
* infrastructure definitions;
* deployment templates;
* scripts;
* initialization utilities;
* comments and TODOs;
* sample payloads;
* technically relevant sample assets.

Do not accept either the original analysis or the review as authoritative without verification.

The project artifacts are the primary evidence base.

## Historical-project principle

Retailizer is a real historical project.

Describe the technical design in the context of the period in which it was created.

Distinguish:

* historically reasonable technical choices;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete implementation;
* apparently unused or experimental elements;
* external platforms and services that later became obsolete;
* current build, security, reliability, and maintainability concerns;
* behavior that cannot be verified statically.

Do not redesign or modernize the system in this mission.

Do not rewrite the root `README.md`.

## Review disposition process

Evaluate every review finding using exactly one of these dispositions:

### Accepted

The finding is supported by project evidence and must affect the final documentation.

### Partially accepted

The finding identifies a real issue, but its interpretation or required correction is only partly supported.

### Rejected

The finding is not supported by project evidence.

### Superseded

A broader or more precise correction resolves the issue.

### Build verification required

The issue cannot be resolved without restoring or compiling the project.

### Runtime verification required

The issue cannot be resolved through static analysis.

### External-service verification required

The issue depends on historical or live behavior of an external service.

### Stakeholder confirmation required

The original intent cannot be established from technical artifacts.

You must evaluate every review finding, but you do not need to preserve a separate disposition document in the final repository.

Use the disposition process internally while producing the final result.

Before cleanup, verify that:

* every accepted finding is reflected in the final documentation;
* every partially accepted finding is addressed to the supported extent;
* every rejected finding was checked against contrary evidence;
* verification-dependent findings remain explicitly visible;
* stakeholder-dependent issues remain visible;
* no review finding was silently ignored.

## Evidence classification

Use these classifications in the final documentation:

### Observed

Directly supported by source code, project files, configuration, infrastructure definitions, data structures, or other implementation artifacts.

### Documented

Explicitly stated in existing project documentation.

### Inferred

A reasoned interpretation supported by multiple artifacts but not explicitly confirmed.

### Unknown

Cannot be determined from the current workspace.

### Conflicting evidence

Different project artifacts support incompatible conclusions.

### Build verification required

Compilation or dependency restoration is required.

### Runtime verification required

Static analysis cannot establish the actual runtime behavior.

### External-service verification required

The conclusion depends on an external platform or managed service.

### Stakeholder confirmation required

The intended design cannot be determined technically.

Do not use Git history as evidence.

Never present intended architecture as confirmed runtime behavior.

Never silently resolve conflicting evidence.

## Final output directory

Create:

`docs/technical/`

If the directory does not exist, create it.

## Required final outputs

Create the following files:

1. `docs/technical/technical-description.md`
2. `docs/technical/component-catalog.md`
3. `docs/technical/integration-catalog.md`
4. `docs/technical/data-model.md`
5. `docs/technical/failure-modes.md`
6. `docs/technical/verification-backlog.md`
7. `docs/technical/diagrams/system-context.mmd`
8. `docs/technical/diagrams/container-view.mmd`
9. `docs/technical/diagrams/device-registration-sequence.mmd`
10. `docs/technical/diagrams/image-processing-sequence.mmd`
11. `docs/technical/diagrams/event-processing-sequence.mmd`
12. `docs/technical/diagrams/deployment-view.mmd`

These files together form the reviewed technical documentation.

The main document must remain readable without requiring the reader to inspect the removed analysis and review artifacts.

The supporting catalogs should preserve structured detail that would make the main document excessively long.

# Final document requirements

## 1. `docs/technical/technical-description.md`

This is the primary reviewed technical description of Retailizer.

Use the following structure.

### 1. Document purpose and status

Explain:

* that this is a reconstruction of a historical project;
* that it is based on static repository evidence;
* that it underwent independent review;
* that build and runtime behavior have not yet been verified;
* that verification-dependent conclusions are explicitly marked;
* that the document does not describe a modernization target.

### 2. Technical executive summary

Summarize:

* the overall system;
* principal device-side and cloud-side components;
* principal runtime flow;
* persistence and analytics model;
* likely proof-of-concept maturity;
* most important technical limitations;
* most important unknowns.

### 3. Historical technical context

Describe:

* the platform generation;
* the historical role of Windows IoT, UWP, Azure IoT Hub, DocumentDB, WebJobs, Stream Analytics, Face APIs, and Power BI;
* which choices appear historically reasonable;
* which limitations are implementation-specific;
* which limitations result from later platform obsolescence.

Avoid unsupported claims about the exact development date or production usage.

### 4. Repository and solution structure

Describe every material project and artifact.

For each project include:

* name;
* type;
* target framework or platform;
* responsibility;
* executable or library status;
* principal dependencies;
* relationship to other projects;
* evidence-backed status.

Clearly distinguish:

* device application;
* backend API;
* queue processor or WebJob;
* shared library;
* Azure initializer;
* deployment project;
* analytics references;
* documentation and sample assets.

### 5. Technology and dependency overview

Summarize:

* languages;
* project formats;
* target frameworks;
* principal SDKs and libraries;
* Azure services;
* device-platform dependencies;
* persistence and messaging technologies;
* external analytics APIs.

For every important dependency state:

* declared version where available;
* observed use;
* role;
* current availability or obsolescence concern;
* whether build verification is required.

Do not propose replacements.

### 6. Component architecture

Provide a concise component overview and link to `component-catalog.md`.

For each major component explain:

* responsibility;
* runtime location;
* inputs and outputs;
* owned state;
* external dependencies;
* trust boundary;
* principal failure mode;
* verification limitation.

### 7. Executable entry points and runtime processes

Describe all executable entry points:

* UWP or Windows IoT background application;
* backend web API;
* WebJob or queue-triggered processor;
* initialization utility;
* deployment or provisioning entry point.

For each describe:

* startup mechanism;
* configuration inputs;
* initialization sequence;
* long-running behavior;
* cancellation or termination behavior;
* failure behavior;
* verification requirements.

### 8. System context

Explain:

* physical cameras and device hardware;
* device-side software;
* backend services;
* Azure managed services;
* external face or emotion-analysis services;
* persistence;
* analytics and reporting consumers.

Reference `diagrams/system-context.mmd`.

### 9. End-to-end runtime flow

Reconstruct the principal flow from camera observation to stored business data.

Cover:

1. camera enumeration and initialization;
2. image capture;
3. local face detection;
4. crop generation;
5. Blob Storage upload;
6. message creation;
7. IoT Hub submission;
8. Stream Analytics or routing;
9. Service Bus delivery;
10. queue-triggered processing;
11. image retrieval;
12. face analysis;
13. person identification or creation;
14. event generation;
15. visit correlation;
16. persistence;
17. analytics consumption.

For every uncertain link state whether it is:

* observed;
* documented;
* inferred;
* conflicting;
* subject to verification.

Do not create a complete flow by silently joining artifacts that are not demonstrably connected.

### 10. Device-registration flow

Describe:

* device identifier acquisition;
* registration request;
* backend endpoint;
* input validation;
* IoT Hub registry interaction;
* credential return;
* credential storage;
* later credential use;
* duplicate registration behavior;
* authentication and authorization evidence;
* external protection that may have existed outside the repository.

Use careful wording:

> No authentication or authorization control was found in the inspected workspace.

Do not state that the endpoint was publicly exposed unless deployment evidence proves it.

Reference `diagrams/device-registration-sequence.mmd`.

### 11. Image capture and local processing

Describe:

* camera enumeration;
* media initialization;
* capture loop;
* frame format;
* local face detection;
* crop logic;
* image encoding;
* storage upload;
* IoT message submission;
* disposal and resource ownership;
* asynchronous behavior;
* cancellation;
* throttling;
* exception behavior.

Include evidence-backed concerns such as:

* unawaited operations;
* fire-and-forget processing;
* potentially unbounded concurrency;
* missing cancellation;
* resource lifecycle ambiguity;
* lack of backpressure.

Only retain concerns supported by actual code.

Reference `diagrams/image-processing-sequence.mmd`.

### 12. Messaging and integration contracts

Summarize:

* producers;
* consumers;
* transports;
* payloads;
* serialization;
* authentication;
* configuration;
* delivery assumptions;
* retries;
* correlation;
* duplicate handling;
* tenant and store propagation.

Link to `integration-catalog.md`.

Explicitly distinguish:

* DTO reuse;
* implicit contracts;
* versioned contracts;
* framework-triggered integrations.

### 13. Event-processing flow

Describe:

* trigger method;
* message deserialization;
* image retrieval;
* external analysis calls;
* face identification;
* person creation;
* event construction;
* event typing;
* persistence;
* visit lookup;
* visit creation or update;
* failure propagation;
* retry duplication risk.

Reference `diagrams/event-processing-sequence.mmd`.

### 14. Visit-correlation behavior

This section must precisely reconstruct the implemented behavior.

Address:

* active-visit lookup;
* matching attributes;
* person matching;
* tenant or store filtering;
* event ordering;
* new-visit creation;
* event append behavior;
* visit completion;
* handling of `enter`, `payment`, and `leave`;
* duplicate messages;
* missing events;
* out-of-order delivery;
* concurrent processing;
* read-modify-write races;
* atomicity;
* idempotence.

Distinguish:

* explicitly implemented logic;
* inferred assumptions;
* behavior requiring runtime or concurrency testing;
* stakeholder-dependent intent.

Do not describe the flow as idempotent, ordered, transactional, or race-free without direct evidence.

### 15. Data and message model

Summarize major entities and contracts and link to `data-model.md`.

Address:

* device;
* device camera;
* inbound message;
* face or person identity;
* event;
* visit;
* tenant and store fields;
* timestamps;
* identifiers;
* mutability;
* serialization;
* persistence;
* relationships;
* absent validation;
* ambiguous invariants.

Do not infer domain guarantees solely from DTO structure.

### 16. Persistence model

Describe:

* DocumentDB or database resources;
* collections;
* entity storage;
* query patterns;
* IDs;
* partitioning evidence;
* create and update behavior;
* consistency assumptions;
* uniqueness;
* concurrency;
* retention;
* deletion;
* tenant filtering.

Explicitly identify:

* provisioned collections versus demonstrably used collections;
* possible cross-tenant query risks;
* read-modify-write behavior;
* non-atomic multi-document operations;
* missing deletion or retention behavior.

Qualify any current Cosmos DB comparison as modern context, not historical runtime fact.

### 17. Infrastructure and deployment topology

Describe:

* hosting resources;
* IoT Hub;
* storage;
* Service Bus;
* Stream Analytics;
* web hosting;
* DocumentDB;
* Face and Emotion API resources;
* reporting or Power BI references.

For each resource classify it as:

* provisioned;
* referenced by code;
* documented;
* apparently unused;
* uncertain.

Reference `diagrams/deployment-view.mmd`.

Do not present a deployment-template connection as a runtime connection without supporting application or configuration evidence.

### 18. Configuration and secret model

Describe:

* application settings;
* connection strings;
* API keys;
* service endpoints;
* resource names;
* device credential storage;
* environment handling;
* placeholders;
* hard-coded values;
* configuration validation.

Do not copy secret values.

Refer only to:

* file;
* configuration key;
* handling pattern;
* technical consequence.

Identify:

* broad credentials;
* missing rotation;
* environment coupling;
* local credential persistence;
* absent validation;
* unclear privilege scope.

### 19. Error handling and failure behavior

Summarize principal failure modes and link to `failure-modes.md`.

Cover:

* camera initialization;
* image capture;
* local face detection;
* upload;
* message submission;
* stream routing;
* queue deserialization;
* missing blob;
* external API failures;
* throttling;
* identity creation;
* persistence;
* visit updates;
* malformed configuration.

For each major flow explain:

* whether exceptions are caught;
* whether retry exists;
* whether state can become partial;
* whether duplication is possible;
* whether the failure is operationally visible.

### 20. Concurrency, retries, idempotence, and backpressure

Provide a focused assessment of:

* asynchronous method structure;
* awaited and unawaited calls;
* retry policies;
* retry limits;
* exponential backoff;
* jitter;
* queue-trigger concurrency;
* duplicate-message handling;
* idempotency keys;
* correlation identifiers;
* visit-update races;
* external API throttling;
* queue growth;
* cancellation;
* backpressure.

Clearly mark framework-default behavior as verification required unless explicitly configured.

### 21. Observability and operations

Describe:

* debug output;
* logging;
* structured telemetry;
* correlation IDs;
* message IDs;
* metrics;
* traces;
* health checks;
* queue monitoring;
* poison messages;
* dead-letter handling;
* alerts;
* dashboards.

Distinguish:

* implemented behavior;
* TODOs;
* platform capabilities;
* configuration not found;
* runtime verification needs.

### 22. Security and trust boundaries

Describe:

* device trust;
* backend API trust;
* IoT Hub credentials;
* storage credentials;
* Service Bus credentials;
* database credentials;
* external API credentials;
* client-supplied tenant and store data;
* authentication;
* authorization;
* input validation;
* transport assumptions;
* privilege boundaries;
* image and biometric-data exposure.

Do not turn this into a full threat model.

Clearly distinguish:

* controls found;
* controls not found;
* controls that may have existed outside the repository;
* findings requiring deployment confirmation.

### 23. Build and execution assumptions

Document:

* operating-system expectations;
* Visual Studio generation;
* .NET and UWP SDKs;
* Windows IoT dependencies;
* target hardware;
* package restoration;
* cloud resources;
* required configuration;
* external service availability;
* runtime assets.

Classify each as:

* declared;
* documented;
* inferred;
* build verification required;
* runtime verification required;
* external-service verification required.

### 24. Incomplete, unused, and contradictory elements

List evidence-backed findings concerning:

* apparently unused projects;
* apparently unused service clients;
* unused deployment resources;
* incomplete methods;
* placeholders;
* TODOs;
* inconsistent names;
* misspellings;
* stale documentation;
* unreachable or unconnected code;
* experimental assets.

Qualify “unused” claims where framework conventions, reflection, configuration, or deployment triggers could apply.

### 25. Known technical limitations

Summarize the most consequential evidence-backed limitations:

* static-only verification;
* obsolete toolchain;
* unavailable or retired services;
* incomplete testability;
* weak observability;
* unclear security controls;
* concurrency and idempotence concerns;
* persistence consistency risks;
* incomplete deployment linkage;
* undocumented operational assumptions.

### 26. Conflicting evidence

For every material conflict include:

* conflicting artifacts;
* possible interpretations;
* technical impact;
* safest current conclusion;
* required verification.

### 27. Verification backlog

Summarize the highest-priority verification work and link to `verification-backlog.md`.

Group by:

* build;
* runtime;
* concurrency;
* external service;
* deployment;
* stakeholder.

### 28. Evidence and traceability

Provide a compact mapping from major sections and conclusions to:

* repository-relative files;
* relevant projects, classes, methods, properties, configuration keys, or resources;
* evidence classification;
* verification requirement.

The main document must remain independently understandable after the analysis evidence index is deleted.

# Supporting final documents

## 2. `docs/technical/component-catalog.md`

Create a reviewed, consolidated component catalog.

For each material component include:

* identifier;
* name;
* type;
* project and path;
* responsibility;
* entry point;
* inputs;
* outputs;
* state;
* direct dependencies;
* external dependencies;
* data owned or accessed;
* deployment target;
* trust boundary;
* failure behavior;
* evidence classification;
* verification requirement;
* unresolved questions.

Remove duplicate, overly granular, or purely incidental components.

Do not omit a component necessary to understand runtime behavior.

## 3. `docs/technical/integration-catalog.md`

Create a reviewed integration catalog.

For every material integration include:

* identifier;
* producer;
* consumer;
* protocol, SDK, or framework;
* endpoint or resource;
* payload or contract;
* serialization;
* authentication;
* configuration source;
* expected delivery semantics;
* observed retry behavior;
* failure behavior;
* correlation support;
* idempotence support;
* evidence classification;
* verification requirement;
* unresolved questions.

Ensure consistency with the final diagrams and main document.

## 4. `docs/technical/data-model.md`

Create a reviewed data and message model document.

For each major entity or contract include:

* identifier;
* type name;
* location;
* technical purpose;
* important fields;
* identifiers;
* relationships;
* serialization;
* persistence;
* tenant or store scope;
* mutable state;
* observed invariant;
* inferred invariant;
* ambiguity;
* verification requirement.

Include relationship diagrams in Mermaid only if they materially improve clarity.

Do not invent cardinality or integrity constraints.

## 5. `docs/technical/failure-modes.md`

Create a consolidated failure-mode catalog.

For every material failure mode include:

* identifier;
* component or flow;
* trigger;
* observed handling;
* retry behavior;
* possible partial state;
* data-loss risk;
* duplication risk;
* security or privacy consequence;
* operational visibility;
* evidence;
* verification requirement;
* mitigation status in the historical implementation.

Do not propose modernization solutions.

It is acceptable to state that no mitigation was found.

## 6. `docs/technical/verification-backlog.md`

Consolidate all unresolved technical verification work.

For every item include:

* identifier;
* question;
* reason verification is required;
* verification type;
* priority;
* prerequisites;
* expected method;
* expected evidence;
* risk if unresolved;
* related documentation sections.

Use verification types:

* build;
* runtime;
* concurrency;
* deployment;
* external service;
* stakeholder.

Use priorities:

* blocking;
* important;
* useful context.

This backlog must preserve the meaningful open questions from the original analysis and review.

## 7–12. Final Mermaid diagrams

Create reviewed versions under:

`docs/technical/diagrams/`

Required files:

* `system-context.mmd`
* `container-view.mmd`
* `device-registration-sequence.mmd`
* `image-processing-sequence.mmd`
* `event-processing-sequence.mmd`
* `deployment-view.mmd`

Diagram requirements:

* represent the historical as-is system;
* match the final written documentation;
* use evidence-backed edges;
* visibly mark inferred or uncertain relationships;
* distinguish application components from managed infrastructure;
* distinguish runtime links from provisioned-only resources;
* avoid target-state elements;
* preserve missing or uncertain links rather than hiding them;
* use valid Mermaid syntax by inspection.

Do not install or run Mermaid tools.

# Review incorporation requirements

Before finalizing, explicitly verify every review finding involving:

* omitted projects or entry points;
* incorrect component classification;
* unsupported end-to-end links;
* device registration;
* capture-loop behavior;
* async and fire-and-forget processing;
* message contracts;
* delivery semantics;
* event processing;
* visit correlation;
* persistence consistency;
* tenant isolation;
* authentication or authorization;
* secret handling;
* retries;
* idempotence;
* duplicate processing;
* backpressure;
* observability;
* deployment topology;
* diagram accuracy;
* build and runtime verification classification.

Do not include a separate full review-disposition document in the final repository.

Instead, ensure that all supported corrections are incorporated into the final outputs.

# Cross-document consistency requirements

Before cleanup, verify consistency across:

* `technical-description.md`
* `component-catalog.md`
* `integration-catalog.md`
* `data-model.md`
* `failure-modes.md`
* `verification-backlog.md`
* all diagrams

Specifically verify:

* component identifiers are stable;
* integration identifiers are stable;
* failure-mode identifiers are stable;
* verification identifiers are stable;
* component names match diagrams;
* integration direction matches sequence diagrams;
* payload names match code;
* deployment resources match deployment artifacts;
* evidence classifications are consistent;
* build and runtime verification requirements are not lost;
* unknowns are not silently resolved;
* no secret values appear;
* no modernization recommendations appear.

# Cleanup objective

After the final documentation has been created and fully validated, remove the intermediate technical-analysis and review artifacts that are no longer needed.

Cleanup must occur only after all necessary content has been incorporated into the final documentation.

# Files and directories to remove after successful validation

Delete everything under:

* `docs/technical-analysis/`
* `docs/technical-review/`

This includes, where present:

* `technical-analysis.md`
* `component-catalog.md`
* `integration-catalog.md`
* `data-model-catalog.md`
* `failure-mode-catalog.md`
* `technical-open-questions.md`
* `technical-evidence-index.md`
* all analysis-stage diagrams
* `technical-analysis-review.md`

After deleting their contents:

* remove `docs/technical-analysis/` if empty;
* remove `docs/technical-review/` if empty.

Do not delete any file outside these two directories.

Do not delete the final files under `docs/technical/`.

# Cleanup safety gate

Do not delete intermediate artifacts until all of the following are true:

1. all required files under `docs/technical/` exist;
2. the main technical description contains all required sections;
3. all material review findings have been evaluated;
4. accepted corrections are incorporated;
5. unresolved findings are preserved as verification or stakeholder items;
6. all important component information is preserved;
7. all important integration information is preserved;
8. all important data-model information is preserved;
9. all important failure modes are preserved;
10. all build, runtime, external-service, and stakeholder questions are preserved;
11. all material evidence references are preserved;
12. all final diagrams reflect the reviewed architecture;
13. the final documentation can stand alone;
14. no unique material information would be lost by deleting the drafts;
15. no review finding is only documented in the review file.

If unique material information would be lost, incorporate it into the appropriate final document before cleanup.

Do not retain intermediate files merely because leaving them is easier.

Do not delete them prematurely.

# Allowed filesystem changes

You may:

* create `docs/technical/`;
* create or update the required files under `docs/technical/`;
* delete files and directories specifically listed under the cleanup section.

Do not modify or delete:

* application source code;
* tests;
* project files;
* dependencies;
* configuration;
* deployment templates;
* scripts;
* root README;
* business documentation;
* historical assets;
* unrelated documentation;
* Git metadata.

# Final validation

Before completion:

1. verify every review finding was evaluated;
2. verify all accepted findings affected the final documentation;
3. verify unresolved findings remain visible;
4. verify the final documentation is self-contained;
5. verify every major architectural conclusion has evidence;
6. verify intended and observed behavior are distinguished;
7. verify runtime behavior is not overstated;
8. verify framework defaults are not presented as configured facts;
9. verify infrastructure provisioning is not presented as runtime use without evidence;
10. verify device-registration security wording is appropriately qualified;
11. verify visit correlation is not described as ordered, atomic, idempotent, or race-free without evidence;
12. verify retry, duplicate-processing, and backpressure risks are preserved;
13. verify persistence assumptions are qualified;
14. verify secret values were not copied;
15. verify diagrams match the final documentation;
16. verify no modernization recommendations were introduced;
17. verify root README was not modified;
18. verify business documentation was not modified;
19. verify only allowed documentation paths changed;
20. verify all intermediate technical-analysis and review files were removed;
21. verify empty intermediate directories were removed;
22. verify no Git operation was performed;
23. verify no build, restore, runtime execution, deployment, or external-service call was performed.

# Required completion report

Provide a concise completion report containing:

1. final files created;
2. intermediate files removed;
3. directories removed;
4. number of review findings evaluated;
5. five most important corrections introduced after review;
6. findings that could not be resolved statically;
7. highest-priority build-verification items;
8. highest-priority runtime-verification items;
9. highest-priority security or deployment confirmation items;
10. confirmation that no unique material information was lost during cleanup;
11. confirmation that the final documentation is self-contained;
12. confirmation that no application code, configuration, deployment file, business documentation, or root README was changed;
13. confirmation that no Git operation was performed;
14. confirmation that no build, restore, runtime execution, deployment, or external-service call was performed;
15. confirmation that no unrelated file was modified or deleted.
