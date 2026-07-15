# Retailizer — Business Description

---

## 1. Document Purpose and Status

This document reconstructs the business view of the Retailizer project — a historical proof-of-concept system for retail visitor analytics using facial recognition, created circa late 2016 during a Microsoft–Blue Dynamic hackfest.

The reconstruction is based entirely on static project evidence: source code, configuration files, deployment templates, and project documentation present in the repository. No runtime execution, external service calls, or Git history analysis were performed.

This document has undergone a three-stage process:

1. **Evidence-based analysis** — systematic examination of all repository artifacts.
2. **Independent review** — critical evaluation by a separate reviewer who verified findings against the codebase.
3. **Final consolidation** — evaluation of every review finding, re-verification against project files, correction of errors and overstatements, and production of this final document.

**Important qualifications:**

- Unresolved areas are explicitly marked throughout.
- Evidence classifications distinguish what is directly observed, what is documented, what is inferred, and what remains unknown.
- This document describes a historical project in its original context. It does not propose modernization, redesign, or future architecture.
- Several critical implementation defects were identified during review that affect capability status assessments. These are documented precisely.

---

## 2. Executive Business Summary

Retailizer was a proof-of-concept system that attempted to provide retail store operators with automated visitor analytics through facial recognition. It was created during a Microsoft hackfest with Blue Dynamic, a Czech company, in approximately September–October 2016.

**Business problem addressed:** Retailers lacked real-time, automated insight into in-store customer behavior — specifically how many people enter, their estimated demographic profile, whether they visit the payment area, how long they stay, and whether they return.

**Information the system attempted to provide:**

- Footfall counting (entry observations)
- Demographic profiling (estimated age, gender, smile score)
- Conversion measurement (proportion of visitors reaching the payment zone)
- Dwell time (duration between entry and exit observations)
- Repeat-visitor recognition (biometric re-identification across visits)

**Maturity:** Early proof of concept. The README explicitly describes the project as a "proof of concept of a new product," and the Power BI dashboard is stated to have "no ambition to be used in production in any form." Configuration values use placeholders, there is no authentication or authorization, no error recovery infrastructure, and several critical implementation defects prevent the end-to-end pipeline from functioning as written.

**Most important limitations:**

1. **Two critical pipeline defects** prevent visit updates and device lookup from functioning (§15, §18).
2. **No multi-tenant isolation** in the biometric recognition layer — all tenants share a single Face API person group.
3. **No deduplication** of repeated observations — continuous camera capture generates duplicate events.
4. **"Payment" does not mean a commercial transaction** — it means a face was observed near a payment-zone camera. No POS integration exists.
5. **"Person" is a service-generated biometric cluster identity**, not a known real-world individual.
6. **No consent mechanism** for biometric data collection from store visitors.

---

## 3. Historical Project Context

**Evidence classification:** Documented + Observed

Retailizer was created in approximately September 2016, as indicated by the user-secrets timestamp `aspnet-Backend-20160921015104` and the technology stack used.

**Technological context:**

- **.NET Framework 4.5.2** for backend services and WebJobs; **ASP.NET Core 1.0** for the Web API; **Universal Windows Platform (UWP)** for the edge device application.
- **Windows 10 IoT Core** running on a Qualcomm DragonBoard 410c with USB cameras.
- **Azure IoT Hub** for device-to-cloud messaging.
- **Azure Cognitive Services Face API** (branded as "Project Oxford" at the time, SDK `Microsoft.ProjectOxford.Face` v1.2.1.1) for face detection, demographic estimation, and re-identification.
- **Azure DocumentDB** (later rebranded to Azure Cosmos DB) for JSON document persistence.
- **Azure Service Bus** for message queuing, with **Azure WebJobs** for queue-triggered processing.
- **Power BI** for data visualization (documented but no artifacts in the repository).
- ARM template API versions: `2016-02-01-preview` (Cognitive Services), `2016-02-03` (IoT Hub).

All technology choices were historically reasonable and typical of the Azure IoT and Cognitive Services landscape in 2016. The "Project Oxford" SDK was the standard Cognitive Services client at that time. DocumentDB was the contemporary NoSQL offering before Cosmos DB rebranding.

**Proof-of-concept nature:** Multiple indicators confirm this was an early-stage demonstrator rather than a production system:

- Explicit "proof of concept" statement in README.
- Configuration values are placeholder strings (`<BackEndUrl>`, `<FaceApiKey>`).
- No authentication or authorization on the backend API.
- No error recovery, monitoring, or operational logging infrastructure.
- TODO comments in Czech indicating known unfinished work.
- Commented-out blob deletion code.
- Use of a public figure's image (`satya.jpg`) for Face API group initialization.
- Critical implementation defects that would prevent end-to-end operation.

**Historical vs. current applicability:** The core product concept — covert biometric surveillance of retail customers — carries fundamental privacy and ethical implications that have become significantly more regulated since 2016. GDPR (effective May 2018), CCPA, BIPA, and similar frameworks now impose strict requirements on facial recognition and biometric data processing that did not apply when this PoC was created. Any consideration of this concept for current deployment would require separate legal, privacy, security, and ethical assessment.

---

## 4. Business Problem

**Evidence classification:** Documented (README) + Inferred (from data model)

**What retailers were expected to learn:**

- **Footfall:** How many visitors enter the store. [Documented: README lines 5–7]
- **Demographics:** Estimated age and gender distribution of visitors. [Documented: README line 17]
- **Conversion:** What proportion of entering visitors reach the payment area. [Inferred: data model supports `EnterOn` / `PaymentOn` comparison; no explicit conversion metric documented]
- **Dwell time:** How long visitors stay. [Inferred: data model captures `EnterOn` and `LeaveOn`; no computation observed]
- **Repeat visitors:** Whether the same person returns across sessions. [Inferred: `PersonId` enables cross-visit correlation; no repeat-rate metric computed]
- **Satisfaction proxy:** Smile score as an indicator of mood. [Documented: README line 17 mentions "emotion"; Observed: `FaceAttributeType.Smile` captured]

**Limitations of traditional measurement this project attempted to address:** Traditional footfall counters (beam-break sensors, thermal sensors) count entries but cannot provide demographic information, identity-based repeat-visitor recognition, or zone-specific tracking within a store. Retailizer attempted to bridge this gap using facial recognition.

**Decisions the output was expected to support (inferred):**

- Staff scheduling based on footfall patterns.
- Marketing effectiveness assessment via demographic mix.
- Store layout optimization via payment-zone conversion rates.
- Customer satisfaction monitoring via smile scores.

**Important qualification:** These business decisions are inferred from the data model's capabilities. No explicit decision-support documentation exists in the repository beyond the general README description.

---

## 5. Product Concept

**Evidence classification:** Documented + Observed

From a business perspective, Retailizer operates as a three-stage pipeline:

**Stage 1 — Observation:** IoT devices with attached USB cameras are deployed in a retail store. Each camera is assigned to monitor a specific zone (entry, payment area, or exit). The device continuously captures images and uses local face detection to identify frames containing human faces. Detected faces are cropped and transmitted to the cloud.

**Stage 2 — Analysis:** In the cloud, face images are processed by the Cognitive Services Face API to extract demographic attributes (estimated age, gender, smile score) and to identify whether the person has been seen before (biometric re-identification). Each face observation is recorded as a business event, classified by zone type based on which camera captured it. Events are correlated into visits — composite records representing a single person's presence in the store.

**Stage 3 — Reporting:** Visit and event data was intended to be visualized in Power BI dashboards. No reporting artifacts exist in the repository; the README references a demo dashboard with "no ambition to be used in production."

**Critical qualification:** Multiple implementation defects prevent the end-to-end pipeline from functioning as written in the repository. Specifically, device metadata is never persisted to the database after registration, which causes a null-reference crash when processing the first event; and visit update logic uses document-creation operations instead of update operations, preventing visit completion. See §15 and §18 for details.

---

## 6. Stakeholders and Actors

### 6.1 Identified Stakeholders

| Stakeholder | Role | Evidence | Classification |
| --- | --- | --- | --- |
| **Blue Dynamic** | Czech partner company; likely intended product owner or integrator | README line 3 | Documented |
| **Microsoft** | Technology partner providing platform, cloud services, and hackfest support | README line 3, LICENSE.md | Documented |
| **Retail organization (tenant)** | Business entity that would consume analytics output | `Device.TenantId` in data model | Inferred |
| **Store** | Physical retail location being monitored | `Device.StoreId` in data model | Inferred |
| **Business analyst** | Consumer of Power BI dashboards for decision-making | README lines 156–158 | Documented (implied) |
| **System administrator** | Deploys infrastructure, runs initializer, configures devices | ARM template, Initializer project | Inferred |
| **Device installer** | Physically places IoT devices and cameras in the store | README line 29 (DragonBoard + USB cameras) | Inferred |
| **Store visitor / shopper** | Person observed by the system; data subject, not a user | Face detection and identification pipeline | Observed |

