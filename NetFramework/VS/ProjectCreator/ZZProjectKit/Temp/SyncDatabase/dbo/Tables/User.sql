CREATE TABLE [dbo].[User] (
    [Id]                INT            NOT NULL,
    [Email]             NVARCHAR (256) NULL,
    [FirstName]         NVARCHAR (50)  NOT NULL,
    [LastName]          NVARCHAR (50)  NOT NULL,
    [Login]             NVARCHAR (50)  NOT NULL,
    [DistinguishedName] NVARCHAR (250) NOT NULL,
    [IsEmployee]        BIT            NOT NULL,
    [IsExternal]        BIT            NOT NULL,
    [ExternalCompany]   NVARCHAR (50)  NULL,
    [Company]           NVARCHAR (50)  NOT NULL,
    [Site]              NVARCHAR (50)  NOT NULL,
    [Manager]           NVARCHAR (250) NULL,
    [Department]        NVARCHAR (50)  NOT NULL,
    [SubDepartment]     NVARCHAR (50)  NULL,
    [Office]            NVARCHAR (20)  NOT NULL,
    [Country]          NVARCHAR (10)  NOT NULL,
    [DAIEnable]         BIT            NOT NULL,
    [DAIDate]           DATETIME       NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Login]
    ON [dbo].[User]([Login] ASC);

