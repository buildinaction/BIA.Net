namespace ZZCompanyNameZZ.ZZProjectNameZZ.Model.DAL.StoredProcedure
{
    using BIA.Net.Model.DAL;

    using System.Collections.Generic;

    /// <summary>
    /// Stored procedure GetUserByCompany
    /// </summary>
    public class GetExampleUsersByCompany : StoredProcedureParameter
    {
        /// <inheritdoc/>
        public override string Name
        {
            get
            {
                return typeof(GetExampleUsersByCompany).Name;
            }
        }

        /// <inheritdoc/>
        public override Dictionary<string, object> Parameters
        {
            get
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                parameters.Add("Company", this.CompanyName);
                parameters.Add("ExternalCompany", this.ExternalCompanyName);

                return parameters;
            }
        }

        /// <summary>
        /// Gets or sets CompanyName
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets ExternalCompanyName
        /// </summary>
        public string ExternalCompanyName { get; set; }
    }
}