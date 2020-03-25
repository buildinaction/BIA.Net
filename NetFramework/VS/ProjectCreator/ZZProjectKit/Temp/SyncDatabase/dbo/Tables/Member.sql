CREATE TABLE [dbo].[Member] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [User_Id] INT NOT NULL,
    [Site_Id]       INT NOT NULL,
    CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserMember] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_SiteMember] FOREIGN KEY ([Site_Id]) REFERENCES [dbo].[Site] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_SiteMember]
    ON [dbo].[Member]([Site_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_UserMember]
    ON [dbo].[Member]([User_Id] ASC);

