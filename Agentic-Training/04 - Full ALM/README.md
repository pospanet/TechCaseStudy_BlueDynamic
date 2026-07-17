# Sada agentic workflow promptů

Obsahuje pět samostatných workflow:

1. Security audit a náprava nálezů
2. Detekce a odstranění antipatternů
3. Výkonnostní optimalizace
4. Upgrade .NET Frameworku
5. Závěrečná integrace všech změn

Každé workflow používá řetězec:

Business zadání → nezávislý review → zapracování → implementační plán → nezávislý review → zapracování → atomické tasky → implementace tasků → code review → opravy → finální validace.

## Práce s chaty

- Každý review spusť v novém chatu.
- Zapracování připomínek dělej v původním autorském chatu.
- Každý rizikový nebo větší atomický task implementuj v samostatném chatu.
- Code review tasku vždy odděl od implementace.
- Páté workflow spusť až po dokončení předchozích čtyř.


## Review artefakty a automatické uklizení

Každý nezávislý review zapisuje nálezy do dočasného Markdown souboru vedle hodnoceného dokumentu. Následující krok zapracování načte review přímo ze souboru, vypořádá všechny stabilní finding IDs, aktualizuje kanonický dokument a po úspěšném ověření dočasný review soubor odstraní.

Používané dočasné názvy:

- `<oblast>-business-requirement.review.md`
- `<oblast>-implementation-plan.review.md`
- `<oblast>-<TASK-ID>-code-review.md`

Po úspěšném zapracování zůstává vždy jen výsledný business dokument, implementační plán nebo kanonický seznam tasků. Pokud zapracování či validace selže, review soubor se nesmí smazat.
