-- ============================================================================
-- Booksy - databaseschema (SQL Server / T-SQL)
--
-- Dit script maakt alle tabellen aan die de DAL-repositories verwachten.
-- De kolomnamen zijn Nederlands en komen exact overeen met de queries in
-- DAL/Repositories/*.cs (bv. BoekID, Naam, AuteurID, Leesstatus, Omslag).
--
-- Het script is idempotent: het kan veilig opnieuw worden uitgevoerd. Bestaande
-- tabellen worden niet overschreven (IF NOT EXISTS).
--
-- Uitvoeren: open in SSMS/Azure Data Studio op de juiste database, of via
--   sqlcmd -S <server> -d <database> -i db/schema.sql
-- ============================================================================

SET NOCOUNT ON;
GO

-- ----------------------------------------------------------------------------
-- Author  (FR-14: auteur als aparte entiteit)
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.Author', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Author
    (
        AuteurID INT IDENTITY(1,1) NOT NULL,
        Naam     NVARCHAR(200)     NOT NULL,
        CONSTRAINT PK_Author PRIMARY KEY (AuteurID),
        -- B-14-01: een auteur moet een naam hebben van minimaal 2 tekens.
        CONSTRAINT CK_Author_Naam_MinLengte CHECK (LEN(Naam) >= 2)
    );
END
GO

-- ----------------------------------------------------------------------------
-- Genre  (FR-15: genre als aparte entiteit)
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.Genre', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Genre
    (
        GenreID INT IDENTITY(1,1) NOT NULL,
        Naam    NVARCHAR(100)     NOT NULL,
        CONSTRAINT PK_Genre PRIMARY KEY (GenreID),
        -- B-15-01: een genre moet een naam hebben van minimaal 2 tekens.
        CONSTRAINT CK_Genre_Naam_MinLengte CHECK (LEN(Naam) >= 2)
    );
END
GO

-- ----------------------------------------------------------------------------
-- Book
--   AuteurID  -> B-14-02: een boek is altijd gekoppeld aan precies een auteur.
--   Leesstatus-> FR-10: "Wil ik lezen" / "Bezig" / "Gelezen".
--   Omslag    -> FR-13: verwijzing (URL/pad) naar de omslag; mag NULL zijn.
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.Book', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Book
    (
        BoekID     INT IDENTITY(1,1) NOT NULL,
        Naam       NVARCHAR(300)     NOT NULL,
        AuteurID   INT               NOT NULL,
        -- B-10-02: een boek moet altijd een status hebben (default + NOT NULL).
        Leesstatus NVARCHAR(20)      NOT NULL CONSTRAINT DF_Book_Leesstatus DEFAULT (N'Wil ik lezen'),
        Omslag     NVARCHAR(1000)    NULL,
        CONSTRAINT PK_Book PRIMARY KEY (BoekID),
        CONSTRAINT FK_Book_Author FOREIGN KEY (AuteurID) REFERENCES dbo.Author (AuteurID),
        -- B-10-01: alleen deze drie statussen zijn geldig.
        CONSTRAINT CK_Book_Leesstatus CHECK (Leesstatus IN (N'Wil ik lezen', N'Bezig', N'Gelezen'))
    );

    CREATE INDEX IX_Book_AuteurID ON dbo.Book (AuteurID);
END
GO

-- ----------------------------------------------------------------------------
-- BookGenre  (koppeltabel, B-15-02: een boek kan meerdere genres hebben)
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.BookGenre', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.BookGenre
    (
        BoekID  INT NOT NULL,
        GenreID INT NOT NULL,
        CONSTRAINT PK_BookGenre PRIMARY KEY (BoekID, GenreID),
        -- Een boek wist eerst zijn koppelingen bij een update; daarom cascade delete.
        CONSTRAINT FK_BookGenre_Book  FOREIGN KEY (BoekID)  REFERENCES dbo.Book (BoekID)   ON DELETE CASCADE,
        CONSTRAINT FK_BookGenre_Genre FOREIGN KEY (GenreID) REFERENCES dbo.Genre (GenreID)
    );

    CREATE INDEX IX_BookGenre_GenreID ON dbo.BookGenre (GenreID);
END
GO

-- ----------------------------------------------------------------------------
-- Review  (datum verplicht: elke review heeft een plaatsdatum)
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.Review', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Review
    (
        ReviewID INT IDENTITY(1,1) NOT NULL,
        BoekID   INT               NOT NULL,
        Tekst    NVARCHAR(MAX)     NOT NULL,
        Rating   INT               NOT NULL,
        Datum    DATETIME2(0)      NOT NULL CONSTRAINT DF_Review_Datum DEFAULT (SYSDATETIME()),
        CONSTRAINT PK_Review PRIMARY KEY (ReviewID),
        CONSTRAINT FK_Review_Book FOREIGN KEY (BoekID) REFERENCES dbo.Book (BoekID) ON DELETE CASCADE,
        -- Rating moet tussen 1 en 5 liggen.
        CONSTRAINT CK_Review_Rating CHECK (Rating BETWEEN 1 AND 5)
    );

    CREATE INDEX IX_Review_BoekID ON dbo.Review (BoekID);
END
GO

-- ----------------------------------------------------------------------------
-- User  (FR-11: registreren/inloggen met e-mail + gehasht wachtwoord)
--   [User] is een gereserveerd woord, daarom altijd tussen haken.
-- ----------------------------------------------------------------------------
IF OBJECT_ID(N'dbo.[User]', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[User]
    (
        GebruikerID    INT IDENTITY(1,1) NOT NULL,
        Naam           NVARCHAR(200)     NOT NULL,
        Email          NVARCHAR(256)     NOT NULL,
        WachtwoordHash NVARCHAR(512)     NOT NULL,
        CONSTRAINT PK_User PRIMARY KEY (GebruikerID),
        -- B-11-01: het e-mailadres moet uniek zijn.
        CONSTRAINT UQ_User_Email UNIQUE (Email)
    );
END
GO

PRINT N'Booksy-schema is aangemaakt of was al aanwezig.';
GO
