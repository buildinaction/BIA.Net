CREATE TABLE [dbo].[Version] (
    [Id]    INT           NOT NULL,
    [Name]  NVARCHAR (50) NOT NULL,
    [Value] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Version] PRIMARY KEY CLUSTERED ([Id] ASC)
);

