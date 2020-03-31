namespace $safeprojectname$.Helpers
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using BIA.Net.Model.DAL;

    using EntityFramework.MoqHelper;

    using Moq;

    using $companyName$.$saferootprojectname$.Model;

    class MockEntityFrameWork
    {
        //Main Container
        private Mock<$saferootprojectname$DBContainer> mockContext;

        //Mock DB Set
        private Mock<DbSet<Site>> mockSiteSet;
        private Mock<DbSet<Member>> mockMemberSet;

        //DataStorage of mock
        private List<Site> sites;
        private List<Member> members;


        public MockEntityFrameWork()
        {
            //Init All DataStorage
            this.sites = new List<Site>();
            this.members = new List<Member>();
            //Create mock for all dbSet
            this.mockSiteSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Site>().SetupForQueryOn(this.sites)
                .WithAdd(this.sites, "Id") // overwritten to simulate behavior of auto-increment database
                .WithFind(this.sites, "Id").WithRemove(this.sites);

            this.mockMemberSet = EntityFrameworkMoqHelper.CreateMockForDbSet<Member>().SetupForQueryOn(this.members)
                .WithAdd(this.members, "Id") // overwritten to simulate behavior of auto-increment database
                .WithFind(this.members, "Id").WithRemove(this.members);


            this.mockContext = EntityFrameworkMoqHelper.CreateMockForDbContext<$saferootprojectname$DBContainer, Site>(this.mockSiteSet);
            this.mockContext.Setup(c => c.Set<Member>()).Returns(this.mockMemberSet.Object);

            this.mockSiteSet.Setup(x => x.AsNoTracking()).Returns(this.mockSiteSet.Object);
            this.mockMemberSet.Setup(x => x.AsNoTracking()).Returns(this.mockMemberSet.Object);

            TDBContainer<$saferootprojectname$DBContainer>.SetMoqContext(this.mockContext.Object);
        }

        public void InitialiseSite()
        {
            this.sites.Add(new Site { Id = 1, Title = "Villemure" });
        }
    }
}
