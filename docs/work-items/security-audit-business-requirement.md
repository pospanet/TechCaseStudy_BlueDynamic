# Security Audit and Remediation — Business Requirement

---

## 1. Business Problem

Retailizer is a historical proof-of-concept retail analytics system that processes **biometric data** (facial images, persistent identity templates, demographic estimates) across multiple trust boundaries (IoT device → backend API → cloud storage → external cognitive services → document database). Static inspection of the codebase reveals that the system was built without authentication, authorization, input validation, tenant isolation enforcement, credential scoping, or data-protection controls.

The system cannot currently be safely demonstrated, reused, or deployed — even in a controlled environment — because:

- Any network-reachable caller can invoke the device registration endpoint and receive IoT Hub symmetric keys. *[Observed: `DeviceController.cs` — no authentication middleware in `Startup.cs`]*
- Biometric data from all tenants is stored in a single shared Face API person group, enabling cross-tenant identity recognition. *[Observed: `Functions.cs` line 21 — hardcoded `personGroupId = "retailizer"`]*
- DocumentDB queries interpolate identifiers without parameterization, creating a potential injection surface. *[Observed: `DocumentDbService.cs` line 58, `Functions.cs` line 228]*
- Face images persist indefinitely in blob storage with no access controls beyond the storage account key. *[Observed: blob deletion commented out in `Functions.cs` lines 80, 168]*
- Credentials (storage connection strings, IoT Hub keys, Face API keys, DocumentDB account keys) are distributed broadly and without least-privilege scoping. *[Observed: `Configuration.cs`, `App.config`, `appsettings.json`, ARM template outputs]*

**Business impact:** Without a structured security audit and remediation, the repository cannot be responsibly shared, demonstrated, or used as a basis for any future work that handles real data.

**Evidence classification:** Observed (source code inspection) + Documented (business description §17, technical description §22, failure modes catalog, verification backlog VER-SEC-001 through VER-SEC-003).

---

## 2. Current State

### 2.1 Authentication and authorization

- **Backend API:** No authentication or authorization middleware. `Startup.Configure` calls only `app.UseMvc()`. No `[Authorize]` attributes on any controller. *[Observed: `Startup.cs` lines 46–52, `DeviceController.cs`]*
- **Device registration:** Any caller can POST to `/api/device` and receive an IoT Hub device symmetric key in plaintext. *[Observed: `DeviceController.cs` lines 21–33]*
- **Tenant validation:** `TenantId` and `StoreId` flow through DTOs and queries but are never validated or enforced at any API boundary. *[Observed: data model; documented: business description §12 BR-TENANT-001]*

### 2.2 Credential and secret management

- UWP device configuration uses placeholder strings for storage connection strings, backend URL, and IoT Hub URL. When populated with real values, the device binary would contain broad storage account credentials. *[Observed: `Configuration.cs` lines 15–21]*
- Backend returns IoT Hub device symmetric keys in HTTP response body without transport-layer or application-layer protection. *[Observed: `DeviceController.cs` line 30]*
- ARM template outputs full account keys for Storage, Service Bus (`RootManageSharedAccessKey`), IoT Hub (`iothubowner`), DocumentDB, Face API, and Emotion API. *[Observed: `template.json` lines 239–271]*
- No credential rotation, key-vault integration, SAS-token scoping, or managed-identity usage is observed anywhere.

### 2.3 Input validation

- Device registration accepts a `Device` object from the request body with no validation. *[Observed: `DeviceController.cs` line 23]*
- WebJob parses queue messages as `JObject` with no schema validation. *[Observed: `Functions.cs` lines 45–50]*
- `GetAllVisitsByPersonIdAsync` builds a SQL string by interpolating `personId` without quoting or parameterization. *[Observed: `DocumentDbService.cs` line 58]*
- `FindOpenVisitForEventAsync` interpolates `TenantId`, `StoreId`, and `PersonId` into SQL strings. Values are string-interpolated with surrounding double quotes but not parameterized. *[Observed: `Functions.cs` line 228]*

### 2.4 Tenant and device isolation

