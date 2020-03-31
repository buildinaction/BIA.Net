// <copyright file="SiteSpecification.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.SpecBuilder
{
    using BIA.Net.Business.Specifications;
    using Business.CTO;
    using Model;
    using System.Linq;

    /// <summary>
    /// The specifications of the site controller.
    /// </summary>
    public class SiteAdvancedFilterSpecBuilder : ISpecBuilder<Site, SiteAdvancedFilterCTO>
    {
        /// <summary>
        /// Search site using the filter.
        /// </summary>
        /// <param name="advancedFilter">The filter.</param>
        /// <returns>The specification.</returns>
        public Specification<Site> BuildSpec(SiteAdvancedFilterCTO advancedFilter)
        {
            Specification<Site> specification = new TrueSpecification<Site>();

            if (advancedFilter.FilterTitle != null && advancedFilter.FilterTitle.Any())
            {
                specification &= new DirectSpecification<Site>(s => advancedFilter.FilterTitle.Contains(s.Title));
            }

            if (advancedFilter.FilterMember != null && advancedFilter.FilterMember.Any())
            {
                specification &= new DirectSpecification<Site>(s => s.Members.Any(a => advancedFilter.FilterMember.Contains(a.User.LastName + " " + a.User.FirstName)));
            }

            return specification;
        }
    }
}