### 6.2 Actor Classification

- **Software users:** Business analyst (Power BI consumer). Possibly store operator.
- **Business consumers of output:** Retail organization, store operator, business analyst.
- **System operators:** System administrator (deployment, initialization, configuration), device installer (physical setup).
- **Observed data subjects:** Store visitors and shoppers — they are subjects of biometric observation without any observed consent mechanism. They do not interact with the system and may not be aware of its operation.

**Uncertainty:** The organizational relationship between Blue Dynamic, the retailer, and the system operator cannot be determined. Whether Blue Dynamic intended to operate as a SaaS provider, systems integrator, or product vendor is unknown.

---

## 7. Business Capability Map

| ID | Capability | Business Purpose | Primary Stakeholder | Status | Principal Limitation |
| --- | --- | --- | --- | --- | --- |
| CAP-001 | Visitor face detection (local) | Filter camera frames to those containing faces before cloud transmission | System operator | Observed implementation | No throttling on capture loop; fire-and-forget processing risks resource exhaustion on IoT hardware |
| CAP-002 | Face capture and cloud upload | Transmit cropped face images to Azure for analysis | System operator | Observed implementation | No local queuing or offline buffering; network loss means lost observations |
| CAP-003 | Cloud face detection and demographic estimation | Extract age, gender, and smile score from face images | Business analyst | Observed implementation | Only first face's demographics used when multiple faces detected; estimates carry inherent bias and error |
| CAP-004 | Repeat-person recognition | Identify whether a visitor has been seen before | Business analyst | Partially implemented | Single person group shared across all tenants; no multi-tenant biometric isolation; continuous retraining after each observation does not scale |
| CAP-005 | Entry-zone observation | Record when a visitor appears at the store entrance | Store operator | Observed implementation | Depends on camera-to-zone configuration existing in database; no deduplication of repeated observations |
| CAP-006 | Payment-zone observation | Record when a visitor appears near the payment area | Store operator | Observed implementation | Does not confirm a commercial transaction; only records camera proximity |
| CAP-007 | Exit-zone observation | Record when a visitor appears at the store exit | Store operator | Observed implementation | Visitors using unmonitored exits will not generate leave events |
| CAP-008 | Visit reconstruction | Correlate entry, payment, and exit events into a single visit record | Business analyst | Broken implementation | Visit update logic uses `CreateDocumentAsync` instead of upsert/replace — updating an existing visit with payment or leave timestamps fails at runtime (creates duplicate or throws 409 Conflict) |
| CAP-009 | Conversion-related analysis | Measure proportion of entering visitors who reach payment zone | Business analyst | Documented intention | No computation in codebase; depends on CAP-008 which is broken; "payment" is proximity, not transaction |
| CAP-010 | Dwell-time analysis | Measure duration of store visits | Business analyst | Documented intention | Requires functional visit completion (CAP-008, broken); no computation in codebase; no visit timeout mechanism |
| CAP-011 | Reporting and visualization | Present analytics in Power BI dashboards | Business analyst | Documented intention | No Power BI artifacts, report definitions, or data connector configuration in repository |
| CAP-012 | Device registration | Register IoT devices with IoT Hub for authenticated communication | System administrator | Broken implementation | Device is registered in IoT Hub but device metadata (tenant, store, cameras) is never persisted to DocumentDB; downstream processing crashes with NullReferenceException |
| CAP-013 | Camera-to-zone configuration | Map physical cameras to business observation zones | System administrator | Not found | Data model supports it (`DeviceCamera.EventType`), but no API, UI, or mechanism to create or update camera configurations exists; must be done via direct database manipulation |
| CAP-014 | Tenant and store separation | Isolate data and operations by tenant and store | Retail organization | Partially implemented (data model only) | TenantId and StoreId flow through events and visits and are used in visit-matching queries, but: no API-level enforcement or authentication; no tenant validation; biometric person group is shared globally across all tenants |
| CAP-015 | Smile/emotion analysis | Capture smile scores as satisfaction proxy | Business analyst | Partially implemented | Smile score captured per event; no aggregation or reporting; Emotion API deployed in ARM template but never called — smile data comes from Face API |

**Status definitions used:** *Observed implementation* — code exists and implements the capability pathway. *Partially implemented* — some supporting code exists but the capability is incomplete. *Broken implementation* — code exists but contains defects that prevent runtime operation. *Documented intention* — described in documentation but no implementation found. *Not found* — no implementation or documentation evidence.

---

## 8. End-to-End Business Scenarios

### 8.1 Device Onboarding

| Field | Value |
| --- | --- |
| **Business objective** | Register an IoT device with the backend so it can send observation data |
| **Participants** | Device installer, system administrator, IoT device |
| **Preconditions** | Backend API deployed; IoT Hub provisioned; device has network access |
| **Main flow** | Device reads its hardware GUID via `EasClientDeviceInformation.Id` → POSTs to `{BackendUrl}/api/device` → Backend registers device in IoT Hub via `IotHubService.RegisterDevice()` → Returns device key → Device caches key for IoT Hub authentication |
| **Resulting business information** | Device can authenticate with IoT Hub and send messages |
| **Implementation limitation** | **Critical defect:** The `DeviceController` registers the device in IoT Hub but never persists the `Device` object (with `TenantId`, `StoreId`, `Cameras`) to DocumentDB. The `DocumentDbService.SaveAsync(Device)` method exists and uses `UpsertDocumentAsync`, but it is never called anywhere in the codebase. When the WebJob later calls `ddbService.GetDevice(deviceId)`, it returns `null`, causing a `NullReferenceException` at the camera-lookup line. |
| **Unresolved assumptions** | Device metadata (tenant, store, camera mappings) was likely pre-seeded directly in DocumentDB for hackfest demonstrations, bypassing the registration API. |

### 8.2 Camera and Zone Setup

| Field | Value |
| --- | --- |
| **Business objective** | Map each physical camera to a business observation zone (entry, payment, exit) |
| **Participants** | System administrator |
| **Preconditions** | Device registered; cameras physically connected |
| **Main flow** | No implementation found. The `DeviceCamera` entity has `Id`, `EventType`, and `ConfidenceLimit` properties, but no API endpoint, UI, or utility creates or updates camera configurations. |
| **Resulting business information** | Camera-to-zone mapping stored in `devices` collection |
| **Implementation limitation** | This capability is entirely absent from the application. Camera configuration must have been performed via direct DocumentDB manipulation. |
| **Unresolved assumptions** | Whether an admin interface existed outside this repository is unknown. |

### 8.3 Visitor Entering a Store

| Field | Value |
| --- | --- |
| **Business objective** | Detect a person at the entry camera, identify them, and create an entry event and visit |
| **Participants** | Store visitor (observed), IoT device, cloud services |
| **Preconditions** | Device running; cameras operational; device metadata with camera mappings in DocumentDB; Face API person group initialized |
| **Main flow** | Camera captures frame → local `FaceDetector` finds face regions → faces cropped with 25% padding → each face uploaded to blob storage as `{deviceId}/{guid}.jpg` → IoT Hub message sent with `{deviceId, blobName, cameraId}` → message routed to Service Bus queue → WebJob triggered → downloads face from blob → calls Face API `DetectAsync` (age, gender, smile) → calls `IdentifyAsync` against person group → identity resolved (existing or new person) → face enrolled and group retrained → Event created → new Visit created with `VisitStatus = Entered` |
| **Resulting business information** | Entry event and open visit recorded |
| **Implementation limitation** | Only first face's demographics used when multiple faces detected; all face IDs sent for identification but first candidate's result used — potential identity/demographics mismatch. No deduplication of repeated observations. Timestamp is server-side processing time, not capture time. |
| **Unresolved assumptions** | Whether the unthrottled capture loop was intentional or an oversight. Whether multi-face scenarios were expected in practice. |

### 8.4 Visitor at Payment Zone

