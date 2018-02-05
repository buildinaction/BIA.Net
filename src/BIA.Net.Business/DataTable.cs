using BIA.Net.Business.JQueryDataTable;
using BIA.Net.Model;
using BIA.Net.Model.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Business
{
    public class DataTable
    {
        public static JQueryDataTableAnswerModel GetPageTableData<T, ProjectDBContext>(IGenericRepository<T, ProjectDBContext> service, JQueryDataTableParameterModel parameter, IQueryable<T> query = null)
                                where T : ObjectRemap, new()
        {
            JQueryDataTableAnswerModel result = new JQueryDataTableAnswerModel();
            result.sEcho = parameter.sEcho;

            if (query == null)
            {
                query = service.GetStandardQuery();
            }

            result.recordsTotal = query.Count();

            if (!string.IsNullOrEmpty(parameter.sSearch))
            {
                query = QueryHelper.ColumnSearch(query, parameter.sSearch, parameter.Columns.Where(c => c.Searchable == true).Select(c => c.SName).ToList());
            }

            result.recordsFiltered = query.Count();

            bool descending = false;
            if (parameter.sSortDir_0 == "desc")
            {
                descending = true;
            }

            if (!string.IsNullOrEmpty(parameter.sColumns) && parameter.iSortCol_0 >= 0)
            {
                string sortColumnName = parameter.Columns.First(c => c.Index == parameter.iSortCol_0).SName;

                query = QueryHelper.OrderBy(query, sortColumnName, descending);
            }
            else
            {
                query = QueryHelper.OrderBy(query, "Id", false);
            }

            List<object> data = new List<object>();
            query = query.Skip(parameter.iDisplayStart).Take(parameter.iDisplayLength);
            List<T> queryResult = query.ToList();

            var methods = typeof(T).GetMethods();
            MethodInfo initForDataTableMethod = typeof(T).GetMethods().FirstOrDefault(m => m.Name == "InitForDatatable");

            foreach (T entity in queryResult)
            {
                if (initForDataTableMethod != null)
                {
                    initForDataTableMethod.Invoke(entity, new object[] { service.GetProjectDBContextForOptim(), parameter.sColumns });
                }

                data.Add(GenerateDynamicObjectFromSColumns(entity, parameter.Columns));
            }

            result.aaData = data;

            return result;
        }

        private static U GenerateDynamicObjectFromSColumns<U>(U entity, IEnumerable<JQueryDataTableParameterColumn> columns) where U : class, new()
        {
            U result = new U();

            foreach (JQueryDataTableParameterColumn column in columns)
            {
                GenerateDynamicObjectFromString(entity, ref result, column.SName);
            }

            return result;
        }

        public static void GenerateDynamicObjectFromString<U>(U entity, ref U result, string column)
            where U : class, new()
        {
            string columnName = column;
            if (columnName.Contains('.'))
            {
                columnName = columnName.Split('.').ElementAt(0);
            }



            PropertyInfo property = typeof(U).GetProperty(columnName);
            var properties = typeof(U).GetProperties();
            if (property != null)
            {
                object value = property.GetValue(entity);
                Type valueType = property.PropertyType;

                if (valueType.Namespace == "System" && property.GetSetMethod() != null)
                {
                    property.SetValue(result, value);
                }
                else if (value != null && valueType.GetInterface("IEnumerable") != null)
                {
                    IEnumerable<object> subValueList = value as IEnumerable<object>;
                    Type subValueType = valueType.GetGenericArguments().Single();

                    if (subValueList != null && subValueList.Count() > 0)
                    {

                        var subResultList = property.GetValue(result);

                        if (subResultList == null)
                        {
                            ConstructorInfo listconstructor = typeof(List<>).MakeGenericType(subValueType).GetConstructor(new Type[] { });
                            var test = listconstructor.Invoke(null);
                            subResultList = listconstructor.Invoke(null);
                        }

                        MethodInfo countMethod = subResultList.GetType().GetMethod("get_Count");
                        int? subResultListCount = countMethod.Invoke(subResultList, null) as int?;

                        if (subResultListCount.HasValue == false)
                        {
                            throw new ApplicationException("Error Generic Search GenerateDynamicObject : subResultListCount is null");
                        }

                        if (subResultListCount == 0)
                        {
                            ConstructorInfo subResultConstructor = subValueType.GetConstructor(new Type[] { });
                            var addMethods = subResultList.GetType().GetMethods();
                            MethodInfo addMethod = subResultList.GetType().GetMethod("Add");

                            for (int i = 0; i < subValueList.Count(); i++)
                            {
                                var subresult = subResultConstructor.Invoke(null);
                                addMethod.Invoke(subResultList, new[] { subresult });
                            }
                        }

                        MethodInfo elementAtMethod = typeof(Enumerable).GetMethod("ElementAt").MakeGenericMethod(subValueType);

                        for (int i = 0; i < subValueList.Count(); i++)
                        {
                            var valueItem = subValueList.ElementAt(i);
                            var subResult = elementAtMethod.Invoke(subResultList, new object[] { subResultList, i });
                            MethodInfo generateDynamicObjectFromSColumnsMethod = typeof(DataTable).GetMethod("GenerateDynamicObjectFromString").MakeGenericMethod(subValueType);
                            generateDynamicObjectFromSColumnsMethod.Invoke(null, new[] { valueItem, subResult, column.Substring(column.IndexOf('.') + 1) });
                        }

                        property.SetValue(result, subResultList);
                    }
                }
                else if (value != null && property.GetSetMethod() != null)
                {
                    var subResult = property.GetValue(result);
                    if (subResult == null)
                    {
                        ConstructorInfo subResultConstructor = valueType.GetConstructor(new Type[] { });
                        subResult = subResultConstructor.Invoke(null);
                    }

                    MethodInfo generateDynamicObjectFromSColumnsMethod = typeof(DataTable).GetMethod("GenerateDynamicObjectFromString").MakeGenericMethod(valueType);
                    generateDynamicObjectFromSColumnsMethod.Invoke(null, new[] { value, subResult, column.Substring(column.IndexOf('.') + 1) });
                    property.SetValue(result, subResult);
                }
            }
        }
    }
}
