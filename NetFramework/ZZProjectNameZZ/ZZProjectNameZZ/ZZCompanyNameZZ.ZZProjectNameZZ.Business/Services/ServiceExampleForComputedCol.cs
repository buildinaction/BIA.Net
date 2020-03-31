// <copyright file="ServiceMember.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services
{
    using BIA.Net.Business.Helpers;
    using BIA.Net.Business.Services;
    using BIA.Net.Business.Specifications;
    using BIA.Net.Common;
    using BIA.Net.DataTable.DTO;
    using Business.DTO;
    using Common;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Linq.Expressions;
    using static BIA.Net.Business.Services.AllServicesDTO;

    /// <summary>
    /// Service to manipulate Member
    /// </summary>
    public class ServiceExampleForComputedCol : TServiceDTO<ExampleTable2CompColDTO, ExampleTable2, ZZProjectNameZZDBContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExampleForComputedCol"/> class.
        /// </summary>
        public ServiceExampleForComputedCol()
        {
        }

        /// <summary>
        /// Return Expression used for sort
        /// </summary>
        /// <param name="colName">name of the column</param>
        /// <returns>Value expression</returns>
        protected override Expression<Func<ExampleTable2, object>> Col2Value(string colName)
        {
            if (colName == "ComputedCol")
            {
                return x => x.Title + "(" + x.Site.Title + ") - " + x.Description;
            }

            return null;
        }

        /// <summary>
        /// Return Expression used for search
        /// </summary>
        /// <param name="colName">name of the column</param>
        /// <param name="search">search value</param>
        /// <returns>boolean search expression</returns>
        protected override Expression<Func<ExampleTable2, bool>> Col2Search(string colName, string search)
        {
            if (colName == "ComputedCol")
            {
                return x => (x.Title + "(" + x.Site.Title + ") - " + x.Description).Contains(search);
            }

            return null;
        }
    }
}