using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

namespace BIA.Net.Model.DAL
{
    public sealed class EntityPropHelper
    {
        private static readonly Lazy<EntityPropHelper> LazyInstance = new Lazy<EntityPropHelper>(() => new EntityPropHelper());
        private static readonly Dictionary<Type, string[]> Dict = new Dictionary<Type, string[]>();
        private static readonly Dictionary<Type, string[]> DictNavProp = new Dictionary<Type, string[]>();
        private EntityPropHelper() { }

        public static EntityPropHelper Instance
        {
            get { return LazyInstance.Value; }
        }

        /// <summary>
        /// Gets the properties of the entity.
        /// </summary>
        /// <typeparam name="Entity">The type of the ntity.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="dbobj">The dbobj.</param>
        /// <returns>Returns the list of properties</returns>
        public static string[] GetProperties<Entity>(DbContext context, Entity dbobj, bool isMock = false)
            where Entity : class
        {
            Type t = EntityTypeHelper.GetModelType(typeof(Entity));
            string[] properties;

            Dict.TryGetValue(t, out properties);
            if (properties != null)
            {
                return properties;
            }

            if (!isMock)
            {
                properties = context.Entry(dbobj).CurrentValues.PropertyNames.ToArray();
            }
            else
            {
                properties = dbobj.GetType().GetProperties().Select(x => x.Name).ToArray();
            }

            Dict.Add(t, properties);

            return properties;
        }

        /// <summary>
        /// Gets the navigation properties for the entity.
        /// </summary>
        /// <typeparam name="Entity">the type of entity</typeparam>
        /// <param name="context">The context.</param>
        /// <returns>the list of navigation properties for the entity</returns>
        public static string[] GetNavigationProperties<Entity>(DbContext context)
        {
            string[] properties;
            Type t = EntityTypeHelper.GetModelType(typeof(Entity));

            DictNavProp.TryGetValue(t, out properties);
            if (properties != null)
            {
                return properties;
            }

            // Get the System.Data.Entity.Core.Metadata.Edm.EntityType
            // associated with the entity.
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            // create method CreateObjectSet with the generic parameter of the base-type
            MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                                     .MakeGenericMethod(t);
            dynamic objectSet = method.Invoke(objectContext, null);
            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.NavigationProperties;
            properties = keyMembers.Select(k => (string)k.Name).ToArray();

            return properties;
        }

        /// <summary>
        /// Gets the properties to treate durring an update action.
        /// </summary>
        /// <param name="typeParentObj">The type parent object.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="addCollection">if set to <c>true</c> [add collection].</param>
        /// <param name="addRequiered">if set to <c>true</c> [add requiered].</param>
        /// <param name="excludeSimple">if set to <c>true</c> [exclude simple].</param>
        /// <returns>The list of properties to treate</returns>
        public static List<PropertyInfo> GetPropertiesToTreate(Type typeParentObj, GenericRepositoryParmeter param, bool addCollection = false, bool addRequiered = false, bool excludeSimple = false)
        {
            List<PropertyInfo> lstProp = typeParentObj.GetProperties().ToList();
            List<PropertyInfo> retLstProp = new List<PropertyInfo>();
            if (param != null)
            {
                if (param.Values2Update != null)
                {
                    foreach (string propname in param.Values2Update)
                    {
                        retLstProp.Add(lstProp.Single(p => p.Name == propname));
                    }

                    if (addCollection)
                    {
                        foreach (PropertyInfo prop in lstProp)
                        {
                            if (prop.PropertyType.Name == "ICollection`1")
                            {
                                retLstProp.Add(prop);
                            }
                        }
                    }
                }
                else if (param.Values2Exclude != null)
                {
                    foreach (PropertyInfo prop in lstProp)
                    {
                        if (!param.Values2Exclude.Contains(prop.Name))
                        {
                            retLstProp.Add(prop);
                        }
                    }
                }
                else
                {
                    retLstProp = lstProp;
                }
            }
            else
            {
                retLstProp = lstProp;
            }

            return retLstProp;
        }
    }
}