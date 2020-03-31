CREATE TABLE [dbo].[ExampleTable3] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Title]           NVARCHAR (50)  NOT NULL,
    [Description]     NVARCHAR (200) NOT NULL,
    [Value]           SMALLINT       NOT NULL,
    [Decimal]         DECIMAL (9, 4) NULL,
    [Double]          FLOAT (53)     NULL,
    [DateOnly]        DATETIME       NULL,
    [DateAndTime]     DATETIME       NULL,
    [TimeOnly]        DATETIME       NULL,
    [TimeSpan]        TIME (7)       NULL,
    [TimeSpanOver24H] BIGINT         NULL,
    [Site_Id]         INT            NOT NULL,
    CONSTRAINT [PK_ExampleTable3] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SiteExampleTable3] FOREIGN KEY ([Site_Id]) REFERENCES [dbo].[Site] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_SiteExampleTable3]
    ON [dbo].[ExampleTable3]([Site_Id] ASC);

