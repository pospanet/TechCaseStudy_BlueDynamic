# Security Audit and Remediation — Business Requirement

---

## 1. Business Problem

Retailizer is a historical proof-of-concept retail analytics system (circa 2016) that captures, transmits, processes, and stores biometric data — cropped face images, persistent biometric identity templates, and machine-inferred demographic attributes — of retail store visitors. The system was created during a Microsoft–Blue Dynamic hackfest and was never hardened for production use.

Static inspection of the repository reveals that:

- The backend registration API (`/api/device`) has **no authentication or authorization**. Any network-reachable caller can register an arbitrary device and receive an IoT Hub symmetric key in the response. [Observed: `DeviceController.cs` — no `[Authorize]` attribute, no middleware; `Startup.cs` — no authentication middleware registered]
- **Credentials are returned in API responses** — the IoT Hub device symmetric key is sent as plaintext JSON to the caller. [Observed: `DeviceController.cs` lines 28–31]
- **No tenant or device authorization** exists at any API boundary. TenantId and StoreId flow through DTOs but are never validated or enforced. [Observed: no authorization middleware in `Startup.cs`; no tenant validation in `Functions.cs`]
- **All tenants share a single Face API person group** (`"retailizer"`), enabling cross-tenant biometric identification. [Observed: `Functions.cs` line 21]
- **Biometric face images persist indefinitely** in Azure Blob Storage with no lifecycle management — deletion code is commented out. [Observed: `Functions.cs` lines 80, 168]
- **Broad shared-secret credentials** (storage account keys, IoT Hub owner connection strings, DocumentDB account keys, Face API subscription keys) are distributed across configuration files. Device-side configuration uses placeholder patterns that, when populated, would embed full storage account connection strings on IoT hardware. [Observed: `Configuration.cs`, `App.config`, `appsettings.json`]
- **No input validation** exists on the registration endpoint or queue message processing. [Observed: `DeviceController.cs`, `Functions.cs`]
- A **potential SQL injection vector** exists in `GetAllVisitsByPersonIdAsync` where `personId` is interpolated without parameterization into a DocumentDB SQL query. [Observed: `DocumentDbService.cs` line 58]
- **No secret rotation**, least-privilege scoping, or credential lifecycle management is implemented or documented.
- **No data retention, deletion, or data-subject-request mechanism** exists for biometric or personal data.

These gaps are historically understandable for a hackfest proof of concept but represent material security and privacy risks if the system is demonstrated, reused, forked, or deployed in any capacity.

**Evidence classification:** All findings above are Observed from static source inspection. Whether external protections (API gateway, App Service authentication, network controls) existed at deployment time is Unknown and requires stakeholder confirmation (see `VER-SEC-001` in verification backlog).

---

## 2. Current State

### Authentication and authorization

- **Backend API:** No authentication middleware, no authorization attributes, no API key validation. The single endpoint `POST /api/device` is open to any caller. [Observed: `Startup.cs`, `DeviceController.cs`]
- **WebJob processing:** Triggered by Service Bus queue; no application-level caller authentication beyond Service Bus connection string. [Observed: `Functions.cs`]
- **Tenant isolation:** TenantId and StoreId are carried in DTOs and used in visit-matching queries but are never validated against an authenticated identity. Any device document with any TenantId would be accepted. [Observed: `Functions.cs` lines 227–228; no validation logic]

### Credential handling

- The registration response includes the IoT Hub device symmetric key as plaintext JSON. [Observed: `DeviceController.cs` lines 28–31]
- Configuration files contain placeholder patterns for storage connection strings, IoT Hub connection strings, DocumentDB keys, Face API keys, and Service Bus keys. When populated, these represent broad account-level credentials. [Observed: `App.config`, `appsettings.json`, `Configuration.cs`]
- The IoT Hub connection string in backend configuration likely uses an `iothubowner` or `registryReadWrite` policy — the code calls `RegistryManager.AddDeviceAsync` and `GetDeviceAsync`, which require registry-write privileges. [Inferred: `IotHubService.cs`]
- No credential rotation, expiration, or revocation mechanism exists.

