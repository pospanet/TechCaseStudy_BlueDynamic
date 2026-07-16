# Mission: Produce the Reviewed Final Business Description and Clean Up Intermediate Artifacts

## Role

Continue as the senior business analyst, product archaeologist, and domain modeller who performed the original evidence-based business analysis of Retailizer.

An independent reviewer has now assessed that analysis.

Your task is to:

1. evaluate every review finding independently;
2. verify each finding against the actual project files;
3. correct errors and unsupported conclusions;
4. preserve unresolved uncertainty where evidence is insufficient;
5. produce the final reviewed business description;
6. validate that the final document is complete and internally consistent;
7. remove intermediate analysis and review artifacts that are no longer needed.

This is the third and final stage of the business-documentation workflow:

1. evidence-based analysis;
2. independent review by another model;
3. reviewed final document and cleanup.

The resulting repository should contain a clean, maintainable business description rather than the internal working artifacts of the analysis process.

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
* make version-control decisions.

The human operator manages all versioning manually.

You may inspect ordinary project files and use normal filesystem operations only within the explicitly allowed documentation scope.

## Required inputs

Read and evaluate all available files under:

* `docs/business-analysis/`
* `docs/business-review/`

Expected inputs include:

### Original analysis artifacts

* `docs/business-analysis/business-analysis.md`
* `docs/business-analysis/business-capability-catalog.md`
* `docs/business-analysis/business-rules-catalog.md`
* `docs/business-analysis/business-glossary-draft.md`
* `docs/business-analysis/business-open-questions.md`
* `docs/business-analysis/business-evidence-index.md`

### Independent review

* `docs/business-review/business-analysis-review.md`

Also inspect the actual Retailizer project as needed to verify findings, including:

* root `README.md`;
* source code;
* solution and project structure;
* DTOs and domain entities;
* constants;
* API endpoints;
* message contracts;
* persistence models;
* configuration;
* infrastructure definitions;
* diagrams;
* initialization utilities;
* analytics references;
* comments and TODOs.

Do not accept either the original analysis or the review as authoritative without verification.

The project artifacts are the primary evidence base.

## Historical-project principle

Retailizer is a real historical project.

Describe it in the context of the period in which it was created.

Distinguish:

* historically reasonable design decisions;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete functionality;
* external technologies that later became obsolete;
* business assumptions requiring current privacy or ethical reconsideration.

Do not redesign or modernize the project in this mission.

Do not rewrite the root `README.md`.

## Review disposition process

Evaluate every review finding using one of these dispositions:

### Accepted

The finding is supported by project evidence and must affect the final document.

### Partially accepted

The finding identifies a real issue, but its interpretation or proposed correction is only partly supported.

### Rejected

The finding is not supported by project evidence.

### Superseded

A broader correction resolves the issue more accurately than the original finding.

### Stakeholder confirmation required

The issue cannot be resolved reliably from the available project evidence.

You must evaluate every review finding, but you do not need to preserve a separate disposition document in the final repository.

Use the disposition process internally while producing the final result.

Before cleanup, verify that:

* every accepted finding is reflected in the final document;
* every partially accepted finding is addressed to the supported extent;
* every rejected finding has been checked against contrary evidence;
* stakeholder-dependent issues remain visible as unresolved questions;
* no review finding has been silently ignored.

## Evidence classification

Use these evidence classifications in the final document:

### Observed

Directly supported by implementation artifacts.

### Documented

Explicitly stated in existing project documentation.

### Inferred

A reasoned interpretation supported by multiple pieces of evidence but not explicitly confirmed.

### Unknown

Cannot be determined from the available workspace.

### Conflicting evidence

Different project artifacts support incompatible conclusions.

### Stakeholder confirmation required

Cannot be resolved safely without original stakeholder knowledge.

Do not include a Git-history evidence category.

Do not present inference as fact.

Do not silently resolve conflicting evidence.

## Final output directory

Create:

`docs/business/`

If the directory does not exist, create it.

## Required final document

Create:

`docs/business/business-description.md`

This must be a self-contained, reviewed, publication-quality description of the business context and domain of Retailizer.

