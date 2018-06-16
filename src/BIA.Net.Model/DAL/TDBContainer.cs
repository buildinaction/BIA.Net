// <copyright file="TGenericContext.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;

    public class TDBContainer<ProjectDBContext> : IDisposable
        where ProjectDBContext : DbContext, new()
    {
        public readonly object SyncRootDb = new object();

        public TDBContainer() {
        }

        ProjectDBContext _db = null;

        public ProjectDBContext db
        {
            get
            {
                if (this._db == null)
                {
                    lock (this.SyncRootDb)
                    {
                        if (this._db == null)
                        {
                            this._db = new ProjectDBContext();

#if DEBUG
                            this._db.Database.Log = message => System.Diagnostics.Debug.WriteLine(message);
#endif
                        }
                    }
                }

                return this._db;
            }
        }

        public bool IsInTransaction = false;

        /// <summary>
        /// Set the context for Test unit
        /// </summary>
        /// <param name="mockContext"></param>
        public static void SetMoqContext(ProjectDBContext mockContext)
        {
            TDBContainer<ProjectDBContext> dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
            dbContainer._db = mockContext;
        }

        /// <summary>
        /// Execute a stored procedure that does not end with a SELECT
        /// </summary>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>The result returned by the database after executing the command.</returns>
        public virtual int ExecuteProcedureNonQuery(StoredProcedureParameter storedProcedureParameter)
        {
            return StoredProcedureHelper.ExecuteProcedureNonQuery(this.db, storedProcedureParameter);
        }

        /// <summary>
        /// Execute a stored procedure of type SELECT
        /// </summary>
        /// <typeparam name="T">Entity or EntityDTO</typeparam>
        /// <param name="storedProcedureParameter"><see cref="StoredProcedureParameter"/></param>
        /// <returns>List of Entity or EntityDTO</returns>
        public virtual List<T> ExecuteProcedureReader<T>(StoredProcedureParameter storedProcedureParameter)
        {
            return StoredProcedureHelper.ExecuteProcedureReader<T>(this.db, storedProcedureParameter);
        }

        public void Dispose()
        {
            if (this._db != null)
            {
                this._db.Dispose();
            }

            this._db = null;
        }
    }
}
