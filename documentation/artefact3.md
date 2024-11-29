# Technická Specifikace: Mail Management

## Konceptuální Doménový Model

Níže je uveden jednoduchý návrh doménového modelu pro informační systém **Mail Management**. Tento model zachycuje hlavní entity systému a jejich vztahy.

### Popis Modelu

### Tabulka Mail

| Column Name    | Data Type        | Description                             |
|----------------|-------------------|-----------------------------------------|
| ID             | INT (PK)          | Unique identifier for the mail          |
| MailType       | ENUM              | Type of mail { Package, Letter, InternalShop, Other } |
| Description    | VARCHAR(255)      | Description of the mail                 |
| RecipientID    | INT (FK)          | Reference to the User (recipient)       |
| SenderID       | INT (FK)          | Reference to the Sender                 |
| Status         | ENUM              | Status of mail { Unclaimed, Claimed, Archived } |
| ReceivedDate   | DATETIME          | Date when mail was received             |
| ClaimedDate    | DATETIME          | Date when mail was claimed              |

### Tabulka User

| Column Name    | Data Type        | Description                             |
|----------------|-------------------|-----------------------------------------|
| ID             | INT (PK)          | Unique identifier for the user          |
| Name           | VARCHAR(100)      | Name of the user                        |
| Email          | VARCHAR(100)      | Email address of the user               |
| Role           | ENUM              | Role of the user { Administrator, Receptionist, Employee } |

### Tabulka EmailNotification

| Column Name       | Data Type        | Description                             |
|-------------------|-------------------|-----------------------------------------|
| ID                | INT (PK)          | Unique identifier for the notification  |
| UserID            | INT (FK)          | Reference to the User                   |
| MailID            | INT (FK)          | Reference to the Mail                   |
| SentDate          | DATETIME          | Date when the email was sent            |
| NotificationType  | ENUM              | Type of notification { Informational, Confirmation } |

### Tabulka Sender

| Column Name  | Data Type        | Description                             |
|--------------|-------------------|-----------------------------------------|
| ID           | INT (PK)          | Unique identifier for the sender        |
| Name         | VARCHAR(100)      | Name of the shipping company            |
| ContactInfo  | VARCHAR(255)      | Contact information (e.g., phone, email)|

## Odhady Velikostí a Množství Entit

### Tabulka Mail

| Entity     | Size per Record | Number of Records       | Total Size (MB)                |
|------------|------------------|-------------------------|--------------------------------|
| Mail       | 1 KB       | 1000 - 100000     | 1000 MB - 10000 MB      |

### Tabulka User

| Entity  | Size per Record | Number of Records | Total Size (MB) |
|---------|------------------|-------------------|-----------------|
| User    | Max 500 KB       | 1000           | 500 MB       |

### Tabulka EmailNotification

| Entity            | Size per Record | Number of Records | Total Size (MB) |
|-------------------|------------------|-------------------|-----------------|
| EmailNotification | 200 KB           | 200000           | 40 MB       |

### Tabulka Sender

| Entity  | Size per Record | Number of Records | Total Size (MB) |
|---------|------------------|-------------------|-----------------|
| Sender  | 1 KB             | 100               | 0.1 MB          |

## Odhad Počtu Uživatů Současně Pracujících se Systémem

- **Průměrný počet uživatelů**: 500
- **Peak počet uživatelů**: 1,000

## Typy Interakcí Uživatelů se Systémem a Odhad jejich Náročnosti

| Typ Interakce                           | Popis                                                                 | Náročnost       |
|-----------------------------------------|-----------------------------------------------------------------------|-----------------|
| Přihlášení do systému                   | Uživatel se autentizuje pomocí API klíče                             | Nízká (IO: čtení, Výpočet: ověření) |
| Evidence nové pošty                     | Recepční zadává údaje o nové zásilce, přiřazuje ji zaměstnanci a vybere odesílatele | Střední (IO: zápis do DB, Výpočet: přiřazení) |
| Předání pošty zaměstnanci               | Recepční aktualizuje stav zásilky na vyzvednutou                     | Střední (IO: aktualizace DB, Výpočet: notifikace) |
| Zobrazení záznamů o zásilkách           | Zaměstnanec prohlíží své zásilky                                      | Nízká až Střední (IO: čtení z DB) |
| Odesílání notifikací emailem            | Systém odesílá emaily zaměstnancům o nových zásilkách               | Vysoká (IO: emailové operace) |
| Správa uživatelů (Administrator)        | Administrator spravuje role a přístupová práva uživatelů              | Střední (IO: čtení/zápis do DB) |

## Operace Systému podle Výpočetní a IO Náročnosti

### Výpočetně Náročné Operace
- **Přiřazování pošty zaměstnancům**: Logika pro přiřazení zásilek na základě různých kritérií.
- **Generování reportů**: Sestavování statistik a historických dat o zásilkách.

### IO Náročné Operace
- **Nahrávání a stahování příloh**: Velikost příloh (1-5 MB) a počet záznamů (až 1,000,000).
- **Odesílání emailových notifikací**: Vysoký počet emailů během peak období.
- **Zálohování databáze**: Pravidelné zálohování velkých datových souborů.

## První Představa o Rozložení Systému a Volba Platforem

### Rozložení Systému
- **Frontend**: Single Page Application (SPA) postavená na Bootstrapu a JavaScriptu, optimalizovaná pro mobilní zařízení.
- **Backend**: ASP.NET REST API, hostované na interním serveru.
- **Databáze**: Microsoft SQL Server běžící v Docker kontejneru na lokálním vývojovém prostředí.

### Volba Platforem
- **Webová aplikace**: Hlavní platforma přístupná přes webové prohlížeče na mobilních i desktopových zařízeních.
- **Mobilní zařízení**: Responzivní design zajišťující optimální uživatelský zážitek na smartphonech a tabletech.

## Použité Technologie a Cílové Platformy

### Technologie
- **Backend**: ASP.NET Core pro vývoj REST API
- **Frontend**: Bootstrap pro responzivní design, JavaScript pro interaktivitu
- **Databáze**: Microsoft SQL Server (běžící v Dockeru pro vývojové účely)
- **Další**: Docker pro kontejnerizaci databázového prostředí

### Cílové Platformy
- **Web**: Intranetová webová aplikace dostupná přes moderní webové prohlížeče (Chrome, Firefox, Edge)
- **Mobilní**: Optimalizace pro mobilní prohlížeče na Android a iOS zařízeních
- **Desktop**: Přístup přes webové prohlížeče na desktopových operačních systémech (Windows, macOS, Linux)

## Závěr

Tato technická specifikace poskytuje základní přehled o návrhu doménového modelu, odhadech velikostí a množství dat, odhadech uživatelského zatížení, typu interakcí a jejich náročnosti, stejně jako o volbě technologií a platform pro vývoj systému **Mail Management**. Další kroky zahrnují podrobnější návrh databáze, implementaci API a vývoj frontendu s ohledem na uvedené specifikace.