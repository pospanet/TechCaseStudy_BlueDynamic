# Prompt Set 4 — .NET Framework Upgrade

## Recommended workflow

| Phase | Model | Skill | Chat |
|---|---|---|---|
| Analysis | Claude Opus 4.6 | `explore-repository` | New chat |
| Independent review | GPT-5.4 | `explore-repository` | New chat |
| Implementation | GPT-5.1-codex | `implement-task` + `validate-project` | New chat |
| Change review | Claude Opus 4.6 | `review-change` | New chat |

## A. Upgrade feasibility analysis

```text
Use @skills:explore-repository.

Perform a read-only feasibility and migration analysis for upgrading the current solution's .NET Framework and related project targets.

Treat this as a conservative modernization of a heterogeneous legacy solution, not as a rewrite to modern .NET.

Follow all applicable AGENTS.md files. Do not modify files, install packages or run destructive commands.

Inspect every project and identify:

- project type and build system
- current target framework or platform target
- SDK-style versus legacy project format
- UWP constraints
- ASP.NET or ASP.NET Core generation and hosting model
- Azure WebJob and deployment-project constraints
- package-management format
- NuGet package compatibility
- direct assembly references
- binding redirects
- app.config, web.config and runtime settings
- language-version constraints
- Azure SDK dependencies
- APIs likely to change behavior or become unavailable
- tooling and operating-system requirements

Determine:

1. the highest practical compatible target for each project
2. whether all projects can move together
3. projects that require separate treatment
4. blocking packages or project types
5. required package upgrades
6. expected source changes
7. build and runtime validation requirements
8. rollback strategy

Explicitly distinguish .NET Framework, .NET Core, modern .NET and UWP targets. Do not recommend changing platform families merely to make versions appear consistent.

Produce:

# Current Target Matrix
# Dependency Compatibility Matrix
# Project-Type Constraints
# Recommended Target State
# Required Changes by Project
# Blocking Issues and Unknowns
# Validation Plan
# Migration Sequence
# Risks and Rollback

End with a bounded first upgrade slice suitable for implementation in this repository.
```

## B. Independent upgrade-plan review

```text
Use @skills:explore-repository.

Independently review the repository for .NET Framework upgrade feasibility. Do not modify files.

Challenge the most likely migration proposal. Verify project targets, package compatibility, Azure SDK constraints, UWP compatibility, binding redirects, deployment scripts and configuration behavior directly from repository evidence.

Focus on:

- target frameworks that cannot actually coexist
- packages whose latest compatible version differs by project
- hidden transitive assembly conflicts
- old project.json or xproj tooling constraints
- WebJob and Azure deployment compatibility
- runtime configuration changes
- APIs with behavioral changes
- projects that should remain unchanged
- validation that cannot be performed in the current environment

Return:

1. confirmed feasible target state
2. disputed recommendations
3. blocking dependencies
4. likely migration failure modes
5. corrected migration sequence
6. acceptance criteria for implementation

Do not modify the repository.
```

## C. Implement the framework upgrade

```text
Use @skills:implement-task.

Upgrade the current solution to the highest practical and mutually compatible .NET Framework or platform targets supported by each project type and its required dependencies.

This is a conservative framework modernization. Do not rewrite the application to modern .NET, replace UWP, redesign the architecture or perform unrelated refactoring.

Before editing:

1. inspect every project target and dependency
2. select the target state supported by repository evidence
3. identify projects that must remain on a different platform family
4. list packages and configuration files requiring change
5. present a concise migration and validation plan

Implementation rules:

- make the smallest coherent set of changes
- update target frameworks consistently where valid
- update packages only when required for compatibility or security
- preserve package-management style unless conversion is necessary
- update binding redirects and runtime configuration correctly
- preserve application behavior and public contracts
- do not silently remove unsupported functionality
- do not expose secrets from configuration files
- document projects that cannot be upgraded and the exact blocker
- avoid unrelated cleanup

After editing, use @skills:validate-project.

Attempt all safe and available restore, build and test commands defined by the repository. Distinguish failures caused by the change from missing legacy tooling, unavailable SDKs, unsupported operating-system components and pre-existing repository defects.

Report:

- final target matrix
- packages changed
- source and configuration changes
- validation commands and results
- unresolved compatibility risks
- required manual runtime tests
- rollback considerations
```

## D. Review the framework upgrade

```text
Use @skills:review-change.

Independently review the current uncommitted .NET Framework and project-target upgrade.

Inspect the complete diff and relevant surrounding configuration. Focus on:

- incorrect or inconsistent target frameworks
- unsupported project and package combinations
- missing package upgrades
- assembly-version conflicts and binding redirects
- runtime and web.config mistakes
- changed serialization, TLS or networking behavior
- Azure WebJob, deployment and UWP compatibility
- APIs that compile but change runtime behavior
- incomplete migration steps
- misleading validation claims
- unrelated changes

Do not modify files.

For every finding provide severity, exact location, failure scenario, impact and recommended correction. Conclude with accept, accept with minor corrections, revise or reject.
```
