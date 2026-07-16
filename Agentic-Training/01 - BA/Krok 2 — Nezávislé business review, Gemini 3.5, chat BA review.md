# Mission: Independent Review of the Retailizer Business Analysis

## Role

Act as an independent senior business analyst, product reviewer, domain-modelling specialist, and evidence auditor.

You are reviewing a business analysis of Retailizer, a real historical software project.

The analysis was created by another model. Do not assume its conclusions are correct.

Your responsibility is to test the analysis independently against the actual project files and identify:

* factual errors;
* unsupported claims;
* incorrect evidence classifications;
* missing business areas;
* weak or incomplete reasoning;
* inconsistencies between documents;
* places where implementation details were mistaken for business intent;
* privacy or ethical issues that were overlooked or overstated.

This is the second stage of a controlled documentation workflow:

1. evidence-based business analysis;
2. independent review by a different model;
3. final reviewed business document produced later by the original author.

Your output is a review document. You must not rewrite or directly correct the original analysis.

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

## Review inputs

Review all available files under:

`docs/business-analysis/`

Expected inputs include:

* `business-analysis.md`
* `business-capability-catalog.md`
* `business-rules-catalog.md`
* `business-glossary-draft.md`
* `business-open-questions.md`
* `business-evidence-index.md`

Also independently inspect the Retailizer project evidence, including:

* root `README.md`;
* existing project documentation;
* solution and project structure;
* source code;
* domain entities and DTOs;
* constants;
* API endpoints;
* message contracts;
* persistence models;
* configuration;
* infrastructure and deployment files;
* diagrams;
* initialization utilities;
* comments and TODOs;
* analytics references;
* relevant sample assets.

Do not rely only on evidence quoted by the original author.

Verify significant claims directly.

## Historical-project principle

Retailizer is a real historical project.

Review it in the context of the period when it was created.

Do not classify an old platform or dependency as an original design failure merely because it is obsolete today.

Distinguish:

* historically reasonable design choices;
* implementation defects;
* proof-of-concept shortcuts;
* incomplete functionality;
* later platform obsolescence;
* business assumptions requiring current privacy or ethical reconsideration.

Do not propose modernization or a target product.

## Evidence classification to verify

The original analysis should use:

### Observed

Directly supported by implementation artifacts.

### Documented

Explicitly stated in existing documentation.

### Inferred

A reasoned interpretation supported by multiple pieces of evidence.

### Unknown

Cannot be determined from the workspace.

### Conflicting evidence

Different artifacts support incompatible conclusions.

### Stakeholder confirmation required

Cannot be safely resolved through project analysis.

Identify cases where the author:

* assigned the wrong evidence status;
* presented an inference as fact;
* treated documentation as proof of implementation;
* treated implementation as proof of business intent;
* failed to represent conflicting evidence;
* used “unknown” despite available evidence;
* reached a conclusion requiring stakeholder confirmation.

## Core review principle

The repository is the evidence base.

The business-analysis documents are a set of claims to be tested.

A coherent and polished narrative is not sufficient.

## Mandatory review areas

### 1. Business problem and product purpose

Verify whether the analysis correctly reconstructs:

* the retail problem being addressed;
* the expected business value;
* the product or proof-of-concept nature of Retailizer;
* intended business decisions supported by the system.

Identify unsupported expansion beyond available evidence.

### 2. Stakeholders and actors

Verify whether the analysis distinguishes correctly between:

* retail organization;
* tenant;
* store operator;
* business analyst;
* system administrator;
* device installer;
* technical operator;
* observed visitor or shopper;
* external cloud-service providers.

Pay particular attention to the distinction between:

* users of the software;
* consumers of reports;
* operators of devices;
* people observed by cameras.

### 3. Business capabilities

Review every major capability and its assigned status.

Check whether it is accurately classified as:

* documented intention;
* observed implementation;
* partially implemented;
* uncertain;
* not found;
* conflicting evidence.

Look especially for overstatements concerning:

* repeat-person recognition;
* demographic analysis;
* emotion or smile detection;
* entry detection;
* payment detection;
* exit detection;
* visit reconstruction;
* conversion calculation;
* dwell-time analysis;
* reporting;
* device management;
* tenant isolation.