### Biometric data protection

- Cropped face JPEGs persist indefinitely in Blob Storage. [Observed: blob deletion commented out in `Functions.cs`]
- Biometric identity templates persist indefinitely in the Face API person group. [Observed: `Functions.cs` lines 159–160]
- All tenants share a single person group `"retailizer"`, creating cross-tenant biometric leakage. [Observed: `Functions.cs` line 21]
- No consent mechanism, data retention policy, deletion capability, or data-subject-request handling exists. [Observed: no such code in any project]
- The initializer seeds the person group with `satya.jpg` — a real person's biometric data. [Observed: `Retailizer.Azure.Initializer/Program.cs`]

### Input validation

- `DeviceController.Register` accepts a `Device` body with no validation of required fields, format, or authorization. [Observed: `DeviceController.cs`]
- `ProcessQueueMessage` parses raw JSON with no schema validation; missing properties cause unhandled `NullReferenceException`. [Observed: `Functions.cs` lines 45–53]
- `GetAllVisitsByPersonIdAsync` interpolates `personId` directly into a SQL string without parameterization. [Observed: `DocumentDbService.cs` line 58]

### Observability for security events

- No audit logging, security event recording, or access monitoring exists. [Observed: only `Debug.WriteLine` and `TextWriter log` in WebJob]

---

## 3. Target State

After remediation, the system must satisfy the following security properties:

1. **Protected API endpoints:** Every API operation that creates, reads, modifies, or deletes a resource requires authenticated and authorized callers. Unauthorized requests are rejected before reaching business logic.
2. **Credential containment:** Sensitive credentials (IoT Hub keys, storage keys, API keys) are not returned in API responses unless strictly necessary for the operation, and then only to authenticated, authorized callers. Device-side configuration does not embed account-level credentials when scoped alternatives (SAS tokens, per-device keys) are available.
3. **Tenant and device isolation:** A device can only produce data for its own registered tenant and store. Biometric identity data is isolated per tenant — a person identified in one tenant's store cannot be recognized in another tenant's store.
4. **Input validation:** All external inputs (API payloads, queue messages, configuration values) are validated before processing. Query construction uses parameterized queries.
5. **Biometric data governance:** A defined retention policy exists, face images can be deleted after processing, and biometric templates can be removed. The system does not retain biometric data beyond its stated purpose.
6. **Credential scoping and lifecycle:** Credentials use the minimum required privilege. A rotation or revocation path is documented. Secrets are not hardcoded in source.
7. **Security observability:** Material security events (authentication failures, authorization denials, registration attempts, credential issuance) are logged in a reviewable format without exposing sensitive values.
8. **Clear deployment and demonstration conditions:** Explicit prerequisites and safeguards are documented for any demonstration, reuse, or deployment of the system.

---

## 4. Explicit Business Objective

**Identify, document, classify, and remediate confirmed security vulnerabilities in the Retailizer codebase so that:**

- no confirmed Critical or High severity finding remains without a remediation or an explicitly approved exception;
- the system can be safely demonstrated, studied, or used as a basis for further development;
- biometric and tenant data is protected against unauthorized access, leakage, and indefinite retention;
- the remediation is verifiable through regression tests and static inspection.

This objective does not include modernizing the technology stack, restoring buildability, achieving regulatory compliance certification, or deploying the system to production.

---

## 5. Scope

### In scope

