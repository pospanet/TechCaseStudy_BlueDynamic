# Retailizer reviewed technical description

## 1. Document purpose and status

This document reconstructs the technical design of the historical Retailizer project from static repository evidence. It describes the as-is system found in the workspace, not a modernization target or recommended replacement architecture.

The reconstruction is based on source code, project files, configuration files, infrastructure templates, sample assets, and existing repository documentation. No Git history, build, restore, runtime execution, deployment, test execution, generated script execution, or live external-service call was used.

The final-stage input expected an independent review file at `docs/technical-review/technical-analysis-review.md`. That file was not present in the inspected workspace. Therefore, direct disposition of individual review findings is Unknown. The final documentation still applies the review criteria by re-checking the material conclusions against project files and preserving verification-dependent issues explicitly.

Evidence classifications used here are Observed, Documented, Inferred, Unknown, Conflicting evidence, Build verification required, Runtime verification required, External-service verification required, and Stakeholder confirmation required.

## 2. Technical executive summary

Retailizer is a historical proof-of-concept retail analytics system that uses camera-equipped Windows IoT devices to detect faces, upload cropped face images, send device messages, process those messages in Azure, identify or create face identities, and persist events and visits for analytics.

Principal device-side components are a UWP IoT startup background task, local face detector, Blob Storage uploader, IoT Hub message sender, and backend registration client. Principal cloud-side components are an ASP.NET Core registration API, an Azure WebJob queue processor, DocumentDB persistence wrappers, Blob Storage access, IoT Hub registry management, Face API integration, and ARM-provisioned Azure resources.

The principal runtime flow is: camera capture, local face detection, crop generation, Blob Storage upload, IoT Hub device-to-cloud message, an uncertain IoT Hub-to-Service Bus bridge, Service Bus-triggered WebJob processing, Face API detection and identification, event creation, visit correlation, DocumentDB persistence, and documented Power BI consumption.

The persistence and analytics model centers on `Device`, `DeviceCamera`, `Event`, `Visit`, and embedded `VisitPerson` data. DocumentDB collections for devices, events, and visits are used by code and created by the initializer. Power BI appears in documentation and images, but its dataset connection is Unknown.

The implementation appears proof-of-concept in maturity: minimal validation, sparse observability, broad shared-secret configuration, incomplete deployment wiring, TODOs in processing paths, no static evidence of registration authentication, no observed idempotency, and concurrency/backpressure risks.

Most important unknowns are the IoT Hub-to-Service Bus route, deployed queue name, external protection for device registration, actual WebJob retry/poison behavior, Face API historical behavior, buildability of historical project formats and packages, and whether runtime infrastructure was completed outside the repository.

## 3. Historical technical context

Retailizer uses a platform generation consistent with mid-2010s Microsoft cloud and device patterns. Existing documentation describes Windows 10 IoT Core on DragonBoard hardware, Azure IoT Hub, Stream Analytics, WebJobs, Service Bus, DocumentDB, Cognitive Services / Project Oxford Face APIs, and Power BI.

Historically reasonable choices include UWP for Windows IoT device applications, IoT Hub for device messaging and device identities, WebJobs for background cloud processing, DocumentDB for JSON-document persistence, Blob Storage for image payloads, and Power BI for demonstration analytics. The use of Project Oxford Face SDK and ARM templates also matches the period reflected by package and template versions.

Implementation-specific limitations include an unbounded capture loop, fire-and-forget image processing, missing cancellation/backpressure, minimal error handling, sparse logging, duplicate event-save behavior, uncertain visit update semantics, and incomplete source-level security controls.

Later platform obsolescence concerns include `xproj/project.json`, ASP.NET Core 1.0, .NET Framework 4.5.2, UWP IoT dependencies, Project Oxford Face SDK/API naming, and DocumentDB branding/API behavior. These are modern availability concerns, not evidence that the historical system failed at the time.

No exact development date, production deployment status, or operational use is asserted beyond repository documentation and project artifacts.

## 4. Repository and solution structure

The solution contains six material projects plus documentation and assets.

