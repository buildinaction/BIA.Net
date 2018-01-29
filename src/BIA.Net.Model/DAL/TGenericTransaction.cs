namespace BIA.Net.Model.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using BIA.Net.Model.Utility;
    using System.Linq;

    public class TGenericTransaction<ProjectDBContext, ProjectDBContainer>
        where ProjectDBContext : DbContext, new()
        where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
    {
        private static DelegateSucces delegateSuccesStatic = null;

        public delegate void DelegateSucces(Guid contextGuid, bool rootModeInTransaction);

        public static DelegateSucces DelegateSuccesStatic
        {
            get { return delegateSuccesStatic; }
            set { delegateSuccesStatic = value; }
        }

        // set in Transaction if not set and return the rootModeInTransaction
        public static bool BeginTransaction(Guid contextGuid)
        {
            ProjectDBContainer dbContainer = TGenericContext<ProjectDBContext, ProjectDBContainer>.GetDbContainer(contextGuid);
            if (dbContainer.isInTransaction)
            {
                return true;
            }

            dbContainer.isInTransaction = true;
            return false;
        }

        public static bool Lock(Guid contextGuid, string tableName)
        {
            try
            {

                ProjectDBContainer dbContainer = TGenericContext<ProjectDBContext, ProjectDBContainer>.GetDbContainer(contextGuid);

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
                return false;
            }
        }

        public static void UnLock(Guid contextGuid, string tableName)
        {
            ProjectDBContainer dbContainer = TGenericContext<ProjectDBContext, ProjectDBContainer>.GetDbContainer(contextGuid);
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

        protected static void BaseEndTransaction(Guid contextGuid, bool rootModeInTransaction)
        {
            if (!rootModeInTransaction)
            {
                ProjectDBContainer dbContainer = TGenericContext<ProjectDBContext, ProjectDBContainer>.GetDbContainer(contextGuid);
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
                    delegateSuccesStatic(contextGuid, rootModeInTransaction);
                }

                dbContainer.isInTransaction = false;
            }
        }
    }
}
