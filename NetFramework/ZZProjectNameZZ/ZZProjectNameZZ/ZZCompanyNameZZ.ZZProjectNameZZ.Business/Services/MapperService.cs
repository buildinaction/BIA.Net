// <copyright file="MapperServiceDTO.cs" company="ZZCompanyNameZZ">
// Copyright (c) ZZCompanyNameZZ. All rights reserved.
// </copyright>

namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services
{
    using BIA.Net.Business.DTO;
    using BIA.Net.Business.Services;
    using Business.DTO;
    using CTO;
    using Model;
    using SpecBuilder;
    using System;
    using System.Collections.Generic;
    using static BIA.Net.Business.Services.MapperServiceDTO;

    /// <summary>
    /// Class used to make a mapping between DTO, service and mapper
    /// </summary>
    public static class MapperService
    {
        /// <summary>
        /// Singleton lock
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        /// InitMapping
        /// </summary>
        public static void InitMapping()
        {
            Dictionary<Type, TypeMapper> serviceMappings = new Dictionary<Type, TypeMapper>
            {
                // Add here your project mapping DTO to Service and Mapper
                { typeof(SiteDTO),                  new TypeMapper(typeof(ServiceSite),                     typeof(SiteMapper)) },
                { typeof(MemberDTO),                new TypeMapper(typeof(ServiceMember),                   typeof(MemberMapper)) },
                { typeof(UserDTO),                  new TypeMapper(typeof(ServiceUser),                     typeof(UserMapper)) },
                { typeof(MemberRoleDTO),            new TypeMapper(typeof(ServiceMemberRole),               typeof(MemberRoleMapper)) },
                { typeof(ViewDTO),                  new TypeMapper(typeof(ServiceView),                     typeof(ViewMapper)) },
                { typeof(SiteViewDTO),              new TypeMapper(typeof(ServiceSiteView),                 typeof(SiteViewMapper)) },
                { typeof(UserViewDTO),              new TypeMapper(typeof(ServiceUserView),                 typeof(UserViewMapper)) },

                // { typeof(ExampleTable1DTO), new TypeMapper(typeof(TServiceDTO<ExampleTable1DTO, ExampleTable1, ZZProjectNameZZDBContainer>), typeof(ExampleTable1Mapper)) },
                // { typeof(ExampleTable2DTO), new TypeMapper(typeof(TServiceDTO<ExampleTable2DTO, ExampleTable2, ZZProjectNameZZDBContainer>), typeof(ExampleTable2Mapper)) },
                { typeof(ExampleTable3DTO), new TypeMapper(typeof(TServiceDTO<ExampleTable3DTO, ExampleTable3, ZZProjectNameZZDBContainer>), typeof(ExampleTable3Mapper)) },
                { typeof(ExampleTable2CompColDTO), new TypeMapper(typeof(ServiceExampleForComputedCol), typeof(ExampleTable2CompColMapper)) },
            };
            Dictionary<Type, Type> specificationMappings = new Dictionary<Type, Type>
            {
                // Add here your project mapping CTO > Specification
                { typeof(SiteAdvancedFilterCTO), typeof(SiteAdvancedFilterSpecBuilder) },
                { typeof(ExampleAjaxAdvancedFilterCTO), typeof(ExampleAjaxAdvancedFilterSpecBuilder) },
                { typeof(ExampleFullAjaxAdvancedFilterCTO), typeof(ExampleFullAjaxAdvancedFilterSpecBuilder) },
            };

            lock (SyncLock)
            {
                MapperServiceDTO.serviceMapping = MapperServiceDTO.serviceMapping ?? new Dictionary<Type, TypeMapper>();

                foreach (var serviceMapping in serviceMappings)
                {
                    if (!MapperServiceDTO.serviceMapping.ContainsKey(serviceMapping.Key))
                    {
                        MapperServiceDTO.serviceMapping.Add(serviceMapping.Key, serviceMapping.Value);
                    }
                }

                MapperServiceDTO.specBuilderMapping = MapperServiceDTO.specBuilderMapping ?? new Dictionary<Type, Type>();

                foreach (var specificationMapping in specificationMappings)
                {
                    if (!MapperServiceDTO.specBuilderMapping.ContainsKey(specificationMapping.Key))
                    {
                        MapperServiceDTO.specBuilderMapping.Add(specificationMapping.Key, specificationMapping.Value);
                    }
                }
            }
        }
    }
}