| Project or artifact | Type | Target or platform | Responsibility | Relationships | Evidence-backed status |
| --- | --- | --- | --- | --- | --- |
| `Retailizer.UWP` | UWP IoT application | UAP 10.0; ARM/x86/x64 package platforms | Device camera capture, local face detection, cropped-image upload, IoT Hub messaging, backend registration | Uses `Common` contracts indirectly through shared DTO expectations; calls Backend, Storage, IoT Hub | Observed source and manifest |
| `Backend` | ASP.NET Core web API | `net452`, ASP.NET Core 1.0.x, Kestrel/IIS | Device registration endpoint | Uses `Common` services and IoT Hub registry | Observed source |
| `Reailizer.Job` | Console/WebJob project | .NET Framework 4.5.2 | Service Bus-triggered queue processing, Face API calls, DocumentDB persistence, visit correlation | Uses `Common`; consumes Service Bus; reads Blob; writes DocumentDB | Observed source; directory name misspells Retailizer |
| `Common` | Class library | .NET Framework 4.5.2 | DTOs and wrappers for DocumentDB, IoT Hub, Face API, Blob Storage | Shared by Backend, WebJob, Initializer, and partly package-referenced by UWP solution | Observed source |
| `Retailizer.Azure.Initializer` | Console utility | .NET Framework 4.5.2 | Creates DocumentDB database/collections, Face person group/person/face, and Blob container | Uses `Common` and Azure SDKs | Observed source; execution not performed |
| `Retailizer.Azure` | Azure deployment project | ARM template and PowerShell script | Provisions Azure managed resources and outputs keys/connection strings | Supports Backend, WebJob, Storage, IoT Hub, DocumentDB, Face/Emotion APIs | Observed template; deployment not verified |
| `README.md` | Historical documentation | Markdown | Business and architecture narrative | Describes flow and cloud components | Documented evidence |
| `images/` | Static image assets | PNG/JPG | Historical diagrams/screenshots and initializer seed face | Supports documentation/demo and Face seed asset | Observed assets |
| `docs/business*` | Existing business documentation | Markdown | Business-level analysis | Not modified by this mission | Observed but outside final technical scope |

## 5. Technology and dependency overview

| Technology or dependency | Declared version where available | Observed use | Role | Current concern | Verification |
| --- | --- | --- | --- | --- | --- |
| C# | n/a | All application projects | Implementation language | Historical language/runtime pairing | Build verification required |
| Visual Studio solution | VS 14 format | `Retailizer.sln` | Project orchestration | Historical tooling | Build verification required |
| ASP.NET Core MVC/Kestrel/IISIntegration | 1.0.x | Backend | HTTP registration API | `xproj/project.json` era obsolete | Build/runtime verification required |
| .NET Framework | 4.5.2 | Common, WebJob, Initializer | Library and console runtimes | Legacy framework availability | Build verification required |
| UWP / Windows 10 IoT | UAP 10.0.14393 target, 10.0.10586 min | Device app | Camera and local FaceAnalysis APIs | Requires compatible SDK/device | Build/runtime verification required |
| Microsoft.Azure.Devices | 1.0.15 / 1.1.0-preview | Backend/Common | IoT Hub registry | Version inconsistency | Build/external-service verification required |
| Microsoft.Azure.Devices.Client | 1.0.19 | UWP | Device-to-cloud messaging | SDK defaults unknown | Runtime verification required |
| Microsoft.Azure.DocumentDB | 1.9.5 / 1.10.0 | Common/Job/Initializer/Backend dependency | Document queries and creates | Historical DocumentDB behavior | Runtime/external-service verification required |
| WindowsAzure.Storage | 7.0.0 / 7.2.1 | UWP/Common/Job/Initializer | Blob upload/download and `TableEntity` base type | Version inconsistency | Build/runtime verification required |
| Azure WebJobs SDK + ServiceBus | 1.1.2 | WebJob | Service Bus queue trigger | Framework defaults not configured in source | Runtime verification required |
| Microsoft.ProjectOxford.Face | 1.2.1.1 | Common/Job/Initializer | Face detect/identify/person/train APIs | Historical/retired service surface | External-service verification required |
| Newtonsoft.Json | 9.0.1 | Multiple | JSON DTO/message handling | Observed use | Build verification required |
| Unity | 4.0.1 | UWP | Dependency injection | Observed use | Build verification required |
| Ngonzalez.ImageProcessorCore | 0.0.1 | UWP | Image crop/JPEG encode | Package availability unknown | Build verification required |
| ARM templates | 2015/2016 API versions | Deployment project | Azure resource provisioning | Deployment behavior not executed | Deployment verification required |
| Power BI | n/a | README/images only | Reporting/visualization | Dataset wiring unknown | Stakeholder/runtime verification required |

