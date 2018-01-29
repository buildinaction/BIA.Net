using BIA.Net.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.MVC.Utility
{
    static public class ValidatorUtility
    {
        public static void ValidOnly<TModelState>(IDictionary<string, TModelState> ModelState, string listOfField)
        {
            List<string> lstFields = listOfField.Split(',').ToList();
            List<string> keysToRemove = new List<string>();
            foreach (string modelStateKey in ModelState.Keys)
            {
                if ( !lstFields.Contains(modelStateKey))
                {
                    keysToRemove.Add(modelStateKey);
                }
            }
            foreach (string key in keysToRemove)
            {
                ModelState.Remove(key);
            }
        }
    }
    static public class ValidatorUtility<TClass>
    {

        public static void ValidOnlyKeysFor<T, TModelState>(IDictionary<string, TModelState> ModelState, Expression<Func<TClass, T>> expression)
        {
            string CheckToRemove = TypeHelper<TClass>.NameOf(expression);
            List<string> keysToRemove = new List<string>();
            foreach (string modelStateKey in ModelState.Keys)
            {
                if (modelStateKey.StartsWith(CheckToRemove + ".") && modelStateKey != CheckToRemove + ".Id")
                {
                    keysToRemove.Add(modelStateKey);
                }
            }
            foreach (string key in keysToRemove)
            {
                ModelState.Remove(key);
            }
        }
    }
}