| Field | Value |
| --- | --- |
| **Business objective** | Record that a visitor appeared near the payment area |
| **Participants** | Store visitor (observed), IoT device, cloud services |
| **Preconditions** | Camera configured with `EventType = "payment"` |
| **Main flow** | Same detection/identification pipeline as entry → Event created with `EventType = "payment"` → `FindOpenVisitForEventAsync` searches for open visit for same PersonId/TenantId/StoreId → if found, sets `PaymentOn` timestamp and re-saves visit → if not found, creates orphan visit |
| **Resulting business information** | Payment-zone presence recorded on visit |
| **Implementation limitation** | **Critical defect:** Visit re-save uses `CreateDocumentAsync`, which will fail (409 Conflict or duplicate document) when attempting to update the existing visit with the `PaymentOn` timestamp. Without a functioning update, payment events cannot be correlated to existing visits. Additionally, "payment" means camera proximity, not a confirmed transaction. |
| **Unresolved assumptions** | Whether POS integration was ever planned. |

### 8.5 Visitor Leaving

| Field | Value |
| --- | --- |
| **Business objective** | Record that a visitor exited and close their visit |
| **Participants** | Store visitor (observed), IoT device, cloud services |
| **Preconditions** | Camera configured with `EventType = "leave"` |
| **Main flow** | Same detection/identification pipeline → Event with `EventType = "leave"` → open visit found → sets `LeaveOn` timestamp and `VisitStatus = Left` → re-saves visit |
| **Resulting business information** | Visit marked as complete |
| **Implementation limitation** | Same critical defect as §8.4 — visit update will fail. No visit timeout exists; visits without a leave event remain open indefinitely. If a person exits through an unmonitored door, the visit is never closed. |
| **Unresolved assumptions** | Whether a visit timeout was planned. What the acceptable maximum visit duration was. |

### 8.6 Repeat Visitor Recognition

| Field | Value |
| --- | --- |
| **Business objective** | Identify that a returning visitor has been seen before, enabling repeat-visitor metrics |
| **Participants** | Store visitor (observed), Face API |
| **Preconditions** | Person previously enrolled in the Face API person group |
| **Main flow** | Face identification returns a candidate with confidence ≥ 0.8 → existing `PersonId` used for the event → new face added to person's collection → person group retrained |
| **Resulting business information** | Same `PersonId` across visits enables repeat-visitor tracking |
| **Implementation limitation** | Single person group for all tenants — a person identified in one tenant's store can be recognized in another's. Retraining after every observation is expensive and does not scale. At 248 faces per person, a random face is deleted to make room. If confidence < 0.8, a new person is created (fragmenting the identity) but `SuggestedPersonId` preserves the candidate for potential future merging (never consumed). |
| **Unresolved assumptions** | The 0.8 threshold rationale. Whether identity merging was planned. |

### 8.7 Reporting and Analysis

| Field | Value |
| --- | --- |
| **Business objective** | Visualize visitor analytics in Power BI dashboards |
| **Participants** | Business analyst |
| **Preconditions** | Event and visit data in DocumentDB; Power BI connected |
| **Main flow** | Not implemented. README describes a demo dashboard; no Power BI artifacts, report definitions, aggregation queries, or data connector configurations exist in the repository. |
| **Resulting business information** | Unknown — the actual dashboard content is not preserved |
| **Implementation limitation** | Entire capability is documented-only. Connection method between DocumentDB and Power BI is unknown. |
| **Unresolved assumptions** | What metrics were displayed. Whether the dashboard ever functioned with real data. |

---

## 9. Domain Model

### Tenant

- **Definition:** An organizational entity (typically a retail company) that owns one or more stores. The top-level data scoping boundary in the data model.
- **Evidence:** `Device.TenantId`, `Event.TenantId`, `Visit.TenantId`, visit-matching query filters by TenantId.
- **Classification:** Observed (data model); Inferred (business meaning).
- **Uncertainty:** No tenant management exists. TenantId values must be pre-configured on device records via direct database access. Biometric data (Face API person group) is not tenant-isolated.

### Store

- **Definition:** A physical retail location belonging to a tenant, equipped with one or more IoT devices.
- **Evidence:** `Device.StoreId`, `Event.StoreId`, `Visit.StoreId`, visit-matching query filters by StoreId.
- **Classification:** Observed (data model); Inferred (business meaning).
- **Uncertainty:** No store management exists. The relationship between store boundaries and camera placement is assumed.

### Device

- **Definition:** A physical IoT computing device (DragonBoard 410c running Windows 10 IoT Core) deployed in a store, with one or more USB cameras attached.
- **Evidence:** `Common/DTO/Device.cs` with `DeviceId`, `StoreId`, `TenantId`, `Cameras` list; `DeviceConfiguration.cs` reads hardware GUID via `EasClientDeviceInformation.Id`; README line 29.
- **Classification:** Observed + Documented.
- **Uncertainty:** Device metadata is never persisted by the registration API (critical defect). Configuration must have been performed externally.

### Camera

- **Definition:** A USB video capture device attached to a DragonBoard, identified by its OS-level `VideoDeviceId`. Each camera is configured to correspond to a specific observation zone via the `EventType` property on its `DeviceCamera` record.
- **Evidence:** `DeviceCamera.cs` with `Id`, `EventType`, `ConfidenceLimit`; `StartupTask.cs` uses `VideoDeviceId`; `Functions.cs` line 53 maps camera ID to event type.
- **Classification:** Observed.
- **Uncertainty:** No mechanism exists to configure camera-to-zone mappings through the application. The `ConfidenceLimit` property was designed but is never read — a hardcoded `0.8` threshold is used instead. Camera-to-zone mapping has no versioning; if a camera is physically moved, historical data semantics are corrupted.

### Observation Zone

- **Definition:** A logical area within a store corresponding to a business-significant location: entry, payment area, or exit. Each zone is monitored by a camera whose `EventType` defines the zone type.
- **Evidence:** Three constants in `Event.cs`: `EVENT_TYPE_ENTER`, `EVENT_TYPE_PAYMENT`, `EVENT_TYPE_LEAVE`; `DeviceCamera.EventType` links camera to zone.
- **Classification:** Inferred. The term "observation zone" does not appear in the code — it is implicit in the camera-to-event-type mapping.
- **Uncertainty:** Only three zone types exist. Whether zones can overlap, whether a camera can serve multiple zones, or whether additional zone types were planned is unknown.

### Person

- **Definition:** A service-generated identity created by the Cognitive Services Face API to represent a unique set of facial features. **Not a real-world known individual.** The system can recognize that the same physical person has been seen again, but it cannot determine who that person actually is.
- **Evidence:** `Functions.cs` line 111 creates a person with a random GUID as name: `CreatePersonAsync(personGroupId, Guid.NewGuid().ToString())`. No personal data (name, email, loyalty number) is ever associated. `PersonId` in events and visits is the Cognitive Services GUID.
- **Classification:** Observed.
- **Important alternative interpretation:** "Person" could be misread as representing a known individual — the implementation confirms it is only a biometric cluster identity. The same real-world person may have multiple Person records if identification confidence is below the threshold, and different people could share a PersonId if false-positive identification occurs.

### Face

- **Definition:** A cropped image region containing a detected human face. On the device: a JPEG with 25% padding around the bounding box. In the cloud: a Face API detection result with attributes and a temporary face ID.
- **Evidence:** `LocalFaceDetector.cs` (detection and cropping); `AzureImagePersister.cs` (upload as `{deviceId}/{guid}.jpg`); `Functions.cs` lines 62–71 (cloud detection).
- **Classification:** Observed.

### Event

- **Definition:** A record of a single face observation at a specific camera, enriched with the camera's zone type (enter/payment/leave), the person's identity, demographic attributes, and a server-side timestamp. The atomic unit of observation in the system.
- **Evidence:** `Event.cs` with `EventType`, `DeviceId`, `CameraId`, `TenantId`, `StoreId`, `PersonId`, `SuggestedPersonId`, `Confidence`, `TimeStamp`, `Person` (VisitPerson with Age, Gender, Smile).
- **Classification:** Observed.
- **Uncertainty:** Events are subject to duplication — every event is saved twice to DocumentDB via `CreateDocumentAsync` (once before visit correlation, once after). No deduplication mechanism exists.

### Visit

