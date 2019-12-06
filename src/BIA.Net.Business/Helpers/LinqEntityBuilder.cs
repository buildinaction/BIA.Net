using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Business.Helpers
{
    public static class LinqEntityBuilder
    {
        /// <summary>
        /// x => x.[propertyName].Contains(val)
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Expression<Func<Entity, bool>> GetDynamicContains<DTO, Entity>(string propertyName, string val)
        {
            // x =>
            var param = Expression.Parameter(typeof(Entity), "x");

            // x.[propertyName]
            Expression prop;
            Type propertyType;
            PrepareProperty<Entity>(propertyName, param, out prop, out propertyType);

            if (Nullable.GetUnderlyingType(propertyType) != null)
            {
                prop = Expression.PropertyOrField(prop, "Value");
                propertyType = propertyType.GetProperty("Value").PropertyType;
            }

            if (propertyType == typeof(DateTime) || propertyType == typeof(TimeSpan))
            {
                string format = DisplayFormat<DTO>(propertyName);
                prop = GetDateFormated(prop, format, propertyType);

            }
            else if (propertyType != typeof(string))
            {
                var methodToString = propertyType.GetMethod("ToString", new Type[] { });
                prop = Expression.Call(prop, methodToString);
            }

            // val
            if (IsNumericType(propertyType))
            {
                val = val.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator, "").Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            }

            var valExpression = Expression.Constant(val, typeof(string));

            // x.[propertyName].Contains(val)
            var method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            Expression body = Expression.Call(prop, method, valExpression);

            // x => x.LastName == "Curry"
            var final = Expression.Lambda<Func<Entity, bool>>(body: body, parameters: param);

            // compiles the expression tree to a func delegate
            return final;
        }

        public static Expression GetDateFormated(Expression prop, string format, Type propertyType)
        {

            var methodConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });

            Expression dateFormated = Expression.Constant("");

            // DateOnly or DateAndTime
            if (format == "{0:d}" || format == "{0:g}" || format == null)
            {
                var dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.Split(CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator[0]);
                var separator = Expression.Constant(CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator);
                dateFormated = Expression.Call(
                        typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
                        FormatPartDate(prop, dateFormat[0], propertyType),
                        separator,
                        FormatPartDate(prop, dateFormat[1], propertyType),
                        separator);
                // dateFormated = Expression.Call(methodConcat, FormatPartDate(prop, dateFormat[0]), separator);
                // dateFormated = Expression.Call(methodConcat, dateFormated, FormatPartDate(prop, dateFormat[1]));
                //dateFormated = Expression.Call(methodConcat, dateFormated, separator);
                dateFormated = Expression.Call(methodConcat, dateFormated, FormatPartDate(prop, dateFormat[2], propertyType));
            }

            // DateAndTime
            if (format == "{0:g}")
            {
                dateFormated = Expression.Call(methodConcat, dateFormated, Expression.Constant(" "));
            }

            // DateAndTime or TimeOnly
            if (format == "{0:g}" || format == "{0:t}")
            {
                string[] timeFormat = null;
                if (propertyType == typeof(TimeSpan))
                {
                    timeFormat = "hh:mm:ss".Split(':');
                }
                else
                {
                    timeFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern.Split(CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator[0]);
                }

                var separator = Expression.Constant(CultureInfo.CurrentCulture.DateTimeFormat.TimeSeparator);
                dateFormated = Expression.Call(
                        typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string), typeof(string) }),
                        dateFormated,
                        FormatPartDate(prop, timeFormat[0], propertyType),
                        separator,
                        FormatPartDate(prop, timeFormat[1].Split(' ')[0], propertyType));
                if (timeFormat.Length > 2)
                {
                    dateFormated = Expression.Call(
                        typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string) }),
                        dateFormated,
                        separator,
                        FormatPartDate(prop, timeFormat[2].Split(' ')[0], propertyType));
                }

                var endTimeFormat = timeFormat[timeFormat.Length - 1].Split(' ');
                if (endTimeFormat.Length > 1)
                {
                    // (x.[prop].Value.Hour > 11 ? "PM" : "AM")
                    //var propHour = Expression.PropertyOrField(prop, "Value");
                    //propHour = Expression.PropertyOrField(propHour, "Hour");
                    var propHour = Expression.PropertyOrField(prop, "Hour");
                    var conditionGreaterThan11 = Expression.GreaterThan(propHour, Expression.Constant(11));

                    // AM, PM or a. m. p. m.
                    var amorpm = Expression.Condition(conditionGreaterThan11, Expression.Constant(CultureInfo.CurrentCulture.DateTimeFormat.PMDesignator), Expression.Constant(CultureInfo.CurrentCulture.DateTimeFormat.AMDesignator));
                    dateFormated = Expression.Call(
                        typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string), typeof(string) }),
                        dateFormated,
                        Expression.Constant(" "),
                        amorpm);
                }
            }

            return dateFormated;
        }

        public static Expression FormatPartDate(Expression prop, string datePartFormat, Type propertyType)
        {
            string datePart = datePartFormat;
            long? nbChar = null;

            switch (datePartFormat)
            {
                case "dd":
                    datePart = "dd";
                    nbChar = 2;
                    break;
                case "d":
                    datePart = "dd";
                    nbChar = null;
                    break;
                case "MM":
                    datePart = "m";
                    nbChar = 2;
                    break;
                case "M":
                    datePart = "m";
                    nbChar = null;
                    break;
                case "HH":
                    datePart = "hh";
                    nbChar = 2;
                    break;
                case "hh":
                    datePart = "hh";
                    nbChar = 2;
                    break;
                case "h":
                    datePart = "hh";
                    nbChar = null;
                    break;
                case "mm":
                    datePart = "n";
                    nbChar = 2;
                    break;
                case "ss":
                    datePart = "ss";
                    nbChar = 2;
                    break;
            }

            Expression partDatePart = null;
            if (propertyType != typeof(TimeSpan) && (datePartFormat == "h" || datePartFormat == "hh"))
            {
                //var propHour = Expression.PropertyOrField(prop, "Value");
                //propHour = Expression.PropertyOrField(propHour, "Hour");
                var propHour = Expression.PropertyOrField(prop, "Hour");
                var propHour12 = Expression.Subtract(propHour, Expression.Constant(12));
                var conditionGreaterThan12 = Expression.GreaterThan(propHour, Expression.Constant(12));
                partDatePart = Expression.Condition(conditionGreaterThan12, propHour12, propHour);
            }
            else
            {
                // SqlFunctions.DatePart("dd", x.[propertyName])
                var methodDatePart = typeof(SqlFunctions).GetMethod("DatePart", new Type[] { typeof(string), NullableTypesCache.Get(propertyType) });
                var formatPart = Expression.Constant(datePart, typeof(string));
                var propNullable = Expression.Convert(prop, NullableTypesCache.Get(propertyType));
                partDatePart = Expression.Call(methodDatePart, formatPart, propNullable);
                partDatePart = Expression.PropertyOrField(partDatePart, "Value");
            }

            // SqlFunctions.DatePart("dd", x.DateOnly).ToString()
            var methodStringConvert = typeof(int).GetMethod("ToString", new Type[] { });
            Expression partConvert = Expression.Call(partDatePart, methodStringConvert);

            if (nbChar != null)
            {
                // "0" + SqlFunctions.DatePart("dd", x.DateOnly).ToString()
                var methodConcat = typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) });
                var zero = Expression.Constant("0", typeof(string));
                Expression partJoin = Expression.Call(methodConcat, zero, partConvert);

                // EntityFunctions.Right("0" + SqlFunctions.DatePart("dd", x.DateOnly).ToString(), 2)
                var methodRight = typeof(EntityFunctions).GetMethod("Right", new[] { typeof(string), typeof(long?) });
                var nbCharExpr = Expression.Constant(nbChar, typeof(long?));
                Expression partFomated = Expression.Call(methodRight, partJoin, nbCharExpr);
                return partFomated;
            }

            return partConvert;
        }

        /// <summary>
        /// x.[propertyName]
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="param"></param>
        /// <param name="prop"></param>
        /// <param name="propertyType"></param>
        public static void PrepareProperty<Entity>(string propertyName, ParameterExpression param, out Expression prop, out Type propertyType)
        {
            try
            {
                prop = param;
                propertyName = propertyName.Replace("__", ".");
                propertyType = typeof(Entity);
                foreach (var property in propertyName.Split('.'))
                {
                    prop = Expression.PropertyOrField(prop, property);
                    propertyType = propertyType.GetProperty(property).PropertyType;
                }
            }
            catch (Exception)
            {
                prop = null;
                propertyType = null;
            }
        }

        /// <summary>
        /// x.[propertyName]
        /// </summary>
        /// <typeparam name="DTO"></typeparam>
        /// <param name="propertyName"></param>
        public static string DisplayFormat<DTO>(string propertyName)
        {
            propertyName = propertyName.Replace("__", ".");
            var propertyType = typeof(DTO);
            PropertyInfo propertyInfo = null;
            foreach (var property in propertyName.Split('.'))
            {
                propertyInfo = propertyType.GetProperty(property);
                propertyType = propertyInfo.PropertyType;
            }

            if (propertyInfo != null)
            {
                return (string)propertyInfo.CustomAttributes.Where(c => c.AttributeType.Name == "DisplayFormatAttribute").Select(c => c.NamedArguments.Where(n => n.MemberName == "DataFormatString").Select(n => n.TypedValue.Value).FirstOrDefault()).FirstOrDefault();
            }

            return null;
        }

        public static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
