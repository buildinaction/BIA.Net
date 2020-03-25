using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Business.Helpers
{
    static class NullableTypesCache
    {
        readonly static ConcurrentDictionary<Type, Type> cache = new ConcurrentDictionary<Type, Type>();
        static NullableTypesCache()
        {
            cache.TryAdd(typeof(DateTime), typeof(Nullable<DateTime>));
            cache.TryAdd(typeof(TimeSpan), typeof(Nullable<TimeSpan>));
            //... 
        }
        readonly static Type NullableBase = typeof(Nullable<>);
        internal static Type Get(Type type)
        {
            // Try to avoid the expensive MakeGenericType method call
            return cache.GetOrAdd(type, t => NullableBase.MakeGenericType(t));
        }
    }
}
