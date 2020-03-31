// <copyright file="GenericModelHelper.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.Utility
{
    using System.Collections.Generic;

    public class GenericModelHelper
    {
        public static bool IdentityInList<T2>(ICollection<T2> originalList, object primaryKey)
        {
            foreach (T2 item in originalList)
            {
                object key = GetPropValue<T2>(item, "Id");
                if (primaryKey.ToString() == key.ToString())
                {
                    return true;
                }
            }

            return false;
        }

        public static object GetPropValue<T2>(T2 src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static void SetPropValue<T2>(T2 src, string propName, object value)
        {
            src.GetType().GetProperty(propName).SetValue(src, value);
        }
    }
}
