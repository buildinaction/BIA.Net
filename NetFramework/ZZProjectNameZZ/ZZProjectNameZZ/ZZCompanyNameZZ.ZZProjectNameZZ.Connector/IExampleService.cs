// <copyright file="IExampleService.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Connector
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Business.DTO;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IExampleService" in both code and config file together.

    /// <summary>
    /// IExampleService
    /// </summary>
    [ServiceContract]
    public interface IExampleService
    {
        /// <summary>
        /// return true if ok.
        /// </summary>
        /// <returns>true if ok</returns>
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Get all User
        /// </summary>
        /// <returns>list of <see cref="UserDTO"/></returns>
        [OperationContract]
        List<UserDTO> GetExampleAllUser();

        /// <summary>
        /// Get list of users by company
        /// </summary>
        /// <param name="companyName">company name</param>
        /// <param name="externalCompanyName">external company name</param>
        /// <returns>list of <see cref="UserDTO"/></returns>
        [OperationContract]
        List<UserDTO> GetExampleUsersByCompany(string companyName, string externalCompanyName);

        // TODO: Add your service operations here
    }
}