- **SEC-SCOPE-01:** Authentication and authorization for the backend registration API (`/api/device`).
- **SEC-SCOPE-02:** Credential exposure in API responses — evaluating whether the device symmetric key must be returned, and if so, ensuring it is only provided to authenticated callers.
- **SEC-SCOPE-03:** Input validation for the registration endpoint and queue message processing.
- **SEC-SCOPE-04:** Parameterized queries — replacing string interpolation in DocumentDB SQL queries.
- **SEC-SCOPE-05:** Tenant isolation at the API and biometric-identity level — ensuring TenantId is validated and biometric person groups are tenant-scoped or otherwise isolated.
- **SEC-SCOPE-06:** Device isolation — ensuring a device can only operate within its registered tenant and store.
- **SEC-SCOPE-07:** Credential scoping — evaluating and documenting the minimum required privilege for each credential, and replacing account-level keys with scoped alternatives where feasible.
- **SEC-SCOPE-08:** Biometric data retention — implementing or documenting a retention and deletion capability for face images in Blob Storage and identity templates in the Face API.
- **SEC-SCOPE-09:** Security audit logging — recording material security events without exposing sensitive values.
- **SEC-SCOPE-10:** Regression tests for every remediated exploit path.
- **SEC-SCOPE-11:** Documentation of deployment and demonstration prerequisites, security constraints, and residual risks.
- **SEC-SCOPE-12:** Severity classification and disposition of all findings — every confirmed Critical or High finding must be remediated or carry an explicitly approved exception.

### Components in scope

| Component | Path | Security relevance |
| --- | --- | --- |
| Backend API | `Backend/` | Registration endpoint, credential return, auth |
| WebJob processor | `Reailizer.Job/` | Queue input validation, tenant/device context |
| Common services | `Common/` | DocumentDB queries, IoT Hub registration, Face API |
| Common DTOs | `Common/DTO/` | Tenant/store/device/camera data structures |
| UWP device app | `Retailizer.UWP/` | Credential storage, configuration patterns |
| Initializer | `Retailizer.Azure.Initializer/` | Seed data, person group creation |
| ARM templates | `Retailizer.Azure/Templates/` | Resource provisioning, credential outputs |
| Configuration files | `*.config`, `appsettings*.json`, `project.json` | Secret placeholders, connection strings |

---

## 6. Out of Scope

- **Technology modernization:** Upgrading .NET Framework, ASP.NET Core, project formats, SDKs, or dependencies beyond what is strictly required for a security fix.
- **Functional defect remediation:** Fixing the visit-update `CreateDocumentAsync` bug (CAP-008), device registration persistence gap (CAP-012), or other non-security functional defects — unless they are also exploitable security vulnerabilities.
- **Regulatory compliance certification:** Achieving formal GDPR, CCPA, BIPA, or other regulatory compliance. The audit documents privacy-relevant gaps but does not produce a compliance assessment.
- **Production deployment:** Deploying the system to any environment. Preparing deployment guidance for demonstration purposes is in scope; executing deployment is not.
- **Performance optimization:** Addressing the unbounded capture loop, Face API retraining frequency, or other performance concerns — unless they create a denial-of-service vulnerability.
- **Power BI or reporting:** Reporting infrastructure, dashboard artifacts, or analytics computation.
- **Stream Analytics or message routing:** The missing IoT Hub-to-Service Bus bridge.
- **UWP device hardware testing:** Runtime verification on IoT hardware.

---

## 7. Stakeholders and Affected Users or Systems

| Stakeholder | Interest | Impact |
| --- | --- | --- |
| **Repository owner / human operator** | Decides scope, risk acceptance, deployment conditions | Approves exceptions, accepts residual risk |
| **Future developers or agents** | Will implement remediation tasks and regression tests | Must understand security requirements and constraints |
| **Independent reviewer** | Reviews remediation for correctness and completeness | Must verify acceptance criteria against evidence |
| **Blue Dynamic (historical partner)** | Original co-creator; may reuse or reference the system | Affected by any reuse or demonstration conditions |
| **Retail tenant organization** | Would consume analytics; data isolation is critical | Cross-tenant data leakage is a direct risk |
| **Store visitors (data subjects)** | Biometric data captured without consent mechanism | Retention and deletion capabilities affect their rights |
| **Backend API** | Registration endpoint is the primary attack surface | Requires authentication and input validation |
| **IoT Hub** | Device identity registry; credentials issued via API | Unauthorized registration creates rogue device identities |
| **DocumentDB / Cosmos DB** | Stores device, event, and visit data | Unvalidated inputs and unparameterized queries are risks |
| **Face API (Cognitive Services)** | Stores biometric templates | Shared person group leaks identity across tenants |
| **Blob Storage** | Stores face images indefinitely | Requires retention policy and access controls |

