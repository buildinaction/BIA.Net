CREATE TABLE [dbo].[ExampleTable3] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NOT NULL,
    [Value]       SMALLINT       NOT NULL,
    CONSTRAINT [PK_ExampleTable3] PRIMARY KEY CLUSTERED ([Id] ASC)
);

