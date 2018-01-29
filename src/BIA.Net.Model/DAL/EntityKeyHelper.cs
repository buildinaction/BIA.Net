// <copyright file="EntityKeyHelper.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Reflection;

    public sealed class EntityKeyHelper
    {
        public class KeyProperties
        {

            public string name;
            public StoreGeneratedPattern StoreGeneratedPattern;
            //public TypeUsage typeUsage;
            public String typeName;
        }

        private static readonly Lazy<EntityKeyHelper> LazyInstance = new Lazy<EntityKeyHelper>(() => new EntityKeyHelper());
        private static readonly Dictionary<Type, KeyProperties[]> _dict = new Dictionary<Type, KeyProperties[]>();
        private EntityKeyHelper() { }

        public static EntityKeyHelper Instance
        {
            get { return LazyInstance.Value; }
        }

        public static KeyProperties[] GetKeysProperties<T>(DbContext context) where T : class
        {
            Type t = typeof(T);

            //retreive the base type
            while (t.BaseType != typeof(ObjectRemap))
            {
                t = t.BaseType;
            }

            KeyProperties[] keys;

            _dict.TryGetValue(t, out keys);
            if (keys != null)
            {
                return keys;
            }

            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;

            //create method CreateObjectSet with the generic parameter of the base-type
            MethodInfo method = typeof(ObjectContext).GetMethod("CreateObjectSet", Type.EmptyTypes)
                                                     .MakeGenericMethod(t);
            dynamic objectSet = method.Invoke(objectContext, null);
            //var objectSet = objectContext.CreateObjectSet<T>();

            IEnumerable<dynamic> keyMembers = objectSet.EntitySet.ElementType.KeyMembers;
            //KeyProperties[] keyNames = keyMembers.Select(k => new KeyProperties() { name = k.Name, StoreGeneratedPattern = k.StoreGeneratedPattern}).ToArray();
            List<KeyProperties> listKeyProperties = new List<KeyProperties>();
            foreach (EdmProperty keyMember in keyMembers)
            {
                KeyProperties keyProp = new KeyProperties();
                keyProp.name = keyMember.Name;
                //keyProp.typeUsage = keyMember.TypeUsage;
                keyProp.typeName = keyMember.TypeName;
                MetadataProperty meta = keyMember.MetadataProperties.FirstOrDefault(mp => mp.Name == "http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern");
                keyProp.StoreGeneratedPattern = StoreGeneratedPattern.Identity;
                if (meta != null)
                {
                    if (meta.Value.ToString().Equals("None"))
                    {
                        keyProp.StoreGeneratedPattern = StoreGeneratedPattern.None;
                    }
                    else if (meta.Value.ToString().Equals("Computed"))
                    {
                        keyProp.StoreGeneratedPattern = StoreGeneratedPattern.Computed;
                    }
                }
                listKeyProperties.Add(keyProp);
            }

            KeyProperties[] keyProperties = listKeyProperties.ToArray();
            _dict.Add(t, keyProperties);

            return keyProperties;
        }

        public static object[] GetKeys<T>(T entity, DbContext context) where T : class
        {
            var keysProperties = GetKeysProperties<T>(context);
            Type type = typeof(T);

            object[] keys = new object[keysProperties.Length];
            for (int i = 0; i < keysProperties.Length; i++)
            {
                keys[i] = type.GetProperty(keysProperties[i].name).GetValue(entity, null);
            }
            return keys;
        }

        public static void SetKeys<T>(T entity, DbContext context, object[] keys) where T : class
        {
            var keysProperties = GetKeysProperties<T>(context);
            Type type = typeof(T);

            for (int i = 0; i < keysProperties.Length; i++)
            {
                type.GetProperty(keysProperties[i].name).SetValue(entity, keys[i]);
            }
        }

        public static T Max<T>(IQueryable<T> source)
            where T : IComparable<T>
        {
            T value = default(T);
            bool hasValue = false;
            foreach (T x in source)
            {
                if (hasValue)
                {
                    if (x.CompareTo(value) > 0) value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            }
            return value;
        }

        public static T ConvertTo<T>(object source)
        {
            if (source is T) return (T)source;
            else
            {
                if (source == null) return default(T);
                try
                {
                    return (T)Convert.ChangeType(source, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }

        public static void AutoPrepareKeysIfRequiered<T>(T entity, DbContext context, DbSet<T> dbSet, Dictionary<string, object> minValue)
            where T : class
        {
            var keysProperties = GetKeysProperties<T>(context);
            Type type = typeof(T);

            for (int i = 0; i < keysProperties.Length; i++)
            {
                if ((keysProperties[i].StoreGeneratedPattern == StoreGeneratedPattern.None))
                {
                    PropertyInfo prop = type.GetProperty(keysProperties[i].name);
                    object actualkey = prop.GetValue(entity, null);
                    if (actualkey == null || actualkey.ToString() == "0")
                    {
                        Type objType = actualkey.GetType();
                        objType = Nullable.GetUnderlyingType(objType) ?? objType;
                        if (objType.IsPrimitive)
                        {
                            if (objType == typeof(Int64) || objType == typeof(Int32) || objType == typeof(Int16)
                                || objType == typeof(UInt64) || objType == typeof(UInt32) || objType == typeof(UInt16))
                            {
                                string keyName = keysProperties[i].name;
                                MethodInfo method = typeof(EntityKeyHelper).GetMethod("ComputeKeyAutoInc");
                                MethodInfo generic = method.MakeGenericMethod(new[] { typeof(T), prop.PropertyType });
                                generic.Invoke(null, new object[] { entity, dbSet, minValue, prop, keyName });

                                //ComputeKeyAutoInc(entity, dbSet, minValue, prop, keyName);
                            }
                        }
                    }
                }
            }
        }

        public static void ComputeKeyAutoInc<T, Tint>(T entity, DbSet<T> dbSet, Dictionary<string, object> minValue, PropertyInfo prop, string keyName) where T : class where Tint : IComparable<Tint>
        {
            Tint result = Max<Tint>(dbSet.Select(keyName) as IQueryable<Tint>);
            result = ConvertTo<Tint>(ConvertTo<Int64>(result) + 1);
            result = ReajustResultOnMin<Tint>(minValue, keyName, result);
            prop.SetValue(entity, result);
        }

        private static T ReajustResultOnMin<T>(Dictionary<string, object> minValue, string keyName, T result) where T : IComparable<T>
        {
            if (minValue != null)
            {
                T minResult = default(T);
                object minResultFind = null;
                minValue.TryGetValue(keyName, out minResultFind);
                if (minResultFind != null) minResult = ConvertTo<T>(minResultFind);
                if (result.CompareTo(minResult) < 0) result = minResult;
            }

            return result;
        }
    }
}