## 6. Component architecture

See `component-catalog.md` for the structured component catalog.

Major components are the UWP startup capture task, local face detector, Azure image persister, device registration client, backend registration API, WebJob host, event queue processor, DocumentDB service, IoT Hub registry service, Face API service, Blob Storage service, Azure initializer, ARM deployment template, and documented Power BI reporting artifact.

Trust boundaries include device hardware to UWP code, device to backend registration API, device to Storage using a storage connection string, device to IoT Hub using a symmetric key, backend to IoT Hub management API, WebJob to Service Bus/Storage/DocumentDB/Face API, and external analytics/reporting access.

Principal failure modes are loss of frames, orphan blobs, lost queue messages from missing routing, duplicate events, visit update ambiguity, external API throttling, malformed message failures, missing configuration, and weak operational visibility.

## 7. Executable entry points and runtime processes

### UWP or Windows IoT background application

`Retailizer.UWP.StartupTask.Run` is declared in the UWP manifest as a startup background task. It obtains a background task deferral, initializes a Unity container, creates a local `FaceDetector` when supported, initializes cameras, and starts continuous capture.

Configuration inputs include static device-side configuration values and backend-derived device credentials. Long-running behavior is an infinite `while(true)` loop across camera devices. No cancellation, deferral completion, throttling delay, or controlled shutdown is observed. Exceptions in startup are caught and written to `Debug`; per-camera capture exceptions are logged and skipped. Runtime verification is required for actual device lifecycle behavior.

### Backend web API

`Backend.Program.Main` starts Kestrel with IIS integration and uses `Backend.Startup`. `Startup` loads JSON configuration, environment variables, and user secrets in development, registers MVC, `IotHubService`, `DocumentDbService`, and `IAppConfiguration`, then calls `app.UseMvc()`.

The observed API surface is device registration through `DeviceController`. No custom exception middleware, authentication middleware, or authorization attributes were found in the inspected workspace. Runtime and deployment confirmation are required for external protection.

### WebJob or queue-triggered processor

`Reailizer.Job.Program.Main` creates a WebJobs `JobHostConfiguration`, disables the dashboard connection string, calls `UseServiceBus()`, and blocks on `host.RunAndBlock()`. `Functions.ProcessQueueMessage` is triggered by `[ServiceBusTrigger("events")]` and is declared `async void`.

Configuration inputs come from `App.config` app settings and connection strings. The processor initializes services per invocation, parses queue JSON, reads blobs, calls Face API, writes DocumentDB events/visits, and may train the Face person group. Runtime verification is required for Service Bus binding, retry, poison, concurrency, and `async void` behavior.

### Initialization utility

`Retailizer.Azure.Initializer.Program.Main` is a console utility that creates a DocumentDB database and collections, creates a Face person group/person, adds a seed face from `satya.jpg`, trains the group, and creates the `faces` blob container. It is an operator-run utility; it is not wired into ARM deployment in source. External-service and runtime verification are required for re-runnability.

### Deployment or provisioning entry point

`Retailizer.Azure/Templates/template.json` and `deploy.ps1` define ARM deployment artifacts. The template provisions managed resources and outputs connection strings/keys. It does not deploy the application binaries, configure backend app settings, deploy the WebJob, create DocumentDB collections, create the Blob container, create the Face person group, create Stream Analytics, or configure the IoT Hub-to-Service Bus bridge.

## 8. System context

Retailizer’s context includes physical cameras attached to a Windows IoT device, device-side UWP software, backend registration services, Azure managed messaging and storage services, external Face API services, DocumentDB persistence, and documented Power BI reporting consumers. Emotion API is provisioned/configured but not used by observed source. Stream Analytics is documented/inferred as a likely route component but not found in source or ARM template.

