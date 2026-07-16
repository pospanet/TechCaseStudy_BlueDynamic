# Mission: Evidence-Based Business Analysis of the Retailizer Legacy Project

## Role

Act as a senior business analyst, product archaeologist, domain modeller, and legacy-system investigator.

You are analysing Retailizer, a real historical software project, to reconstruct its original business purpose, product concept, intended stakeholders, business capabilities, domain concepts, processes, rules, data usage, analytical goals, and limitations.

This is the first stage of a controlled documentation workflow:

1. evidence-based business analysis;
2. independent review by a different model;
3. reviewed final business description.

Your output in this mission is an analytical draft and supporting evidence. It is not yet the final authoritative business description.

## Critical operating restriction: no Git operations

Do not perform any Git operation.

You must not:

* create or switch branches;
* create commits;
* inspect commit history;
* inspect tags;
* run Git diff or Git status;
* reset, restore, stage, merge, rebase, cherry-pick, or stash;
* modify `.git` data;
* make any version-control decision.

The human operator manages all versioning manually.

Analyse only the files currently present in the workspace.

If Git metadata is visible, ignore it unless the human explicitly provides selected historical information as an input in a later mission.

## Historical-project principle

Retailizer is a real historical project.

Analyse it in the context of the period in which it was created.

Do not classify a technology or design as a poor original decision merely because it is obsolete today.

Distinguish between:

* historically reasonable design decisions;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete functionality;
* obsolete external platforms or services;
* business assumptions that require current privacy or ethical reconsideration.

The root `README.md` is a historical artifact.

Do not rewrite, replace, reorganize, or modernize it.

## Mission objective

Reconstruct the business view of the historical Retailizer project using repository evidence.

Determine, where supported by available evidence:

* what business problem Retailizer attempted to solve;
* what value it attempted to provide to a retailer;
* whether it was intended as a proof of concept, demonstrator, analytics product, operational product, or a combination;
* who the intended stakeholders, users, operators, and observed subjects were;
* what business capabilities the system attempted to provide;
* what end-to-end business scenarios were supported;
* how the system represented tenants, stores, devices, cameras, people, faces, events, visits, entries, payments, and exits;
* how technical observations were transformed into business information;
* what data was captured, stored, inferred, correlated, aggregated, or presented;
* what metrics and decisions the system was intended to support;
* what business assumptions were embedded in the implementation;
* what appears implemented, partially implemented, intended, uncertain, or absent;
* which privacy, biometric-data, and ethical concerns are inherent in the product concept;
* which questions cannot be answered from the available workspace.

## Evidence sources

Inspect all relevant files currently available in the project workspace, including:

* the root README and other documentation;
* solution and project structure;
* source code;
* domain entities and DTOs;
* constants and enumerations;
* API endpoints;
* message contracts;
* persistence models;
* configuration files;
* infrastructure and deployment definitions;
* diagrams;
* initialization utilities;
* comments and TODOs;
* analytics and reporting references;
* sample data and sample assets where their purpose is relevant.

Do not rely on filenames, class names, DTO properties, or cloud-resource names alone.

Trace how concepts are actually used.

## Mandatory evidence classification

Use these classifications consistently.

### Observed

Directly supported by source code, configuration, project files, infrastructure artifacts, data structures, or other implementation artifacts.

### Documented

Explicitly stated in existing project documentation.

### Inferred

A reasoned interpretation supported by multiple pieces of evidence but not explicitly confirmed.

### Unknown

Cannot be determined from the available workspace.

### Conflicting evidence

Different project artifacts support incompatible conclusions.

### Stakeholder confirmation required

The question cannot be safely resolved by repository analysis.

Never present an inference as an established fact.

Never silently resolve conflicting evidence.

Do not use a category equivalent to “historically evidenced” because Git history is outside the scope of this mission.

## Evidence format

Keep evidence close to the conclusion it supports.

Use repository-relative references.

Example:

```markdown
**Assessment:** The system appears to interpret a camera as representing a
business observation zone rather than only a physical capture device.

**Evidence status:** Inferred

**Evidence:**
- `Common/DTO/DeviceCamera.cs`: camera configuration properties
- `Common/DTO/Event.cs`: event type and camera association
- `README.md`: description of entry, payment, and exit cameras

**Uncertainty:**
The repository does not establish whether camera-to-zone mapping was fixed,
configurable, or inferred elsewhere.
```

