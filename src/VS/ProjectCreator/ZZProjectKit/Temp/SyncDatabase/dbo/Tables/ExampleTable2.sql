CREATE TABLE [dbo].[ExampleTable2] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (10)  NOT NULL,
    [Description] NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_ExampleTable2] PRIMARY KEY CLUSTERED ([Id] ASC)
);

