USE [WebApplication_BIA.Net_Test]
GO

/****** Object:  View [dbo].[AspNetUserRoles]    Script Date: 13/03/2016 23:08:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[AspNetUserRoles]
AS
SELECT     Role_Id AS RoleId, User_Id AS UserId
FROM         dbo.AspNetUserRole

GO