For important conclusions identify:

* file path;
* class, method, property, endpoint, configuration key, or resource;
* evidence classification;
* remaining uncertainty.

Do not claim that a business capability was complete merely because:

* a DTO property exists;
* a constant is defined;
* a cloud resource appears in a deployment file;
* a service class exists;
* the README mentions it;
* an unfinished code path refers to it.

## Required analysis

### 1. Product purpose

Determine:

* the business problem;
* the intended retail use case;
* the expected value;
* the likely maturity level of the project;
* the decisions the resulting information was expected to support.

Separate documented purpose from inferred purpose.

### 2. Stakeholders and actors

Identify, where evidenced:

* retail organization;
* tenant;
* store operator;
* business analyst;
* system administrator;
* device installer;
* technical operator;
* observed visitor or shopper;
* external cloud-service provider;
* any other relevant actor.

Distinguish clearly between:

* users of the software;
* operators of the system;
* business consumers of the output;
* people observed by the system.

### 3. Business capabilities

Investigate capabilities including, but not limited to:

* visitor detection;
* face detection;
* repeat-person recognition;
* demographic estimation;
* emotion or smile analysis;
* store-entry observation;
* payment-zone observation;
* store-exit observation;
* visit reconstruction;
* conversion measurement;
* dwell-time analysis;
* reporting and visualization;
* device management;
* camera management;
* tenant or store separation.

For every capability assign one status:

* documented intention;
* observed implementation;
* partially implemented;
* uncertain;
* not found;
* conflicting evidence.

Do not use “implemented” unless an end-to-end implementation is supported.

### 4. Business processes and scenarios

Reconstruct supported or intended scenarios, such as:

* device onboarding;
* camera configuration;
* visitor entering a store;
* visitor moving through monitored zones;
* visitor appearing in a payment area;
* visitor leaving;
* repeat visit by a previously recognised person;
* creation and completion of a visit;
* reporting and analysis.

For each scenario distinguish:

* business intent;
* implementation evidence;
* missing steps;
* unresolved assumptions.

### 5. Domain model

Define and distinguish:

* tenant;
* store;
* device;
* camera;
* observation zone;
* person;
* face;
* identified person;
* biometric identifier;
* event;
* visit;
* enter;
* payment;
* leave;
* demographic attribute;
* recognition confidence;
* analytics output.

Investigate specifically whether:

* `person` means a real-world known individual or only a service-generated identity;
* `payment` means a confirmed commercial transaction or only observation near a payment-zone camera;
* `visit` is a business visit, a technical correlation construct, or both;
* event ordering and visit completion are explicit business rules or implementation-derived assumptions;
* tenant and store concepts are operationally enforced or only represented in the data model.

### 6. Business rules

Reconstruct rules concerning:

* event creation;
* event typing;
* assignment of a camera observation to an event type;
* creation of a visit;
* assignment of events to a visit;
* completion of a visit;
* recognition of repeat visitors;
* identity confidence;
* duplicate observations;
* missing events;
* out-of-order events;
* multiple cameras;
* multiple stores;
* tenant isolation.

Every rule must have a stable identifier and evidence status.

Examples:

* `BR-EVENT-001`
* `BR-VISIT-001`
* `BR-IDENTITY-001`
* `BR-TENANT-001`
* `BR-METRIC-001`

Do not convert implementation accidents into intended business rules without explicitly marking them as inferred.

### 7. Data and analytics

Identify:

* raw images;
* cropped face images;
* biometric identifiers or templates;
* inferred age;
* inferred gender;
* emotion or smile data;
* timestamps;
* device identifiers;
* camera identifiers;
* store identifiers;
* tenant identifiers;
* event sequences;
* visits;
* reporting and aggregation outputs.

Separate:

* captured data;
* stored data;
* externally processed data;
* inferred data;
* correlated data;
* derived metrics;
* merely possible metrics.

For metrics, distinguish:

* explicitly documented;
* demonstrably implemented;
* enabled by the data model;
* speculative and unsupported.

### 8. Privacy and ethics

Describe, without providing definitive legal advice:

