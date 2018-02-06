// <copyright file="EntityTypeHelper.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;

    public sealed class EntityTypeHelper
    {
        // private static readonly Dictionary<Type, Type> _dict = new Dictionary<Type, Type>();
        private EntityTypeHelper() {
        }

        public static Type GetModelType(Type torigin)
        {
            Type t = torigin;
            if (t.BaseType != typeof(ObjectRemap))
            {
                if (t.Name.StartsWith(t.BaseType.Name + "_"))
                {
                    t = t.BaseType;
                }
            }

            return t;
        }
    }
}