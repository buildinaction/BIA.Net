CREATE TABLE [dbo].[SiteViews] (
    [ViewId]    INT NOT NULL,
    [SiteId]    INT NOT NULL,
    [IsDefault] BIT DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SiteViews] PRIMARY KEY CLUSTERED ([SiteId] ASC, [ViewId] ASC),
    CONSTRAINT [FK_SiteSiteView] FOREIGN KEY ([SiteId]) REFERENCES [dbo].[Site] ([Id]),
    CONSTRAINT [FK_ViewSiteView] FOREIGN KEY ([ViewId]) REFERENCES [dbo].[View] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_FK_ViewSiteView]
    ON [dbo].[SiteViews]([ViewId] ASC);