See `diagrams/system-context.mmd`.

## 9. End-to-end runtime flow

1. Camera enumeration and initialization: Observed in `StartupTask.InitializeCameraAsync` using `DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)` and `MediaCapture.InitializeAsync`.
2. Image capture: Observed in `GetPhotoStreamAsync` using `CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg())`.
3. Local face detection: Observed in `LocalFaceDetector.ProcessImageAsync` using Windows `FaceDetector`.
4. Crop generation: Observed using enlarged face rectangles, image cropping, and JPEG encoding.
5. Blob Storage upload: Observed in `AzureImagePersister.UploadImageToBlobAsync` to `{deviceId}/{guid}.jpg`.
6. Message creation: Observed JSON payload `{ deviceId, blobName, cameraId }`.
7. IoT Hub submission: Observed through `DeviceClient.SendEventAsync`.
8. Stream Analytics or routing: Documented/Inferred but not implemented in inspected source/templates. The WebJob expects a Service Bus message with an `IoTHub.ConnectionDeviceId` envelope, but the producer of that envelope is Unknown.
9. Service Bus delivery: Observed only at consumer side through `[ServiceBusTrigger("events")]`; queue name conflicts with `parameters.json` value `event_queue`.
10. Queue-triggered processing: Observed in `Functions.ProcessQueueMessage`.
11. Image retrieval: Observed through `StorageService.DownloadBlockBlob`.
12. Face analysis: Observed through `FaceApiService.DetectAsync` requesting age, gender, and smile.
13. Person identification or creation: Observed through `IdentifyAsync`, `GetPersonAsync`, `CreatePersonAsync`, `AddPersonFaceAsync`, and `TrainPersonGroupAsync`.
14. Event generation: Observed construction of `Event` from device, camera, Face API, and timestamp data.
15. Visit correlation: Observed in `ProcessEventAsync` and `FindOpenVisitForEventAsync`.
16. Persistence: Observed DocumentDB event/visit writes through `DocumentDbService`.
17. Analytics consumption: Documented only through README/images; data-source mechanism is Unknown.

The flow is not statically complete because the IoT Hub-to-Service Bus bridge is missing from source and infrastructure definitions.

## 10. Device-registration flow

The UWP device obtains an identifier from `EasClientDeviceInformation.Id`, serializes `{ id = deviceId }`, and posts it to `configuration.BackendUrl + "/api/device"`. The backend `DeviceController.Register([FromBody] Device device)` calls `_iotHubService.RegisterDevice(device)` and returns JSON containing `deviceKey` and `deviceId`.

`IotHubService.RegisterDevice` attempts `RegistryManager.AddDeviceAsync(new Device(device.DeviceId))`. If IoT Hub reports `DeviceAlreadyExistsException`, it calls `GetDeviceAsync(device.DeviceId)` and returns the existing identity. Other exceptions are not locally handled.

The returned device key is cached in the UWP `DeviceConfiguration` instance field `_deviceKey`. No durable local credential persistence is observed. The key is later used to create `DeviceClient` with `DeviceAuthenticationWithRegistrySymmetricKey`.

No authentication or authorization control was found in the inspected workspace for this registration endpoint. This does not prove public exposure; external gateway, App Service, network, or operational controls may have existed outside the repository. Deployment and stakeholder confirmation are required.

See `diagrams/device-registration-sequence.mmd`.

## 11. Image capture and local processing

The device enumerates all video capture devices, initializes a `MediaCapture` per camera, selects the preview stream property with the greatest width, and enters an infinite capture loop. Each camera capture produces a JPEG stream and `BitmapDecoder`.

The call to `filter?.ProcessImageAsync(bitmapDecoder, imageStream, cameraId)` is intentionally unawaited, with compiler warning 4014 suppressed. This is evidence of fire-and-forget local processing. Because the loop has no delay, cancellation token, queue, semaphore, or backpressure mechanism, concurrent work can accumulate under load.

