namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers
{
    using BIA.Net.Authentication.Business.Synchronize;
    using BIA.Net.Business.Services;
    using BIA.Net.Common.Helpers;
    using Business.Services;
    using Business.Services.Authentication;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Configure unity in Model layer
    /// </summary>
    public static class UnityConfigBusiness
    {
        /// <summary>Registers the type mappings with the Unity container.</summary>
        public static void RegisterTypes()
        {
            MapperService.InitMapping();

            List<Type> listService = typeof(UnityConfigBusiness).Assembly.GetTypes().ToList();
            listService = listService.Where(t => t.GetInterfaces().Any(i => i.Name == "TServiceDTO`3" || i.Name == "ISpecBuilder`2")).ToList();
            listService = listService.Union(MapperServiceDTO.ServiceMapping.Select(sm => sm.Value.ServiceType)).ToList();
            listService = listService.Union(MapperServiceDTO.SpecBuilderMapping.Select(sm => sm.Value)).ToList();
            foreach (var serviceType in listService)
            {
                BIAUnity.RegisterType(serviceType);
            }

            BIAUnity.RegisterType<IServiceSynchronizeUser, ServiceSynchronizeUser>();
        }
    }
}