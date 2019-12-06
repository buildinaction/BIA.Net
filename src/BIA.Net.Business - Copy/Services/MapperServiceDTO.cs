// <copyright file="MapperServiceDTO.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Business.Services
{
    using BIA.Net.Business.DTO.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// Class used to make a mapping between DTO, service and mapper
    /// </summary>
    public static class MapperServiceDTO
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static readonly object SyncLock = new object();

        /// <summary>
        /// The service mapping
        /// </summary>
        public static Dictionary<Type, TypeMapper> serviceMapping = null;

        /// <summary>
        /// The mapper container
        /// </summary>
        private static Dictionary<Type, object> mapperContainer = new Dictionary<Type, object>();

        /// <summary>
        /// Gets the service mapping.
        /// </summary>
        /// <value>
        /// The service mapping.
        /// </value>
        public static Dictionary<Type, TypeMapper> ServiceMapping
        {
            get { return serviceMapping; }
        }

        /// <summary>
        /// Gets the mapper corresponding to the DTO.
        /// </summary>
        /// <typeparam name="Entity">The type of the ntity.</typeparam>
        /// <typeparam name="DTO">The type of to.</typeparam>
        /// <returns>The mapper corresponding to the DTO</returns>
        public static MapperBase<Entity, DTO> GetMapper<Entity, DTO>()
        {
            Type mapperType = MapperServiceDTO.ServiceMapping[typeof(DTO)].MapperType;
            if (!mapperContainer.Keys.Contains(mapperType))
            {
                lock (SyncLock)
                {
                    if (!mapperContainer.Keys.Contains(mapperType))
                    {
                        mapperContainer.Add(mapperType, Activator.CreateInstance(mapperType));
                    }
                }
            }

            return (MapperBase<Entity, DTO>)mapperContainer[mapperType];
        }

        /// <summary>
        /// use to contain the mapping for MapperServiceDTO
        /// </summary>
        public class TypeMapper
        {
            /// <summary>
            /// the iservice type
            /// </summary>
            private Type iServiceType;

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeMapper"/> class.
            /// </summary>
            /// <param name="pServiceType">Type of the p service.</param>
            /// <param name="pMapperType">Type of the p mapper.</param>
            public TypeMapper(Type pServiceType, Type pMapperType)
            {
                ServiceType = pServiceType;
                MapperType = pMapperType;
            }

            /// <summary>
            /// Gets the type of the service.
            /// </summary>
            /// <value>
            /// The type of the service.
            /// </value>
            public Type ServiceType { get; }
            /*
            /// <summary>
            /// Gets the type of the iService.
            /// </summary>
            public Type IServiceType
            {
                get
                {
                    if (iServiceType == null)
                    {
                        iServiceType = GetIservice(ServiceType);
                        Debug.Assert(iServiceType != null, "Problem to find interface for : " + this.ServiceType.Name);
                    }

                    return iServiceType;
                }
            }

            public static Type GetIservice(Type ServiceType)
            {
                Type[] interfaces = ServiceType.GetInterfaces();
                return interfaces.Last();
            }*/

            /// <summary>
            /// Gets the type of the mapper.
            /// </summary>
            /// <value>
            /// The type of the mapper.
            /// </value>
            public Type MapperType { get; }
        }
    }
}