`LocalFaceDetector` converts the frame to a grayscale `SoftwareBitmap`, calls `DetectFacesAsync`, enlarges each face box, crops each face, encodes it to JPEG, and passes streams to `IImagePersiter.PersistAsync`. It catches all exceptions and writes to `Debug`.

`AzureImagePersister` uploads each face image before sending an IoT Hub message. It disposes the face stream after successful upload and send. If upload or send throws before disposal, the observed code does not dispose that stream in a `finally` block. Capture streams created in `StartupTask` are not visibly disposed after fire-and-forget dispatch.

See `diagrams/image-processing-sequence.mmd`.

## 12. Messaging and integration contracts

See `integration-catalog.md`.

The device-to-cloud message contract is implicit JSON: `deviceId`, `blobName`, and `cameraId`. It is not versioned and has no explicit schema file. The WebJob inbound contract expects the same fields plus an `IoTHub.ConnectionDeviceId` object path, implying an upstream route or transformation not found in source.

Contracts are DTO-based and implicit rather than versioned. Framework-triggered integrations include the WebJobs `ServiceBusTrigger`. Delivery assumptions, retries, duplicate handling, and dead-letter behavior are not configured in source and require runtime verification. Tenant and store values are not sent in the device message; they are loaded from the `Device` document during WebJob processing.

## 13. Event-processing flow

`ProcessQueueMessage` logs the raw message, parses it as `JObject`, extracts `deviceId`, `IoTHub.ConnectionDeviceId`, `blobName`, and lower-cased `cameraId`, loads the `Device`, maps the camera ID to `EventType`, downloads the blob, detects faces, identifies the person, possibly creates a new Face API person, adds the face to that person, trains the person group, creates an `Event`, saves the event, and calls `ProcessEventAsync`.

If no faces are detected, the function returns without saving an event. A TODO notes writing an event without face data; blob deletion is commented out.

No local try/catch surrounds the processing flow. Malformed JSON, missing properties, missing device, missing camera mapping, blob failure, Face API failure, or DocumentDB failure can escape to the WebJobs host. Actual retry and poison-message behavior are Runtime verification required.

See `diagrams/event-processing-sequence.mmd`.

## 14. Visit-correlation behavior

`ProcessEventAsync` returns immediately if `PersonId` is empty. For `enter`, it creates a new `Visit` from the event, sets `_event.VisitId`, saves the visit, and saves the event again. For all other event types, it calls `FindOpenVisitForEventAsync`; if no visit is found, it creates and saves a new visit; if a visit is found, it updates `PaymentOn` for `payment`, or `LeaveOn` and `VisitStatus = Left` for `leave`, then saves the visit. It then sets `_event.VisitId` and saves the event.

Active-visit lookup filters by `TenantId`, `StoreId`, `PersonId`, `VisitStatus != Left`, and `VisitStatus != None`, ordered by `EnterOn DESC`, then selects the first result. `enter` is explicitly excluded from open-visit lookup.

No ordering guarantee, idempotency key, duplicate-message detection, optimistic concurrency, ETag handling, lock, transaction, or atomic multi-document update is observed. Duplicate messages can lead to repeated event saves. Concurrent messages for the same tenant/store/person can race between query and create/update. The use of `CreateDocumentAsync` for both new and existing visits creates ambiguity: actual update-vs-duplicate-vs-conflict behavior requires runtime verification.

## 15. Data and message model

See `data-model.md`.

Major entities/contracts are `Device`, `DeviceCamera`, `Event`, `VisitPerson`, `Visit`, `VisitStatus`, device-to-cloud JSON message, WebJob inbound JSON contract, registration request, and registration response.

DTO fields carry tenant/store/person identifiers, event type, timestamps, face attributes, suggested person, confidence, and visit status. Static analysis does not establish domain guarantees such as uniqueness, valid event ordering, complete tenant isolation, or data retention. Validation is sparse or absent in observed code.

## 16. Persistence model

`DocumentDbService` locates a configured database and collections for devices, events, and visits. It queries devices by `id`, queries visits by SQL strings, creates event documents, creates visit documents, and upserts device documents.

The initializer creates a database and collections named from configuration keys. Collection creation uses only `Id`; no partition key, unique key policy, throughput, retention, consistency, or indexing policy is visible in source.