- Single Face API person group `"retailizer"` shared across all tenants. *[Observed: `Functions.cs` line 21]*
- Device lookup uses only `id` — no tenant or authorization check. *[Observed: `DocumentDbService.cs` lines 44–53]*
- DocumentDB collections have no partition-key policy, unique-key policy, or access-control differentiation per tenant. *[Observed: initializer creates collections with `Id` only]*

### 2.5 Biometric and sensitive data protection

- Cropped face JPEG images persist indefinitely in blob storage. Deletion code is commented out. *[Observed: `Functions.cs` lines 80, 168]*
- Biometric identity templates persist indefinitely in Cognitive Services. *[Observed: no deletion or expiration mechanism]*
- No consent, privacy notice, opt-out, data-subject access, or deletion mechanism exists. *[Documented: business description §17]*
- `satya.jpg` — a real person's image — is used as Face API seed data. *[Observed: `Retailizer.Azure.Initializer/Program.cs` line 70]*
- No data retention policy, lifecycle management, or purge mechanism is implemented. *[Documented: business description §17]*

### 2.6 Operational security

- Logging is limited to `Debug.WriteLine` and `TextWriter log.WriteLine`. *[Observed: across all projects]*
- No structured audit trail, correlation IDs, security event logging, or anomaly detection. *[Documented: technical description §21]*
- WebJob dashboard connection string is disabled. *[Observed: `Reailizer.Job/Program.cs`]*

---

## 3. Target State

After successful audit and remediation:

1. **Every protected operation** (device registration, credential issuance, data access, administrative actions) **requires authenticated and authorized callers.**
2. **Credentials are scoped** to the minimum privilege required for each component and are not embedded in client binaries or returned unnecessarily in API responses.
3. **Tenant and device data are isolated** — a caller in one tenant cannot access or influence another tenant's devices, events, visits, or biometric identities.
4. **Biometric data** (face images, identity templates) has defined retention, access controls, and deletion capabilities.
5. **Input validation** prevents injection, malformed payloads, and unauthorized identifier substitution at all trust boundaries.
6. **Security-relevant events** (registration attempts, authentication failures, authorization denials, data access) are logged in a structured, auditable format.
7. **All confirmed Critical and High severity findings** are either remediated or have an explicitly approved and documented exception.
8. **Regression tests** exist for every remediated exploit path.
9. **Clear conditions** are documented under which the system may be demonstrated, reused, or deployed.

---

## 4. Explicit Business Objective

Conduct a systematic security audit of the Retailizer codebase and its documented architecture, identify all security vulnerabilities with severity classification, and remediate confirmed Critical and High findings — so that the repository can be responsibly shared, demonstrated, or used as a foundation for future work without exposing biometric data, credentials, or tenant information to unauthorized access.

---

## 5. Scope

### 5.1 In scope

- **Static security analysis** of all production source code (`Backend/`, `Common/`, `Reailizer.Job/`, `Retailizer.UWP/`, `Retailizer.Azure.Initializer/`).
- **Configuration and infrastructure review** (`*.config`, `appsettings*.json`, `project.json`, `*.csproj`, `*.xproj`, ARM templates, `parameters.json`, `deploy.ps1`).
- **Trust-boundary analysis** for all integration points identified in `docs/technical/integration-catalog.md`.
- **Authentication and authorization** assessment for all API endpoints, service connections, and data access paths.
- **Credential lifecycle** review — distribution, storage, scoping, rotation, and exposure risk.
- **Input validation** assessment at all external-facing and cross-component interfaces.
- **Tenant and device isolation** verification in data model, queries, and biometric service usage.
- **Biometric data protection** — storage, retention, access, and deletion capabilities for face images and identity templates.
- **Sensitive data in repository** — identification of any committed secrets, credentials, or personal data.
- **Severity classification** of each finding (Critical / High / Medium / Low / Informational).
- **Remediation** of all confirmed Critical and High findings.
- **Regression tests** for remediated exploit paths.
- **Conditions documentation** — defining under which circumstances the system may be demonstrated, reused, or deployed.

