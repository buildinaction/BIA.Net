using System.Collections.Generic;

namespace BIA.Net.Model.Utility
{
    public class ObjUtil
    {


        public static string GetSafeSId<T>(T obj) where T : ObjectRemap
        {
            return (obj == null) ? "0" : GenericModelHelper.GetPropValue<T>(obj, "Id").ToString(); 
        }
        public static int GetSafeId<T>(T obj) where T : ObjectRemap
        {
            return (obj == null) ? 0 : int.Parse(GenericModelHelper.GetPropValue<T>(obj, "Id").ToString());
        }
        public static T GetObjectWithOnlyId<T>(int Id) where T : ObjectRemap, new()
        {
            T obj = new T();
            GenericModelHelper.SetPropValue(obj, "Id", Id);
            return obj;
        }
        public static List<T> GetObjectWithOnlyIds<T>(int[] Ids) where T : ObjectRemap, new()
        {
            List<T> lstObjs = null;
            if (Ids != null)
            {
                lstObjs = new List<T>();
                foreach (int zone in Ids)
                {
                    lstObjs.Add(GetObjectWithOnlyId<T>(zone));
                }
            }

            return lstObjs;
        }

        public static List<int> GetIds<T>(ICollection<T> Collections) where T : ObjectRemap, new()
        {
            List<int> lstIds = null;
            if (Collections != null)
            {
                lstIds = new List<int>();
                foreach (T Obj in Collections)
                {
                    lstIds.Add(GetSafeId<T>(Obj));
                }
            }

            return lstIds;
        }
    }
}
