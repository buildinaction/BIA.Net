// <copyright file="StoredProcedureHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;

    internal static class StoredProcedureHelper
    {
        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <typeparam name="T">Entity or EntityDTO</typeparam>
        /// <param name="context"><see cref="DbContext"/></param>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity or EntityDTO</returns>
        internal static List<T> ExecuteProcedureReader<T>(DbContext context, StoredProcedureParameter storedProcedureParameter)
        {
            if (storedProcedureParameter == null)
            {
                throw new ArgumentNullException("storedProcedureParameter");
            }

            List<SqlParameter> sqlParameters = null;
            string sql = null;

            StoredProcedureHelper.FillSqlParameter(storedProcedureParameter, out sqlParameters, out sql);

            return context.Database.SqlQuery<T>(sql, sqlParameters != null ? sqlParameters.ToArray() : null).ToList();
        }

        /// <summary>
        /// Execute a stored procedure that does not end with a SELECT
        /// </summary>
        /// <param name="context"><see cref="DbContext"/></param>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>The result returned by the database after executing the command.</returns>
        internal static int ExecuteProcedureNonQuery(DbContext context, StoredProcedureParameter storedProcedureParameter)
        {
            if (storedProcedureParameter == null)
            {
                throw new ArgumentNullException("storedProcedureParameter");
            }

            List<SqlParameter> sqlParameters = null;
            string sql = null;

            StoredProcedureHelper.FillSqlParameter(storedProcedureParameter, out sqlParameters, out sql);

            return context.Database.ExecuteSqlCommand(sql, sqlParameters != null ? sqlParameters.ToArray() : null);
        }

        /// <summary>
        /// Fill list of SqlParameter and sql command
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <param name="sqlParameters">list of SqlParameter</param>
        /// <param name="sql">sql command</param>
        private static void FillSqlParameter(StoredProcedureParameter storedProcedureParameter, out List<SqlParameter> sqlParameters, out string sql)
        {
            sqlParameters = null;
            sql = storedProcedureParameter.Name;
            if (storedProcedureParameter.Parameters != null)
            {
                sqlParameters = storedProcedureParameter.Parameters.Where(x => x.Value != null).Select(x => new SqlParameter(x.Key, x.Value)).ToList();

                if (sqlParameters != null && sqlParameters.Any())
                {
                    sql += " @" + string.Join(", @", sqlParameters.Select(x => x.ParameterName).ToList());
                }
            }
        }
    }
}