### 5.2 Boundaries

- This initiative covers the repository as-is. It does not include modernization of the technology stack, dependency upgrades, or platform migration (those are separate initiatives).
- Remediation must preserve existing business behavior unless a security fix necessarily changes it (documented as a business constraint).
- The audit covers what is statically verifiable. Runtime, deployment, and external-service verification items that cannot be performed in the current environment are documented as verification-required items, not as resolved.

---

## 6. Out of Scope

- Technology-stack modernization (framework upgrades, project-format conversion, SDK replacement).
- Functional defect remediation unrelated to security (e.g., visit-update `CreateDocumentAsync` bug, double event persistence, unthrottled capture loop) — unless a functional defect is itself a security vulnerability.
- Performance optimization.
- UI/UX changes.
- New feature development.
- Privacy impact assessment or legal compliance certification (the audit identifies gaps but does not constitute legal advice).
- Deployment to any external environment.
- Modification of governance documents (`AGENTS.md`, constitution, permission matrix).
- Modification of approved business or technical documentation (findings are documented separately).
- Operational monitoring and alerting implementation beyond security-event logging.

---

## 7. Stakeholders and Affected Users or Systems

| Stakeholder | Interest | Impact |
| --- | --- | --- |
| **Repository owner / human operator** | Responsible for acceptance of all security changes (Level C). Decides risk tolerance and exception approvals. | Decision authority for all remediation. |
| **Future developers / agents** | Will work on the codebase after remediation. Need clear security boundaries and patterns. | Consume remediated code and security documentation. |
| **Blue Dynamic (historical partner)** | Original partner company. May have interest if the codebase is reused. | Stakeholder confirmation may be needed for business intent. |
| **Store visitors (data subjects)** | Subjects of biometric observation. No direct system access. | Biometric data protection directly affects their privacy rights. |
| **Retail organization (tenant)** | Business entity whose data must be isolated. | Tenant isolation failures expose their customer and operational data. |
| **IoT devices** | Edge devices that authenticate and transmit biometric data. | Credential management and device identity directly affect device security posture. |
| **Azure platform services** | External services (IoT Hub, Face API, DocumentDB, Blob Storage, Service Bus). | Credential scoping and access controls determine blast radius of compromise. |

---

## 8. Business Value

1. **Risk reduction:** Eliminates known credential-exposure, injection, and unauthorized-access paths before the codebase is shared or reused.
2. **Responsible stewardship of biometric data:** Establishes minimum controls for face images and identity templates — data categories with significant legal and ethical sensitivity.
3. **Demonstrability:** Enables the repository to be used for demonstrations, training, or case-study purposes without exposing real credentials or enabling exploitation.
4. **Foundation for further work:** All subsequent development tasks can build on a baseline where security controls exist rather than having to work around their absence.
5. **Regulatory readiness:** Addresses the most critical gaps relative to GDPR Article 9, CCPA, BIPA, and similar biometric-data frameworks, reducing future compliance effort.
6. **Historical evidence preservation:** Documents what the original PoC lacked, clearly separated from what is being added — maintaining the repository's value as an honest historical artifact.

---

## 9. Risks of Doing Nothing

| Risk | Severity | Likelihood | Consequence |
| --- | --- | --- | --- |
| Credential exposure if repository is shared | Critical | High if shared | IoT Hub, Storage, DocumentDB, Face API keys compromised |
| Unauthorized device registration enables IoT Hub access | High | High if backend is reachable | Attacker obtains device symmetric key, can send arbitrary messages |
| Cross-tenant biometric leakage | High | Certain (by design) | Person identified in tenant A is recognizable in tenant B |
| SQL/NoSQL injection via unparameterized queries | Medium–High | Depends on input path | Data exfiltration or unauthorized data access |
| Indefinite biometric data retention without controls | High | Certain (by design) | Regulatory non-compliance; no ability to respond to data-subject requests |
| Repository used as template propagates vulnerabilities | High | Medium | Security anti-patterns replicated in derived projects |
| `satya.jpg` seed data — ethical and reputational risk | Medium | Low–Medium | Using a real person's biometric data without consent in distributed code |

