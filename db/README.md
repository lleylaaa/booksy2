# Database

SQL-scripts voor de Booksy-database (SQL Server). De kolomnamen zijn Nederlands
en komen exact overeen met de queries in `DAL/Repositories/*.cs`.

## Bestanden

- `schema.sql` — maakt alle tabellen aan (Author, Genre, Book, BookGenre, Review, User)
  met primary keys, foreign keys en check-constraints. Idempotent.
- `seed.sql` — vult dezelfde voorbeelddata als de in-memory modus
  (3 boeken/auteurs/genres, 2 reviews, gebruiker Leyla). Idempotent.

## Uitvoeren

Draai eerst `schema.sql`, daarna `seed.sql`, tegen de database uit de
connectionstring (`Booksy/appsettings.json`, sleutel `DefaultConnection`).

Met `sqlcmd`:

```bash
sqlcmd -S mssqlstud.fhict.local -d dbi580427 -U dbi580427 -P <wachtwoord> -i db/schema.sql
sqlcmd -S mssqlstud.fhict.local -d dbi580427 -U dbi580427 -P <wachtwoord> -i db/seed.sql
```

Of open de bestanden in SSMS / Azure Data Studio (juiste database geselecteerd)
en voer ze uit.

## Inloggen na seed

- E-mail: `leyla@booksy.nl`
- Wachtwoord: `geheim123`

Het wachtwoord staat in `seed.sql` als PBKDF2-hash (formaat
`iteraties.salt.hash`), zodat `ServiceLibrary`'s `PasswordHasher.Verify` het
accepteert. Nieuwe gebruikers die via de app registreren krijgen automatisch
hun eigen gehashte wachtwoord.

## Let op

De app draait ook zonder SQL Server via de in-memory modus
(`USE_IN_MEMORY=true`); dan zijn deze scripts niet nodig. Gebruik ze alleen voor
de echte SQL Server-database.
