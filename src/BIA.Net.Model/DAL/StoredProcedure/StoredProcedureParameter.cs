// <copyright file="StoredProcedureParameter.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System.Collections.Generic;

    /// <summary>
    /// StoredProcedure Parameter
    /// </summary>
    public abstract class StoredProcedureParameter
    {
        /// <summary>
        /// Gets name of stored procedure
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets list of input parameters
        /// </summary>
        public abstract Dictionary<string, object> Parameters { get; }
    }

    // Example of implementation
#pragma warning disable SA1005 // SingleLineCommentsMustBeginWithSingleSpace

    ///// <summary>
    ///// Stored procedure GetAspNetUsersByCompany
    ///// </summary>
    //public class GetAspNetUsersByCompany : StoredProcedureParameter
    //{
    //    /// <inheritdoc/>
    //    public override string Name
    //    {
    //        get
    //        {
    //            return typeof(GetAspNetUsersByCompany).Name;
    //        }
    //    }

    //    /// <inheritdoc/>
    //    public override Dictionary<string, object> Parameters
    //    {
    //        get
    //        {
    //            var parameters = new Dictionary<string, object>();

    //            if (!string.IsNullOrWhiteSpace(this.CompanyName))
    //            {
    //                parameters.Add("Company", this.CompanyName);
    //            }

    //            if (!string.IsNullOrWhiteSpace(this.ExternalCompanyName))
    //            {
    //                parameters.Add("ExternalCompany", this.ExternalCompanyName);
    //            }

    //            return parameters;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets or sets CompanyName
    //    /// </summary>
    //    public string CompanyName { get; set; }

    //    /// <summary>
    //    /// Gets or sets ExternalCompanyName
    //    /// </summary>
    //    public string ExternalCompanyName { get; set; }
    //}

    //  ALTER PROCEDURE[dbo].[GetAspNetUsersByCompany]
    //  @Company nvarchar(50),
    //  @ExternalCompany nvarchar(50) = NULL
    //  AS
    //  BEGIN
    //    SET NOCOUNT ON;
    //    SELECT *
    //    FROM [dbo].[AspNetUsers]
    //    WHERE [Company] = @Company AND
    //    (@ExternalCompany IS NULL OR [ExternalCompany] = @ExternalCompany)
    //  END
#pragma warning restore SA1005 // SingleLineCommentsMustBeginWithSingleSpace
}