---

## 10. Constraints and Dependencies

| ID | Constraint or dependency | Type | Impact |
| --- | --- | --- | --- |
| C-01 | Obsolete toolchain — ASP.NET Core 1.0 `xproj`, .NET Framework 4.5.2, UWP IoT, Project Oxford SDK. Building requires a historical environment. | Technical | Remediation must work within the existing framework or document required framework changes as separate prerequisites. |
| C-02 | No existing test suite. | Technical | Regression tests must be created from scratch. Test infrastructure may need to be established as a prerequisite task. |
| C-03 | All security-related changes are Level C under the repository governance. | Governance | Requires explicit scope, impact analysis, independent specialist review, disposition of every material finding, and explicit human approval. |
| C-04 | Protected areas (production source, configuration, ARM templates) require explicit task authorization for modification. | Governance | Each remediation task must explicitly list writable paths. |
| C-05 | Build verification is blocking (VER-BUILD-001, VER-BUILD-002). | Technical | Runtime verification of security fixes may be impossible until the build environment is reconstructed. |
| C-06 | Face API / Project Oxford SDK is historical and potentially retired. | External | Biometric isolation remediation may require service-level changes that cannot be tested against the original API. |
| C-07 | Business behavior is protected — security remediation must not change business semantics without explicit authorization. | Governance | Authentication, validation, and isolation changes must preserve existing functional contracts where possible. |
| C-08 | No runtime or deployment environment is currently available. | Technical | Verification is limited to static analysis and design review until an environment is established. |

---

## 11. Assumptions

| ID | Assumption | Evidence | Classification | Consequence if invalid |
| --- | --- | --- | --- | --- |
| A-01 | The repository will not be deployed to a production environment handling real customer data without separate legal, privacy, and security assessment beyond this audit. | Task scope limits to audit and remediation of code-level findings. | Inferred | If deployed without further assessment, code-level fixes alone are insufficient for regulatory compliance. |
| A-02 | Build environment reconstruction is a separate prerequisite initiative that will be completed before or in parallel with remediation implementation. | VER-BUILD-001 is blocking in verification backlog. | Inferred | If build is not available, remediation can be designed and reviewed but not compiled or runtime-verified. |
| A-03 | The backend API had no external protection (gateway, App Service auth, network controls) in the historical deployment. | No source-level auth found (VER-SEC-001). | Unknown — stakeholder verification required | If external protection existed, some findings may be reclassified from Critical to Medium. The code-level absence remains a finding regardless. |
| A-04 | Storage account keys were embedded in device builds for the hackfest demonstration. | Static configuration pattern in `Configuration.cs`. | Inferred (VER-SEC-002) | If keys were injected via a secure build pipeline, device credential exposure risk is lower than assessed. |
| A-05 | Single Face API person group across all tenants was an intentional hackfest simplification, not a design decision. | Hardcoded constant with no parameterization. | Inferred | If intentional, tenant isolation remediation requires a business decision about backward compatibility. |
| A-06 | The `GetAllVisitsByPersonIdAsync` method's string interpolation pattern is reachable with attacker-controlled input. | Method exists but no direct API endpoint calls it in observed code. Call path requires further verification. | Verification Required | If not reachable, the injection finding is reduced to Informational. The pattern should still be fixed as defense-in-depth. |

---

## 12. Non-Functional Expectations

| Category | Expectation |
| --- | --- |
| **Security** | All API endpoints require authentication. Credential issuance requires authorization. Secrets are not returned in API responses unless strictly necessary and then only over authenticated channels. |
| **Data isolation** | Tenant A's data (devices, events, visits, biometric identities) is inaccessible to tenant B at every layer — API, persistence, and external service. |
| **Data protection** | Biometric data (face images, identity templates) has defined maximum retention, access controls, and a deletion mechanism. |
| **Input integrity** | All external inputs are validated before processing. Database queries use parameterized patterns. |
| **Auditability** | Security-relevant events are logged with sufficient detail for post-incident analysis without exposing sensitive data values. |
| **Backward compatibility** | Existing API contracts, message formats, and data schemas are preserved unless a security fix necessarily changes them. Breaking changes are documented. |
| **Testability** | Every remediated exploit path has at least one automated regression test that verifies the fix. |
| **Documentation** | Conditions for safe demonstration, reuse, and deployment are explicitly documented. |

