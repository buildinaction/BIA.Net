namespace Safran.eScrap.Test.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using BIA.Net.Business.Services;
    using BIA.Net.Model.DAL;

    using EntityFramework.MoqHelper;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    

    using $companyName$.$saferootprojectname$.Business.DTO;
    using $companyName$.$saferootprojectname$.Common;
    using $companyName$.$saferootprojectname$.MVC.Controllers;
    using $safeprojectname$.Helpers;

    [TestClass]
    public class SiteControllerTests
    {
        private SiteController controllerSite;

        private MockEntityFrameWork MEF;

        public SiteControllerTests()
        {
            UnityConfig.RegisterTypes();
        }

        [TestMethod]
        public void DeleteSiteByController()
        {
            UserInfoContainer.SetUserInfo(0, string.Empty, new List<string> { Constants.RoleAdmin });

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((HttpStatusCodeResult)this.controllerSite.Delete(null)).StatusCode);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((HttpStatusCodeResult)this.controllerSite.Delete(1)).StatusCode);
            
            // Data
            this.MEF.InitialiseSite();

            // Assert
            var siteDtos = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(1, siteDtos.Count());

            // Act

            this.controllerSite.Delete(1);
            this.controllerSite.DeleteConfirmed(1);

            // Assert
            siteDtos = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(0, siteDtos.Count());
        }

        [TestMethod]
        public void UpdateSiteByController()
        {
            UserInfoContainer.SetUserInfo(0, string.Empty, new List<string> { Constants.RoleAdmin });

            Assert.AreEqual((int)HttpStatusCode.BadRequest, ((HttpStatusCodeResult)this.controllerSite.Edit(new int?())).StatusCode);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((HttpStatusCodeResult)this.controllerSite.Edit(1)).StatusCode);

            // Data
            this.MEF.InitialiseSite();

            // Assert
            var siteDtos = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(1, siteDtos.Count());

            // Act

            PartialViewResult listSites = (PartialViewResult)this.controllerSite._List(null);
            Assert.AreEqual(1, ((IEnumerable<SiteDTO>)listSites.Model).Count());

            ViewResult selectOneSite = (ViewResult)this.controllerSite.Details(1);
            Assert.IsNotNull(selectOneSite.Model);

            ViewResult result = (ViewResult)this.controllerSite.Edit(1);
            SiteDTO selectedSite = (result.Model as SiteDTO);
            selectedSite.Title = "Auch";

            var resultEdit = this.controllerSite.Edit(selectedSite) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultEdit);
            Assert.AreEqual("Index", resultEdit.RouteValues["action"]);

            ViewResult editedSite = (ViewResult)this.controllerSite.Details(1);
            Assert.AreEqual("Auch", (editedSite.Model as SiteDTO).Title);

        }

        [TestMethod]
        public void InsertSiteByController()
        {
            // Data 
            SiteDTO siteDTO = new SiteDTO();
            siteDTO.Title = "Villemure";


            // Act
            var result = this.controllerSite.Create(siteDTO) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);

            var siteDTOs = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(1, siteDTOs.Count());
        }

        [TestInitialize]
        public void TestInit()
        {

            MEF = new MockEntityFrameWork();
            UserInfoContainer.SetUserInfo(0, string.Empty, new List<string> { Constants.RoleAdmin });
            var mockContextHttp = new MockContext();
            var controllerContext = new ControllerContext { HttpContext = mockContextHttp.Http.Object };
            this.controllerSite = new SiteController { ControllerContext = controllerContext };
        }

        [TestMethod]
        public void UpdateSiteByService()
        {
            UserInfoContainer.SetUserInfo(0, string.Empty, new List<string> { Constants.RoleAdmin });

            this.controllerSite.Create(new SiteDTO { Title = "Villemur" });
            var siteDTO = AllServicesDTO.GetAll<SiteDTO>().FirstOrDefault();
            siteDTO.Title = "Auch";
            this.controllerSite.Edit(siteDTO);
            var siteDTOs = AllServicesDTO.GetAll<SiteDTO>();
            Assert.AreEqual(siteDTOs.FirstOrDefault().Title, "Auch");
        }
    }
}