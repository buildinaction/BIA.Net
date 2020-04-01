// <copyright file="MemberSpecification.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Aggregate
{
    using BIA.Net.Specification;
    using MyCompany.BIATemplate.Domain.Dto.User;

    /// <summary>
    /// The specifications of the member entity.
    /// </summary>
    public static class MemberSpecification
    {
        /// <summary>
        /// Search member using the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchGetAll(MemberFilterDto filter)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (filter.SiteId != 0)
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.SiteId == filter.SiteId);
            }

            return specification;
        }

        /// <summary>
        /// Search member for login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The specification.</returns>
        public static Specification<Member> SearchForLogin(string login)
        {
            Specification<Member> specification = new TrueSpecification<Member>();

            if (!string.IsNullOrWhiteSpace(login))
            {
                specification &= new DirectSpecification<Member>(s =>
                    s.User.Login == login);
            }

            return specification;
        }
    }
}