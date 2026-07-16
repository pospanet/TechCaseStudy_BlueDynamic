# Mission: Evidence-Based Technical Analysis of the Retailizer Legacy Project

## Role

Act as a senior software architect, repository archaeologist, distributed-systems engineer, cloud architect, and legacy-system analyst.

You are analysing Retailizer, a real historical software project, to reconstruct its as-is technical architecture, runtime behavior, component responsibilities, data flows, integrations, deployment model, configuration, failure behavior, security boundaries, and technical limitations.

This is the first stage of a controlled technical-documentation workflow:

1. evidence-based technical analysis;
2. independent technical review by a different model;
3. reviewed final technical description and cleanup.

Your output in this mission is an analytical draft and supporting technical catalogs.

It is not yet the final authoritative technical description.

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

If Git metadata is visible, ignore it.

## Historical-project principle

Retailizer is a real historical project.

Analyse it in the context of the period in which it was created.

Do not automatically classify an old framework, SDK, cloud service, deployment model, or project format as an original design failure merely because it is obsolete today.

Distinguish between:

* historically reasonable technical choices;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete components;
* unused or abandoned implementation;
* external technologies that later became obsolete;
* runtime assumptions that cannot be verified statically;
* current maintainability, security, or operability concerns.

Do not modernize or redesign the system in this mission.

The root `README.md` is a historical artifact.

Do not rewrite, replace, reorganize, or modernize it.

## Mission objective

Reconstruct the technical view of the historical Retailizer project using only evidence present in the current workspace.

Determine, where supported by evidence:

* repository and solution structure;
* projects and component responsibilities;
* executable entry points;
* framework and dependency versions;
* runtime processes;
* device-side processing;
* cloud-side processing;
* device registration;
* image capture and face detection;
* storage upload;
* messaging;
* queue processing;
* face analysis and recognition;
* event creation;
* visit correlation;
* persistence;
* reporting and analytics;
* infrastructure and deployment topology;
* configuration and secrets;
* integration contracts;
* error handling;
* retries;
* concurrency;
* idempotence;
* message-delivery assumptions;
* backpressure;
* resource lifecycle;
* observability;
* authentication and authorization;
* tenant and store isolation;
* trust boundaries;
* build and execution assumptions;
* incomplete, unused, contradictory, or unverifiable elements.

## Static-analysis boundary

This mission is a static technical analysis.

Do not:

* build the project;
* restore dependencies;
* run the application;
* create a compatibility environment;
* deploy infrastructure;
* invoke Azure resources;
* call live external services;
* execute initialization tools against real services;
* modify configuration to make the system run;
* add test harnesses.

Where static analysis is insufficient, classify the conclusion as:

* runtime verification required;
* build verification required;
* external-service verification required;
* stakeholder confirmation required.

A later workflow will cover build reproduction and runtime verification.

## Evidence sources

Inspect all relevant files currently present in the workspace, including:

* root README and existing documentation;
* solution files;
* project files;
* source code;
* application entry points;
* package and dependency manifests;
* DTOs and entities;
* interfaces;
* service implementations;
* controllers and endpoints;
* background tasks;
* queue handlers;
* configuration files;
* deployment templates;
* infrastructure definitions;
* scripts;
* diagrams;
* initialization utilities;
* comments and TODOs;
* sample payloads;
* sample assets where technically relevant.

Do not rely on file names, class names, package references, or infrastructure-resource declarations alone.

Trace how components and data are actually connected.

## Mandatory evidence classification

Use these classifications consistently.

### Observed

Directly supported by source code, project files, configuration, deployment definitions, data structures, or other implementation artifacts.

### Documented

Explicitly stated in existing project documentation.

### Inferred

A reasoned technical interpretation supported by multiple pieces of evidence but not explicitly confirmed.

### Unknown

Cannot be determined from the current workspace.

### Conflicting evidence

Different project artifacts support incompatible conclusions.

### Build verification required

The conclusion depends on restoring or compiling the project.

### Runtime verification required

Static analysis cannot establish actual runtime behavior.

### External-service verification required

The conclusion depends on historical or live behavior of an external service.

### Stakeholder confirmation required

The design intent cannot be safely determined from technical artifacts.

Never present intended architecture as confirmed runtime behavior.