---

## 13. Mandatory Acceptance Criteria

### AC-01 — Unauthenticated caller cannot invoke protected operations

**Given** an HTTP client without authentication credentials,
**When** the client sends a POST request to `/api/device`,
**Then** the server responds with HTTP 401 Unauthorized,
**And** no IoT Hub device key is returned in the response.

### AC-02 — Client identifiers are validated and authorized

**Given** an authenticated caller with a valid tenant context,
**When** the caller submits a device registration request with a `DeviceId` that does not belong to the caller's tenant,
**Then** the server rejects the request with HTTP 403 Forbidden,
**And** no IoT Hub device is created or returned.

### AC-03 — Sensitive credentials are not unnecessarily returned or embedded in client responses

**Given** a successful device registration,
**When** the server returns the registration response,
**Then** the response does not include the IoT Hub device symmetric key in plaintext unless a documented, reviewed security design justifies this as the only viable credential-distribution mechanism,
**And** if the key must be returned, it is returned only over an authenticated, encrypted channel with the justification documented.

### AC-04 — Tenant and device isolation is verifiable

**Given** two tenants (Tenant A and Tenant B), each with registered devices,
**When** Tenant A's device sends observation data,
**Then** the resulting events and visits are stored with Tenant A's identifiers,
**And** Face API identification uses a person group scoped to Tenant A (not a shared global group),
**And** Tenant B's API calls cannot retrieve Tenant A's events, visits, or device information.

### AC-05 — Regression tests exist for remediated exploit paths

**Given** each confirmed Critical or High finding that has been remediated,
**Then** at least one automated test exists that:

- attempts the original exploit path,
- verifies the exploit is blocked by the remediation,
- and fails if the remediation is reverted.

### AC-06 — No confirmed Critical or High finding remains without approved exception

**Given** the completed security audit report with severity-classified findings,
**When** the remediation phase is complete,
**Then** every finding classified as Critical or High is either:

- remediated and verified with a regression test, or
- covered by a documented exception that includes: justification, residual risk, compensating controls, approver, and expiration/review date,

**And** the exception has explicit human approval.

### AC-07 — Database queries use parameterized patterns

**Given** all DocumentDB/Cosmos DB query constructions in the codebase,
**When** the queries are inspected,
**Then** no query interpolates external input directly into a SQL string,
**And** all queries use SDK parameterization or equivalent safe construction.

### AC-08 — Biometric data has defined retention and deletion capability

**Given** face images stored in blob storage and identity templates stored in Face API,
**When** a data retention period expires or a deletion request is received,
**Then** a documented and implementable mechanism exists to:

- delete the specific face images from blob storage,
- delete the specific person and associated face templates from the Face API person group,
- and remove or anonymize the corresponding event and visit records in DocumentDB.

### AC-09 — Conditions for demonstration, reuse, and deployment are documented

**Given** the completed remediation,
**Then** a document exists that specifies:

- minimum security prerequisites for any demonstration environment,
- what data may and may not be used (no real biometric data without consent),
- what credentials must be provisioned and how,
- what tenant isolation guarantees the system provides,
- and what regulatory or legal assessments are required before any deployment handling real personal data.

---

## 14. Desirable Acceptance Criteria

### DAC-01 — Structured security event logging

Security-relevant events (authentication success/failure, authorization denial, device registration, data access) are logged in a structured format (e.g., JSON) with correlation IDs, timestamps, caller identity, and action outcome — without exposing sensitive data values.

### DAC-02 — Credential rotation capability

The system's credential dependencies (IoT Hub keys, storage keys, Face API keys, DocumentDB keys) are documented with rotation procedures. Where feasible, the system uses managed identities or SAS tokens with limited lifetime instead of long-lived account keys.

