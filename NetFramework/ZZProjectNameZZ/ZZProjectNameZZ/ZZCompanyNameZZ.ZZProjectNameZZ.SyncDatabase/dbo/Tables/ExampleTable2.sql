CREATE TABLE [dbo].[ExampleTable2] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (10)  NOT NULL,
    [Description] NVARCHAR (200) NOT NULL,
    [Site_Id]     INT            NOT NULL,
    CONSTRAINT [PK_ExampleTable2] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SiteExampleTable2] FOREIGN KEY ([Site_Id]) REFERENCES [dbo].[Site] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_SiteExampleTable2]
    ON [dbo].[ExampleTable2]([Site_Id] ASC);

