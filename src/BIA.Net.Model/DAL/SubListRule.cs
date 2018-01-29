
namespace BIA.Net.Model
{
    using System;
    using System.Collections.Generic;
    public class SubListRule
    {
        public List<string> UnicityKeys = null;
        public bool SuppressItemNotFound = true;
        public bool CascadeUpdate = false;
        public Func<object, object, int> ItemAdding = null;
        public Func<object, object, int> ItemDeleting = null;
        public Func<object, object, object, int> ItemUpdating = null;
    }

}
