CREATE TABLE [dbo].[ExampleTable1] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Title]                NVARCHAR (10)  NOT NULL,
    [Description]          NVARCHAR (200) DEFAULT ('Description by default') NOT NULL,
    [Date]                 DATETIME       DEFAULT ('01/01/2018 00:00:00') NULL,
    [ExampleTable2_Id]     INT            NOT NULL,
    [ExampleTable2_0_1_Id] INT            NULL,
    CONSTRAINT [PK_ExampleTable1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExampleTable2ExampleTable1] FOREIGN KEY ([ExampleTable2_Id]) REFERENCES [dbo].[ExampleTable2] ([Id]),
    CONSTRAINT [FK_ExampleTable2ExampleTable11] FOREIGN KEY ([ExampleTable2_0_1_Id]) REFERENCES [dbo].[ExampleTable2] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ExampleTable2ExampleTable11]
    ON [dbo].[ExampleTable1]([ExampleTable2_0_1_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ExampleTable2ExampleTable1]
    ON [dbo].[ExampleTable1]([ExampleTable2_Id] ASC);

