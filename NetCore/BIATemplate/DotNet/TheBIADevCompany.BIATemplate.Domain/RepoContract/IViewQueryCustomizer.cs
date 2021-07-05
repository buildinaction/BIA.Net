// <copyright file="IViewQueryCustomizer.cs" company="TheBIADevCompany">
//     Copyright (c) TheBIADevCompany. All rights reserved.
// </copyright>

namespace TheBIADevCompany.BIATemplate.Domain.RepoContract
{
    using BIA.Net.Core.Domain.RepoContract.QueryCustomizer;
    using TheBIADevCompany.BIATemplate.Domain.ViewModule.Aggregate;

    /// <summary>
    /// interface use to customize the request on Member entity.
    /// </summary>
    public interface IViewQueryCustomizer : IQueryCustomizer<View>
    {
    }
}
