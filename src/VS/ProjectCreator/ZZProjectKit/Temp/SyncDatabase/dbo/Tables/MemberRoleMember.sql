CREATE TABLE [dbo].[MemberRoleMember] (
    [MemberRole_Id] INT NOT NULL,
    [Members_Id]    INT NOT NULL,
    CONSTRAINT [PK_MemberRoleMember] PRIMARY KEY CLUSTERED ([MemberRole_Id] ASC, [Members_Id] ASC),
    CONSTRAINT [FK_MemberRoleMember_Member] FOREIGN KEY ([Members_Id]) REFERENCES [dbo].[Member] ([Id]),
    CONSTRAINT [FK_MemberRoleMember_MemberRole] FOREIGN KEY ([MemberRole_Id]) REFERENCES [dbo].[MemberRole] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_MemberRoleMember_Member]
    ON [dbo].[MemberRoleMember]([Members_Id] ASC);