Provisioned by ARM: DocumentDB account. Created by initializer: database and collections. Demonstrably used by code: `devices`, `events`, and `visits` collections.

Possible cross-tenant risk exists if device documents are wrong or if query logic is incomplete; visit lookup filters tenant and store, but device lookup uses only `id`. Multi-document operations are non-atomic. No deletion/retention behavior is implemented for events/visits. Blob deletion is commented out.

Modern Cosmos DB comparisons are only current-context terminology; the project itself uses DocumentDB-era SDK and naming.

## 17. Infrastructure and deployment topology

See `diagrams/deployment-view.mmd`.

| Resource | Evidence-backed classification |
| --- | --- |
| Cognitive Services Face account | Provisioned by ARM; referenced by code/config; externally verified behavior required |
| Cognitive Services Emotion account | Provisioned by ARM/configured; no observed code use; apparently unused or incomplete |
| IoT Hub | Provisioned by ARM; used by backend registry and UWP device client |
| IoT Hub storage endpoint for `faces` | Provisioned in ARM, but file-upload notifications disabled; not the observed image upload path |
| DocumentDB account | Provisioned by ARM; database/collections created by initializer; used by code |
| Service Bus namespace and queue | Provisioned by ARM; WebJob consumes queue `events`; parameter file conflicts with `event_queue` |
| Storage account | Provisioned by ARM; used for cropped face blobs |
| App Service plan and Web App | Provisioned by ARM; backend deployment settings not in template |
| WebJob deployment | Required by architecture; not provisioned in ARM template |
| Stream Analytics | Documented in README; not provisioned or implemented in inspected source/templates |
| Power BI | Documented/images only; runtime dataset wiring Unknown |

Provisioning a resource is not treated as evidence of a working runtime connection unless source/config also shows the connection.

## 18. Configuration and secret model

Configuration keys include `DocumentDb:Endpoint`, `DocumentDb:Key`, `DocumentDb:Database`, collection names, `IotHub:ConnectionString`, `FaceApiSubscriptionKey`, `EmotionApiSubscriptionKey`, `AzureWebJobsStorage`, `AzureWebJobsServiceBus`, `StorageConnectionString`, `FaceApiKey`, `BackendUrl`, `IotHubUrl`, and storage container/account settings. Secret values are not copied here.

Backend configuration is loaded from appsettings, environment-specific JSON, environment variables, and user secrets in development. WebJob and initializer use `App.config`. UWP has static configuration patterns and obtains the IoT Hub device key from backend registration.

Risks and uncertainties include broad storage account credentials on the device if real values were embedded, IoT Hub owner/registry credentials in backend configuration, RootManageSharedAccessKey-style Service Bus output, database account keys, no observed rotation flow, no validation of required keys, and unclear deployed privilege scope.

## 19. Error handling and failure behavior

See `failure-modes.md`.

Device startup and per-camera capture failures are caught and written to `Debug`; processing continues where possible. Local face detection catches all exceptions and logs to `Debug`. Blob upload and IoT Hub send exceptions are not caught in `AzureImagePersister`, and because caller processing is fire-and-forget, operational visibility is weak.

Face API quota errors are retried by `FaceApiService` with exponential backoff up to ten attempts. Non-quota Face API failures are not custom-retried. WebJob processing lacks local try/catch; host behavior is Runtime verification required. No poison/dead-letter handling is configured in source. Persistence operations can leave partial state: blob without message, event without visit, duplicate event save, visit ambiguity, and Face API person state changes before DocumentDB failure.

## 20. Concurrency, retries, idempotence, and backpressure

Asynchronous structure is high risk. `StartupTask.Run` is `async void`, `ProcessQueueMessage` is `async void`, and device image processing is deliberately unawaited. The capture loop has no delay, cancellation, bounded queue, backpressure, or concurrency limiter.

Observed retry behavior is limited to Face API quota errors. SDK default retries for Storage, IoT Hub, DocumentDB, Service Bus, and WebJobs are not treated as configured facts. Queue-trigger concurrency and poison handling require runtime verification.