---

## 8. Business Value

- **Risk reduction:** Eliminates known exploit paths that could allow unauthorized device registration, credential theft, cross-tenant biometric leakage, and uncontrolled biometric data retention.
- **Safe reuse:** Establishes clear conditions under which the system can be safely demonstrated, studied, forked, or extended — preventing accidental exposure of the unprotected historical code.
- **Governance compliance:** Satisfies the repository's own governance requirements (AGENTS.md §12, §17; Constitution §15, §18, §19) for security-related verification and remediation.
- **Technical debt documentation:** Produces a severity-classified finding inventory that enables prioritized decision-making about further investment.
- **Regression safety:** Creates test evidence that remediations are effective and do not regress.

---

## 9. Risks of Doing Nothing

| Risk | Severity | Likelihood | Impact |
| --- | --- | --- | --- |
| Unauthorized device registration and IoT Hub key theft via unprotected API | Critical | High if API is network-reachable | Rogue devices in IoT Hub; compromised device keys |
| Cross-tenant biometric identification via shared person group | High | Certain if multiple tenants exist | Privacy violation; identity leakage between organizations |
| Indefinite retention of biometric face images without deletion capability | High | Certain | Regulatory exposure; inability to fulfill data-subject requests |
| SQL injection in `GetAllVisitsByPersonIdAsync` | High | Medium (requires DocumentDB SQL injection surface) | Data exfiltration or unauthorized data access |
| Credential exposure in registration response to unauthenticated caller | High | High if API is network-reachable | IoT Hub key compromise |
| Account-level storage keys potentially embedded on IoT devices | High | Unknown (depends on deployment) | Full storage account compromise if device is captured |
| No security event logging | Medium | Certain | Security incidents cannot be detected or investigated |
| Demonstration or fork without security awareness | Medium | Medium | New deployments inherit all vulnerabilities |

---

## 10. Constraints and Dependencies

### Constraints

- **Historical codebase:** The project uses obsolete toolchains (ASP.NET Core 1.0 xproj, .NET Framework 4.5.2, Project Oxford SDK). Security remediations must work within these constraints unless a specific dependency change is separately authorized.
- **No test infrastructure:** No existing tests exist. Regression tests must be created as part of the remediation.
- **Build environment uncertainty:** Whether the project can be built in its historical form is unverified (`VER-BUILD-001`, `VER-BUILD-002`). Remediations that require compilation will depend on resolving build prerequisites.
- **External service retirement:** The Project Oxford Face API has been superseded. Face API behavior for person-group isolation may require external-service verification.
- **Protected artifacts:** `README.md`, `LICENSE.md`, `images/`, `satya.jpg`, governance documents, and other protected areas must not be modified (AGENTS.md §12).
- **Read-only analysis for this document:** This business requirement is produced through static analysis only. No build, runtime, or external-service verification has been performed.

### Dependencies

- **Build verification:** Before implementing code-level remediations, the build environment must be established or an alternative verification approach must be agreed upon.
- **Stakeholder confirmation:** Several security assessments depend on deployment-context information that is not available in the repository (see `VER-SEC-001`, `VER-SEC-002`, `VER-SEC-003` in verification backlog).
- **Implementation plan:** A separate implementation plan document must decompose these requirements into atomic, independently reviewable tasks before implementation begins.
- **Independent review:** Per the constitution (§18.5), security-related changes require Level C review — independent specialist review plus explicit human approval.

---

## 11. Assumptions

