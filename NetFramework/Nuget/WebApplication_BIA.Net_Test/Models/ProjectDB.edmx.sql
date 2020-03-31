
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/17/2017 17:35:45
-- Generated from EDMX file: C:\Source\BIA.Net\Main\WebApplication_BIA.Net_Test\Models\ProjectDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebApplication_BIA.Net_Test];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_FamilyPlanning]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Plannings] DROP CONSTRAINT [FK_FamilyPlanning];
GO
IF OBJECT_ID(N'[dbo].[FK_FamilleMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Membres] DROP CONSTRAINT [FK_FamilleMember];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUsersMembre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Membres] DROP CONSTRAINT [FK_AspNetUsersMembre];
GO
IF OBJECT_ID(N'[dbo].[FK_FamilyRoleMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Membres] DROP CONSTRAINT [FK_FamilyRoleMember];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[Familles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Familles];
GO
IF OBJECT_ID(N'[dbo].[Membres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Membres];
GO
IF OBJECT_ID(N'[dbo].[Version]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Version];
GO
IF OBJECT_ID(N'[dbo].[Plannings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Plannings];
GO
IF OBJECT_ID(N'[dbo].[FamilleRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FamilleRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'Familles'
CREATE TABLE [dbo].[Familles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Titre] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Membres'
CREATE TABLE [dbo].[Membres] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmailInvitation] nvarchar(256)  NULL,
    [SecurityTokenInvitation] uniqueidentifier  NULL,
    [Famille_Id] int  NOT NULL,
    [AspNetUser_Id] nvarchar(128)  NULL,
    [FamilyRole_Id] int  NOT NULL
);
GO

-- Creating table 'Version'
CREATE TABLE [dbo].[Version] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Plannings'
CREATE TABLE [dbo].[Plannings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Titre] nvarchar(25)  NOT NULL,
    [Family_Id] int  NOT NULL
);
GO

-- Creating table 'FamilleRoles'
CREATE TABLE [dbo].[FamilleRoles] (
    [Id] int  NOT NULL,
    [Titre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AspNetUserRole'
CREATE TABLE [dbo].[AspNetUserRole] (
    [Role_Id] nvarchar(128)  NOT NULL,
    [User_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Familles'
ALTER TABLE [dbo].[Familles]
ADD CONSTRAINT [PK_Familles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Membres'
ALTER TABLE [dbo].[Membres]
ADD CONSTRAINT [PK_Membres]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Version'
ALTER TABLE [dbo].[Version]
ADD CONSTRAINT [PK_Version]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Plannings'
ALTER TABLE [dbo].[Plannings]
ADD CONSTRAINT [PK_Plannings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FamilleRoles'
ALTER TABLE [dbo].[FamilleRoles]
ADD CONSTRAINT [PK_FamilleRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Role_Id], [User_Id] in table 'AspNetUserRole'
ALTER TABLE [dbo].[AspNetUserRole]
ADD CONSTRAINT [PK_AspNetUserRole]
    PRIMARY KEY CLUSTERED ([Role_Id], [User_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Role_Id] in table 'AspNetUserRole'
ALTER TABLE [dbo].[AspNetUserRole]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [User_Id] in table 'AspNetUserRole'
ALTER TABLE [dbo].[AspNetUserRole]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRole]
    ([User_Id]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [Family_Id] in table 'Plannings'
ALTER TABLE [dbo].[Plannings]
ADD CONSTRAINT [FK_FamilyPlanning]
    FOREIGN KEY ([Family_Id])
    REFERENCES [dbo].[Familles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FamilyPlanning'
CREATE INDEX [IX_FK_FamilyPlanning]
ON [dbo].[Plannings]
    ([Family_Id]);
GO

-- Creating foreign key on [Famille_Id] in table 'Membres'
ALTER TABLE [dbo].[Membres]
ADD CONSTRAINT [FK_FamilleMember]
    FOREIGN KEY ([Famille_Id])
    REFERENCES [dbo].[Familles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FamilleMember'
CREATE INDEX [IX_FK_FamilleMember]
ON [dbo].[Membres]
    ([Famille_Id]);
GO

-- Creating foreign key on [AspNetUser_Id] in table 'Membres'
ALTER TABLE [dbo].[Membres]
ADD CONSTRAINT [FK_AspNetUsersMembre]
    FOREIGN KEY ([AspNetUser_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUsersMembre'
CREATE INDEX [IX_FK_AspNetUsersMembre]
ON [dbo].[Membres]
    ([AspNetUser_Id]);
GO

-- Creating foreign key on [FamilyRole_Id] in table 'Membres'
ALTER TABLE [dbo].[Membres]
ADD CONSTRAINT [FK_FamilyRoleMember]
    FOREIGN KEY ([FamilyRole_Id])
    REFERENCES [dbo].[FamilleRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FamilyRoleMember'
CREATE INDEX [IX_FK_FamilyRoleMember]
ON [dbo].[Membres]
    ([FamilyRole_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------