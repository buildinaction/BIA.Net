// <copyright file="TGenericTransaction.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using BIA.Net.Common.Helpers;
    using Utility;

    public class TGenericTransaction<ProjectDBContext>
        where ProjectDBContext : DbContext, new()
    {
        private static DelegateSucces delegateSuccesStatic = null;

        public delegate void DelegateSucces(bool rootModeInTransaction);

        public static DelegateSucces DelegateSuccesStatic
        {
            get { return delegateSuccesStatic; }
            set { delegateSuccesStatic = value; }
        }

        // set in Transaction if not set and return the rootModeInTransaction
        public static bool BeginTransaction()
        {
            TDBContainer<ProjectDBContext> dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
            if (dbContainer.IsInTransaction)
            {
                return true;
            }

            dbContainer.IsInTransaction = true;
            return false;
        }

        public static bool Lock(string tableName)
        {
            try
            {
                TDBContainer<ProjectDBContext> dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
                dbContainer.db.Database.BeginTransaction();

                string query = "SELECT [Value] FROM [dbo].[" + tableName + "] WHERE [Name] = 'LockTable'";
                var item = dbContainer.db.Database.SqlQuery<string>(query).FirstOrDefault<string>();

                if (string.IsNullOrEmpty(item))
                {
                    dbContainer.db.Database.ExecuteSqlCommand("INSERT INTO [dbo].[" + tableName + "] (Name , Value) VALUES ('LockTable', 'true')");
                    dbContainer.db.Database.ExecuteSqlCommand("UPDATE [dbo].[" + tableName + "] WITH(UPDLOCK) SET Value = 'true' WHERE Name = 'LockTable'");
                    return true;
                }
                else if (item == "false")
                {
                    dbContainer.db.Database.ExecuteSqlCommand("UPDATE [dbo].[" + tableName + "] WITH(UPDLOCK) SET Value = 'true' WHERE Name = 'LockTable'");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
                // return false;
            }
        }

        public static void UnLock(string tableName)
        {
            TDBContainer<ProjectDBContext> dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
            try
            {
                dbContainer.db.Database.ExecuteSqlCommand("UPDATE [dbo].[" + tableName + "] SET Value = 'false' WHERE Name = 'LockTable'");
                dbContainer.db.Database.CurrentTransaction.Commit();
            }
            catch
            {
                dbContainer.db.Database.CurrentTransaction.Rollback();
            }
        }

        protected static void BaseEndTransaction(bool rootModeInTransaction)
        {
            if (!rootModeInTransaction)
            {
                TDBContainer<ProjectDBContext> dbContainer = BIAUnity.Resolve<TDBContainer<ProjectDBContext>>();
                try
                {
                    dbContainer.db.SaveChanges();

                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    DBUtil.ReformatDBConstraintError(dbEx);
                }
                catch (DbUpdateException dbEx)
                {
                    DBUtil.ReformatDBUpdateError(dbEx);
                }

                if (delegateSuccesStatic != null)
                {
                    delegateSuccesStatic(rootModeInTransaction);
                }

                dbContainer.IsInTransaction = false;
            }
        }
    }
}
