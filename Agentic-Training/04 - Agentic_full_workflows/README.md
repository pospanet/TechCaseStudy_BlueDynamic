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
