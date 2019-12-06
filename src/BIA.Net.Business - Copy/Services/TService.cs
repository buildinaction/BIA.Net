namespace BIA.Net.Business.Services
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.Model.DAL;
    using System.Data.Entity;

    /// <summary>
    /// Generic service to access database/storedProcedure and translate them in DTO
    /// </summary>
    /// <typeparam name="DBContext">The Entity framework DB context of the entity.</typeparam>
    public class TService<DBContext> 
         where DBContext : DbContext, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TService{DBContext}"/> class.
        /// </summary>
        public TService()
        {
        }

        /// <summary>
        /// The dbContainer
        /// </summary>
        private TDBContainer<DBContext> dbContainer = null;

        /// <summary>
        /// Gets the repository
        /// </summary>
        protected TDBContainer<DBContext> DBContainer
        {
            get
            {
                if (dbContainer == null)
                {
                    dbContainer = BIAUnity.Resolve<TDBContainer<DBContext>>();
                }

                return dbContainer;
            }
        }
    }
}