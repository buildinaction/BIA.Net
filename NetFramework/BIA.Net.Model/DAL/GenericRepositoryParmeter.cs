// <copyright file="GenericRepositoryParmeter.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model
{
    using System.Collections.Generic;

    public class GenericRepositoryParmeter
    {
        public List<object> Ref2Exclude = null;
        public List<string> Values2Exclude = null;

        // !!!!!Important Value2Clone are List with several elements that contain the object as an unique reference!!!!!//
        public List<string> Values2Clone = null;

        public List<string> Values2Update = null;

        public Dictionary<string, SubListRule> SubListRules = null;
        public Dictionary<string, FieldRule> FieldRules = null;

        public Dictionary<string, GenericRepositoryParmeter> CascadeParams = null;
    }
}
