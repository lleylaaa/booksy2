-- ============================================================================
-- Booksy - seeddata (SQL Server / T-SQL)
--
-- Vult de database met dezelfde voorbeelddata als de in-memory modus, zodat de
-- echte database hetzelfde gedrag toont. Draai db/schema.sql eerst.
--
-- Idempotent: rijen worden alleen toegevoegd als ze nog niet bestaan, dus het
-- script kan veilig opnieuw worden uitgevoerd.
--
-- Gebruiker Leyla heeft het wachtwoord "geheim123". De hash hieronder is een
-- echte PBKDF2-hash (formaat iteraties.salt.hash) die door ServiceLibrary's
-- PasswordHasher.Verify wordt geaccepteerd.
-- ============================================================================

SET NOCOUNT ON;
GO

-- ----------------------------------------------------------------------------
-- Auteurs
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Author (Naam)
SELECT v.Naam
FROM (VALUES (N'Harry Mulisch'), (N'Herman Koch'), (N'Gerard Reve')) AS v(Naam)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Author a WHERE a.Naam = v.Naam);
GO

-- ----------------------------------------------------------------------------
-- Genres
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Genre (Naam)
SELECT v.Naam
FROM (VALUES (N'Roman'), (N'Thriller'), (N'Klassieker')) AS v(Naam)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Genre g WHERE g.Naam = v.Naam);
GO

-- ----------------------------------------------------------------------------
-- Boeken (gekoppeld aan auteur op naam, met leesstatus)
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Book (Naam, AuteurID, Leesstatus, Omslag)
SELECT v.Naam, a.AuteurID, v.Leesstatus, NULL
FROM (VALUES
        (N'De ontdekking van de hemel', N'Harry Mulisch', N'Gelezen'),
        (N'Het diner',                   N'Herman Koch',   N'Bezig'),
        (N'De avonden',                  N'Gerard Reve',   N'Wil ik lezen')
     ) AS v(Naam, Auteur, Leesstatus)
JOIN dbo.Author a ON a.Naam = v.Auteur
WHERE NOT EXISTS (SELECT 1 FROM dbo.Book b WHERE b.Naam = v.Naam);
GO

-- ----------------------------------------------------------------------------
-- Boek <-> genre koppelingen (elk seedboek krijgt een genre)
-- ----------------------------------------------------------------------------
INSERT INTO dbo.BookGenre (BoekID, GenreID)
SELECT b.BoekID, g.GenreID
FROM (VALUES
        (N'De ontdekking van de hemel', N'Roman'),
        (N'Het diner',                   N'Thriller'),
        (N'De avonden',                  N'Klassieker')
     ) AS v(Boek, Genre)
JOIN dbo.Book  b ON b.Naam = v.Boek
JOIN dbo.Genre g ON g.Naam = v.Genre
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.BookGenre bg WHERE bg.BoekID = b.BoekID AND bg.GenreID = g.GenreID
);
GO

-- ----------------------------------------------------------------------------
-- Reviews bij het eerste boek
-- ----------------------------------------------------------------------------
INSERT INTO dbo.Review (BoekID, Tekst, Rating, Datum)
SELECT b.BoekID, v.Tekst, v.Rating, v.Datum
FROM (VALUES
        (N'Prachtig geschreven, een echte aanrader.', 5, CAST(N'2026-06-01T19:30:00' AS DATETIME2(0))),
        (N'Mooi boek, maar wel aan de lange kant.',    4, CAST(N'2026-06-03T21:05:00' AS DATETIME2(0)))
     ) AS v(Tekst, Rating, Datum)
JOIN dbo.Book b ON b.Naam = N'De ontdekking van de hemel'
WHERE NOT EXISTS (SELECT 1 FROM dbo.Review r WHERE r.BoekID = b.BoekID AND r.Tekst = v.Tekst);
GO

-- ----------------------------------------------------------------------------
-- Gebruiker Leyla (wachtwoord: geheim123)
-- ----------------------------------------------------------------------------
INSERT INTO dbo.[User] (Naam, Email, WachtwoordHash)
SELECT N'Leyla', N'leyla@booksy.nl',
       N'100000.VaN5BB5fTFSystYe71dy6w==.4OhtZQLqQpYdPv4L5aIz7Uaa2SXzDPLlRVJp2/6Qkvk='
WHERE NOT EXISTS (SELECT 1 FROM dbo.[User] u WHERE u.Email = N'leyla@booksy.nl');
GO

PRINT N'Booksy-seeddata is ingevoerd (of was al aanwezig).';
GO