Never silently resolve conflicting evidence.

Do not use Git history as evidence.

## Evidence format

Keep evidence close to the conclusion it supports.

Use repository-relative references.

Example:

```markdown
**Assessment:** The device application appears to perform local face detection
before uploading cropped face images.

**Evidence status:** Observed

**Evidence:**
- `Retailizer.UWP/StartupTask.cs`: image-capture loop
- `Retailizer.UWP/LocalFaceDetector.cs`: face-detection and crop processing
- `Common/Services/StorageService.cs`: blob upload behavior

**Verification limitation:**
Actual camera compatibility and runtime resource behavior require execution on
the original or compatible Windows IoT environment.
```

For important conclusions identify:

* file path;
* project;
* class or type;
* method, endpoint, property, configuration key, queue, resource, or contract;
* evidence classification;
* remaining verification requirement.

Do not claim that a component or integration was operational merely because:

* a project exists;
* a package is referenced;
* a service class exists;
* a resource appears in an ARM template;
* a configuration key exists;
* the README shows a diagram;
* an unfinished method references it.

## Required analysis

### 1. Repository and solution structure

Inventory:

* solution files;
* application projects;
* libraries;
* deployment projects;
* initialization utilities;
* device-side components;
* cloud-side components;
* documentation;
* samples;
* generated or vendor artifacts if present.

For each project identify:

* project type;
* target framework;
* expected runtime;
* principal responsibility;
* executable or library status;
* direct dependencies;
* apparent consumers;
* current evidence-backed status.

### 2. Technology and dependency inventory

Identify:

* programming languages;
* target frameworks;
* project formats;
* SDKs;
* NuGet dependencies;
* Azure SDKs;
* Windows or device-platform dependencies;
* serialization libraries;
* web frameworks;
* persistence clients;
* messaging clients;
* image-processing APIs;
* analytics technologies;
* infrastructure-as-code formats.

For every material dependency record:

* declared version;
* project usage;
* apparent purpose;
* whether actual use is observed;
* obsolescence or availability concern;
* build verification requirement.

Do not turn this section into a modernization plan.

### 3. Executable entry points

Identify every executable entry point, including:

* device background application;
* web API;
* WebJob or queue processor;
* initialization utility;
* deployment or provisioning entry points;
* scripts.

For each entry point describe:

* startup mechanism;
* configuration inputs;
* major initialization steps;
* long-running behavior;
* termination or cancellation behavior;
* external dependencies;
* failure behavior;
* runtime verification needs.

### 4. Component catalog

Reconstruct all major components, including where present:

* UWP or Windows IoT application;
* local face detector;
* image-capture logic;
* device-configuration client;
* backend API;
* IoT Hub integration;
* Blob Storage integration;
* Service Bus integration;
* Stream Analytics role;
* queue-processing WebJob;
* Face API client;
* Emotion API client;
* DocumentDB persistence;
* visit-correlation logic;
* initializer;
* deployment template;
* reporting integration.

For each component determine:

* responsibility;
* inputs;
* outputs;
* state;
* dependencies;
* deployment location;
* trust boundary;
* error handling;
* operational assumptions;
* evidence status;
* unknowns.

### 5. End-to-end runtime architecture

Reconstruct the principal runtime path from camera observation to business data.

Trace, where supported:

1. camera discovery and initialization;
2. frame capture;
3. local face detection;
4. crop creation;
5. storage upload;
6. message creation;
7. IoT Hub submission;
8. stream or messaging routing;
9. Service Bus delivery;
10. WebJob processing;
11. image retrieval;
12. external face analysis;
13. person identification or creation;
14. event creation;
15. visit correlation;
16. persistence;
17. analytics or Power BI consumption.

For every hop identify:

* producer;
* consumer;
* payload;
* protocol or SDK;
* authentication;
* configuration source;
* delivery semantics;
* retry behavior;
* failure behavior;
* evidence status.

Clearly mark missing or only documented links.

### 6. Device-registration flow

Trace:

* how a device identifies itself;
* registration request construction;
* backend endpoint;
* validation;
* IoT Hub device creation;
* credential return;
* local credential storage;
* subsequent usage;
* duplicate registration behavior;
* authentication and authorization;
* trust assumptions.

Pay special attention to whether device registration appears protected.

