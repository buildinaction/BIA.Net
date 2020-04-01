// <copyright file="ViewDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto.View
{
    /// <summary>
    /// The DTO used for views.
    /// </summary>
    public class ViewDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the table Id.
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the view description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets view type.
        /// </summary>
        public int ViewType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this views is the default one for the site.
        /// </summary>
        public bool IsSiteDefault { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this views is the default one for the user.
        /// </summary>
        public bool IsUserDefault { get; set; }

        /// <summary>
        /// Gets or sets the table preference.
        /// </summary>
        public string Preference { get; set; }
    }
}