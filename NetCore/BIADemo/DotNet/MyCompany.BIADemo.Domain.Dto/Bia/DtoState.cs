// <copyright file="DtoState.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.Bia
{
    /// <summary>
    /// The state of a DTO.
    /// </summary>
    public enum DtoState
    {
        /// <summary>
        /// The DTO is unchanged.
        /// </summary>
        Unchanged,

        /// <summary>
        /// The DTO is new and marked to be added to the context.
        /// </summary>
        Added,

        /// <summary>
        /// The DTO has been modified.
        /// </summary>
        Modified,

        /// <summary>
        /// The DTO is marked to be deleted in the context.
        /// </summary>
        Deleted,
    }
}