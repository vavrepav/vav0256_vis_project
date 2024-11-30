# Architektura: Mail Management

## Přehled Architektury

Architektura systému **Mail Management** je navržena s důrazem na modularitu, škálovatelnost a snadnou údržbu. Systém je rozdělen do logických a fyzických vrstev, které spolu komunikují prostřednictvím definovaných rozhraní. Níže jsou uvedeny dva hlavní diagramy, které znázorňují komponenty systému a jejich propojení.

## Diagram Komponent

Diagram komponent znázorňuje softwarové komponenty použité v systému **Mail Management** a jejich vzájemné interakce. Komponenty zahrnují jak vlastní vývoj, tak i třetí strany.

![Component Diagram](path/to/component_diagram.png)

*Instrukce pro vytvoření Component Diagramu pomocí Lucidchart AI:*

1. **Vytvoření komponent:**
   - **Frontend**
     - Technologie: Bootstrap, JavaScript
     - Popis: Responzivní uživatelské rozhraní optimalizované pro mobilní zařízení.
   - **Backend**
     - Technologie: ASP.NET Core REST API
     - Popis: Logika aplikace, zpracování požadavků a komunikace s databází.
   - **Databáze**
     - Technologie: Microsoft SQL Server (běžící v Docker kontejneru)
     - Popis: Ukládání dat o uživatelích, poště, notifikacích a odesílatelích.
   - **Email Služba**
     - Technologie: SMTP server nebo třetí strana (např. SendGrid)
     - Popis: Odesílání emailových notifikací zaměstnancům.
   - **Docker**
     - Technologie: Docker Engine
     - Popis: Kontejnerizace databázového prostředí pro snadnou správu a nasazení.

2. **Definování rozhraní a interakcí:**
   - **Frontend** komunikuje s **Backendem** prostřednictvím REST API.
   - **Backend** interaguje s **Databází** pro CRUD operace.
   - **Backend** využívá **Email Službu** pro odesílání notifikací.
   - **Docker** hostí **Databázi** a umožňuje izolované prostředí pro vývoj a nasazení.

3. **Propojení komponent:**
   - Nakreslete čáry mezi komponentami znázorňující jejich komunikaci.
   - Ujistěte se, že jsou správně označeny tok dat a závislosti.

## Diagram Fyzické Architektury

Diagram fyzické architektury znázorňuje hardwarové a síťové komponenty, na kterých systém **Mail Management** běží. Tento diagram poskytuje přehled o nasazení systému v reálném prostředí.

![Physical Architecture Diagram](path/to/physical_architecture_diagram.png)

*Instrukce pro vytvoření Physical Architecture Diagramu pomocí Lucidchart AI:*

1. **Identifikace fyzických komponent:**
   - **Server Frontend**
     - Popis: Server hostující frontendovou aplikaci.
   - **Server Backend**
     - Popis: Server hostující ASP.NET Core REST API.
   - **Databázový Server**
     - Popis: Server běžící Microsoft SQL Server v Docker kontejneru.
   - **SMTP Server**
     - Popis: Server nebo služba pro odesílání emailů.
   - **Síťová Infrastruktura**
     - Popis: Interní síť kancelářského komplexu, zabezpečení přístupu a firewall.

2. **Rozložení komponent na fyzické vrstvě:**
   - Nakreslete jednotlivé servery a jejich umístění v síti.
   - Uveďte propojení mezi servery a dalšími síťovými zařízeními.

3. **Definování komunikace a zabezpečení:**
   - Zobrazte, jak se data přenášejí mezi frontendem, backendem a databází.
   - Přidejte prvky zabezpečení, jako jsou firewally a VPN, pokud je to relevantní.

## Technologické Komponenty a Platformy

### Technologie

- **Backend:**
  - **ASP.NET Core** pro vývoj REST API
- **Frontend:**
  - **Bootstrap** pro responzivní design
  - **JavaScript** pro interaktivitu
- **Databáze:**
  - **Microsoft SQL Server** běžící v **Dockeru**
- **Email Služba:**
  - **SMTP Server** nebo třetí strana jako **SendGrid**
- **Kontejnerizace:**
  - **Docker** pro izolované prostředí databáze

### Cílové Platformy

- **Webová Aplikace:**
  - Intranetová aplikace dostupná přes moderní webové prohlížeče (Chrome, Firefox, Edge)
- **Mobilní Zařízení:**
  - Responzivní design optimalizovaný pro smartphony a tablety
- **Desktop:**
  - Přístup přes webové prohlížeče na desktopových operačních systémech (Windows, macOS, Linux)