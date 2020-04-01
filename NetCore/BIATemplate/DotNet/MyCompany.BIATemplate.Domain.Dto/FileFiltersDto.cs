// <copyright file="FileFiltersDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIATemplate.Domain.Dto
{
    using System.Collections.Generic;
    using MyCompany.BIATemplate.Domain.Dto.Bia;

    /// <summary>
    /// The Dto use to generate a file (csv for example).
    /// </summary>
    public class FileFiltersDto : LazyLoadDto
    {
        /// <summary>
        /// Name of the property and her translation.
        /// </summary>
        public Dictionary<string, string> Columns { get; set; }
    }
}
