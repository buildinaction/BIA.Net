// <copyright file="ViewType.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.ViewModule.Aggregate
{
    /// <summary>
    /// The type of view.
    /// </summary>
    public enum ViewType
    {
        /// <summary>
        /// The default view type.
        /// </summary>
        System,

        /// <summary>
        /// The site view.
        /// </summary>
        Site,

        /// <summary>
        /// The user view.
        /// </summary>
        User,
    }
}