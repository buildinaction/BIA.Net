CREATE TABLE [dbo].[UserViews] (
    [UserId]    INT NOT NULL,
    [ViewId]    INT NOT NULL,
    [IsDefault] BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserViews] PRIMARY KEY CLUSTERED ([UserId] ASC, [ViewId] ASC),
    CONSTRAINT [FK_UserUserView] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]),
    CONSTRAINT [FK_ViewUserView] FOREIGN KEY ([ViewId]) REFERENCES [dbo].[View] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_ViewUserView]
    ON [dbo].[UserViews]([ViewId] ASC);

