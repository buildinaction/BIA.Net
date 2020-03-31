namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services
{
    using BIA.Net.Business.Services;
    using Business.DTO;
    using Model;
    using System.Collections.Generic;

    /// <summary>
    /// Service Example For Stored Procedure
    /// </summary>
    public class ServiceExampleForStoredProcedure : TService<ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// Get list of users by company
        /// </summary>
        /// <param name="companyName">company name</param>
        /// <param name="externalCompanyName">external company name</param>
        /// <returns>list of <see cref="UserDTO"/></returns>
        public List<UserDTO> GetExampleUsersByCompany(string companyName = null, string externalCompanyName = null)
        {
            Model.DAL.StoredProcedure.GetExampleUsersByCompany sp = new Model.DAL.StoredProcedure.GetExampleUsersByCompany();

            sp.CompanyName = companyName;
            sp.ExternalCompanyName = externalCompanyName;

            return DBContainer.ExecuteProcedureReader<UserDTO>(sp);
        }
    }
}