Do not state that it is unauthenticated unless the complete relevant code supports that conclusion.

If protection cannot be found, state:

* no protection was found in the inspected workspace;
* runtime or external-gateway controls may still have existed;
* stakeholder or deployment confirmation is required.

### 7. Image-capture and local-processing flow

Analyse:

* camera enumeration;
* media initialization;
* capture loop;
* frame representation;
* face detection;
* crop calculation;
* image encoding;
* disposal of image and stream resources;
* concurrency;
* throttling;
* cancellation;
* exception handling;
* upload triggering.

Identify risks such as:

* uncontrolled loops;
* fire-and-forget calls;
* concurrent processing;
* resource leaks;
* unbounded work;
* failure suppression.

Only report risks supported by actual code.

### 8. Message and integration contracts

Identify all message and API contracts.

For each contract record:

* producer;
* consumer;
* transport;
* fields;
* required identifiers;
* serialization;
* versioning mechanism;
* validation;
* optionality;
* duplicate-detection support;
* correlation identifiers;
* tenant and store propagation;
* compatibility assumptions.

Distinguish DTO reuse from explicit integration contracts.

### 9. Event-processing flow

Trace:

* queue trigger;
* message deserialization;
* image retrieval;
* face detection or analysis;
* face identification;
* person creation;
* emotion or demographic handling;
* event creation;
* event typing;
* persistence;
* visit lookup;
* visit creation or update;
* completion behavior;
* error handling.

Identify where external API results directly affect domain data.

### 10. Visit-correlation behavior

Reconstruct the implemented algorithm or behavior for:

* locating an active visit;
* creating a new visit;
* adding an event;
* completing a visit;
* handling `enter`, `payment`, and `leave`;
* matching by person;
* matching by tenant or store;
* event ordering;
* duplicate events;
* missing events;
* concurrent messages;
* race conditions;
* persistence consistency.

Clearly distinguish:

* explicit logic;
* implementation-derived assumptions;
* unknown behavior;
* behavior requiring runtime or concurrency testing.

### 11. Data model

Document the principal entities and DTOs.

For each include:

* purpose;
* identifiers;
* relationships;
* mutable state;
* timestamps;
* tenant and store attributes;
* persistence mapping;
* partitioning clues;
* serialization behavior;
* validation;
* default values;
* ambiguity.

Include at least, where present:

* device;
* device camera;
* event;
* visit;
* face or person representation;
* integration message.

Do not infer strong domain invariants from DTO structure alone.

### 12. Persistence model

Analyse:

* databases;
* collections;
* document types;
* IDs;
* partition keys where visible;
* query patterns;
* create, update, and lookup behavior;
* consistency assumptions;
* concurrency handling;
* uniqueness assumptions;
* retention behavior;
* deletion behavior;
* multi-tenant filtering.

Distinguish provisioned collections from collections demonstrably used.

### 13. Infrastructure and deployment topology

Inspect deployment definitions and reconstruct:

* deployed resource types;
* resource relationships;
* configuration outputs;
* application hosting;
* messaging topology;
* storage topology;
* database topology;
* analytics topology;
* external APIs;
* device connectivity.

For each resource state whether it is:

* provisioned;
* referenced by application code;
* documented;
* apparently unused;
* uncertain.

Do not treat resource provisioning as proof of end-to-end usage.

### 14. Configuration and secrets

Identify:

* configuration files;
* connection strings;
* API keys;
* endpoints;
* resource names;
* environment separation;
* local device configuration;
* cloud application configuration;
* defaults and placeholders.

Do not expose actual secret values in documentation.

If secret-looking values are present:

* refer to the key name and location;
* redact values;
* document the handling pattern;
* do not copy the value.

Analyse:

* secret distribution;
* credential persistence;
* privilege scope;
* rotation support;
* configuration validation;
* environment coupling.

### 15. Error handling and failure paths

Inspect:

* try/catch behavior;
* swallowed exceptions;
* retries;
* logging;
* fallback behavior;
* poison-message handling;
* dead-letter handling;
* partial processing;
* cleanup;
* resource disposal;
* external-service failures;
* persistence failures;
* malformed messages.

Create an evidence-backed failure-mode catalog.

### 16. Concurrency, retries, idempotence, and backpressure

