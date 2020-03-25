namespace BIA.Net.Business.Helpers
{
    using BIA.Net.Common;
    using System;
    using System.Data.Entity;

    /// <summary>
    /// DbContext Read Performance
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DbContextReadPerformance : IDisposable
    {
        /// <summary>
        /// The database context
        /// </summary>
        private DbContext dbContext;

        /// <summary>
        /// The current automatic detect changes enabled
        /// </summary>
        private bool currentAutoDetectChangesEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextReadPerformance"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DbContextReadPerformance(DbContext dbContext)
        {
            this.dbContext = dbContext;
            currentAutoDetectChangesEnabled = this.dbContext.Configuration.AutoDetectChangesEnabled;
            this.dbContext.Configuration.AutoDetectChangesEnabled = false;
        }

        #region IDisposable Support

        /// <summary>
        /// The disposed value to detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.dbContext.Configuration.AutoDetectChangesEnabled = currentAutoDetectChangesEnabled;
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
