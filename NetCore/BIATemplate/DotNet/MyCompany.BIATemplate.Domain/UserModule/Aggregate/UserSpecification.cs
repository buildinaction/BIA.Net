// <copyright file="UserSpecification.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.UserModule.Aggregate
{
    using BIA.Net.Specification;

    /// <summary>
    /// The specifications of the user entity.
    /// </summary>
    public static class UserSpecification
    {
        /// <summary>
        /// Search users using the filter on lastname, firstname or login.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The specification.</returns>
        public static Specification<User> Search(string filter)
        {
            Specification<User> specification = new TrueSpecification<User>();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                specification &= new DirectSpecification<User>(u =>
                    u.LastName.Contains(filter) || u.FirstName.Contains(filter) || u.Login.Contains(filter));
            }

            specification &= SearchActive();

            return specification;
        }

        /// <summary>
        /// Search users that are active and DAIEnable.
        /// </summary>
        /// <returns>The specification.</returns>
        public static Specification<User> SearchActive()
        {
            Specification<User> specification = new TrueSpecification<User>();

            specification &= new DirectSpecification<User>(u => u.IsActive);

            return specification;
        }
    }
}