| ID | Assumption | Basis | Consequence if invalid |
| --- | --- | --- | --- |
| ASM-01 | The backend API had no external authentication protection (API gateway, App Service auth) during its historical use. | No evidence of external protection found in repository; `VER-SEC-001` is unresolved. | If external protection existed, the API authentication finding severity may be lower, but the source-level gap remains a risk for reuse or redeployment. |
| ASM-02 | The system was used only for hackfest demonstration, not production deployment with real customer data. | README states "proof of concept"; placeholder configuration values; multiple critical functional defects. | If real customer biometric data was processed, data retention and deletion become urgent rather than preventive. |
| ASM-03 | Remediations can be implemented within the existing .NET Framework 4.5.2 and ASP.NET Core 1.0 constraints without requiring a platform migration. | Security controls (authentication middleware, input validation, parameterized queries) are available in the historical framework versions. | If the historical framework lacks required security primitives, a dependency or platform change task would need to be separately authorized. |
| ASM-04 | DocumentDB SQL injection through string interpolation is exploitable. | `GetAllVisitsByPersonIdAsync` interpolates `personId` without quotes or parameterization. DocumentDB SQL semantics may limit exploitability, but the pattern is unsafe. | If DocumentDB rejects the injection payload, severity may be reduced, but the unsafe pattern should still be fixed. Runtime verification required. |
| ASM-05 | Per-tenant Face API person groups are technically feasible with the historical Project Oxford SDK. | The SDK supports creating multiple person groups with distinct IDs. | If the SDK or service limits prevent per-tenant groups, an alternative isolation mechanism must be designed. |
| ASM-06 | The `satya.jpg` seed image will not be removed or replaced as part of this initiative — it is a protected historical artifact. | AGENTS.md §12 designates `Retailizer.Azure.Initializer/satya.jpg` as read-only. | The initializer's use of a real person's biometric data will be documented as a residual risk and demonstration constraint. |

---

## 12. Non-Functional Expectations

- **Backward compatibility:** Remediations must not break existing API contracts, message schemas, or persistence formats unless the break is a necessary security fix (e.g., removing credential exposure from a response). Any contract change must include compatibility analysis.
- **Minimal change:** Each remediation must be the smallest change that addresses the finding. No opportunistic refactoring, modernization, or cleanup.
- **Testability:** Every remediation must be accompanied by a regression test that demonstrates the vulnerability is closed.
- **Reviewability:** Changes must be independently reviewable — small, focused, and traceable to a specific finding and acceptance criterion.
- **No runtime dependency on new external services:** Remediations should not introduce new external service dependencies unless separately authorized.
- **Configuration-driven:** Security controls (e.g., authentication mechanism, retention period) should be configurable rather than hardcoded where practical, to support different demonstration and deployment scenarios.

---

## 13. Mandatory Acceptance Criteria

### AC-01: Unauthorized callers cannot invoke protected operations

**Given** the backend API is running  
**When** an unauthenticated HTTP request is sent to `POST /api/device`  
**Then** the response status code is 401 Unauthorized  
**And** no IoT Hub device is created  
**And** no device key is returned in the response body

### AC-02: Client identifiers are validated and authorized

**Given** an authenticated caller sends a registration request  
**When** the request body contains a `DeviceId` that is empty, null, malformed, or exceeds length limits  
**Then** the response status code is 400 Bad Request  
**And** no IoT Hub device is created

**Given** an authenticated caller sends a registration request  
**When** the request body contains a valid `DeviceId`  
**Then** the device is registered successfully  
**And** the response contains only the minimum required credential information

### AC-03: Sensitive credentials are not unnecessarily returned or embedded in the client

**Given** a device registration is successful  
**When** the response is returned to the authenticated caller  
**Then** the response does not contain account-level credentials (storage account keys, IoT Hub owner connection strings, DocumentDB keys, Face API keys)  
**And** device-specific credentials (if returned) are scoped to the minimum required privilege

**Given** the UWP device configuration  
**When** credentials are required for Blob Storage access  
**Then** the device uses a scoped credential (SAS token or per-device key) rather than an account-level storage connection string — OR the use of account-level credentials is documented as an accepted risk with explicit justification

### AC-04: Tenant and device isolation is verifiable

**Given** two tenants (Tenant-A and Tenant-B) each with registered devices  
**When** Tenant-A's device processes a face image  
**Then** the face identity is enrolled in a tenant-scoped person group (not the shared `"retailizer"` group)  
**And** the face cannot be identified by Tenant-B's processing pipeline

