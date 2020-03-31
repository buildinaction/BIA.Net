
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/07/2020 10:30:48
-- Generated from EDMX file: D:\Sources\Innovation Factory\ZZProjectNameZZ\Main\ZZProjectNameZZ\ZZCompanyNameZZ.ZZProjectNameZZ.Model\ProjectDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ZZProjectNameZZ];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Member] DROP CONSTRAINT [FK_UserMember];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberRoleMember_MemberRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberRoleMember] DROP CONSTRAINT [FK_MemberRoleMember_MemberRole];
GO
IF OBJECT_ID(N'[dbo].[FK_MemberRoleMember_Member]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MemberRoleMember] DROP CONSTRAINT [FK_MemberRoleMember_Member];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Member] DROP CONSTRAINT [FK_SiteMember];
GO
IF OBJECT_ID(N'[dbo].[FK_ExampleTable2ExampleTable1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable1] DROP CONSTRAINT [FK_ExampleTable2ExampleTable1];
GO
IF OBJECT_ID(N'[dbo].[FK_ExampleTable2ExampleTable11]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable1] DROP CONSTRAINT [FK_ExampleTable2ExampleTable11];
GO
IF OBJECT_ID(N'[dbo].[FK_ExampleTable3ExampleTable1_ExampleTable3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable3ExampleTable1] DROP CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable3];
GO
IF OBJECT_ID(N'[dbo].[FK_ExampleTable3ExampleTable1_ExampleTable1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable3ExampleTable1] DROP CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable1];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteExampleTable1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable1] DROP CONSTRAINT [FK_SiteExampleTable1];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteExampleTable2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable2] DROP CONSTRAINT [FK_SiteExampleTable2];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteExampleTable3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExampleTable3] DROP CONSTRAINT [FK_SiteExampleTable3];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserView]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserViews] DROP CONSTRAINT [FK_UserUserView];
GO
IF OBJECT_ID(N'[dbo].[FK_SiteSiteView]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiteViews] DROP CONSTRAINT [FK_SiteSiteView];
GO
IF OBJECT_ID(N'[dbo].[FK_ViewSiteView]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SiteViews] DROP CONSTRAINT [FK_ViewSiteView];
GO
IF OBJECT_ID(N'[dbo].[FK_ViewUserView]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserViews] DROP CONSTRAINT [FK_ViewUserView];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ExampleTable1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExampleTable1];
GO
IF OBJECT_ID(N'[dbo].[ExampleTable2]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExampleTable2];
GO
IF OBJECT_ID(N'[dbo].[ExampleTable3]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExampleTable3];
GO
IF OBJECT_ID(N'[dbo].[Version]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Version];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[Member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Member];
GO
IF OBJECT_ID(N'[dbo].[MemberRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberRole];
GO
IF OBJECT_ID(N'[dbo].[Site]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Site];
GO
IF OBJECT_ID(N'[dbo].[View]', 'U') IS NOT NULL
    DROP TABLE [dbo].[View];
GO
IF OBJECT_ID(N'[dbo].[SiteViews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SiteViews];
GO
IF OBJECT_ID(N'[dbo].[UserViews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserViews];
GO
IF OBJECT_ID(N'[dbo].[MemberRoleMember]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MemberRoleMember];
GO
IF OBJECT_ID(N'[dbo].[ExampleTable3ExampleTable1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExampleTable3ExampleTable1];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ExampleTable1'
CREATE TABLE [dbo].[ExampleTable1] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [Title] nvarchar(10)  NOT NULL ,
    [Description] nvarchar(200)  NOT NULL DEFAULT ('Description by default') ,
    [Date] datetime  NULL DEFAULT ('01/01/2018 00:00:00') ,
    [ExampleTable2_Id] int  NOT NULL ,
    [ExampleTable2_0_1_Id] int  NULL ,
    [Site_Id] int  NOT NULL 
);
GO

-- Creating table 'ExampleTable2'
CREATE TABLE [dbo].[ExampleTable2] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [Title] nvarchar(10)  NOT NULL ,
    [Description] nvarchar(200)  NOT NULL ,
    [Site_Id] int  NOT NULL 
);
GO

-- Creating table 'ExampleTable3'
CREATE TABLE [dbo].[ExampleTable3] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [Title] nvarchar(50)  NOT NULL ,
    [Description] nvarchar(200)  NOT NULL ,
    [Value] smallint  NOT NULL ,
    [MyDecimal] decimal(9,4)  NULL ,
    [MyDouble] float  NULL ,
    [DateOnly] datetime  NULL ,
    [DateAndTime] datetime  NULL ,
    [TimeOnly] datetime  NULL ,
    [MyTimeSpan] time  NULL ,
    [TimeSpanOver24H] bigint  NULL ,
    [Site_Id] int  NOT NULL 
);
GO

-- Creating table 'Version'
CREATE TABLE [dbo].[Version] (
    [Id] int  NOT NULL ,
    [Name] nvarchar(50)  NOT NULL ,
    [Value] nvarchar(50)  NOT NULL 
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int  NOT NULL ,
    [Email] nvarchar(256)  NULL ,
    [FirstName] nvarchar(50)  NOT NULL ,
    [LastName] nvarchar(50)  NOT NULL ,
    [Login] nvarchar(50)  NOT NULL ,
    [DistinguishedName] nvarchar(250)  NOT NULL ,
    [IsEmployee] bit  NOT NULL ,
    [IsExternal] bit  NOT NULL ,
    [ExternalCompany] nvarchar(50)  NULL ,
    [Company] nvarchar(50)  NOT NULL ,
    [Site] nvarchar(50)  NOT NULL ,
    [Manager] nvarchar(250)  NULL ,
    [Department] nvarchar(50)  NOT NULL ,
    [SubDepartment] nvarchar(50)  NULL ,
    [Office] nvarchar(20)  NOT NULL ,
    [Country] nvarchar(10)  NOT NULL ,
    [DAIEnable] bit  NOT NULL ,
    [DAIDate] datetime  NOT NULL 
);
GO

-- Creating table 'Member'
CREATE TABLE [dbo].[Member] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [User_Id] int  NOT NULL ,
    [Site_Id] int  NOT NULL 
);
GO

-- Creating table 'MemberRole'
CREATE TABLE [dbo].[MemberRole] (
    [Id] int  NOT NULL ,
    [Title] nvarchar(100)  NOT NULL 
);
GO

-- Creating table 'Site'
CREATE TABLE [dbo].[Site] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [Title] nvarchar(max)  NOT NULL 
);
GO

-- Creating table 'View'
CREATE TABLE [dbo].[View] (
    [Id] int IDENTITY(1,1) NOT NULL ,
    [TableId] nvarchar(100)  NOT NULL ,
    [Name] nvarchar(50)  NOT NULL ,
    [Description] nvarchar(500)  NULL ,
    [Preference] nvarchar(max)  NOT NULL ,
    [ViewType] int  NOT NULL DEFAULT ('-1') 
);
GO

-- Creating table 'SiteViews'
CREATE TABLE [dbo].[SiteViews] (
    [ViewId] int  NOT NULL ,
    [SiteId] int  NOT NULL ,
    [IsDefault] bit  NOT NULL DEFAULT (0) 
);
GO

-- Creating table 'UserViews'
CREATE TABLE [dbo].[UserViews] (
    [UserId] int  NOT NULL ,
    [ViewId] int  NOT NULL ,
    [IsDefault] bit  NOT NULL DEFAULT (0) 
);
GO

-- Creating table 'MemberRoleMember'
CREATE TABLE [dbo].[MemberRoleMember] (
    [MemberRole_Id] int  NOT NULL ,
    [Members_Id] int  NOT NULL 
);
GO

-- Creating table 'ExampleTable3ExampleTable1'
CREATE TABLE [dbo].[ExampleTable3ExampleTable1] (
    [ExampleTable3_Id] int  NOT NULL ,
    [ExampleTable1_Id] int  NOT NULL 
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ExampleTable1'
ALTER TABLE [dbo].[ExampleTable1]
ADD CONSTRAINT [PK_ExampleTable1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExampleTable2'
ALTER TABLE [dbo].[ExampleTable2]
ADD CONSTRAINT [PK_ExampleTable2]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExampleTable3'
ALTER TABLE [dbo].[ExampleTable3]
ADD CONSTRAINT [PK_ExampleTable3]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Version'
ALTER TABLE [dbo].[Version]
ADD CONSTRAINT [PK_Version]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Member'
ALTER TABLE [dbo].[Member]
ADD CONSTRAINT [PK_Member]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MemberRole'
ALTER TABLE [dbo].[MemberRole]
ADD CONSTRAINT [PK_MemberRole]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Site'
ALTER TABLE [dbo].[Site]
ADD CONSTRAINT [PK_Site]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'View'
ALTER TABLE [dbo].[View]
ADD CONSTRAINT [PK_View]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SiteId], [ViewId] in table 'SiteViews'
ALTER TABLE [dbo].[SiteViews]
ADD CONSTRAINT [PK_SiteViews]
    PRIMARY KEY CLUSTERED ([SiteId], [ViewId] ASC);
GO

-- Creating primary key on [UserId], [ViewId] in table 'UserViews'
ALTER TABLE [dbo].[UserViews]
ADD CONSTRAINT [PK_UserViews]
    PRIMARY KEY CLUSTERED ([UserId], [ViewId] ASC);
GO

-- Creating primary key on [MemberRole_Id], [Members_Id] in table 'MemberRoleMember'
ALTER TABLE [dbo].[MemberRoleMember]
ADD CONSTRAINT [PK_MemberRoleMember]
    PRIMARY KEY CLUSTERED ([MemberRole_Id], [Members_Id] ASC);
GO

-- Creating primary key on [ExampleTable3_Id], [ExampleTable1_Id] in table 'ExampleTable3ExampleTable1'
ALTER TABLE [dbo].[ExampleTable3ExampleTable1]
ADD CONSTRAINT [PK_ExampleTable3ExampleTable1]
    PRIMARY KEY CLUSTERED ([ExampleTable3_Id], [ExampleTable1_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'Member'
ALTER TABLE [dbo].[Member]
ADD CONSTRAINT [FK_UserMember]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserMember'
CREATE INDEX [IX_FK_UserMember]
ON [dbo].[Member]
    ([User_Id]);
GO

-- Creating foreign key on [MemberRole_Id] in table 'MemberRoleMember'
ALTER TABLE [dbo].[MemberRoleMember]
ADD CONSTRAINT [FK_MemberRoleMember_MemberRole]
    FOREIGN KEY ([MemberRole_Id])
    REFERENCES [dbo].[MemberRole]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Members_Id] in table 'MemberRoleMember'
ALTER TABLE [dbo].[MemberRoleMember]
ADD CONSTRAINT [FK_MemberRoleMember_Member]
    FOREIGN KEY ([Members_Id])
    REFERENCES [dbo].[Member]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberRoleMember_Member'
CREATE INDEX [IX_FK_MemberRoleMember_Member]
ON [dbo].[MemberRoleMember]
    ([Members_Id]);
GO

-- Creating foreign key on [Site_Id] in table 'Member'
ALTER TABLE [dbo].[Member]
ADD CONSTRAINT [FK_SiteMember]
    FOREIGN KEY ([Site_Id])
    REFERENCES [dbo].[Site]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteMember'
CREATE INDEX [IX_FK_SiteMember]
ON [dbo].[Member]
    ([Site_Id]);
GO

-- Creating foreign key on [ExampleTable2_Id] in table 'ExampleTable1'
ALTER TABLE [dbo].[ExampleTable1]
ADD CONSTRAINT [FK_ExampleTable2ExampleTable1]
    FOREIGN KEY ([ExampleTable2_Id])
    REFERENCES [dbo].[ExampleTable2]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExampleTable2ExampleTable1'
CREATE INDEX [IX_FK_ExampleTable2ExampleTable1]
ON [dbo].[ExampleTable1]
    ([ExampleTable2_Id]);
GO

-- Creating foreign key on [ExampleTable2_0_1_Id] in table 'ExampleTable1'
ALTER TABLE [dbo].[ExampleTable1]
ADD CONSTRAINT [FK_ExampleTable2ExampleTable11]
    FOREIGN KEY ([ExampleTable2_0_1_Id])
    REFERENCES [dbo].[ExampleTable2]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExampleTable2ExampleTable11'
CREATE INDEX [IX_FK_ExampleTable2ExampleTable11]
ON [dbo].[ExampleTable1]
    ([ExampleTable2_0_1_Id]);
GO

-- Creating foreign key on [ExampleTable3_Id] in table 'ExampleTable3ExampleTable1'
ALTER TABLE [dbo].[ExampleTable3ExampleTable1]
ADD CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable3]
    FOREIGN KEY ([ExampleTable3_Id])
    REFERENCES [dbo].[ExampleTable3]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ExampleTable1_Id] in table 'ExampleTable3ExampleTable1'
ALTER TABLE [dbo].[ExampleTable3ExampleTable1]
ADD CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable1]
    FOREIGN KEY ([ExampleTable1_Id])
    REFERENCES [dbo].[ExampleTable1]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExampleTable3ExampleTable1_ExampleTable1'
CREATE INDEX [IX_FK_ExampleTable3ExampleTable1_ExampleTable1]
ON [dbo].[ExampleTable3ExampleTable1]
    ([ExampleTable1_Id]);
GO

-- Creating foreign key on [Site_Id] in table 'ExampleTable1'
ALTER TABLE [dbo].[ExampleTable1]
ADD CONSTRAINT [FK_SiteExampleTable1]
    FOREIGN KEY ([Site_Id])
    REFERENCES [dbo].[Site]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteExampleTable1'
CREATE INDEX [IX_FK_SiteExampleTable1]
ON [dbo].[ExampleTable1]
    ([Site_Id]);
GO

-- Creating foreign key on [Site_Id] in table 'ExampleTable2'
ALTER TABLE [dbo].[ExampleTable2]
ADD CONSTRAINT [FK_SiteExampleTable2]
    FOREIGN KEY ([Site_Id])
    REFERENCES [dbo].[Site]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteExampleTable2'
CREATE INDEX [IX_FK_SiteExampleTable2]
ON [dbo].[ExampleTable2]
    ([Site_Id]);
GO

-- Creating foreign key on [Site_Id] in table 'ExampleTable3'
ALTER TABLE [dbo].[ExampleTable3]
ADD CONSTRAINT [FK_SiteExampleTable3]
    FOREIGN KEY ([Site_Id])
    REFERENCES [dbo].[Site]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteExampleTable3'
CREATE INDEX [IX_FK_SiteExampleTable3]
ON [dbo].[ExampleTable3]
    ([Site_Id]);
GO

-- Creating foreign key on [UserId] in table 'UserViews'
ALTER TABLE [dbo].[UserViews]
ADD CONSTRAINT [FK_UserUserView]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SiteId] in table 'SiteViews'
ALTER TABLE [dbo].[SiteViews]
ADD CONSTRAINT [FK_SiteSiteView]
    FOREIGN KEY ([SiteId])
    REFERENCES [dbo].[Site]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ViewId] in table 'SiteViews'
ALTER TABLE [dbo].[SiteViews]
ADD CONSTRAINT [FK_ViewSiteView]
    FOREIGN KEY ([ViewId])
    REFERENCES [dbo].[View]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ViewSiteView'
CREATE INDEX [IX_FK_ViewSiteView]
ON [dbo].[SiteViews]
    ([ViewId]);
GO

-- Creating foreign key on [ViewId] in table 'UserViews'
ALTER TABLE [dbo].[UserViews]
ADD CONSTRAINT [FK_ViewUserView]
    FOREIGN KEY ([ViewId])
    REFERENCES [dbo].[View]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ViewUserView'
CREATE INDEX [IX_FK_ViewUserView]
ON [dbo].[UserViews]
    ([ViewId]);
GO


-- --------------------------------------------------
-- Creating all Index
-- --------------------------------------------------

CREATE UNIQUE NONCLUSTERED INDEX [UX_Login] ON [dbo].[User]
(
	[Login] ASC
)
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