- **Definition:** A composite record correlating events into a representation of a person's presence in a store. Contains timestamps for entry, payment-zone proximity, and exit, plus the person's identity and visit status.
- **Evidence:** `Visit.cs` with `EnterOn`, `PaymentOn`, `LeaveOn`, `VisitStatus` (None/Entered/Left), `PersonId`, `TenantId`, `StoreId`.
- **Classification:** Observed.
- **Important qualification:** Visit is both a business concept (a customer's store visit) and a technical correlation construct (event grouping by PersonId within a tenant/store). Its fidelity to actual customer behavior is limited by: identification accuracy, lack of deduplication, absence of visit timeout, and a critical defect in the update logic that prevents visit completion. A single real visit may generate multiple Visit records if identification produces different PersonIds. Conversely, the lack of visit timeout means a returning visitor may be matched to a weeks-old open visit.

### Payment (Event Type)

- **Definition:** An event indicating that a face was observed at a camera configured as monitoring the payment area. **Does not represent a confirmed commercial transaction.** There is no POS integration, no transaction amount, no receipt data, and no mechanism to confirm that a purchase actually occurred.
- **Evidence:** `Event.cs` line 8: `EVENT_TYPE_PAYMENT = "payment"`; `Visit.cs` line 34: `PaymentOn` timestamp; no transaction data anywhere in the codebase.
- **Classification:** Observed.
- **Important alternative interpretation:** The term "payment" strongly implies a commercial transaction to business consumers. In practice, the system records only spatial proximity to a payment-zone camera. This is the most semantically misleading term in the domain model.

### Visit Status

- **Definition:** The lifecycle state of a visit: `None` (default/unknown), `Entered` (entry recorded, visit open), `Left` (exit recorded, visit closed).
- **Evidence:** `Visit.cs` lines 8–13: `enum VisitStatus { None, Entered, Left }`.
- **Classification:** Observed.

### Demographic Attribute

- **Definition:** A machine-estimated characteristic of a detected face: age (double), gender (string — binary), smile (double, 0–1).
- **Evidence:** `VisitPerson` class in `Event.cs`; `FaceAttributeType.Age, Gender, Smile` requested from Face API.
- **Classification:** Observed.
- **Uncertainty:** These are estimates with inherent inaccuracy and bias. Gender classification is binary and may misrepresent non-binary individuals. Age and smile estimates vary across observations of the same person.

### Recognition Confidence

- **Definition:** A numeric score (0–1) returned by the Face API indicating how likely a detected face matches a known person. A threshold of 0.8 is hardcoded to decide whether to accept or reject a match.
- **Evidence:** `Event.Confidence`, `Visit.Confidence`, `Functions.cs` line 120: `if (candidate.Confidence < 0.8)`.
- **Classification:** Observed.
- **Uncertainty:** The `DeviceCamera.ConfidenceLimit` property was designed for per-camera thresholds but is never read. The 0.8 threshold rationale is unknown.

### SuggestedPersonId

- **Definition:** When identification returns a candidate below the confidence threshold, the candidate's PersonId is recorded as `SuggestedPersonId` — a hint for potential future identity merging.
- **Evidence:** `Event.SuggestedPersonId`, `Visit.SuggestedPersonId`, `Functions.cs` line 130.
- **Classification:** Observed.
- **Uncertainty:** Never consumed by any downstream logic. Appears to be a provision for future functionality that was not implemented.

---

## 10. Event Model

### How observations become events

A face observation becomes an event through the following pipeline:

1. **Capture:** The UWP app captures a JPEG frame from a USB camera in an unthrottled infinite loop.
2. **Local detection:** Windows `FaceDetector` API finds face bounding boxes. Faces are cropped with 25% padding.
3. **Upload:** Each cropped face is uploaded to Azure Blob Storage as `{deviceId}/{guid}.jpg`. An IoT Hub message is sent containing `{deviceId, blobName, cameraId}`.
4. **Routing:** IoT Hub messages are routed to a Service Bus queue. The routing mechanism is undetermined — the README mentions Stream Analytics, but no Stream Analytics resource exists in the ARM template.
5. **Processing:** A WebJob triggered by the Service Bus queue downloads the face image, calls Face API for detection and identification, resolves person identity, and creates an Event record.

### Event types

Three event types are defined as constants:

- `enter` — face observed at an entry-zone camera.
- `payment` — face observed at a payment-zone camera.
- `leave` — face observed at an exit-zone camera.

The event type is determined entirely by the camera's configured `EventType` property, not by any behavioral analysis. There is no system intelligence in zone classification — it is a static camera-to-zone mapping.

### Event business meaning

An event represents a single observation of a face at a specific location. It is not a confirmed action (entering, purchasing, leaving) — it is a camera-proximity record classified by zone assignment. The "payment" event type is particularly susceptible to misinterpretation (see §9 Domain Model — Payment).

### Event ordering and reliability

Event ordering is not guaranteed. The pipeline is fully asynchronous: images are uploaded from the device, messages are queued, and processing occurs independently. A payment event could be processed before its corresponding entry event if the entry camera's image takes longer to process (e.g., due to Face API throttling). The current visit-assignment logic does not handle out-of-order events — a payment event processed before the entry event will create an orphan visit.

### Duplicate events

Events are duplicated in two ways:

1. **No observation deduplication:** If a person stands in front of a camera, every captured frame generates a separate event. With the unthrottled capture loop, this could generate many events per second for a single person.
2. **Double persistence:** Every event is saved to DocumentDB twice — once before visit correlation (without `VisitId`) and once after (with `VisitId`). Both calls use `CreateDocumentAsync`, creating two distinct documents. This affects all event types.

### Multi-face handling

When multiple faces appear in a single camera frame:

- All face IDs are sent to the Face API for identification.
- Only the first face's (`detectionResult[0]`) demographic attributes (age, gender, smile) are recorded.
- The first candidate from the first identification result is used for identity resolution.

This creates a potential mismatch: if the identification result ordering does not correspond to the detection result ordering, the demographic attributes may belong to a different person than the one whose identity is assigned to the event. Only one event is created per queue message regardless of the number of faces detected.

---

## 11. Visit Model

### Visit creation

- **Enter event:** Always creates a new visit with `VisitStatus = Entered` and `EnterOn` set to the event timestamp. No check is performed for existing open visits for the same person — if a person is observed at the entry camera multiple times (due to lack of deduplication), multiple open visits are created.
- **Payment or leave event without a matching open visit:** Creates a new "orphan" visit with only the corresponding timestamp populated (no entry time). This is the system's graceful-degradation behavior for out-of-order or unmatched events.

### Event-to-visit assignment

For non-enter events, `FindOpenVisitForEventAsync` queries DocumentDB for the most recent open visit matching `PersonId`, `TenantId`, and `StoreId` where `VisitStatus != Left AND VisitStatus != None`, ordered by `EnterOn DESC`. The first matching result is used. If multiple open visits exist for the same person, only the most recent one is updated.

### Visit completion

A visit transitions to status `Left` only when a leave event is processed and matched to it. There is no timeout, expiration, or automatic closure mechanism. Visitors who leave through unmonitored exits, or whose faces are not detected by the exit camera, will have permanently open visits.

**Critical defect:** Visit updates (setting `PaymentOn` or `LeaveOn` on an existing visit) use `DocumentDbService.SaveAsync(Visit)`, which calls `CreateDocumentAsync`. This attempts to create a new document rather than updating the existing one. DocumentDB will either reject the operation with HTTP 409 Conflict (if the document `id` already exists) or create a duplicate. In either case, visit updates do not function as intended. This defect affects both payment and leave event processing, making visit completion non-functional as written.

### Repeat visitors and correlation

When the same person (same `PersonId`) is recognized on a subsequent visit, a new visit is created on the new entry event. The `PersonId` links visits across sessions, enabling repeat-visitor analysis in principle. However:

- If identification fails and creates a new `PersonId`, a single person's visits will be split across multiple identities.
- Without visit timeout, a returning visitor may be matched to a weeks-old open visit instead of having a new visit created.

### Multi-camera and multi-store uncertainty

Each visit is scoped to a single `TenantId` + `StoreId` + `PersonId` combination. Cross-store visit correlation (tracking a person across multiple stores) is not explicitly implemented as a feature, but the shared person group makes it technically possible — and represents a privacy concern.

---

## 12. Business Rules

### Event Rules

| ID | Rule Statement | Domain | Evidence | Exception / Ambiguity | Stakeholder Confirmation |
| --- | --- | --- | --- | --- | --- |
| BR-EVENT-001 | Event type is determined by the camera's configured `EventType`, not by behavioral analysis | Event creation | Observed: `Functions.cs` line 53 | If camera ID is not found in device configuration, a `NullReferenceException` occurs. Czech TODO comment acknowledges this gap. | Confirm whether camera-to-zone mapping was intended to be static or dynamic |
| BR-EVENT-002 | Each queue message produces at most one event, using only the first detected face's demographics | Event creation | Observed: `Functions.cs` lines 84–98, 93–95 | All face IDs sent for identification but only first candidate used — potential identity/demographics mismatch in multi-face frames | Was multi-face handling discussed? |
| BR-EVENT-003 | Observations with no detected faces are silently dropped | Event creation | Observed: `Functions.cs` lines 74–81 | Czech TODO suggests recording these was planned but not implemented | Should no-face observations be counted? |
| BR-EVENT-004 | Event timestamp is server-side processing time (`DateTime.Now`), not device-side capture time | Event creation | Observed: `Functions.cs` line 97 | Queue latency and API throttling retries (up to ~17 minutes backoff) can cause significant drift | Was device-side timestamping considered? |
| BR-EVENT-005 | Every event is persisted twice — once without `VisitId`, once with — creating duplicate documents | Event creation | Observed: `Functions.cs` line 162 (first save), lines 190/214 (second save); both use `CreateDocumentAsync` | Affects all event types, not just enter events. Inflates event counts in the database. | Was this intentional? |

### Visit Rules

| ID | Rule Statement | Domain | Evidence | Exception / Ambiguity | Stakeholder Confirmation |
| --- | --- | --- | --- | --- | --- |
| BR-VISIT-001 | An enter event always creates a new visit regardless of existing open visits | Visit lifecycle | Observed: `Functions.cs` lines 186–191 | Without deduplication, repeated entry observations create multiple visits for a single person | Should existing open visits be checked on entry? |
| BR-VISIT-002 | Non-enter events are matched to the most recent open visit by PersonId + TenantId + StoreId | Visit lifecycle | Observed: `Functions.cs` lines 219–241 | Only first result used; query effectiveness depends on DocumentDB index configuration | How should multiple open visits be handled? |
| BR-VISIT-003 | If no open visit exists for a non-enter event, a new orphan visit is created | Visit lifecycle | Observed: `Functions.cs` lines 194–198 | Creates visits without entry data; may indicate out-of-order processing or missed entry | Are orphan visits valid records or data quality issues? |
| BR-VISIT-004 | A visit is closed only by a leave event — no timeout or auto-closure | Visit lifecycle | Observed: `Functions.cs` lines 207–209; no scheduled cleanup exists | Permanently open visits distort all time-based metrics | Was a visit timeout planned? What is a reasonable maximum duration? |
| BR-VISIT-005 | Visit updates use `CreateDocumentAsync` instead of upsert/replace, causing runtime failure | Visit lifecycle | Observed: `DocumentDbService.cs` line 80; `Functions.cs` lines 204–211 | This is a critical implementation defect, not an intentional rule. Payment and leave events cannot update existing visits. | Were visit updates ever tested during the hackfest? |

### Identity Rules

| ID | Rule Statement | Domain | Evidence | Exception / Ambiguity | Stakeholder Confirmation |
| --- | --- | --- | --- | --- | --- |
| BR-IDENTITY-001 | Face identification confidence threshold is hardcoded at 0.8 | Person identification | Observed: `Functions.cs` line 120 | `DeviceCamera.ConfidenceLimit` was designed for per-camera thresholds but is never read | What was the 0.8 rationale? Should it vary by camera? |
| BR-IDENTITY-002 | Below-threshold match records `SuggestedPersonId` but creates a new person | Person identification | Observed: `Functions.cs` lines 130–132 | `SuggestedPersonId` is never consumed; appears to be provision for future identity merging | Was identity merging planned? |
| BR-IDENTITY-003 | Person group is retrained after every face addition | Person identification | Observed: `Functions.cs` lines 159–160 | Training is asynchronous — subsequent identifications during training may use stale data. Does not scale with high observation frequency. | Was batched training considered? |
| BR-IDENTITY-004 | At 248 persisted faces per person, a random face is deleted | Person identification | Observed: `Functions.cs` lines 141–145 | Random deletion may remove high-quality reference faces | Was quality-based retention considered? |
| BR-IDENTITY-005 | All tenants share a single Face API person group `"retailizer"` | Person identification | Observed: `Functions.cs` line 21 | A person identified in one tenant's store can be recognized in another's — severe multi-tenant biometric isolation failure | Was this an intentional hackfest simplification? |

### Tenant Rules

| ID | Rule Statement | Domain | Evidence | Exception / Ambiguity | Stakeholder Confirmation |
| --- | --- | --- | --- | --- | --- |
| BR-TENANT-001 | TenantId and StoreId flow through the data model and visit-matching queries but have no API-level enforcement | Multi-tenancy | Observed: `Device.cs`, `Event.cs`, `Visit.cs`, `Functions.cs` line 228 | No authentication, authorization, or tenant validation on device registration or any other endpoint | Was multi-tenant API security planned? |

### Metric Rules

| ID | Rule Statement | Domain | Evidence | Exception / Ambiguity | Stakeholder Confirmation |
| --- | --- | --- | --- | --- | --- |
| BR-METRIC-001 | Conversion is implied by payment-zone camera proximity, not by transaction data | Analytics | Observed: `Event.cs` line 8, `Visit.cs` line 34; no POS integration | "Payment" is the most semantically misleading term in the domain model | Was POS integration planned? |
| BR-METRIC-002 | No deduplication of repeated observations exists | Data quality | Observed: unthrottled capture loop in `StartupTask.cs`; no suppression logic in `Functions.cs` | README line 57 mentions IoT Hub deduplication possibility but nothing was implemented | What deduplication strategy was intended? |

---

## 13. Data Captured, Transmitted, Processed, Stored, Inferred, and Derived

### Captured raw data

| Data | Source | Evidence |
| --- | --- | --- |
| Camera frame (JPEG) | USB camera via UWP MediaCapture | `StartupTask.cs` — transient, in-memory only |
| Device hardware GUID | `EasClientDeviceInformation.Id` | `DeviceConfiguration.cs` |
| Camera video device ID | OS-level `VideoDeviceId` | `StartupTask.cs` line 128 |

### Locally processed data

| Data | Processing | Evidence |
| --- | --- | --- |
| Face bounding boxes | Windows `FaceDetector` API (on-device) | `LocalFaceDetector.cs` |
| Cropped face image (JPEG with 25% padding) | Bitmap cropping from detected bounding box | `LocalFaceDetector.cs` |

### Transmitted data

| Data | Destination | Evidence |
| --- | --- | --- |
| Cropped face JPEG | Azure Blob Storage (`faces/{deviceId}/{guid}.jpg`) | `AzureImagePersister.cs` |
| IoT Hub message (`{deviceId, blobName, cameraId}`) | Azure IoT Hub | `AzureImagePersister.cs` |

### Externally processed data

| Data | Service | Evidence |
| --- | --- | --- |
| Face detection (bounding box, temporary face ID) | Azure Face API `DetectAsync` | `FaceApiService.cs`, `Functions.cs` |
| Face attributes (age, gender, smile) | Azure Face API `DetectAsync` with attribute types | `Functions.cs` lines 66–70 |
| Face identification (candidate PersonId, confidence) | Azure Face API `IdentifyAsync` | `Functions.cs` lines 102–103 |
| Person creation (new PersonId) | Azure Face API `CreatePersonAsync` | `Functions.cs` lines 110–111 |
| Face enrollment in person group | Azure Face API `AddPersonFaceAsync` | `Functions.cs` line 159 |
| Person group retraining | Azure Face API `TrainPersonGroupAsync` | `Functions.cs` line 160 |

### Stored images

| Data | Location | Retention | Evidence |
| --- | --- | --- | --- |
| Cropped face JPEGs | Azure Blob Storage, `faces` container | Indefinite — no lifecycle management; deletion code commented out | `AzureImagePersister.cs`, `Functions.cs` lines 80, 168 |

### Stored identifiers and records

| Data | Location | Evidence |
| --- | --- | --- |
| Device records (DeviceId, TenantId, StoreId, Cameras) | DocumentDB `devices` collection | `DocumentDbService.cs` — note: never populated via the API |
| Event records | DocumentDB `events` collection | `DocumentDbService.SaveAsync(Event)` |
| Visit records | DocumentDB `visits` collection | `DocumentDbService.SaveAsync(Visit)` |
| Biometric face templates and PersonIds | Cognitive Services Face API person group `"retailizer"` | `Functions.cs` line 21; templates are opaque, managed by the service |

### Inferred attributes per event

| Attribute | Source | Accuracy |
| --- | --- | --- |
| Age (double) | Face API estimation | Machine estimate; varies across observations |
| Gender (string, binary) | Face API estimation | Binary classification; may misrepresent non-binary individuals |
| Smile score (double, 0–1) | Face API estimation | Machine estimate; interpretation as "satisfaction" is an inferential leap |
| Person identity | Face API identification with 0.8 threshold | Depends on recognition accuracy; same person may get multiple IDs |
| Event type | Camera configuration lookup | Depends on camera-to-zone mapping being correctly configured |

### Correlated data

| Data | Derivation | Evidence |
| --- | --- | --- |
| Visit record | Events correlated by PersonId + TenantId + StoreId + VisitStatus | `Functions.cs` `ProcessEventAsync` |

### Potential derived metrics (not computed)

| Metric | Feasibility | Qualification |
| --- | --- | --- |
| Visitor count | Enabled by enter-event count | Inflated by lack of deduplication |
| Repeat visitors | Enabled by PersonId correlation across visits | Degraded by identity fragmentation |
| Visit duration / dwell time | Enabled by `EnterOn` and `LeaveOn` timestamps | Requires functional visit completion (currently broken); no timeout for unclosed visits |
| Payment-zone appearance rate | Enabled by visits with `PaymentOn` set | Requires functional visit update (currently broken); "payment" is proximity, not transaction |
| Demographic distribution | Enabled by per-event age/gender data | Only first face's demographics used; machine estimates with inherent error |
| Smile distribution | Enabled by per-event smile score | Machine estimate; interpretation as satisfaction is speculative |

---

## 14. Reporting and Analytical Goals

### Explicitly documented reporting

The README states: "We designed a Power BI dashboard to visualize captured data" and "We have created Power BI dashboard showing overall data gathered and processed by Retailizer." The dashboard is described as "for demo purposes" with "no ambition to be used in production in any form." A screenshot reference exists (`images/4-outputs.png`) but no actual Power BI artifacts (`.pbix`, `.pbit`) are in the repository.

**Classification:** Documented intention.

### Reporting connected in implementation

No reporting connection exists in the codebase. There are no:

- Power BI report definitions or templates.
- Reporting API endpoints in the backend.
- Aggregation queries in `DocumentDbService`.
- Data export mechanisms.
- Data connector configurations.

The method by which Power BI connected to DocumentDB (direct connector, data export, intermediate warehouse) is unknown.

**Classification:** Not found.

### Metrics enabled by data model

The event and visit data model would support these metrics if visit completion functioned correctly and deduplication were implemented:

- Visitor count (count of enter events or visits per period).
- Repeat-visitor rate (visits per unique PersonId).
- Visit duration (LeaveOn − EnterOn).
- Payment-zone appearance rate (visits with PaymentOn / total visits).
- Age, gender, and smile distributions.
- Peak-hour analysis (event counts by time period).

### Metrics that are only plausible

- **Conversion rate** as a business metric — would require "payment" to reliably indicate a purchase, which it does not.
- **Customer satisfaction index** from smile scores — requires interpreting a momentary facial expression as overall satisfaction.
- **Cross-store comparison** — data model supports it, but no aggregation logic exists.
- **Movement patterns between zones** — would require per-person event sequencing analysis, which is not implemented.

---

## 15. Implemented versus Intended Capabilities

| Capability | Documented Intent | Observed Implementation | Final Assessment | Key Uncertainty |
| --- | --- | --- | --- | --- |
| Edge face detection | Detect faces locally before sending to cloud | UWP app with Windows FaceDetector, cropping, upload | **Implemented** — but unthrottled capture loop risks resource exhaustion | Was throttling omitted intentionally or was it an oversight? |
| Cloud face detection + demographics | Determine age, gender, emotion | Face API `DetectAsync` with Age, Gender, Smile attributes | **Implemented** | Only first face's demographics used in multi-face frames |
| Face identification | Identify repeat visitors | Face API `IdentifyAsync` with 0.8 threshold | **Implemented** — but single shared person group, no tenant isolation | Was per-tenant person group planned? |
| Event creation | Record observations as events | Event created with all attributes and persisted | **Implemented** — but duplicated (saved twice) | Was double-save intentional? |
| Visit reconstruction | Correlate events into visits | Visit creation and update logic exists | **Broken** — `CreateDocumentAsync` used for updates; visit completion fails at runtime | Were visit updates tested during the hackfest? |
| Device registration | Register IoT devices | IoT Hub registration works | **Broken** — device metadata never saved to DocumentDB; downstream processing crashes | Were devices pre-seeded in the database? |
| Camera configuration | Map cameras to zones | Data model supports it (`DeviceCamera.EventType`) | **Not implemented** — no API or UI to configure | How were cameras configured for demos? |
| Tenant/store separation | Isolate data by tenant | TenantId/StoreId in data model and queries | **Partially implemented** — data model only; no API enforcement; biometric data shared globally | Was multi-tenant a real requirement? |
| Emotion API | Advanced emotion analysis | ARM template deploys Emotion API resource; config key reserved | **Not implemented** — never called; smile data comes from Face API | Was Emotion API used in an earlier version? |
| Stream Analytics | Route IoT Hub messages | README describes it; ARM template defaults queue name to `"events"` | **Not found** — no Stream Analytics resource in ARM template | Was it configured manually in Azure portal? |
| Power BI dashboards | Visualize analytics | README describes demo dashboard with screenshot | **Documented only** — no artifacts in repository | What did the dashboard actually show? |
| Blob cleanup | Delete face images after processing | Commented-out code in `Functions.cs` lines 80, 168 | **Not implemented** — images persist indefinitely | Why was deletion disabled? |
| Deduplication | Suppress repeated observations | README line 57 mentions IoT Hub deduplication possibility | **Not implemented** — no deduplication at any level | What strategy was intended? |
| Per-camera confidence | Configurable identity thresholds | `DeviceCamera.ConfidenceLimit` property with XML documentation | **Not implemented** — hardcoded 0.8 used instead | Was 0.8 empirically determined? |
| Edge offline handling | Buffer data during network loss | Not documented | **Not found** — no local queuing; network loss means lost observations | Was offline operation considered? |

---

## 16. Business Assumptions

| ID | Assumption | Evidence | Classification | Consequence if Invalid |
| --- | --- | --- | --- | --- |
| BA-001 | A face observation at a camera represents a visitor at that zone | Camera-to-EventType mapping | Inferred | Passersby or staff visible to cameras would be counted as visitors |
| BA-002 | Each camera monitors exactly one zone, and the mapping is static | `DeviceCamera.EventType` is a single value per camera | Observed | If cameras overlap zones or are repositioned, events are mistyped and historical data is corrupted |
| BA-003 | A person enters before they pay or leave | Visit lifecycle assumes enter → payment → leave ordering | Inferred | Out-of-order event processing creates orphan visits and split records |
| BA-004 | Face identification reliably produces the same PersonId for the same real person | Visit correlation depends on stable PersonId | Inferred | Misidentification fragments visits and inflates visitor counts |
| BA-005 | A payment-zone camera observation is a useful proxy for a purchase | `EVENT_TYPE_PAYMENT` mapped to payment cameras | Inferred | Without POS integration, conversion metrics measure proximity, not transactions |
| BA-006 | One face per frame is the normal case | Only `detectionResult[0]` used for demographics | Inferred | Groups of people produce mismatched demographic data |
| BA-007 | Server-side processing timestamp is an acceptable proxy for observation time | `DateTime.Now` at WebJob processing time | Observed | Queue latency and API throttling retries (up to ~17 minutes) distort temporal analysis |
| BA-008 | Continuous retraining after each face addition is acceptable | `TrainPersonGroupAsync` after every enrollment | Observed | Does not scale; training is asynchronous and identification during training may use stale data |
| BA-009 | A single biometric person group suffices | Hardcoded `personGroupId = "retailizer"` | Observed | No multi-tenant isolation; cross-tenant biometric leakage |
| BA-010 | Captured demographics (age, gender, smile) are sufficiently accurate for retail analytics | Face API attribute estimation | Inferred | Machine estimates carry inherent bias and error; accuracy varies by conditions |

---

## 17. Privacy, Biometric-Data, and Ethical Considerations

All considerations in this section are specific to the Retailizer system as implemented. This section does not provide legal advice. Any current real-world deployment of this concept would require a separate legal, privacy, security, and ethical assessment.

### Facial-image capture

The system captures high-resolution face images of store visitors via USB cameras. Cropped face JPEGs are uploaded to Azure Blob Storage and persist indefinitely — blob deletion code exists but is commented out (`Functions.cs` lines 80, 168). No image expiration, lifecycle management, or access controls beyond the storage account key are observed.

**Evidence:** `LocalFaceDetector.cs`, `AzureImagePersister.cs`, `Functions.cs`.

### Persistent repeat-person recognition

The system creates persistent biometric templates in the Cognitive Services Face API person group `"retailizer"`. These templates enable cross-session recognition of the same individual without any time limit. Templates persist in the Cognitive Services cloud service — the application stores only the `PersonId` reference.

**Evidence:** `Functions.cs` lines 21, 159–160; `Retailizer.Azure.Initializer/Program.cs`.

### Demographic estimation

Age, gender, and smile are machine-inferred from facial images. These estimates carry inherent bias: age estimation has known accuracy limitations that vary by demographic group; gender classification is binary (string type) and misrepresents non-binary individuals; smile detection as a satisfaction proxy involves a significant inferential leap from a momentary facial expression.

**Evidence:** `VisitPerson` class; `FaceAttributeType.Age, Gender, Smile`.

### Repeat-visitor tracking

The system tracks individuals across multiple visits to the same store. Because the person group is shared globally (single `personGroupId`), tracking extends across all stores and all tenants. A person observed in one organization's store can be recognized in another's.

**Evidence:** `Functions.cs` line 21; visit query in `FindOpenVisitForEventAsync`.

### Data retention uncertainty

No data retention or deletion policy is implemented or documented:

- Face images persist indefinitely in blob storage.
- Biometric templates persist indefinitely in Cognitive Services.
- Event and visit records persist indefinitely in DocumentDB.
- No lifecycle management, archival, or purge mechanisms exist.

### Transparency and consent

No consent mechanism, privacy notice, opt-out capability, or visitor notification is implemented or referenced. The system operates covertly — store visitors are observed without their knowledge. This was historically common for proof-of-concept systems in 2016, but would be a critical gap for any deployment under GDPR (Article 9), CCPA, BIPA, or similar frameworks.

### Purpose limitation

The data model captures more information than strictly necessary for basic footfall counting. Full demographic profiles and persistent biometric identities go beyond simple counting. The system does not distinguish between purpose-limited data collection and comprehensive surveillance.

### Data minimization

The system stores full cropped face images even after demographic attributes have been extracted. A data-minimization approach would delete images after processing. This was clearly considered (commented-out deletion code) but not implemented.

### Data access and deletion

No administrative API, data-export mechanism, or deletion process exists. Under modern data protection regulations, data subjects (store visitors) may have rights to access, correct, or delete their biometric data. The system provides no means to fulfill such requests.

### Misclassification risk

Both identity misclassification (assigning the wrong PersonId) and demographic misclassification (incorrect age or gender estimation) can produce misleading analytics. The system has no mechanism to detect, flag, or correct misclassifications.

### Cross-tenant biometric leakage

The single shared person group means that biometric templates enrolled by one tenant's store operations are accessible during identification in another tenant's store. This represents a data leakage path between separate organizations.

**Evidence:** `Functions.cs` line 21: `personGroupId = "retailizer"` (hardcoded constant).

### Sample data ethics

The environment initializer includes `satya.jpg` — an image of a public figure (Microsoft CEO Satya Nadella) used to seed the Face API person group during hackfest demonstrations. While appropriate for an informal hackfest context, using a real person's biometric data for system initialization raises ethical considerations for any broader distribution.

**Evidence:** `Retailizer.Azure.Initializer/Program.cs` line 70; `satya.jpg` (23,527 bytes).

### Historical proof-of-concept context

In 2016, facial recognition in retail was an emerging technology area. GDPR was not yet in force, and regulatory awareness around biometric surveillance was significantly lower. The proof-of-concept nature means privacy engineering was not a priority. However, the inherent product concept — covert biometric surveillance and tracking of retail customers — carries fundamental privacy and ethical implications regardless of implementation maturity or historical context.

---

## 18. Business Limitations

1. **Critical pipeline defects:** Device metadata is never persisted after registration (crashes event processing); visit update logic uses document creation instead of upsert (prevents visit completion). These defects prevent the end-to-end pipeline from functioning.
2. **No multi-tenant biometric isolation:** All tenants share a single Face API person group; biometric data leaks across organizational boundaries.
3. **No deduplication:** Unthrottled capture loop generates continuous events for any person visible to a camera; no suppression at any pipeline stage. Combined with BR-VISIT-001 (every enter event creates a new visit), a person standing at the entry camera for 10 seconds could generate dozens of open visits.
4. **No visit timeout:** Visits without a leave event remain open indefinitely, distorting dwell time, conversion, and active-visitor metrics.
5. **Misleading payment semantics:** "Payment" records camera proximity, not a commercial transaction.
6. **Server-side timestamps:** Event time reflects cloud processing time, not observation time. Face API throttling retries can back off up to approximately 17 minutes (1s, 2s, 4s, ... doubling for 10 retries), causing significant timestamp drift.
7. **No edge resilience:** No local queuing or buffering. Network loss means lost observations with no recovery.
8. **Compilation defect:** `Reailizer.Job/AppConfiguration.cs` does not implement `IotHubConnectionString` from `IAppConfiguration`, causing compiler error CS0535. The code in the repository cannot be compiled as-is.
9. **Incomplete business workflows:** No camera configuration, tenant management, store management, or device management beyond initial IoT Hub registration.
10. **Unverified metrics:** No analytics computation exists in the codebase. All metrics are theoretical capabilities of the data model.
11. **Missing operational controls:** No monitoring, alerting, health checks, logging infrastructure, or operational dashboards.
12. **Missing data governance:** No data retention policy, deletion mechanism, access controls, or data-subject-request handling.

---

## 19. Conflicting Evidence

### CE-001: Visit Persistence Model

- **Conflict:** `Visit` class extends `Azure.Storage.Table.TableEntity` with `PartitionKey`/`RowKey` encoding and `ReadEntity`/`WriteEntity` overrides. Actual persistence uses DocumentDB via `DocumentDbService`.
- **Evidence:** `Visit.cs` line 16 (inherits `TableEntity`), lines 72–103 (Table Storage methods); `DocumentDbService.cs` lines 78–81 (persists to DocumentDB); README line 92: "we also architecture a persistence layer built upon Table Storage, but that's not part of the current solution."
- **Impact:** The Table Storage inheritance is dead code. DocumentDB ignores the `TableEntity` base class, `PartitionKey`, `RowKey`, `ReadEntity`, and `WriteEntity`. This creates confusion about the intended persistence model.
- **Safest conclusion:** Table Storage was a designed alternative that was abandoned in favor of DocumentDB. The inheritance is vestigial code that was never cleaned up.

### CE-002: Emotion API

- **Conflict:** The ARM template deploys an Emotion API Cognitive Services resource (`kind: Emotion`). `App.config` reserves an `EmotionApiSubscriptionKey` placeholder. No code references the Emotion API.
- **Evidence:** `template.json` lines 53–64; `App.config` line 5; no Emotion API SDK import or call in any source file.
- **Impact:** Deploys an unused (and billable) Azure resource. May mislead stakeholders into assuming advanced multi-modal emotion analysis when only a basic smile score from the Face API is captured.
- **Safest conclusion:** The Emotion API was likely part of an early design that planned separate face and emotion analysis. The Face API's built-in smile attribute made the Emotion API redundant, but infrastructure and configuration were not cleaned up.

### CE-003: Stream Analytics

- **Conflict:** README describes Stream Analytics as part of the message routing pipeline ("pass messages directly to Azure Stream Analytics"). The ARM template contains no `Microsoft.StreamAnalytics` resource.
- **Evidence:** README lines 56 and 101; `template.json` (no Stream Analytics resource); WebJob uses `ServiceBusTrigger`, not a Stream Analytics output.
- **Impact:** The automated deployment template cannot reproduce the full pipeline described in the README. The actual IoT Hub to Service Bus routing mechanism is undetermined.
- **Safest conclusion:** Message routing was likely configured manually in the Azure portal (IoT Hub built-in routing rules) or Stream Analytics was manually provisioned outside the ARM template. The README description may reflect an intended or earlier architecture.

### CE-004: Queue Name Mismatch

- **Conflict:** The WebJob triggers on queue `"events"` (hardcoded in `Functions.cs` line 39). The ARM template defaults the queue name to `"events"` (`template.json` line 19). But `parameters.json` overrides this to `"event_queue"` (line 9).
- **Evidence:** `Functions.cs` line 39; `template.json` line 19; `parameters.json` line 9.
- **Impact:** If deployed using the parameters file as-is, the Service Bus queue would be named `"event_queue"` but the WebJob would listen on `"events"`, resulting in an idle WebJob that never processes messages.
- **Safest conclusion:** Either the parameters file reflects a different deployment configuration, or this is an error. The default value in the template (`"events"`) matches the WebJob code.

### CE-005: ConfidenceLimit Property

- **Conflict:** `DeviceCamera.ConfidenceLimit` exists with XML documentation ("If confidence is lower than ConfidenceLimit, new person is created") but the actual confidence threshold is hardcoded as `0.8` in `Functions.cs` line 120.
- **Evidence:** `DeviceCamera.cs` lines 8–11; `Functions.cs` line 120.
- **Impact:** Per-camera configurable confidence limits were designed but never wired into the processing logic. All cameras use the same hardcoded threshold.
- **Safest conclusion:** This was a planned feature that was not implemented. The hardcoded value was likely a development shortcut.

---

## 20. Confirmed Unknowns and Stakeholder Questions

### Product Purpose

- **[Blocking]** Was Retailizer intended to evolve into a commercial product, or was it purely a technology demonstration?
- **[Important]** What was Blue Dynamic's intended business model — SaaS, on-premises, integration, consulting?
- **[Important]** Was a production or pilot deployment ever attempted with a real retailer?

### Capability Semantics

- **[Important]** Was an administration interface for devices, cameras, tenants, and stores planned or implemented outside this repository?
- **[Important]** How were devices assigned to tenants and stores for hackfest demonstrations? Manual database seeding?
- **[Useful context]** Was the Emotion API ever used in an earlier version before the Face API's built-in smile attribute made it redundant?

### Event and Visit Rules

- **[Critical]** How was the system demonstrated during the hackfest without encountering DocumentDB 409 Conflict exceptions on visit updates? Were payment and leave update paths bypassed or untested?
- **[Critical]** Was device registration metadata manually pre-seeded in the database, explaining why the missing `SaveAsync(device)` call in `DeviceController` was never caught?
- **[Important]** What deduplication strategy was intended for repeated observations?
- **[Important]** Was a visit timeout or auto-close mechanism planned?
- **[Important]** Was device-side timestamping considered?
- **[Useful context]** Was out-of-order event handling discussed?

### Identity

- **[Important]** What was the rationale for the 0.8 confidence threshold?
- **[Important]** Was a mechanism for merging fragmented person identities planned?
- **[Important]** Was per-tenant Face API person group isolation considered?
- **[Useful context]** What was the expected number of unique persons the system would need to track?

### Metrics and Reporting

- **[Important]** What specific measures and dimensions were visualized in the Power BI demo dashboard?
- **[Useful context]** How was Power BI connected to DocumentDB?

### Privacy and Governance

- **[Blocking]** Was any privacy impact assessment or legal review performed?
- **[Important]** Was a consent or notification mechanism for store visitors considered?
- **[Important]** Was a data retention and deletion policy defined?

### Operational Use

- **[Important]** How were IoT Hub messages routed to Service Bus? Manual portal configuration, Stream Analytics, or IoT Hub routing rules?
- **[Useful context]** What is the maximum acceptable frame-capture frequency on the edge device?
- **[Useful context]** Why was blob deletion after processing disabled (commented out)?

---

## 21. Evidence and Traceability

### Key source files

| File | Relevance | Classification |
| --- | --- | --- |
| `README.md` | Project overview, architecture, purpose, design decisions | Documented |
| `Common/DTO/Device.cs` | Device entity with DeviceId, StoreId, TenantId, Cameras | Observed |
| `Common/DTO/DeviceCamera.cs` | Camera entity with Id, EventType, ConfidenceLimit (unused) | Observed; Conflicting (CE-005) |
| `Common/DTO/Event.cs` | Event entity; three event type constants; VisitPerson demographics | Observed |
| `Common/DTO/Visit.cs` | Visit entity; VisitStatus enum; TableEntity inheritance (dead code) | Observed; Conflicting (CE-001) |
| `Common/Services/DocumentDbService.cs` | Persistence layer; `CreateDocumentAsync` for events and visits (defect); `UpsertDocumentAsync` for devices | Observed |
| `Common/Services/FaceApiService.cs` | Face API wrapper with retry logic (up to ~17 min backoff) | Observed |
| `Common/Services/IotHubService.cs` | IoT Hub device registration | Observed |
| `Common/Services/StorageService.cs` | Blob storage operations (download, upload, delete) | Observed |
| `Common/IAppConfiguration.cs` | Configuration interface including `IotHubConnectionString` | Observed |
| `Reailizer.Job/Functions.cs` | Core business logic: queue processing, face detection/identification, event creation, visit correlation. Hardcoded `personGroupId`, `0.8` threshold, 248-face limit. Czech TODO comments. | Observed |
| `Reailizer.Job/AppConfiguration.cs` | Config implementation; missing `IotHubConnectionString` (CS0535) | Observed; Defective |
| `Backend/Controllers/DeviceController.cs` | Device registration API; registers in IoT Hub only, never saves to DocumentDB | Observed; Defective |
| `Backend/Startup.cs` | DI setup; no authentication middleware | Observed |
| `Retailizer.UWP/StartupTask.cs` | Edge app entry point; unthrottled capture loop; fire-and-forget processing | Observed |
| `Retailizer.UWP/LocalFaceDetector.cs` | Local face detection and cropping with 25% padding | Observed |
| `Retailizer.UWP/AzureImagePersister.cs` | Blob upload and IoT Hub messaging | Observed |
| `Retailizer.UWP/DeviceConfiguration.cs` | Device identity from hardware GUID; auto-registration | Observed |
| `Retailizer.UWP/Configuration.cs` | Static configuration with placeholder values | Observed |
| `Retailizer.Azure/Templates/template.json` | ARM template; deploys Face API + Emotion API (unused), IoT Hub, DocumentDB, Service Bus, Storage, App Service. No Stream Analytics. | Observed; Conflicting (CE-002, CE-003) |
| `Retailizer.Azure/Templates/parameters.json` | `nameRoot: "retailizerdev"`, `eventsQueueName: "event_queue"` (conflicts with code) | Observed; Conflicting (CE-004) |
| `Retailizer.Azure.Initializer/Program.cs` | Creates DocumentDB collections, Face API person group, blob container; seeds with `satya.jpg` | Observed |

### Critical defects identified

| Defect | File | Line(s) | Impact |
| --- | --- | --- | --- |
| Visit update uses `CreateDocumentAsync` instead of upsert/replace | `Common/Services/DocumentDbService.cs` | 80 | Visit completion (payment/leave) fails at runtime |
| Device metadata never saved to DocumentDB after registration | `Backend/Controllers/DeviceController.cs` | 21–33 | Event processing crashes with NullReferenceException on device lookup |
| Missing `IotHubConnectionString` implementation | `Reailizer.Job/AppConfiguration.cs` | 6–24 | Compiler error CS0535; project cannot be compiled |
| Unthrottled capture loop | `Retailizer.UWP/StartupTask.cs` | 124–150 | CPU/memory exhaustion risk on IoT hardware |
| Double event persistence | `Reailizer.Job/Functions.cs` | 162, 190, 214 | Duplicate event documents in database |

### Source code comments and TODOs

| Location | Text | Language | Significance |
| --- | --- | --- | --- |
| `Reailizer.Job/Functions.cs:54` | `//TODO: co když camera nebude` | Czech ("What if camera doesn't exist") | Known gap: no null-check after camera lookup |
| `Reailizer.Job/Functions.cs:77` | `//TODO: zapsat event, bez fotky a dalších dat` | Czech ("Write event, without photo and other data") | Planned: record events when no face detected |
| `Retailizer.UWP/LocalFaceDetector.cs:66` | `//ToDo Logging` | English | Missing operational logging |
| `Reailizer.Job/Functions.cs:80, 168` | `//await storageService.DeleteBlockBlob(...)` | Commented-out code | Blob cleanup considered but disabled |

### Naming anomalies

| Item | Issue |
| --- | --- |
| `Reailizer.Job` (project folder and name) | Misspelling of "Retailizer" (missing 't') |
| `IImagePersiter.cs` (interface file) | Misspelling of "Persister" (missing 's') |