**Given** a device registered to Tenant-A and Store-1  
**When** the device sends an observation  
**Then** the resulting event and visit records contain the device's registered TenantId and StoreId  
**And** TenantId and StoreId are derived from the authenticated device identity, not from unvalidated client input

### AC-05: Regression tests exist for remediated exploit paths

**Given** each remediated security finding  
**Then** at least one automated test exists that:

- reproduces the pre-remediation vulnerable condition (negative test), AND
- verifies that the remediation correctly prevents the exploit (positive test)

Tests must cover at minimum:

- unauthenticated API access rejection,
- malformed input rejection,
- parameterized query usage (no string interpolation of user input in SQL),
- tenant-scoped person group isolation,
- credential scoping in API responses.

### AC-06: No confirmed Critical or High finding remains without approved exception

**Given** the completed security audit finding inventory  
**When** all remediations are applied  
**Then** every finding classified as Critical or High severity is either:

- remediated with passing regression tests, OR
- documented with an explicit exception approved by the human operator, including: finding description, residual risk, justification, and compensating controls (if any)

### AC-07: Query parameterization

**Given** any DocumentDB query that includes external input  
**When** the query is constructed  
**Then** external values are passed as parameters (e.g., `SqlParameterCollection`) rather than interpolated into the SQL string

**Specifically:** `GetAllVisitsByPersonIdAsync` and `FindOpenVisitForEventAsync` must use parameterized queries.

### AC-08: Biometric data retention is bounded

**Given** a face image is processed by the WebJob  
**When** face detection, identification, and event creation are complete  
**Then** the face image blob is either:

- deleted from Blob Storage, OR
- marked for deletion according to a documented retention schedule

**Given** a biometric identity template in the Face API person group  
**Then** a documented mechanism exists to delete the template (manual procedure or automated process), and the retention policy is documented.

### AC-09: Security events are logged

**Given** a security-relevant event occurs (authentication failure, authorization denial, device registration attempt, credential issuance)  
**When** the event is processed  
**Then** a log entry is created that includes: timestamp, event type, caller identity (if available), outcome (success/failure), and affected resource  
**And** the log entry does not contain secret values, credentials, or biometric data

### AC-10: Deployment and demonstration conditions are documented

**Given** someone intends to demonstrate, fork, or deploy the system  
**Then** a document exists that specifies:

- required security prerequisites (authentication configuration, credential provisioning, tenant setup),
- known residual risks,
- regulatory considerations for biometric data,
- constraints on the use of seed biometric data (`satya.jpg`),
- minimum required Azure resource configuration for safe operation

---

## 14. Desirable Acceptance Criteria

### DAC-01: Credential rotation documentation

A documented procedure exists for rotating each credential type (IoT Hub connection string, storage keys, DocumentDB keys, Face API keys, Service Bus keys) without system downtime.

### DAC-02: Device deregistration

An authenticated administrative operation exists to remove a device identity from IoT Hub and delete its associated metadata from DocumentDB, preventing further message acceptance from that device.

### DAC-03: Biometric data purge by tenant

An administrative procedure or utility exists to delete all Face API person group data and Blob Storage face images for a specific tenant.

### DAC-04: Rate limiting on registration endpoint

The registration API enforces rate limiting to prevent enumeration or denial-of-service attacks against IoT Hub device registration.

### DAC-05: HTTPS enforcement

The backend API enforces HTTPS for all connections. HTTP requests are either rejected or redirected to HTTPS.

### DAC-06: Sensitive configuration validation

Application startup validates that all required configuration keys are present and non-placeholder before accepting requests, failing fast with a clear error rather than a runtime `NullReferenceException`.

---

## 15. Success Metrics