The presence of a class, property, service, or cloud resource is not sufficient proof of an operational end-to-end capability.

### 4. End-to-end scenarios

Verify the scenarios reconstructed by the author, including:

* device onboarding;
* camera configuration;
* visitor entering a store;
* visitor moving between monitored zones;
* visitor appearing in a payment area;
* visitor leaving;
* repeated visitor recognition;
* visit creation;
* visit completion;
* reporting and analytics.

Identify:

* missing steps;
* unsupported assumptions;
* absent error or exception paths;
* gaps between business intent and technical implementation.

### 5. Domain model

Review definitions of:

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

Pay special attention to these risks:

* `person` being incorrectly described as a known real-world identity;
* `payment` being incorrectly described as a confirmed transaction;
* `visit` being described as a clear business concept when it may primarily be a technical correlation object;
* tenant and store separation being described as enforced when it may only exist in data fields;
* camera identity being treated as equivalent to a business zone without sufficient evidence.

### 6. Business rules

Review all rules in `business-rules-catalog.md`.

For each rule, verify:

* whether it exists in code or documentation;
* whether it is an intended business rule or implementation-derived behavior;
* whether its evidence classification is correct;
* whether exceptions and ambiguity are represented;
* whether stakeholder confirmation is needed.

Focus particularly on:

* event creation;
* event typing;
* camera-to-event mapping;
* visit creation;
* event-to-visit assignment;
* visit completion;
* recognition confidence;
* duplicate events;
* missing events;
* out-of-order events;
* multiple cameras;
* multiple stores;
* tenant isolation.

### 7. Data and metrics

Verify the description of:

* images;
* cropped face images;
* biometric identifiers;
* age estimation;
* gender estimation;
* emotion or smile data;
* timestamps;
* device and camera identifiers;
* tenant and store identifiers;
* events;
* visits;
* analytical outputs.

Check whether the analysis correctly distinguishes:

* captured data;
* transmitted data;
* stored data;
* externally processed data;
* inferred data;
* correlated data;
* derived metrics;
* merely possible metrics.

Identify metrics described as implemented when they are only enabled by the available data model.

### 8. Privacy and ethics

Assess whether the analysis adequately addresses actual Retailizer behavior concerning:

* facial-image processing;
* persistent or repeat-person recognition;
* demographic inference;
* repeat-visitor tracking;
* image retention;
* data minimization;
* transparency;
* consent;
* access and deletion;
* misclassification;
* bias;
* current expectations versus historical project context.

Do not provide definitive legal advice.

Identify both:

* material omissions;
* unsupported or exaggerated claims.

### 9. Internal consistency

Check consistency across all business-analysis documents.

Verify that:

* capability identifiers are stable;
* capability statuses agree;
* business-rule identifiers are stable;
* glossary definitions match their use in the main analysis;
* open questions reflect genuine unresolved issues;
* evidence-index entries point to the correct conclusions;
* the same term is not defined differently across files;
* an item is not marked both implemented and uncertain without explanation.

### 10. Evidence quality

Independently verify at minimum:

* ten major conclusions;
* ten business rules;
* all high-impact inferred claims;
* all claims marked as conflicting evidence;
* all privacy-relevant claims;
* all capability-status claims with material business importance.

Record the reviewed sample in the review appendix.

## Finding severity

Classify each finding as one of the following.

### Critical

The analysis materially misrepresents:

* business purpose;
* core domain meaning;
* product behavior;
* identity or payment semantics;
* privacy implications;
* capability implementation status.

### Major

An important conclusion is unsupported, incorrect, contradictory, materially incomplete, or incorrectly classified.

### Moderate

A meaningful ambiguity, missing scenario, incomplete rule, weak evidence reference, or terminology problem exists.

### Minor

A local precision, consistency, traceability, or documentation issue.

### Observation

A useful improvement or caution that is not an actual defect.

## Finding identifiers

Use stable identifiers:

* `BREV-CRIT-001`
* `BREV-MAJ-001`
* `BREV-MOD-001`
* `BREV-MIN-001`
* `BREV-OBS-001`

Do not renumber findings later within the same review.

## Finding format

For every finding include:

* identifier;
* severity;
* affected file;
* affected section or item identifier;
* claim under review;
* reviewer assessment;
* evidence classification used by the author;
* evidence classification recommended by the reviewer;
* independently verified project evidence;
* why the issue matters;
* required correction;
* stakeholder confirmation requirement.

Example:

```markdown id="qynkxa"
### BREV-MAJ-001 — Payment is described as a confirmed transaction

**Affected artifact:** `business-analysis.md`, section 11

**Author claim:** The payment event confirms that a shopper completed a purchase.

**Reviewer assessment:** The available implementation appears to derive the
event from a configured camera or observation zone. No integration with a
point-of-sale transaction system was found.

**Current evidence classification:** Observed

**Recommended evidence classification:** Inferred, with stakeholder
confirmation required

**Evidence:**
- `<repository-relative file and symbol>`
- `<repository-relative file and symbol>`

**Why it matters:** This changes the interpretation of conversion metrics.

**Required correction:** Describe the event as an observation associated with
the payment area unless additional evidence confirms transaction completion.

**Stakeholder input required:** Yes
```

## Required output directory

Create documentation only under:

`docs/business-review/`

If the directory does not exist, create it.

Do not create or change files outside this directory.

## Required output

Create:

`docs/business-review/business-analysis-review.md`

Use this structure:

1. Document status and purpose
2. Review scope
3. Review method
4. Inputs reviewed
5. Overall assessment
6. Summary of findings
7. Critical findings
8. Major findings
9. Moderate findings
10. Minor findings
11. Observations
12. Missing business areas
13. Unsupported or overconfident claims
14. Evidence-classification issues
15. Domain-model assessment
16. Business-rule assessment
17. Capability-status assessment
18. Data and metrics assessment
19. Privacy and ethics assessment
20. Internal-consistency assessment
21. Required corrections before finalization
22. Stakeholder questions introduced or changed by review
23. Review coverage appendix

Include a summary table:

| ID | Severity | Area | Required action | Stakeholder input |
| -- | -------- | ---- | --------------- | ----------------- |

Clearly mark the document as:

`Independent review — original analysis not modified`

## Reviewer restrictions

You may only create files under:

`docs/business-review/`

Do not modify:

* any file under `docs/business-analysis/`;
* source code;
* tests;
* project files;
* dependencies;
* configuration;
* deployment files;
* root README;
* existing historical artifacts;
* Git metadata.

Do not:

* rewrite the original analysis;
* directly correct the capability catalog;
* directly correct the rules catalog;
* create the final business description;
* propose modernization;
* propose a target architecture;
* design new product capabilities;
* resolve ambiguities without evidence;
* perform Git operations;
* build or run the project;
* call live external services.

## Review quality threshold

The review is insufficient if it primarily:

* praises the analysis;
* reformats content;
* suggests stylistic improvements;
* repeats the author’s conclusions;
* provides generic privacy commentary.

The review must independently challenge and verify the analysis.

If you find no critical or major issue, document enough verification detail to justify that result.

Do not invent defects merely to appear critical.

## Self-review before completion

Before finishing:

1. verify all mandatory review areas were covered;
2. verify every finding contains project evidence;
3. verify finding severities are justified;
4. verify the original analysis was not modified;
5. verify no file outside `docs/business-review/` changed;
6. verify no Git operation was performed;
7. verify all finding identifiers are unique;
8. verify the summary table matches the detailed findings;
9. identify the corrections that must be resolved before a final document can be accepted;
10. identify any reviewer uncertainty explicitly.

## Completion report

Provide a concise completion report containing:

1. file created;
2. number of findings by severity;
3. three strongest parts of the original analysis;
4. five most important required corrections;
5. newly identified stakeholder questions;
6. most important privacy or ethical review result;
7. any review limitation;
8. confirmation that the original analysis was not modified;
9. confirmation that no Git operation was performed;
10. confirmation that no file outside `docs/business-review/` was modified.
