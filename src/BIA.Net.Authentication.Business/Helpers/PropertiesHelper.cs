namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common;
    using System;
    using System.Linq;
    using System.Reflection;
    using static BIA.Net.Common.Configuration.AuthenticationElement.SourcesElement.UserPropertiesElement;

    public static class PropertiesHelper
    {
        public static TUserDB PrepareProperties<TUserInfo,TUserDB>(TUserInfo userInfo)
            where TUserInfo : AUserInfo<TUserDB>, new()
            where TUserDB : IUserDB, new()
        {
            TUserDB userProperties = new TUserDB();
            ADHelper.SetPropertiesFromAD(userInfo.Login, userProperties.UserAdInDB);
            ObjectFieldsCollection objects = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.Objects;
            if (objects != null && objects.Count > 0)
            {
                foreach (ObjectFieldsCollection.ObjectFieldElement value in objects)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        if (!string.IsNullOrEmpty(value.Object))
                        {
                            if (value.Object == "UserInfo")
                            {
                                PropertyInfo propertyInfoSrc = userInfo.GetType().GetProperty(value.Field);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(propertyInfoSrc.GetValue(userInfo), propertyInfo.PropertyType));
                            }
                        }
                    }
                }
            }

            FunctionsCollection functions = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.Functions;
            if (functions != null && functions.Count > 0)
            {
                foreach (FunctionsCollection.FunctionElement value in functions)
                {
                    PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                    if (propertyInfo != null)
                    {
                        if (value.Type != null)
                        {
                            if (!string.IsNullOrEmpty(value.Method))
                            {
                                object result = value.Type.GetMethod(value.Method).Invoke(null, null);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                            else
                            {
                                object result = value.Type.GetProperty(value.Property).GetValue(null, null);
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(result, propertyInfo.PropertyType));
                            }
                        }
                    }
                }
            }

            return userProperties;
        }
    }
}
