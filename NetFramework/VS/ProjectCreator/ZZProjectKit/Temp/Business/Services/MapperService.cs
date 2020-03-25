// <copyright file="MapperServiceDTO.cs" company="$companyName$">
// Copyright (c) $companyName$. All rights reserved.
// </copyright>

namespace $safeprojectname$.Services
{
    using BIA.Net.Business.Services;
    using Business.DTO;
    using Model;
    using System;
    using System.Collections.Generic;
    using static BIA.Net.Business.Services.MapperServiceDTO;

    /// <summary>
    /// Class used to make a mapping between DTO, service and mapper
    /// </summary>
    public static class MapperService
    {
        /// <summary>
        /// InitMapping
        /// </summary>
        public static void InitMapping()
        {
            MapperServiceDTO.serviceMapping = new Dictionary<Type, TypeMapper>
            {
                // Add here your project mapping
                { typeof(SiteDTO),            new TypeMapper(typeof(ServiceSite),                             typeof(SiteMapper)) },
                { typeof(MemberDTO),          new TypeMapper(typeof(ServiceMember),                           typeof(MemberMapper)) },
                { typeof(UserDTO),     new TypeMapper(typeof(ServiceUser),                      typeof(UserMapper)) },
                { typeof(MemberRoleDTO),      new TypeMapper(typeof(TServiceDTO<MemberRoleDTO, MemberRole, $saferootprojectname$DBContainer>),  typeof(MemberRoleMapper)) },
                { typeof(ExampleTable3DTO),      new TypeMapper(typeof(TServiceDTO<ExampleTable3DTO, ExampleTable3, $saferootprojectname$DBContainer>),  typeof(ExampleTable3Mapper)) }

                // End of project mapping
            };
        }
    }
}