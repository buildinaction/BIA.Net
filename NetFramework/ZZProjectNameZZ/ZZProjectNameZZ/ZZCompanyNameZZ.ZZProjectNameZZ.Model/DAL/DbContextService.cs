namespace Safran.ZZProjectNameZZ.Model.DAL
{
    using BIA.Net.Common.Helpers;
    using System;

    /// <summary>
    /// Container context
    /// </summary>
    public class DbContextService : IDbContextService
    {
        /// <summary>
        /// context key for trace
        /// </summary>
        private readonly Guid contextKey = Guid.NewGuid();

        /// <summary>
        /// context entity
        /// </summary>
        private ZZProjectNameZZDBContainer context;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextService"/> class.
        /// </summary>
        /// <param name="context">context container.</param>
        public DbContextService(ZZProjectNameZZDBContainer context = null)
        {
            this.context = context;
        }

        /// <summary>
        /// Set the context for Test unit
        /// </summary>
        /// <param name="mockContext"></param>
        public static void SetMoqContext(ZZProjectNameZZDBContainer mockContext)
        {
            (BIAUnity.Resolve<Model.DAL.IDbContextService>() as DbContextService).context = mockContext;
        }

        /// <summary>
        /// Get context entity
        /// </summary>
        /// <returns>context entity</returns>
        internal ZZProjectNameZZDBContainer GetContext()
        {
            if (this.context == null)
            {
                BIA.Net.Common.TraceManager.Debug("DbContextService", "GetContext", "new context " + contextKey);
                this.context = new ZZProjectNameZZDBContainer();
            }
            else
            {
                BIA.Net.Common.TraceManager.Debug("DbContextService", "GetContext", "get existing context " + contextKey);
            }

            return this.context;
        }

        #region IDisposable Support

#pragma warning disable SA1201 // Elements must appear in the correct order
        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;
#pragma warning restore SA1201 // Elements must appear in the correct order

        /// <summary>
        /// Dispose object
        /// </summary>
        /// <param name="disposing">dispose managed state if true</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects).
                    if (this.context != null)
                    {
                        BIA.Net.Common.TraceManager.Debug("DbContextService", "Dispose", "context.Dispose() " + contextKey);
                        this.context.Dispose();
                    }
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.
                disposedValue = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DbContextService()
        // {
        //    // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //    Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
#pragma warning disable SA1202 // Elements must be ordered by access
        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);

            // uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}