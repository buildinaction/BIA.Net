CREATE TABLE [dbo].[ExampleTable3ExampleTable1] (
    [ExampleTable3_Id] INT NOT NULL,
    [ExampleTable1_Id] INT NOT NULL,
    CONSTRAINT [PK_ExampleTable3ExampleTable1] PRIMARY KEY CLUSTERED ([ExampleTable3_Id] ASC, [ExampleTable1_Id] ASC),
    CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable1] FOREIGN KEY ([ExampleTable1_Id]) REFERENCES [dbo].[ExampleTable1] ([Id]),
    CONSTRAINT [FK_ExampleTable3ExampleTable1_ExampleTable3] FOREIGN KEY ([ExampleTable3_Id]) REFERENCES [dbo].[ExampleTable3] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ExampleTable3ExampleTable1_ExampleTable1]
    ON [dbo].[ExampleTable3ExampleTable1]([ExampleTable1_Id] ASC);

