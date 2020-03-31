// <copyright file="BaseDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.Bia
{
    /// <summary>
    /// The base class for DTO.
    /// </summary>
    public class BaseDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the state of the DTO regarding to the DB context.
        /// </summary>
        public DtoState DtoState { get; set; }
    }
}