It must not depend on the reader having access to the removed analysis or review artifacts.

## Required structure

Use the following structure.

### 1. Document purpose and status

Explain:

* that the document reconstructs the business view of a historical project;
* that it is based on static project evidence;
* that it has undergone independent review;
* that unresolved areas remain explicitly marked;
* that it does not describe a future modernization target.

### 2. Executive business summary

Provide a concise explanation of:

* what Retailizer was;
* what business problem it attempted to solve;
* what information it attempted to provide;
* its likely proof-of-concept maturity;
* the most important limitations.

### 3. Historical project context

Describe:

* the technological and product context;
* the likely proof-of-concept nature;
* the role of cloud-based face and analytics services;
* the difference between historical suitability and current applicability.

Avoid unsupported historical claims.

### 4. Business problem

Explain:

* what retailers were expected to learn;
* which limitations of traditional footfall or conversion measurement the project attempted to address;
* which decisions the output was expected to support.

Clearly distinguish documented and inferred purposes.

### 5. Product concept

Describe the product concept from a business perspective:

* camera-based observation;
* local face detection;
* cloud analysis;
* event generation;
* visit correlation;
* analytics and reporting.

Do not turn this into a detailed technical architecture section.

### 6. Stakeholders and actors

Identify and distinguish:

* retail organization;
* tenant;
* store;
* store or business analyst;
* operational or technical administrator;
* device installer;
* device and camera operator;
* observed visitor or shopper;
* external analytics and cloud-service providers.

Explicitly distinguish:

* software users;
* business consumers of output;
* system operators;
* observed data subjects.

### 7. Business capability map

For every material capability include:

* identifier;
* capability name;
* business purpose;
* primary stakeholder;
* evidence-backed status;
* principal limitation or uncertainty.

Use statuses consistently:

* documented intention;
* observed implementation;
* partially implemented;
* uncertain;
* not found;
* conflicting evidence.

Capabilities should cover, where relevant:

* visitor observation;
* face detection;
* repeat-person recognition;
* demographic estimation;
* smile or emotion analysis;
* entry-zone observation;
* payment-zone observation;
* exit-zone observation;
* visit reconstruction;
* conversion-related analysis;
* dwell-time analysis;
* reporting and visualization;
* device onboarding;
* camera configuration;
* tenant and store separation.

Do not describe a capability as implemented merely because one supporting class or resource exists.

### 8. End-to-end business scenarios

Describe the principal scenarios.

For each scenario include:

* business objective;
* participants;
* preconditions;
* main flow;
* resulting business information;
* implementation limitation;
* unresolved assumptions.

Include, where supported:

* device onboarding;
* camera or zone setup;
* visitor entering;
* visitor appearing in a payment zone;
* visitor leaving;
* repeat visitor recognition;
* visit creation and completion;
* reporting and analysis.

### 9. Domain model

Define the important domain concepts.

At minimum address:

* tenant;
* store;
* device;
* camera;
* observation zone;
* face;
* person;
* identified person;
* biometric or service-generated identity;
* event;
* visit;
* enter;
* payment;
* leave;
* demographic attribute;
* confidence;
* analytics output.

For each materially ambiguous term include:

* final proposed definition;
* evidence status;
* important alternative interpretation;
* remaining uncertainty.

Pay special attention to:

#### Person

Do not imply a known real-world identity unless the project evidence demonstrates that.

#### Payment

Do not describe it as a confirmed commercial transaction unless a point-of-sale or equivalent confirmation mechanism is evidenced.

#### Visit

Explain whether it is:

* a business representation of store attendance;
* a technical correlation object;
* or both.

#### Camera and observation zone

Do not assume that every camera was formally modelled as a configurable business zone unless supported.

### 10. Event model

Describe:

* how observations become events;
* supported or intended event types;
* event business meaning;
* event source;
* relationship to camera or zone;
* event ordering;
* duplicate and missing-event uncertainty;
* confidence or identity implications.

Keep technical detail only where necessary to explain business semantics.

### 11. Visit model

Describe:

* how visits are created;
* how events are assigned;
* how visits appear to be completed;
* how repeat visitors affect correlation;
* missing or out-of-order event implications;
* multi-camera and multi-store uncertainty;
* the distinction between observed implementation and confirmed business rule.

### 12. Business rules

Provide a consolidated catalog of material business rules.

Use stable identifiers such as:

* `BR-EVENT-001`
* `BR-VISIT-001`
* `BR-IDENTITY-001`
* `BR-TENANT-001`
* `BR-METRIC-001`

For each rule include:

* rule statement;
* domain area;
* evidence classification;
* important exception or ambiguity;
* stakeholder confirmation requirement.

Include only rules necessary to understand or safely develop the system.

Do not preserve duplicate or low-value rules from the analytical draft.

### 13. Data captured, transmitted, processed, stored, inferred, and derived

Separate clearly:

* captured raw data;
* locally processed data;
* transmitted data;
* externally processed data;
* stored images;
* stored identifiers;
* inferred demographic or emotional attributes;
* correlated events;
* visits;
* derived reporting information;
* potential but unconfirmed metrics.

Address, where supported:

* full images;
* cropped face images;
* face identifiers;
* persistent person identifiers;
* age estimation;
* gender estimation;
* emotion or smile values;
* timestamps;
* device identifiers;
* camera identifiers;
* store identifiers;
* tenant identifiers;
* event records;
* visit records.

### 14. Reporting and analytical goals

Distinguish:

* explicitly documented reporting;
* reporting demonstrably connected in implementation;
* metrics enabled by the data model;
* metrics that are only plausible.

Address potential metrics such as:

* visitor count;
* repeat visitors;
* visit duration;
* payment-zone appearance;
* conversion proxy;
* demographic distribution;
* emotion or smile distribution;
* movement between observation zones.

Do not claim that a dashboard or metric was complete merely because Power BI or Stream Analytics was referenced.

### 15. Implemented versus intended capabilities

Provide a concise comparison table:

| Capability | Documented intent | Observed implementation | Final assessment | Key uncertainty |
| ---------- | ----------------- | ----------------------- | ---------------- | --------------- |

This section must expose gaps rather than smoothing them over.

### 16. Business assumptions

Document material assumptions embedded in the product concept, such as:

* a face observation representing a visitor;
* a camera representing a business zone;
* identity matching being sufficiently accurate;
* a payment-zone event acting as a purchase proxy;
* event sequences representing complete visits;
* captured demographics being useful for retail analysis.

Classify each assumption and explain its consequence.

### 17. Privacy, biometric-data, and ethical considerations

Tie this section directly to Retailizer.

Address:

* facial-image capture;
* persistent or repeat-person recognition;
* demographic estimation;
* emotion or smile analysis;
* repeat-visitor tracking;
* data retention uncertainty;
* transparency and consent;
* purpose limitation;
* data minimization;
* data access and deletion;
* misclassification;
* bias and demographic inference;
* historical proof-of-concept context;
* current deployment concerns.

Do not provide definitive legal advice.

State clearly that any current real-world deployment would require a separate legal, privacy, security, and ethical assessment.

### 18. Business limitations

Summarize evidence-backed limitations such as:

* proof-of-concept maturity;
* incomplete business workflows;
* unverified metrics;
* uncertain accuracy;
* ambiguous payment semantics;
* missing stakeholder documentation;
* unclear retention and governance;
* dependencies on external analytics services;
* limited operational controls.

### 19. Conflicting evidence

List material conflicts among:

* README;
* source code;
* configuration;
* infrastructure definitions;
* data model;
* comments or diagrams.

For each conflict include:

* conflicting interpretations;
* evidence;
* impact;
* safest current conclusion.

### 20. Confirmed unknowns and stakeholder questions

Include only meaningful unresolved questions that remain after analysis and review.

Group them by:

* product purpose;
* capability semantics;
* event and visit rules;
* identity;
* metrics;
* stores and tenants;
* privacy and governance;
* operational use.

Prioritize:

* blocking;
* important;
* useful context.

### 21. Evidence and traceability

Provide a compact evidence index mapping major sections and conclusions to:

* repository-relative file paths;
* relevant classes, methods, properties, endpoints, or resources;
* evidence classification;
* unresolved issue where applicable.