| Metric | Target | Measurement method |
| --- | --- | --- |
| Critical findings remediated or excepted | 100% | Finding inventory with disposition |
| High findings remediated or excepted | 100% | Finding inventory with disposition |
| Regression test coverage of remediated findings | ≥ 1 test per remediated finding | Test inventory mapped to findings |
| Unauthenticated API calls blocked | 100% of test cases | Automated tests |
| Cross-tenant biometric identification prevented | 0 cross-tenant matches in test | Automated or documented verification |
| Parameterized queries | 100% of queries with external input | Static inspection |
| Documentation of deployment conditions | 1 document | Existence check |

---

## 16. Required Validation Evidence

| Evidence | Purpose | Method | Timing |
| --- | --- | --- | --- |
| Security finding inventory with severity classification | Baseline of all findings | Static analysis of repository | Before implementation |
| Regression test results for each remediated finding | Prove remediation effectiveness | Automated test execution | After each remediation task |
| Static inspection of parameterized queries | Verify no string interpolation of external input | Code review | After query remediation |
| Authentication test — unauthenticated request rejected | Prove API protection | Automated test or documented manual test | After auth implementation |
| Tenant isolation test — cross-tenant identification blocked | Prove biometric isolation | Automated test or documented verification procedure | After person-group isolation |
| Credential exposure review — API responses inspected | Verify no unnecessary credential leakage | Code review + test | After credential scoping |
| Deployment conditions document | Verify documentation exists and is complete | Document review | Before final acceptance |
| Independent review disposition | All material findings addressed | Review by different model/reviewer | Before human acceptance |
| Finding exception approvals | All Critical/High exceptions explicitly approved | Human operator sign-off | Before final acceptance |

---

## 17. Rollout and Operational Considerations

### Rollout sequence

1. **Finding inventory:** Complete the security audit finding inventory with severity classifications before any remediation begins.
2. **Implementation plan:** Produce a detailed implementation plan decomposing requirements into atomic tasks, reviewed independently before implementation.
3. **Wave-based implementation:** Implement remediations in priority order (Critical → High → Medium), with independent review after each wave.
4. **Regression testing:** Each remediation task includes its regression tests; tests must pass before the task is considered complete.
5. **Final validation:** A final validation pass verifies all mandatory acceptance criteria with evidence.
6. **Human acceptance:** The human operator reviews the finding inventory, remediation evidence, exception requests, and residual risks before accepting the initiative as complete.

### Operational considerations

- **No production deployment is authorized** by this initiative. Remediations prepare the codebase for safe demonstration or further development, not for production use.
- **Build environment:** Remediations assume the historical build environment can be established. If build verification fails, remediations may be limited to static-inspection-verified changes with runtime verification deferred.
- **External services:** No live Face API, IoT Hub, DocumentDB, or other external service calls are authorized by this initiative unless explicitly permitted by a specific task.
- **Backward compatibility:** Authentication changes will break the existing unauthenticated device registration flow. This is intentional — the UWP device client must be updated to provide authentication credentials. The change must be coordinated across Backend and UWP projects.
- **Rollback:** Each remediation task must be independently reversible. If a remediation introduces a regression, it can be reverted without affecting other remediations.

---

## 18. Open Questions

| ID | Question | Blocking | Rationale |
| --- | --- | --- | --- |
| OQ-01 | Was the `/api/device` endpoint protected by an external gateway, App Service authentication, or network controls during historical use? | No — remediation proceeds regardless, but the answer affects severity assessment of the historical risk. | `VER-SEC-001` in verification backlog |
| OQ-02 | Were real storage account keys embedded in UWP device builds? | No — credential scoping is in scope regardless, but the answer determines whether historical key compromise is a concern. | `VER-SEC-002` in verification backlog |
| OQ-03 | Was real customer biometric data ever processed by the system? | No — retention and deletion capabilities are in scope regardless, but the answer determines urgency. | If real data exists, deletion becomes an immediate operational requirement. |
| OQ-04 | Is the DocumentDB SQL injection vector in `GetAllVisitsByPersonIdAsync` exploitable given DocumentDB's SQL dialect limitations? | No — parameterization is required regardless as a defense-in-depth measure. | Runtime verification required (`ASM-04`). |
| OQ-05 | Does the historical Project Oxford Face SDK support multiple person groups with sufficient capacity for per-tenant isolation? | Yes — this affects the feasibility of AC-04. | If per-tenant groups are not feasible, an alternative isolation mechanism must be designed. |
| OQ-06 | What authentication mechanism is appropriate for the backend API given the ASP.NET Core 1.0 framework constraints? | Yes — the implementation plan must identify a feasible authentication approach. | Options include API key middleware, shared-secret header validation, or ASP.NET Core 1.0 authentication middleware. Framework capabilities require verification. |
| OQ-07 | Should the initializer's use of `satya.jpg` be documented as a demonstration constraint, or should the initializer be modified to use synthetic data? | No — `satya.jpg` is a protected artifact. Documentation of the constraint is in scope; modification is not. | AGENTS.md §12 protects this file. |