Analyse:

* async call structure;
* awaited versus fire-and-forget operations;
* loops;
* parallelism;
* WebJob concurrency;
* external API retries;
* exponential backoff;
* jitter;
* retry limits;
* duplicate message handling;
* idempotency keys;
* race conditions;
* visit-update conflicts;
* work-queue growth;
* throttling;
* cancellation.

Where behavior depends on framework defaults, mark it as verification required unless explicitly configured.

### 17. Observability and operations

Identify:

* logging framework;
* debug output;
* structured logging;
* correlation IDs;
* message IDs;
* metrics;
* tracing;
* health checks;
* queue monitoring;
* external API monitoring;
* deployment diagnostics;
* operational dashboards;
* alerting.

Distinguish implementation from TODOs or documented intention.

### 18. Security and trust boundaries

Analyse:

* device trust;
* backend API access;
* IoT Hub credentials;
* storage credentials;
* Service Bus credentials;
* database credentials;
* external face-service credentials;
* transport security assumptions;
* authentication;
* authorization;
* tenant isolation;
* input validation;
* data exposure;
* privilege boundaries.

Create a trust-boundary description without turning it into a full security audit.

A dedicated security review may follow later.

### 19. Build and execution assumptions

Without building, identify:

* expected operating systems;
* SDK requirements;
* Visual Studio or toolchain expectations;
* target frameworks;
* project formats;
* device hardware assumptions;
* cloud dependencies;
* expected configuration;
* likely unavailable services or SDKs;
* assets required at runtime.

Classify each item as:

* directly declared;
* documented;
* inferred;
* build verification required;
* runtime verification required.

### 20. Incomplete, unused, and contradictory elements

Identify evidence for:

* unused projects;
* unused service clients;
* infrastructure resources not connected to code;
* code paths not referenced;
* placeholders;
* TODOs;
* inconsistent naming;
* spelling mismatches;
* incomplete methods;
* documentation-code divergence;
* duplicate concepts;
* experimental artifacts.

Do not label code as unused solely because a simple textual reference search finds no consumer. Consider reflection, framework conventions, configuration binding, and deployment triggers.

## Required output directory

Create documentation only under:

`docs/technical-analysis/`

If the directory does not exist, create it.

Do not create or change files outside this directory.

## Required output files

### 1. `docs/technical-analysis/technical-analysis.md`

This is the principal technical analytical draft.

Use this structure:

1. Document status and scope
2. Analysis method and limitations
3. Technical executive summary
4. Historical technical context
5. Repository and solution structure
6. Technology and dependency inventory
7. Component architecture
8. Executable entry points
9. Runtime architecture
10. Device-registration flow
11. Image-capture and local-processing flow
12. End-to-end data flow
13. Event-processing flow
14. Visit-correlation behavior
15. Data and message model
16. Persistence model
17. External integrations
18. Infrastructure and deployment topology
19. Configuration and secrets model
20. Error handling and failure paths
21. Concurrency, retries, idempotence, and backpressure
22. Observability and operational behavior
23. Security and trust boundaries
24. Build and execution assumptions
25. Incomplete, unused, and contradictory elements
26. Known technical limitations
27. Conflicting evidence
28. Build verification requirements
29. Runtime verification requirements
30. External-service verification requirements
31. Stakeholder confirmation required
32. Evidence appendix

Clearly mark the document as:

`Analysis draft — not yet independently reviewed`

### 2. `docs/technical-analysis/component-catalog.md`

For every component include:

* identifier;
* component name;
* type;
* project and location;
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

Use identifiers such as:

* `COMP-DEVICE-001`
* `COMP-API-001`
* `COMP-JOB-001`
* `COMP-PERSISTENCE-001`

### 3. `docs/technical-analysis/integration-catalog.md`

For every integration include:

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

Use identifiers such as:

* `INT-IOTHUB-001`
* `INT-STORAGE-001`
* `INT-SERVICEBUS-001`
* `INT-FACEAPI-001`

### 4. `docs/technical-analysis/data-model-catalog.md`

For every major entity or contract include:

* identifier;
* type name;
* location;
* purpose;
* fields;
* identifiers;
* relationships;
* serialization behavior;
* persistence behavior;
* tenant or store scope;
* invariants found;
* ambiguity;
* evidence classification;
* verification requirement.