The final document must be independently understandable without the deleted evidence-index draft.

## Writing quality

Write in English.

The audience is software developers and architects preparing to work with the legacy project.

Use precise, neutral language.

Avoid:

* marketing language;
* generic business-analysis filler;
* repetitive evidence labels;
* unsupported certainty;
* speculation presented as likely fact;
* future architecture;
* modernization recommendations;
* implementation instructions.

The document should be comprehensive but not bloated.

Consolidate duplicate findings.

Prefer one clear statement with strong evidence over several repetitive statements.

## Review incorporation requirements

Before finalizing, explicitly verify:

* every critical review finding;
* every major review finding;
* every review finding affecting identity semantics;
* every review finding affecting payment semantics;
* every review finding affecting visit semantics;
* every privacy-related review finding;
* every capability-status correction;
* every evidence-classification correction.

Do not include a separate full review-disposition document in the final repository.

Instead, ensure that the final document reflects all supported corrections.

## Cleanup objective

After the final document has been created and fully validated, remove intermediate business-analysis and review artifacts that are no longer needed.

The cleanup must happen only after all necessary content has been incorporated into the final document.

## Files and directories to remove after successful validation

Delete:

* `docs/business-analysis/business-analysis.md`
* `docs/business-analysis/business-capability-catalog.md`
* `docs/business-analysis/business-rules-catalog.md`
* `docs/business-analysis/business-glossary-draft.md`
* `docs/business-analysis/business-open-questions.md`
* `docs/business-analysis/business-evidence-index.md`
* `docs/business-review/business-analysis-review.md`

After deleting the files:

* remove `docs/business-analysis/` if it is empty;
* remove `docs/business-review/` if it is empty.

Do not delete any file outside these two directories.

Do not delete the final document.

## Cleanup safety gate

Do not delete intermediate artifacts until all of the following are true:

1. `docs/business/business-description.md` exists;
2. all required final sections are present;
3. every material review finding has been evaluated;
4. accepted corrections are incorporated;
5. unresolved questions are preserved;
6. important evidence references are incorporated;
7. capability statuses are preserved;
8. business rules necessary for future development are preserved;
9. glossary definitions necessary for understanding are preserved;
10. privacy and ethical findings are preserved;
11. the final document can stand alone;
12. no unique material information would be lost by deleting the drafts.

If any unique material information would be lost, first incorporate it into the final document.

Do not retain intermediate files merely because they are easier to leave in place.

Do not delete them prematurely.

## Allowed filesystem changes

You may:

* create `docs/business/`;
* create or update `docs/business/business-description.md`;
* delete files specifically listed under the cleanup section;
* remove the two intermediate directories if they become empty.

Do not modify or delete:

* application source code;
* tests;
* project files;
* dependencies;
* configuration;
* deployment templates;
* scripts;
* root README;
* historical assets;
* unrelated documentation;
* Git metadata.

## Final validation

Before completion:

1. verify the final document is self-contained;
2. verify all required sections exist;
3. verify major claims contain evidence;
4. verify intended and implemented capabilities are distinguished;
5. verify inference is not presented as fact;
6. verify conflicting evidence remains visible;
7. verify payment is not overstated as a confirmed transaction;
8. verify person identity is not overstated;
9. verify visit semantics are appropriately qualified;
10. verify privacy and ethical concerns are system-specific;
11. verify unresolved stakeholder questions remain;
12. verify no modernization recommendations were introduced;
13. verify the root README was not modified;
14. verify only allowed documentation paths changed;
15. verify all listed intermediate files were removed;
16. verify empty intermediate directories were removed;
17. verify no Git operation was performed.

## Required completion report

Provide a concise completion report containing:

1. final document created;
2. intermediate files removed;
3. directories removed;
4. five most important corrections introduced after review;
5. review findings that could not be resolved;
6. most important remaining stakeholder questions;
7. confirmation that no unique material information was lost during cleanup;
8. confirmation that the final document is self-contained;
9. confirmation that no source code or root README was changed;
10. confirmation that no Git operation was performed;
11. confirmation that no unrelated file was modified or deleted.
