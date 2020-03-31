// <copyright file="ObjUtil.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.Utility
{
    using System.Collections.Generic;

    public class ObjUtil
    {
        public static string GetSafeSId<T>(T obj)
            where T : ObjectRemap
        {
            return (obj == null) ? "0" : GenericModelHelper.GetPropValue<T>(obj, "Id").ToString();
        }

        public static int GetSafeId<T>(T obj)
            where T : ObjectRemap
        {
            return (obj == null) ? 0 : int.Parse(GenericModelHelper.GetPropValue<T>(obj, "Id").ToString());
        }

        public static T GetObjectWithOnlyId<T>(int id)
            where T : ObjectRemap, new()
        {
            T obj = new T();
            GenericModelHelper.SetPropValue(obj, "Id", id);
            return obj;
        }

        public static List<T> GetObjectWithOnlyIds<T>(int[] ids)
            where T : ObjectRemap, new()
        {
            List<T> lstObjs = null;
            if (ids != null)
            {
                lstObjs = new List<T>();
                foreach (int zone in ids)
                {
                    lstObjs.Add(GetObjectWithOnlyId<T>(zone));
                }
            }

            return lstObjs;
        }

        public static List<int> GetIds<T>(ICollection<T> collections)
            where T : ObjectRemap, new()
        {
            List<int> lstIds = null;
            if (collections != null)
            {
                lstIds = new List<int>();
                foreach (T obj in collections)
                {
                    lstIds.Add(GetSafeId<T>(obj));
                }
            }

            return lstIds;
        }
    }
}
