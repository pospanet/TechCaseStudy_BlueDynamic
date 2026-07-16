# Prompt Set 1 — Security Audit and Remediation

## Recommended workflow

| Phase | Model | Skill | Chat |
|---|---|---|---|
| Analysis | Claude Opus 4.6 | `explore-repository` | New chat |
| Independent review | GPT-5.4 | `explore-repository` | New chat |
| Implementation | GPT-5.1-codex | `implement-task` + `validate-project` | New chat |
| Change review | Claude Sonnet 4.6 | `review-change` | New chat |

## A. Security analysis

```text
Use @skills:explore-repository.

Perform a comprehensive read-only security audit of the current repository.

Follow all applicable AGENTS.md files and repository governance instructions. Do not modify files, install dependencies, build the solution, execute application code, access external services, or reveal secret values.

First establish the system architecture, entry points, trust boundaries, external integrations, privileged identities, attacker-controlled inputs, sensitive data flows, authentication model, authorization model, tenant and device identity model, and deployment assumptions.

Then inspect the complete relevant source and configuration for concrete security weaknesses, especially:

- missing or broken authentication
- missing or broken authorization
- insecure direct object access
- trust in client-supplied identifiers
- device identity and registration abuse
- credential issuance, disclosure, storage, rotation and revocation
- secrets or broad service credentials embedded in client applications
- excessive Azure permissions
- tenant, customer, store or device isolation failures
- input validation and injection risks
- unsafe query construction, parsing, serialization or deserialization
- path, blob-name or storage-object manipulation
- sensitive-data exposure and unsafe logging
- insecure error handling and information disclosure
- replay, duplicate processing and idempotency weaknesses
- race conditions and concurrent update risks
- denial-of-service and unbounded resource consumption
- insecure deployment defaults
- obsolete dependencies and supply-chain risks
- facial-image, biometric and demographic-data handling
- missing retention, deletion, consent or data-subject controls

Prioritize findings supported by an actual attacker-controlled source, execution path, missing or bypassable control, affected asset and realistic impact. Do not report generic recommendations as vulnerabilities.

For every finding provide:

1. identifier and title
2. severity: Critical, High, Medium, Low or Informational
3. confidence: High, Medium or Low
4. evidence classification: Observed, Documented, Inferred, Unknown or Conflicting Evidence
5. relevant CWE or security category
6. exact affected files, classes and methods
7. trust boundary and attacker prerequisites
8. complete attack or failure path
9. impact
10. existing mitigating controls
11. missing or ineffective controls
12. recommended remediation
13. required static, runtime, deployment or stakeholder verification

Separate confirmed vulnerabilities, probable risks, architectural concerns, privacy risks and false-positive candidates.

Produce:

# Executive Summary
# Scope and Constraints
# Architecture and Trust Boundaries
# Sensitive Data Inventory
# Attack Surface
# Findings by Severity
# Credential and Secret-Handling Assessment
# Authorization and Isolation Assessment
# Biometric and Privacy Risk Assessment
# Azure and Deployment Security Assessment
# Dependency and Supply-Chain Risk
# Existing Security Controls
# Exploit Chains
# Recommended Remediation Order
# Required Dynamic Verification
# Review Coverage and Limitations

End with the five highest-value remediation candidates ranked by risk reduction, implementation effort and regression risk.
```

## B. Independent security review

```text
Use @skills:explore-repository.

Perform an independent second-pass security review of the current repository. Do not rely on conclusions from any previous audit and do not modify files.

Challenge the most likely first-pass conclusions and search specifically for:

- missed attack paths through wrappers, background jobs and asynchronous processing
- authorization enforced only in UI or client code
- inconsistent validation between entry points
- privilege escalation through device registration or credential recovery
- cross-tenant, cross-store or cross-device data access
- insecure interactions between UWP, backend, WebJob, storage, IoT Hub, Service Bus, DocumentDB and Face API
- exploit chains formed by several individually moderate weaknesses
- findings that appear severe but are not actually exploitable
- undocumented mitigations or controls already present in code
- deployment assumptions that cannot be verified statically

For each finding, trace attacker-controlled input to the affected asset and identify the exact missing or bypassed control. Cite repository-relative files and symbols.

Return:

1. independently confirmed findings
2. new findings missed by a typical first pass
3. likely false positives and why
4. disputed or uncertain findings requiring runtime evidence
5. the five most important security tests to execute
6. an overall security-risk assessment

Do not modify the repository.
```

## C. Security remediation implementation

```text
Use @skills:implement-task.

Implement the smallest coherent set of high-confidence security remediations in the current repository.

First inspect the repository and determine which confirmed security findings can be fixed safely without redesigning the complete system. Prioritize exploitable authentication, authorization, credential-handling, input-validation and sensitive-data-exposure defects. Do not implement speculative privacy policy, legal compliance or large architectural redesigns as code changes.

Before editing, present a concise plan containing:

- selected findings and why they were chosen
- files expected to change
- intended security behavior
- compatibility implications
- validation strategy
- excluded findings and why they are out of scope

Implementation rules:

- make the smallest complete changes
- preserve existing architecture and behavior where security permits
- do not perform unrelated refactoring or dependency upgrades
- do not introduce or expose secret values
- reject invalid or unauthorized requests explicitly
- use least privilege where achievable within the existing design
- add focused regression tests for each changed security behavior where the repository supports tests
- document configuration or deployment actions that cannot be implemented in source
- do not claim a vulnerability is fixed when a required external control remains unverified

After implementation, use @skills:validate-project to run all safe and relevant checks. Inspect the complete diff and report:

- findings remediated
- files changed
- tests added or updated
- validation commands and results
- security risks that remain
- deployment or secret-rotation steps still required
```

## D. Review the security changes

```text
Use @skills:review-change.

Independently review the current uncommitted security-remediation changes.

Focus on:

- whether each intended vulnerability is actually closed
- alternate or bypass paths
- authentication and authorization regressions
- accidental credential or sensitive-data exposure
- insecure error handling
- compatibility regressions
- missing negative tests
- changes that create denial-of-service or availability risks
- findings incorrectly declared fixed despite external dependencies
- unrelated changes or excessive scope

Inspect the complete diff and surrounding execution paths. Do not modify files.

For every finding provide severity, exact location, concrete bypass or failure scenario, impact and recommended correction. End with a disposition: accept, accept with minor corrections, revise or reject.
```