### DAC-03 — Least-privilege credential scoping

Each component uses credentials scoped to only the operations it requires:

- The device uses a SAS token or device-scoped credential for blob upload rather than a full storage account key.
- The backend uses a registry-scoped IoT Hub policy rather than `iothubowner`.
- The WebJob uses read-only storage access for blob retrieval and scoped DocumentDB permissions.

### DAC-04 — ARM template hardening

The ARM template does not output full account keys. Outputs are limited to resource identifiers. Key distribution uses Key Vault references or post-deployment configuration.

### DAC-05 — `satya.jpg` removal or replacement

The Face API seed image `satya.jpg` is replaced with a synthetic or consented image, or the initializer is modified to not require a seed face image.

### DAC-06 — Rate limiting and abuse prevention

The device registration endpoint and any other externally reachable endpoints have rate limiting or throttling to prevent brute-force or enumeration attacks.

---

## 15. Success Metrics

| Metric | Target | Measurement method |
| --- | --- | --- |
| Critical findings remediated | 100% (or 100% with approved exception) | Audit report finding count vs. remediation/exception count |
| High findings remediated | 100% (or 100% with approved exception) | Audit report finding count vs. remediation/exception count |
| Regression test coverage for exploit paths | ≥ 1 test per remediated Critical/High finding | Test count vs. remediated finding count |
| Unauthenticated API access blocked | 100% of protected endpoints return 401 without valid credentials | Test execution results |
| Tenant isolation verified | 0 cross-tenant data leakage paths | Isolation test results |
| Unparameterized queries remaining | 0 | Static code analysis |
| Conditions document exists and is reviewed | Document created and accepted | Document existence and human acceptance |

---

## 16. Required Validation Evidence

| Validation | Method | Status constraint |
| --- | --- | --- |
| Authentication enforcement on all API endpoints | Automated test (Given/When/Then AC-01) | Requires build environment |
| Authorization enforcement for tenant-scoped operations | Automated test (AC-02) | Requires build environment |
| Credential response sanitization | Automated test + code review (AC-03) | Code review is static; test requires build |
| Tenant isolation at API, persistence, and biometric layers | Automated test (AC-04) | Requires build environment + Face API test instance (or mock) |
| Regression test existence per remediated finding | Test inventory audit (AC-05) | Static verification of test files |
| Finding disposition completeness | Audit report review (AC-06) | Static document review |
| Query parameterization | Static code analysis (AC-07) | Can be performed without build |
| Data retention and deletion mechanism | Design review + automated test if buildable (AC-08) | Design review is static; functional test requires build |
| Conditions document review | Document inspection (AC-09) | Static review |
| Independent security review of all remediation changes | Review by a different model or reviewer, per Level C governance requirements | Required before human acceptance |

**Verification honesty note:** Given constraint C-05 (build environment not currently available) and C-08 (no runtime environment), many validation items can only be verified through static code review until the build environment is reconstructed. The completion report must clearly distinguish which validations were statically verified, which require runtime verification, and which are deferred.

---

## 17. Rollout and Operational Considerations

### 17.1 Task decomposition

This initiative should be decomposed into independently reviewable tasks:

1. **Security audit** (read-only analysis) — identify and classify all findings. *[P0 + P1 for report output]*
2. **Authentication and authorization remediation** — add auth middleware, protect endpoints. *[P5, Level C]*
3. **Input validation remediation** — parameterize queries, validate payloads. *[P4/P5, Level C]*
4. **Credential scoping remediation** — reduce privilege, remove unnecessary key exposure. *[P5, Level C]*
5. **Tenant isolation remediation** — scope biometric data, add tenant validation. *[P5, Level C]*
6. **Biometric data protection** — retention, access controls, deletion capability. *[P5, Level C]*
7. **Security logging** — structured audit events. *[P4, Level B/C]*
8. **Regression test suite** — tests for all remediated paths. *[P3, Level C due to security scope]*
9. **Conditions documentation** — safe demonstration/reuse/deployment conditions. *[P1, Level B]*

