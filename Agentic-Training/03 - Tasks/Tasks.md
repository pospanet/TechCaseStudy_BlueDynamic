\# Doporučené pořadí agentic workflow



| Fáze dema | Analýza | Implementace | Review změn |

|---|---|---|---|

| Security audit | Claude Opus 4.6 | GPT-5.1-codex | Claude Sonnet 4.6 |

| Detekce antipatternů | Claude Opus 4.6 | Claude Sonnet 4.6 | GPT-5.4 |

| Výkonnostní optimalizace | Claude Opus 4.6 | GPT-5.1-codex | Claude Sonnet 4.6 |

| Upgrade .NET Frameworku | Claude Opus 4.6 | GPT-5.1-codex | Claude Opus 4.6 |

| Závěrečný integrační review | GPT-5.4 | GPT-5.1-codex | Claude Sonnet 4.6 |



\## Použité skills



\- `explore-repository` — analýza repozitáře, architektury, bezpečnosti, antipatternů a výkonu

\- `implement-task` — implementace vybraných změn

\- `review-change` — nezávislý review provedených změn

\- `validate-project` — bezpečné spuštění dostupných validačních kroků



\## Doporučení pro použití



Každou hlavní fázi spusť v novém chatu. Analýzu, implementaci a review změn drž odděleně, aby implementační agent nebyl příliš ovlivněn vlastními předchozími závěry a reviewer zůstal nezávislý.



Závěrečný integrační review proveď až po dokončení všech předchozích oblastí. Ověří vzájemné dopady bezpečnostních oprav, refaktoringu, výkonnostních změn a upgradu .NET Frameworku.