No message IDs, idempotency keys, deduplication records, optimistic concurrency, or correlation IDs are observed. Blob names are GUIDs, reducing collision risk but not providing business idempotency. Visit read-modify-write logic is non-atomic and race-prone under concurrent messages.

## 21. Observability and operations

Observed visibility consists of `Debug.WriteLine`, initializer console/debug output, `TextWriter log.WriteLine(message)` in the WebJob, ASP.NET Core console/debug logging registration, and disabled WebJob dashboard connection string. No structured telemetry, correlation IDs, health checks, metrics, traces, alert rules, queue monitoring configuration, poison-message handling, or operational dashboard implementation is found.

Power BI is documented as a visualization/reporting output, not as an operational monitoring implementation. Platform diagnostics may have existed outside the repository; deployment confirmation is required.

## 22. Security and trust boundaries

Security-relevant boundaries include unregistered device to backend registration API, device to Storage, device to IoT Hub, backend to IoT Hub registry, WebJob to Service Bus/Storage/DocumentDB/Face API, and reporting access to persisted data. The system handles face images and derived biometric/demographic data, so external API and storage boundaries are material.

Controls found include IoT Hub symmetric device keys after registration, shared-secret SDK authentication for Azure services, and Service Bus queue configuration with `isAnonymousAccessible: false`. Controls not found include source-level authentication/authorization for `/api/device`, input validation of registration payload, tenant authorization middleware, least-privilege credential scoping, secret rotation, SAS-based device blob upload, or explicit data-protection handling.

No authentication or authorization control was found in the inspected workspace. External protection may have existed outside the repository and requires deployment/stakeholder confirmation.

## 23. Build and execution assumptions

Declared or inferred assumptions include Windows/Visual Studio 2015-era tooling, .NET Framework 4.5.2, ASP.NET Core 1.0 `xproj/project.json`, UWP SDK 10.0.14393, Windows IoT hardware with compatible cameras, WebJobs SDK 1.x, historical NuGet packages, Azure resources matching configuration, existing DocumentDB database/collections, existing Blob container, Face API account/person group, and configured secrets.

Build verification is required for solution load, package restore, dependency version conflicts, UWP compilation, backend `xproj`, WebJob references, and historical package availability. Runtime verification is required for camera behavior, WebJob trigger binding, configuration loading, DocumentDB create/update behavior, and process lifecycles. External-service verification is required for Face API, IoT Hub, DocumentDB, and Azure platform behavior.

## 24. Incomplete, unused, and contradictory elements

Evidence-backed items:

- `Reailizer.Job` directory is misspelled while namespace/project identity uses `Retailizer.Job`.
- `eventsQueueName` default in template is `events`, WebJob trigger uses `events`, but `parameters.json` sets `event_queue`.
- Emotion API resource and keys are provisioned/configured, but no observed code path calls Emotion API.
- README references Stream Analytics, but no Stream Analytics resource/query/job is present in source/templates.
- `deviceName = mess["IoTHub"]["ConnectionDeviceId"]` is read but unused.
- WebJob TODOs mention missing camera handling and event-without-face behavior; blob deletion is commented.
- Backend registers `DocumentDbService`, but observed registration endpoint does not persist `Device` documents.
- `GetAllVisitsByPersonIdAsync` appears to build a SQL string without quoting `personId`; effect requires runtime verification.
- `Visit` inherits Azure Table `TableEntity`, but observed persistence writes visits to DocumentDB.
- Power BI is documented/images-only from source perspective.

Unused claims are qualified because framework conventions, deployment packaging, or omitted artifacts could exist outside the workspace.

## 25. Known technical limitations

Known limitations include static-only verification, obsolete toolchain and service dependencies, incomplete runtime wiring evidence, unclear registration protection, broad shared-secret handling, minimal validation, weak observability, unbounded device processing, fire-and-forget async behavior, limited retries, no observed idempotency, persistence consistency risks, incomplete deployment linkage, and undocumented operational assumptions.

## 26. Conflicting evidence