Each task follows the governance requirement for independent review and human approval.

### 17.2 Prerequisites

- Build environment reconstruction (separate initiative) should precede or run in parallel with remediation implementation tasks.
- Security audit (task 1) must complete before remediation tasks begin.

### 17.3 Sequencing

Authentication/authorization (task 2) should be implemented first, as it is the foundation for tenant isolation and credential scoping. Input validation (task 3) can proceed in parallel. Tenant isolation (task 5) depends on authentication being in place. Regression tests (task 8) are developed alongside each remediation task.

### 17.4 Rollback

All changes are version-controlled. Each remediation task is a separate, independently revertible unit. No deployment or external-state mutation occurs as part of this initiative.

---

## 18. Open Questions

| ID | Question | Priority | Impact on scope | Required source |
| --- | --- | --- | --- | --- |
| OQ-01 | Was the backend API protected by an external gateway, App Service authentication, or network controls in any historical deployment? | Important | May reclassify some Critical findings to Medium, but code-level remediation is still required. | Stakeholder (VER-SEC-001) |
| OQ-02 | Were real storage account keys embedded in UWP device builds? | Important | Determines actual historical credential exposure risk. | Stakeholder (VER-SEC-002) |
| OQ-03 | Is per-tenant Face API person group isolation the approved approach, or should biometric re-identification be removed entirely from the remediated system? | Blocking for task 5 | Determines scope of biometric isolation remediation. | Human operator decision |
| OQ-04 | Should the `satya.jpg` seed image be removed, replaced, or left as-is with a documented exception? | Important for DAC-05 | Affects initializer modification scope. | Human operator decision |
| OQ-05 | What authentication mechanism is preferred — API key, Azure AD/Entra ID, certificate-based, or other? | Blocking for task 2 | Determines the remediation design for AC-01 and AC-02. | Human operator architectural decision |
| OQ-06 | Is `GetAllVisitsByPersonIdAsync` reachable with attacker-controlled input through any code path? | Important for finding severity classification | If unreachable, the SQL interpolation finding is Informational; if reachable, it is High. | Static analysis during audit task |
| OQ-07 | Should the audit scope include the ARM template and deployment scripts, or only application code? | Clarification | Determines whether infrastructure-level findings (e.g., `iothubowner` key output, unused Emotion API billing) are in scope. | Human operator decision |
| OQ-08 | Is the build environment reconstruction initiative approved and scheduled? | Scheduling | Determines whether remediation tasks can be runtime-verified or are limited to static verification. | Human operator |

---

## 19. Decision Log

| ID | Decision | Date | Rationale | Decided by |
| --- | --- | --- | --- | --- |
| DL-01 | This document is created as a business requirement, not as the audit itself. The audit is a separate downstream task. | 2025-07-16 | Separation of analysis from remediation, per governance requirements for Level C work. | Agent (advisory — pending human acceptance) |
| DL-02 | Functional defects (visit-update bug, double event persistence, capture loop) are explicitly out of scope unless they are themselves security vulnerabilities. | 2025-07-16 | Scope minimization per AGENTS.md §8. Functional defects are tracked separately. | Agent (advisory — pending human acceptance) |
| DL-03 | Privacy impact assessment and legal compliance certification are out of scope. The audit identifies technical gaps but does not constitute legal advice. | 2025-07-16 | Legal and regulatory compliance require specialist assessment beyond code-level analysis. | Agent (advisory — pending human acceptance) |
| DL-04 | All remediation changes are Level C per repository governance (security, privacy, identity, biometric data, persistence). | 2025-07-16 | AGENTS.md §10, constitution §18.5. | Documented governance requirement |
| DL-05 | Static analysis is the primary verification method. Runtime verification is documented as required but deferred until build environment is available. | 2025-07-16 | Constraint C-05 and C-08 — no build or runtime environment currently available. | Agent (advisory — pending human acceptance) |

---

*This document is advisory until explicitly accepted by the human operator. It does not constitute approval to begin remediation work. Each downstream task requires its own explicit scope, review, and human approval per repository governance.*
