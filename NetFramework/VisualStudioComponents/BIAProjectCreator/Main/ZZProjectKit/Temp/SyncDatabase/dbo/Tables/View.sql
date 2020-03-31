CREATE TABLE [dbo].[View] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [TableId]     NVARCHAR (100) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (500) NULL,
    [Preference]  NVARCHAR (MAX) NOT NULL,
    [ViewType]    INT            DEFAULT ('-1') NOT NULL,
    CONSTRAINT [PK_View] PRIMARY KEY CLUSTERED ([Id] ASC)
);