### 5. `docs/technical-analysis/failure-mode-catalog.md`

For every material failure mode include:

* identifier;
* component or flow;
* trigger;
* observed handling;
* retry behavior;
* state impact;
* data-loss or duplication risk;
* operational visibility;
* evidence;
* verification requirement.

Use identifiers such as:

* `FAIL-CAPTURE-001`
* `FAIL-UPLOAD-001`
* `FAIL-QUEUE-001`
* `FAIL-VISIT-001`

### 6. `docs/technical-analysis/technical-open-questions.md`

Group questions into:

* repository structure;
* build and toolchain;
* device runtime;
* backend API;
* messaging;
* external APIs;
* event processing;
* visit correlation;
* persistence;
* infrastructure;
* configuration and secrets;
* security;
* reliability;
* observability;
* unused or incomplete components.

Prioritize every question:

* blocking;
* important;
* useful context.

Also specify the required resolution method:

* static code inspection;
* build experiment;
* runtime experiment;
* external documentation;
* stakeholder confirmation.

### 7. `docs/technical-analysis/technical-evidence-index.md`

Map major conclusions to:

* conclusion identifier;
* document section;
* evidence classification;
* repository artifacts;
* relevant symbol or resource;
* verification requirement;
* unresolved issue.

### 8. Mermaid diagrams

Create these diagram sources:

* `docs/technical-analysis/diagrams/system-context.mmd`
* `docs/technical-analysis/diagrams/container-view.mmd`
* `docs/technical-analysis/diagrams/device-registration-sequence.mmd`
* `docs/technical-analysis/diagrams/image-processing-sequence.mmd`
* `docs/technical-analysis/diagrams/event-processing-sequence.mmd`
* `docs/technical-analysis/diagrams/deployment-view.mmd`

Diagram requirements:

* represent the as-is historical system;
* use evidence-backed relationships;
* visibly mark inferred or uncertain relationships;
* do not include a future target architecture;
* do not hide missing links;
* keep labels technically precise;
* ensure Mermaid syntax is valid by inspection.

Do not install tools to render or validate Mermaid.

## Documentation requirements

Write all documentation in English.

The primary audience is software developers and architects.

Use precise technical language.

Avoid:

* generic architecture filler;
* modernization recommendations;
* target-state design;
* unsupported statements about runtime behavior;
* claims that infrastructure resources were used without application evidence;
* repeated descriptions that belong in catalogs;
* secret values.

The main analysis should explain the system.

The catalogs should carry structured detail.

## Change restrictions

You may only create files under:

`docs/technical-analysis/`

Do not modify:

* source code;
* tests;
* project files;
* dependencies;
* configuration;
* deployment templates;
* scripts;
* sample assets;
* root README;
* existing business documentation;
* any Git metadata.

Do not:

* build the project;
* restore packages;
* run the project;
* deploy infrastructure;
* call external services;
* add tests;
* add logging;
* fix defects;
* modernize dependencies;
* convert project formats;
* create compatibility wrappers;
* propose a target architecture.

## Self-review before completion

Before finishing:

1. verify all required files exist;
2. verify every major conclusion has evidence;
3. verify intended and observed behavior are distinguished;
4. search for runtime claims that static analysis cannot prove;
5. verify all such claims are marked with verification requirements;
6. verify component and integration identifiers are stable;
7. verify catalog entries are consistent with the main analysis;
8. verify diagrams match the written analysis;
9. verify infrastructure resources are not presented as used without evidence;
10. verify secret values were not copied;
11. verify no file outside `docs/technical-analysis/` was modified;
12. verify no Git operation was performed;
13. identify areas requiring special attention during independent review.

## Completion report

Provide a concise completion report containing:

1. files created;
2. reconstructed end-to-end runtime flow;
3. five most important technical findings;
4. five most consequential technical unknowns;
5. most important contradictions;
6. most serious reliability concern;
7. most serious security or trust-boundary concern;
8. highest-priority build and runtime verification requirements;
9. areas requiring special attention during independent review;
10. confirmation that no Git operation was performed;
11. confirmation that no build, restore, deployment, or live external call was performed;
12. confirmation that no file outside `docs/technical-analysis/` was modified.