| Conflict | Artifacts | Possible interpretations | Impact | Safest conclusion | Verification |
| --- | --- | --- | --- | --- | --- |
| IoT Hub to Service Bus bridge | README, ARM IoT Hub/Service Bus, WebJob trigger | Stream Analytics/job existed outside repo; omitted work; stale docs | End-to-end runtime cannot be proven | Bridge is Documented/Inferred but Unknown in implementation | Deployment/stakeholder verification |
| Queue name | ARM template default `events`, `parameters.json` `event_queue`, WebJob `events` | Parameter override mismatch; manually configured queue; stale parameter file | WebJob may listen to wrong queue | Conflicting evidence | Deployment/runtime verification |
| Emotion API | ARM/config keys vs no source use | Planned feature; omitted code; abandoned resource | Analytics claims may be overstated | Provisioned/configured but apparently unused | Stakeholder confirmation |
| Visit storage model | `Visit : TableEntity` vs DocumentDB saves | Abandoned Table Storage design; DTO reuse | Persistence semantics ambiguous | Conflicting evidence | Stakeholder/runtime verification |
| Event save count | `SaveAsync(newEvent)` before and after visit correlation | Intentional audit; accidental duplicate; DocumentDB behavior dependent | Duplicate event documents possible | Duplicate risk, not confirmed intended behavior | Runtime/stakeholder verification |

## 27. Verification backlog

See `verification-backlog.md`.

Highest-priority groups are build/toolchain reconstruction, runtime device/WebJob behavior, concurrency/duplicate-processing tests, external Face API behavior, deployment/routing confirmation, and stakeholder confirmation of intended demo vs production assumptions.

## 28. Evidence and traceability

| Conclusion area | Repository-relative evidence | Symbols/resources | Classification | Verification requirement |
| --- | --- | --- | --- | --- |
| Solution structure | `Retailizer.sln` | Project entries | Observed | Build verification |
| UWP startup task | `Retailizer.UWP/Package.appxmanifest`, `StartupTask.cs` | `Retailizer.UWP.StartupTask`, `iot:Task` | Observed | Runtime verification |
| Local face crop | `Retailizer.UWP/LocalFaceDetector.cs` | `DetectFacesAsync`, crop/JPEG logic | Observed | Runtime verification |
| Blob upload and IoT message | `Retailizer.UWP/AzureImagePersister.cs` | `UploadFromStreamAsync`, `SendEventAsync` | Observed | Runtime verification |
| Device registration | `Retailizer.UWP/DeviceConfiguration.cs`, `Backend/Controllers/DeviceController.cs`, `Common/Services/IotHubService.cs` | `/api/device`, `RegisterDevice` | Observed | Security/deployment verification |
| Backend host | `Backend/Program.cs`, `Backend/Startup.cs`, `Backend/project.json` | Kestrel, IISIntegration, MVC | Observed | Build/runtime verification |
| WebJob processor | `Reailizer.Job/Program.cs`, `Reailizer.Job/Functions.cs` | `UseServiceBus`, `ServiceBusTrigger("events")` | Observed | Runtime verification |
| Missing route | `README.md`, `Retailizer.Azure/Templates/template.json`, `Functions.cs` | IoT Hub, Service Bus, Stream Analytics mention | Conflicting evidence/Inferred | Deployment/stakeholder verification |
| Face API retry | `Common/Services/FaceApiService.cs` | quota retry wrapper | Observed | External-service verification |
| Event/visit persistence | `Reailizer.Job/Functions.cs`, `Common/DTO/Event.cs`, `Common/DTO/Visit.cs`, `Common/Services/DocumentDbService.cs` | `Event`, `Visit`, `SaveAsync` | Observed/Conflicting evidence | Runtime/concurrency verification |
| ARM resources | `Retailizer.Azure/Templates/template.json`, `parameters.json` | Face, Emotion, IoT Hub, DocumentDB, Service Bus, Storage, App Service | Observed/Conflicting evidence | Deployment verification |
| Power BI | `README.md`, `images/4-outputs.png` | Dashboard/output imagery | Documented | Stakeholder/runtime verification |
| Configuration/secrets | `Backend/appsettings.json`, `Reailizer.Job/App.config`, `Retailizer.UWP/Configuration.cs`, initializer config | Key names and placeholders | Observed | Deployment/security verification |
