# Vision Dokumentu: Mail Management

## Proč

Hlavním důvodem pro potřebu nového informačního systému je zefektivnění a zpřehlednění procesu evidence a předávání příchozích zásilek v kancelářském komplexu. Současné metody jsou neefektivní a mohou vést ke ztrátám zásilek či zmatkům při jejich předávání zaměstnancům.

## Co

Je potřeba vytvořit webovou aplikaci **Mail Management**, která umožní:

- **Recepčním** evidovat příchozí zásilky a přiřazovat je konkrétním zaměstnancům.
- **Zaměstnancům** přihlásit se do systému, zobrazit své nevyzvednuté i historické zásilky a zjistit jejich umístění.
- **Recepčním** označit zásilky jako vyzvednuté po jejich předání zaměstnancům.

Aplikace bude postavena na technologii ASP.NET s REST API a frontend bude vyvíjen s důrazem na mobilní zařízení pomocí Bootstrapu a JavaScriptu.

## Kdo

- **Administrátor**: Má plný přístup k systému, může spravovat uživatele a měnit jejich role.
- **Recepční**: Eviduje příchozí zásilky, přiřazuje je zaměstnancům a označuje je jako vyzvednuté.
- **Zaměstnanec**: Přihlašuje se do systému, aby zjistil informace o svých zásilkách.

## Kde

Systém bude fungovat jako intranetová webová aplikace dostupná pouze zaměstnancům kancelářského komplexu. Přístup bude možný z jakéhokoliv zařízení připojeného k interní síti.

## Kdy

Systém bude v provozu během pracovní doby, kdy dochází k přijímání a předávání zásilek. Zaměstnanci budou mít možnost kdykoliv zkontrolovat stav svých zásilek prostřednictvím webové aplikace.

## Jak

Projekt bude realizován agilním způsobem, což umožní postupné přidávání funkcionalit a rychlé reagování na případné změny požadavků. Vývoj bude probíhat bez použití externích autentizačních služeb a databázových frameworků, s důrazem na jednoduchost a efektivitu.