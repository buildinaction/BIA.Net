// <copyright file="DBUtil.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Model.Utility
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Text;
    using Common;

    public class DBUtil
    {
        public static void ReformatDBConstraintError(System.Data.Entity.Validation.DbEntityValidationException dbEx)
        {
            Exception raise = dbEx;
            var builder = new StringBuilder(string.Empty);
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string message = string.Format(
                        "{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                    builder.Append(message + "\n");

                    // raise a new exception nesting
                    // the current instance as InnerException
                    TraceManager.Error("DBUtil", "ReformatDBConstraintError", message);
                }
            }

            raise = new InvalidOperationException(builder.ToString(), raise);
            throw raise;
        }

        public static void ReformatDBUpdateError(DbUpdateException dbEx)
        {
            Exception raise = HandleDbUpdateException(dbEx);
            string message = string.Empty;
            Exception exp = raise;
            while (exp.InnerException != null)
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message = message + " \n\n";
                }

                message += exp.InnerException.Message;
                exp = exp.InnerException;
            }

            TraceManager.Error("DBUtil", "ReformatDBUpdateError", message);
            raise = new InvalidOperationException(message, raise);
            throw raise;
        }

        private static Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. \n");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. \n", result.Entity.GetType().Name);
                }

                if (dbu.InnerException != null)
                {
                    Exception cycleEx = dbu;
                    while (cycleEx.InnerException != null)
                    {
                        builder.AppendFormat("Message : {0}\n", cycleEx.InnerException.Message);
                        cycleEx = cycleEx.InnerException;
                    }
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }

        public static void UndoChange(DbContext context)
        {
            // Undo the changes of the all entries.
            foreach (DbEntityEntry entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    // Under the covers, changing the state of an entity from
                    // Modified to Unchanged first sets the values of all
                    // properties to the original values that were read from
                    // the database when it was queried, and then marks the
                    // entity as Unchanged. This will also reject changes to
                    // FK relationships since the original value of the FK
                    // will be restored.
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    // If the EntityState is the Deleted, reload the date from the database.
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            }
        }
    }
}
