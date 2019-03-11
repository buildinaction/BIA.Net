namespace $safeprojectname$
{
    using BIA.Net.Business.Services;
    using BIA.Net.Model.DAL;
    using EntityFramework.MoqHelper;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Model;
    using Moq;
    using Business.DTO;
    using Common;
    using Test.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestClass]
    public class UnitTest
    {

        public UnitTest()
        {
            UnityConfig.RegisterTypes();
        }

        [TestMethod]
        public void MemberRole_CRUD_operations_basics()
        {
            var memberRoles = new List<MemberRole>() { new MemberRole() { Id = 8, Title = "Old MemberRole" } };

            var mockMemberRoleSet = EntityFrameworkMoqHelper.CreateMockForDbSet<MemberRole>()
                                                            .SetupForQueryOn(memberRoles)
                                                            .WithAdd(memberRoles, "Id") // overwritten to simulate behavior of auto-increment database
                                                            .WithFind(memberRoles, "Id")
                                                            .WithRemove(memberRoles);

            var mockContext = new Mock<$saferootprojectname$DBContainer>();
            mockContext.Setup(c => c.Set<MemberRole>()).Returns(mockMemberRoleSet.Object);

            TDBContainer<$saferootprojectname$DBContainer>.SetMoqContext(mockContext.Object);

            UserInfoContainer.SetUserInfo(0,String.Empty, new List<string>() { Constants.RoleAdmin });

            var memberRoleDTO = AllServicesDTO.Insert<MemberRoleDTO>(new MemberRoleDTO() { Title = "New MemberRole" });
            var memberRoleDTOs = AllServicesDTO.GetAll<MemberRoleDTO>();
            Assert.AreEqual(2, memberRoles.Count());

            memberRoleDTO = AllServicesDTO.UpdateValues<MemberRoleDTO>(new MemberRoleDTO() { Id = memberRoleDTO.Id, Title = "New MemberRole 2" }, new List<string>() { nameof(MemberRoleDTO.Title) });
            memberRoleDTOs = AllServicesDTO.GetAll<MemberRoleDTO>();
            Assert.AreEqual(memberRoleDTO.Title, "New MemberRole 2");

            AllServicesDTO.DeleteById<MemberRoleDTO>(memberRoleDTO.Id);
            memberRoleDTOs = AllServicesDTO.GetAll<MemberRoleDTO>();
            Assert.AreEqual(1, memberRoleDTOs.Count);
        }

        /// <summary>
        /// Check Access Right
        /// </summary>
        [TestMethod]
        public void Site_CRUD_Check_Right_UserCannotUpadate()
        {
            #region Build Data

            // User
            User connectedUser = new User() { Id = 1, Login = "tester" };
            UserInfoContainer.SetUserInfo(connectedUser.Id, connectedUser.Login, new List<string>() { "User" });
            int SiteReadId = 2;
            Mock<$saferootprojectname$DBContainer> mockContext = PrepareMockForRigthTest(connectedUser);

            #endregion Build Data
            TDBContainer<$saferootprojectname$DBContainer>.SetMoqContext(mockContext.Object);


            string newTitle = "New Title";
            // BEGIN USER

            var siteDTOs = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(2, siteDTOs.Count);

            //Update should not work due to right
            var siteDTO = AllServicesDTO.UpdateValues<SiteDTO>(new SiteDTO() { Id = SiteReadId, Title = newTitle }, new List<string>() { nameof(SiteDTO.Title) });
            Assert.IsNull(siteDTO);
            siteDTO = AllServicesDTO.Find<SiteDTO>(SiteReadId);
            Assert.IsNotNull(siteDTO);
            //Verify that Title do not change
            Assert.AreNotEqual("New Title", siteDTO.Title);
            // END USER
        }
        /// <summary>
        /// Check Access Right
        /// </summary>
        [TestMethod]
        public void Site_CRUD_Check_Right_AdminCanUpadate()
        {
            #region Build Data

            // User
            User connectedUser = new User() { Id = 1, Login = "tester" };
            UserInfoContainer.SetUserInfo(connectedUser.Id, connectedUser.Login, new List<string>() { Constants.RoleAdmin });
            int SiteReadId = 2;
            Mock<$saferootprojectname$DBContainer> mockContext = PrepareMockForRigthTest(connectedUser);

            #endregion Build Data
            TDBContainer<$saferootprojectname$DBContainer>.SetMoqContext(mockContext.Object);

            string newTitle = "New Title";

            // BEGIN ADMIN
            var siteDTOs = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(3, siteDTOs.Count);

            var siteDTO = AllServicesDTO.UpdateValues<SiteDTO>(new SiteDTO() { Id = SiteReadId, Title = newTitle }, new List<string>() { nameof(SiteDTO.Title) });
            Assert.IsNotNull(siteDTO);
            Assert.AreEqual(newTitle, siteDTO.Title);
            siteDTO = AllServicesDTO.Find<SiteDTO>(SiteReadId);
            Assert.AreEqual(newTitle, siteDTO.Title);

            AllServicesDTO.DeleteById<SiteDTO>(SiteReadId);
            siteDTOs = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(2, siteDTOs.Count);
            // END ADMIN

        }

        private static Mock<$saferootprojectname$DBContainer> PrepareMockForRigthTest(User connectedUser)
        {

            // MemberRole
            var memberRoleSiteAdmin = new MemberRole() { Id = (int)Constants.RoleSiteAdminId, Title = Constants.RoleSiteAdmin };
            var memberRoleOther = new MemberRole() { Id = 2, Title = "Other Role" };

            // Site
            Site siteReadWrite = new Site() { Id = 1, Title = "SiteReadWrite" };
            Site siteRead = new Site() { Id = 2, Title = "SiteRead" };
            Site siteUnauthorize = new Site() { Id = 3, Title = "SiteUnauthorize" };

            //Member
            Member memberReadWrite = new Member() { Id = 1, User = connectedUser, MemberRole = new List<MemberRole>() { memberRoleSiteAdmin }, Site = siteReadWrite };
            Member memberRead = new Member() { Id = 2, User = connectedUser, MemberRole = new List<MemberRole>() { memberRoleOther }, Site = siteRead };

            siteReadWrite.Members = new List<Member>() { memberReadWrite };
            siteRead.Members = new List<Member>() { memberRead };

            List<Site> sites = new List<Site>() { siteReadWrite, siteRead, siteUnauthorize };

            var mockSiteSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Site>()
                                                            .SetupForQueryOn(sites)
                                                            .WithAdd(sites, "Id") // overwritten to simulate behavior of auto-increment database
                                                            .WithFind(sites, "Id")
                                                            .WithRemove(sites);
            return EntityFrameworkMoqHelper.CreateMockForDbContext<$saferootprojectname$DBContainer, Site>(mockSiteSet);
        }
    }
}
