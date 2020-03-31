namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.SpecBuilder
{
    using BIA.Net.Business.Specifications;
    using Business.CTO;
    using Model;
    using System.Linq;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class ExampleTable2FACompColAdvancedFilterSpecBuilder : ISpecBuilder<ExampleTable2, ExampleTable2FACompColAdvancedFilterCTO>
    {
        /// <summary>
        /// Search ExampleTable2 using the filter.
        /// </summary>
        /// <param name="advancedFilter">The filter.</param>
        /// <returns>The specification.</returns>
        public Specification<ExampleTable2> BuildSpec(ExampleTable2FACompColAdvancedFilterCTO advancedFilter)
        {
            Specification<ExampleTable2> specification = new TrueSpecification<ExampleTable2>();
            if (advancedFilter.FilterTitle != null && advancedFilter.FilterTitle.Any())
            {
                specification &= new DirectSpecification<ExampleTable2>(entity => advancedFilter.FilterTitle.Contains(entity.Title.ToString()));
            }

            if (advancedFilter.FilterDescription != null && advancedFilter.FilterDescription.Any())
            {
                specification &= new DirectSpecification<ExampleTable2>(entity => advancedFilter.FilterDescription.Contains(entity.Description.ToString()));
            }

            if (advancedFilter.FilterSite != null && advancedFilter.FilterSite.Any())
            {
                specification &= new DirectSpecification<ExampleTable2>(entity => advancedFilter.FilterSite.Contains((entity.Site == null) ? string.Empty : entity.Site.Title));
            }

            return specification;
        }
    }
}