---

## 19. Decision Log

| ID | Decision | Rationale | Date | Status |
| --- | --- | --- | --- | --- |
| DEC-01 | Scope limited to security audit and remediation; no technology modernization. | Modernization is a separate initiative with different risk profile and authorization requirements. Mixing security fixes with platform changes increases risk and review complexity. | Document creation | Proposed — awaiting human acceptance |
| DEC-02 | Functional defects (visit update bug, device persistence gap) are out of scope unless they are also exploitable security vulnerabilities. | These are documented functional defects with their own remediation path. Security remediation should not be conflated with functional bug fixes. | Document creation | Proposed — awaiting human acceptance |
| DEC-03 | `satya.jpg` is not modified or removed — documented as a residual risk and demonstration constraint. | Protected historical artifact per AGENTS.md §12. The ethical concern is documented but the file is not within the authorized modification scope. | Document creation | Proposed — awaiting human acceptance |
| DEC-04 | All mandatory acceptance criteria use Given/When/Then format to ensure testability. | Requirement from the task specification. Enables unambiguous automated and manual verification. | Document creation | Applied |
| DEC-05 | Cross-tenant biometric isolation (AC-04) requires per-tenant Face API person groups or equivalent isolation. | The shared `"retailizer"` person group is the most severe multi-tenant security gap. Data-model-level TenantId filtering is insufficient when biometric templates are shared globally. | Document creation | Proposed — awaiting human acceptance; feasibility depends on OQ-05 |
| DEC-06 | Review level for this document is Level B (authoritative business documentation). Review level for implementation tasks is Level C (security-related changes). | Per constitution §18.4 and §18.5; per permission matrix §8. | Document creation | Applied |

---

## Document Metadata

| Field | Value |
| --- | --- |
| **Title** | Security Audit and Remediation — Business Requirement |
| **Initiative** | Security audit and remediation |
| **Author** | AI agent (Cascade) |
| **Date** | 2026-07-16 |
| **Status** | Draft — awaiting independent review and human acceptance |
| **Review level** | Level B — independent review required before acceptance |
| **Permission profile** | P1 — Documentation Write |
| **Authoritative inputs** | `docs/business/business-description.md`, `docs/technical/technical-description.md`, `docs/technical/component-catalog.md`, `docs/technical/integration-catalog.md`, `docs/technical/data-model.md`, `docs/technical/failure-modes.md`, `docs/technical/verification-backlog.md`, `AGENTS.md`, `docs/agentic/agentic-constitution.md`, `docs/agentic/permission-matrix.md` |
| **Source evidence** | `Backend/Controllers/DeviceController.cs`, `Backend/Startup.cs`, `Common/Services/DocumentDbService.cs`, `Reailizer.Job/Functions.cs`, `Common/Services/IotHubService.cs`, `Common/Services/FaceApiService.cs`, `Retailizer.UWP/Configuration.cs`, `Retailizer.UWP/DeviceConfiguration.cs`, `Backend/appsettings.json`, `Reailizer.Job/App.config` |
| **Files created** | `docs/work-items/security-audit-business-requirement.md` |
| **Files modified** | None |
| **Validation** | Static inspection only; no build, runtime, or external-service verification performed |