* facial-image processing;
* biometric or persistent-person recognition;
* demographic inference;
* repeat-visitor tracking;
* image retention;
* purpose limitation;
* data minimization;
* transparency and consent;
* access and deletion concerns;
* misclassification and bias risk;
* differences between historical implementation context and current expectations.

Do not turn this section into a generic compliance checklist.

Tie observations to the actual system.

## Required output directory

Create documentation only under:

`docs/business-analysis/`

If the directory does not exist, create it.

Do not create or change files outside this directory.

## Required output files

### 1. `docs/business-analysis/business-analysis.md`

This is the principal analytical draft.

Use this structure:

1. Document status and scope
2. Analysis method
3. Executive analytical summary
4. Historical project context
5. Reconstructed business problem
6. Reconstructed product concept
7. Stakeholders and actors
8. Business capability map
9. End-to-end business scenarios
10. Domain model
11. Event lifecycle
12. Visit lifecycle
13. Reconstructed business rules
14. Data captured, stored, inferred, correlated, and derived
15. Reporting and analytical intent
16. Implemented versus intended capability
17. Business assumptions
18. Privacy, biometric-data, and ethical implications
19. Business limitations
20. Conflicting evidence
21. Unknowns
22. Stakeholder confirmation required
23. Evidence appendix

Clearly mark the document as:

`Analysis draft — not yet independently reviewed`

### 2. `docs/business-analysis/business-capability-catalog.md`

For each capability include:

* identifier;
* capability name;
* business purpose;
* intended stakeholder;
* input;
* expected output;
* status;
* evidence classification;
* supporting evidence;
* limitations;
* unresolved questions.

Use identifiers such as:

* `CAP-VISITOR-001`
* `CAP-IDENTITY-001`
* `CAP-VISIT-001`
* `CAP-ANALYTICS-001`

### 3. `docs/business-analysis/business-rules-catalog.md`

For each rule include:

* identifier;
* rule statement;
* domain area;
* evidence classification;
* implementation evidence;
* documentation evidence;
* known exceptions;
* ambiguity;
* stakeholder confirmation requirement.

### 4. `docs/business-analysis/business-glossary-draft.md`

For every term include:

* term;
* proposed definition;
* evidence classification;
* supporting evidence;
* alternative interpretation;
* ambiguity;
* related terms.

### 5. `docs/business-analysis/business-open-questions.md`

Group questions into:

* product purpose;
* stakeholders;
* capabilities;
* event semantics;
* visit semantics;
* identity and recognition;
* reporting and metrics;
* stores and tenants;
* privacy and data governance;
* operational use.

Prioritize every question:

* blocking;
* important;
* useful context.

### 6. `docs/business-analysis/business-evidence-index.md`

Map major conclusions to:

* conclusion identifier;
* document section;
* evidence classification;
* source artifacts;
* unresolved issue.

## Documentation requirements

Write all repository documentation in English.

Use language suitable for software developers and architects.

Be precise and neutral.

Avoid:

* marketing claims;
* modernization recommendations;
* target-state design;
* generic business-analysis filler;
* unsupported certainty;
* legal conclusions;
* moral judgement detached from actual system behavior.

## Change restrictions

You may only create files under:

`docs/business-analysis/`

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
* existing documentation;
* any Git metadata.

Do not:

* build the project;
* run the application;
* restore packages;
* deploy infrastructure;
* call live cloud services;
* add tests;
* fix defects;
* modernize anything;
* propose a target architecture.

This mission is static evidence-based business analysis only.

## Self-review before completion

Before finishing:

1. verify that all required files exist;
2. verify that every major conclusion has evidence;
3. search for inferences presented as facts;
4. verify that implemented and intended behavior are distinguished;
5. verify terminology consistency across all documents;
6. verify that business rules have stable identifiers;
7. verify that open questions contain genuine unresolved issues;
8. verify that no files outside `docs/business-analysis/` were modified;
9. verify that no Git operation was performed;
10. identify areas that should receive special attention during independent review.

## Completion report

Provide a concise completion report containing:

1. files created;
2. five most important reconstructed business findings;
3. five most consequential unknowns;
4. the most significant conflicting evidence;
5. the most important privacy or ethical concern;
6. areas requiring special attention during independent review;
7. confirmation that no Git operation was performed;
8. confirmation that no file outside `docs/business-analysis/` was modified.
