---
name: review-change
description: Independently review a branch, diff, pull request, implementation, or documentation change for defects, regressions, security risks, missing validation, and requirement violations. Use when asked to review, audit, critique, verify, assess, or challenge completed work.
---

# Review Change

## Objective

Perform an independent, evidence-based review. Do not validate the author's conclusions by default.

## Preparation

1. Read the applicable `AGENTS.md` files.
2. Determine the review target:
   - working-tree changes,
   - current branch against its base,
   - specified commit range,
   - pull request,
   - named files or documents.
3. Identify the intended requirements and acceptance criteria.
4. Inspect the complete relevant diff.
5. Read surrounding code or documentation needed to understand the change.
6. Inspect tests and configuration affected by the change.

## Review dimensions

Evaluate:

- functional correctness,
- requirement coverage,
- regressions,
- boundary and failure cases,
- error handling,
- security and data exposure,
- concurrency and transaction behavior,
- compatibility,
- architectural consistency,
- maintainability,
- test adequacy,
- documentation accuracy,
- operational and deployment impact.

For documentation changes also evaluate:

- internal consistency,
- factual support,
- separation of facts and assumptions,
- traceability to source material,
- ambiguous or unverifiable claims,
- missing decisions and ownership.

## Finding requirements

Report only concrete findings supported by evidence.

Each finding must include:

- severity,
- location,
- problem,
- failure scenario or consequence,
- recommended correction.

Severity levels:

- **Critical** — likely catastrophic impact, security compromise, data loss, or unusable result.
- **Major** — material functional failure or substantial requirement violation.
- **Moderate** — meaningful defect with limited scope or a significant maintainability risk.
- **Minor** — localized issue with low immediate impact.
- **Observation** — relevant note that is not a confirmed defect.

## Rules

- Prioritize correctness over style preferences.
- Do not invent hypothetical findings without a realistic failure path.
- Do not modify reviewed files unless explicitly requested.
- Do not treat passing tests as proof that the implementation is correct.
- Check whether tests actually exercise the claimed behavior.
- Identify unsupported claims separately from confirmed defects.
- Explicitly state when no material defect was found.
- Include residual risks and areas that could not be verified.

## Output

1. Review scope
2. Overall assessment
3. Findings ordered by severity
4. Validation performed
5. Residual risks
6. Recommended disposition:
   - accept,
   - accept with minor corrections,
   - revise,
   - reject.