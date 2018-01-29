
namespace BIA.Net.Model.DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using BIA.Net.Common;

    public class TDBContainer<ProjectDBContext>
        where ProjectDBContext : DbContext, new()
    {
        public readonly object SyncRootDb = new object();

        public TDBContainer() { }

        public TDBContainer(ProjectDBContext pDB, bool isMoq = false)
        {
            _db = pDB;
            this.IsMoq = isMoq;
        }

        ProjectDBContext _db = null;

        public ProjectDBContext db
        {
            get
            {
                if (this._db == null)
                {
                    lock (SyncRootDb)
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

                return _db;
            }

            set
            {
                _db = value;
            }
        }

        public bool isInTransaction = false;
        public bool IsMoq = false;

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }

            _db = null;
        }
    }

    public class TGenericContext<ProjectDBContext, ProjectDBContainer>
        where ProjectDBContext : DbContext, new() 
        where ProjectDBContainer : TDBContainer<ProjectDBContext>, new()
    {
        private static readonly object SyncRootDbs = new object();

        private static Dictionary<Guid, ProjectDBContainer> _dbs = new Dictionary<Guid, ProjectDBContainer>();

        public static void Init(Guid guid, ProjectDBContainer projectDBContainer = null)
        {
            TraceManager.Debug("Model.DAL.ProjectDBContainer", "Init", "Nb Context: " + _dbs.Count);
            if (guid != default(Guid) && !_dbs.ContainsKey(guid))
            {
                lock (SyncRootDbs)
                {
                    if (guid != default(Guid) && !_dbs.ContainsKey(guid))
                    {
                        _dbs.Add(guid, projectDBContainer ?? new ProjectDBContainer());
                    }
                }
            }
        }

        public static ProjectDBContainer GetDbContainer(Guid guid)
        {
            ProjectDBContainer dbCont = null;

            _dbs.TryGetValue(guid, out dbCont);

            return dbCont;
        }

        public static void Dispose(Guid guid)
        {
            ProjectDBContainer dbCont = null;

            if (guid != default(Guid) && _dbs.TryGetValue(guid, out dbCont))
            {
                lock (SyncRootDbs)
                {
                    if (guid != default(Guid) && _dbs.TryGetValue(guid, out dbCont))
                    {
                        if (dbCont != null && dbCont.db != null)
                        {
                            lock (dbCont.SyncRootDb)
                            {
                                if (dbCont != null && dbCont.db != null)
                                {
                                    dbCont.db.Dispose();
                                }
                            }
                        }

                        _dbs.Remove(guid);
                        TraceManager.Debug("Model.DAL.ProjectDBContainer", "Dispose", "Nb Context: " + _dbs.Count);
                    }
                }
            }
        }
    }
}
