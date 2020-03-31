// <copyright file="ExampleFullAjaxSpecification.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.SpecBuilder
{
    using BIA.Net.Business.Specifications;
    using Business.CTO;
    using Model;
    using System.Linq;

    /// <summary>
    /// The specifications of the Example Full Ajax controller.
    /// </summary>
    public class ExampleFullAjaxAdvancedFilterSpecBuilder : ISpecBuilder<ExampleTable3, ExampleFullAjaxAdvancedFilterCTO>
    {
        /// <summary>
        /// Search exemple 3 using the filter.
        /// </summary>
        /// <param name="advancedFilter">The filter.</param>
        /// <returns>The specification.</returns>
        public Specification<ExampleTable3> BuildSpec(ExampleFullAjaxAdvancedFilterCTO advancedFilter)
        {
            Specification<ExampleTable3> specification = new TrueSpecification<ExampleTable3>();
            if (advancedFilter != null)
            {
                // Manage the advanced filter panel associated to the grid
                if (advancedFilter.FilterTitle != null && advancedFilter.FilterTitle.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterTitle.Contains(s.Title));
                }

                if (advancedFilter.FilterDescription != null && advancedFilter.FilterDescription.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterDescription.Contains(s.Description));
                }

                if (advancedFilter.FilterValue != null && advancedFilter.FilterValue.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterValue.Contains(s.Value.ToString()));
                }

                if (advancedFilter.FilterMyDecimal != null && advancedFilter.FilterMyDecimal.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterMyDecimal.Contains(s.MyDecimal.ToString()));
                }

                if (advancedFilter.FilterMyDouble != null && advancedFilter.FilterMyDouble.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterMyDouble.Contains(s.MyDouble.ToString()));
                }

                if (advancedFilter.FilterDateOnly != null && advancedFilter.FilterDateOnly.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterDateOnly.Contains(s.DateOnly.ToString()));
                }

                if (advancedFilter.FilterDateAndTime != null && advancedFilter.FilterDateAndTime.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterDateAndTime.Contains(s.DateAndTime.ToString()));
                }

                if (advancedFilter.FilterMyTimeSpan != null && advancedFilter.FilterMyTimeSpan.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterMyTimeSpan.Contains(s.MyTimeSpan.ToString()));
                }

                if (advancedFilter.FilterTimeSpanOver24H != null && advancedFilter.FilterTimeSpanOver24H.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterTimeSpanOver24H.Contains(s.TimeSpanOver24H.ToString()));
                }

                if (advancedFilter.FilterSite != null && advancedFilter.FilterSite.Any())
                {
                    specification &= new DirectSpecification<ExampleTable3>(s => advancedFilter.FilterSite.Contains(s.Site.Title));
                }
            }

            return specification;
        }
    }
}