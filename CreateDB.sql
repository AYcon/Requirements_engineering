/* Create DB (idempotent) */
IF DB_ID(N'ProjectRequirements') IS NULL
BEGIN
  CREATE DATABASE ProjectRequirements;
END
GO

USE ProjectRequirements;
GO

/* === Tables === */

/* 1) Users */
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Users
  (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    [Name]         NVARCHAR(200)        NOT NULL,
    Email          NVARCHAR(320)        NOT NULL, -- up to 320 per RFC
    Password_Hash  VARBINARY(8000)      NOT NULL, -- safer than text; store hash bytes
    Created_At     DATETIME2(3)         NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME())
  );

  /* Unique, case-insensitive by default (depends on DB collation) */
  CREATE UNIQUE INDEX UX_Users_Email ON dbo.Users(Email);
END
GO

/* 2) Projects */
IF OBJECT_ID('dbo.Projects', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Projects
  (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Title         NVARCHAR(400)    NOT NULL,
    Created_At    DATETIME2(3)     NOT NULL CONSTRAINT DF_Projects_CreatedAt DEFAULT (SYSUTCDATETIME()),
    Completed_At  DATETIME2(3)     NULL,
    Created_By    INT              NOT NULL,  -- FK -> Users

    CONSTRAINT FK_Projects_CreatedBy_Users
      FOREIGN KEY (Created_By) REFERENCES dbo.Users(Id)
      ON UPDATE NO ACTION ON DELETE NO ACTION
  );

  CREATE INDEX IX_Projects_Created_By ON dbo.Projects(Created_By);
END
GO

/* 3) Priorities */
IF OBJECT_ID('dbo.Priorities', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Priorities
  (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    [Description] NVARCHAR(200) NOT NULL
  );
END
GO

/* 4) Severities */
IF OBJECT_ID('dbo.Severities', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Severities
  (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    [Description] NVARCHAR(200) NOT NULL
  );
END
GO

/* 5) Categories (consistent singular table name is OK; keeping your plural capitalization) */
IF OBJECT_ID('dbo.Categories', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Categories
  (
    Category_Id   INT IDENTITY(1,1) PRIMARY KEY,
    [Description] NVARCHAR(200) NOT NULL
  );
END
GO

/* 6) Iterations */
IF OBJECT_ID('dbo.Iterations', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Iterations
  (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    Project_Id     INT            NOT NULL,  -- FK -> Projects
    Version_Number INT            NOT NULL,
    Compiled_At    DATETIME2(3)   NULL,
    Created_By     INT            NOT NULL,  -- FK -> Users (preferred over NVARCHAR)
    Accepted       BIT            NOT NULL CONSTRAINT DF_Iterations_Accepted DEFAULT (0),

    CONSTRAINT FK_Iterations_Project_Projects
      FOREIGN KEY (Project_Id) REFERENCES dbo.Projects(Id)
      ON UPDATE NO ACTION ON DELETE CASCADE,  -- delete project -> delete its iterations

    CONSTRAINT FK_Iterations_CreatedBy_Users
      FOREIGN KEY (Created_By) REFERENCES dbo.Users(Id)
      ON UPDATE NO ACTION ON DELETE NO ACTION
  );

  /* Ensure version numbers are unique per project */
  CREATE UNIQUE INDEX UX_Iterations_Project_Version
    ON dbo.Iterations(Project_Id, Version_Number);

  CREATE INDEX IX_Iterations_ProjectId ON dbo.Iterations(Project_Id);
  CREATE INDEX IX_Iterations_CreatedBy ON dbo.Iterations(Created_By);
END
GO

/* 7) Requirements */
IF OBJECT_ID('dbo.Requirements', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Requirements
  (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Iteration_Id  INT            NOT NULL,   -- FK -> Iterations
    Category_Id   INT            NOT NULL,   -- FK -> Categories
    [Text]        NVARCHAR(MAX)  NOT NULL,
    Priority_Id   INT            NULL,       -- FK -> Priorities
    Severity_Id   INT            NULL,       -- FK -> Severities
    Created_At    DATETIME2(3)   NOT NULL CONSTRAINT DF_Requirements_CreatedAt DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT FK_Requirements_Iteration
      FOREIGN KEY (Iteration_Id) REFERENCES dbo.Iterations(Id)
      ON UPDATE NO ACTION ON DELETE CASCADE, -- delete iteration -> delete its requirements

    CONSTRAINT FK_Requirements_Category
      FOREIGN KEY (Category_Id) REFERENCES dbo.Categories(Category_Id)
      ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_Requirements_Priority
      FOREIGN KEY (Priority_Id) REFERENCES dbo.Priorities(Id)
      ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_Requirements_Severity
      FOREIGN KEY (Severity_Id) REFERENCES dbo.Severities(Id)
      ON UPDATE NO ACTION ON DELETE NO ACTION
  );

  /* Helpful indexes */
  CREATE INDEX IX_Requirements_Iteration ON dbo.Requirements(Iteration_Id);
  CREATE INDEX IX_Requirements_Category  ON dbo.Requirements(Category_Id);
  CREATE INDEX IX_Requirements_Priority  ON dbo.Requirements(Priority_Id);
  CREATE INDEX IX_Requirements_Severity  ON dbo.Requirements(Severity_Id);
END
GO
