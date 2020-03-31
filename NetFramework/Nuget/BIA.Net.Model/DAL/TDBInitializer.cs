using BIA.Net.Common;
using BIA.Net.Model.Utility;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace BIA.Net.Model.DAL
{
    public static class DbSetExtensions
    {
        public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null) 
            where T : class, new()
        {
            var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
            return !exists ? dbSet.Add(entity) : null;
        }

        public static T AddIfNotExistsOrModifyReference<T, ProjectDBContext, ProjectDBContainer>( this DbSet<T> dbSet, ProjectDBContainer dbContainer, T entity, Expression<Func<T, bool>> predicate, GenericRepositoryParmeter param) 
            where T : ObjectRemap, new()
            where ProjectDBContext : DbContext, new()
            where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
        {

            var exists = dbSet.Any(predicate);
            T ret = null;
            if (!exists)
            {
                ret = dbSet.Add(entity);
            }
            else
            {
                IQueryable<T> list = dbSet.Where(predicate);
                if (list.Count() == 1)
                {
                    T item = list.First();
                    TGenericRepository<T, ProjectDBContext, ProjectDBContainer> repository = new TGenericRepository<T, ProjectDBContext, ProjectDBContainer>(dbContainer);
                    repository.RemapReferences(item, entity, param);
                }
                else
                {
                    throw new Exception("PB multi value in Initializer.");
                }
            }

            return ret;
        }

        public static T AddIfNotExistsOrModifySimpleProperties<T, ProjectDBContext, ProjectDBContainer>(this DbSet<T> dbSet, ProjectDBContainer dbContainer, T entity, Expression<Func<T, bool>> predicate)
            where T : ObjectRemap, new()
            where ProjectDBContext : DbContext, new()
            where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
        {

            var exists = dbSet.Any(predicate);
            T ret = null;
            if (!exists) ret = dbSet.Add(entity);
            else
            {

                IQueryable<T> list = dbSet.Where(predicate);
                if (list.Count() == 1)
                {
                    T item = list.First();
                    dbContainer.db.Entry(item).CurrentValues.SetValues(entity);
                }
                else
                {
                    throw new Exception("PB multi value in Initializer.");
                }
            }
            return ret;
        }

        public static void CleanTable<T>(this DbSet<T> dbSet) where T : class, new()
        {
            foreach (T item in dbSet)
            {
                dbSet.Remove(item);
            }
        }

        public static void DeleteElementIfExist<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate) where T : class, new()
        {
            var exists = dbSet.Any(predicate);
            if (exists)
            {
                IQueryable<T> list = dbSet.Where(predicate);
                if (list.Count() == 1)
                {
                    T item = list.First();
                    dbSet.Remove(item);
                }
                else
                {
                    throw new Exception("PB multi value to delete in Initializer.");
                }
            }
        }

    }


    public abstract partial class TDBInitializer<ProjectDBContext, ProjectDBContainer, Version> 
        where ProjectDBContext : DbContext, new() 
        where Version : class,  new()
        where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
    {

        protected abstract string GetAddCodeVersion();
        protected abstract int GetInitializedDataVersion();

        private string AddCodeVersion {get { return GetAddCodeVersion(); }} //"1.5.0";
        private string InitializedDataVersion {get { return AddCodeVersion + "." + GetInitializedDataVersion(); }}// ".3"; //Warning: To Increase each time you change the Initializer

        protected System.Version VersionCurrent;


        static bool initalizationInLoad = false;



        public void Seed(ProjectDBContext context)
        {
            if (initalizationInLoad) throw new Exception("Initialiation is pending on this server.");
            initalizationInLoad = true;

            try
            {

                ProjectDBContainer dbContainer = new ProjectDBContainer();
                dbContainer.db = context;

                var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;

                Version VersionAppCode = null;
                Version VersionInitializedData = null;
                Version VersionPendingInitialization = null;

                InitVersion(dbContainer);
                GetVersion(dbContainer, out VersionAppCode, out VersionInitializedData, out VersionPendingInitialization);

                string VersionAppCodeValue = GenericModelHelper.GetPropValue<Version>(VersionAppCode, "Value").ToString();
                string VersionInitializedDataValue = GenericModelHelper.GetPropValue<Version>(VersionInitializedData, "Value").ToString();
                string VersionPendingInitializationValue = GenericModelHelper.GetPropValue<Version>(VersionPendingInitialization, "Value").ToString();

                if (VersionPendingInitializationValue != "false")
                {
                    throw new Exception("Initialiation is pending on an other server.");
                }

                try
                {
                    VersionCurrent = new System.Version(VersionInitializedDataValue);

                }
                catch (Exception)
                {
                    VersionCurrent = new System.Version(VersionAppCodeValue);
                }

                bool versionIsUptodate = true;
                try
                {
                    if ((new System.Version(VersionInitializedDataValue)).CompareTo(new System.Version(InitializedDataVersion)) < 0)
                    {
                        versionIsUptodate = false;
                    }
                }
                catch (Exception)
                {
                    versionIsUptodate = false;
                }


                if (!versionIsUptodate)
                {
                    GenericModelHelper.SetPropValue<Version>(VersionPendingInitialization, "Value", "true");
                    context.SaveChanges();

                    try
                    {
                        LoadDatas(dbContainer);
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        TraceManager.Error("TDBInitializer", "Seed", "Error DbEntityValidationException.");
                        DBUtil.UndoChange(context);
                        DBUtil.ReformatDBConstraintError(dbEx);
                    }
                    catch (DbUpdateException dbEx)
                    {
                        TraceManager.Error("TDBInitializer", "Seed", "Error DbUpdateException.");
                        DBUtil.UndoChange(context);
                        DBUtil.ReformatDBUpdateError(dbEx);
                    }
                    catch(Exception e)
                    {
                        DBUtil.UndoChange(context);
                        throw e;
                    }
                    finally
                    {
                        GetVersion(dbContainer, out VersionAppCode, out VersionInitializedData, out VersionPendingInitialization);
                        GenericModelHelper.SetPropValue<Version>(VersionPendingInitialization, "Value", "false");
                        context.SaveChanges();
                        TraceManager.Info("TDBInitializer", "Seed", "VersionPendingInitialization reset to false.");
                    }

                    GenericModelHelper.SetPropValue<Version>(VersionAppCode, "Value", AddCodeVersion);
                    GenericModelHelper.SetPropValue<Version>(VersionInitializedData, "Value", InitializedDataVersion);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                initalizationInLoad = false;
            }
        }

        abstract protected void InitVersion(ProjectDBContainer container);
        abstract protected void GetVersion(ProjectDBContainer container, out Version VersionAppCode, out Version VersionInitializedData, out Version VersionPendingInitialization);
        abstract protected void LoadDatas(ProjectDBContainer container);
    }
}
