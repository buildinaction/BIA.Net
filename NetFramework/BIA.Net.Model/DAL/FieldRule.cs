// <copyright file="FieldRule.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model
{
    using System;

    public class FieldRule
    {
        public Func<object, object, int> ItemModifing = null;
    }
}
