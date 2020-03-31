// <copyright file="PlaneModelBuilder.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using MyCompany.BIADemo.Domain.PlaneModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for plane domain.
    /// </summary>
    public static class PlaneModelBuilder
    {
        /// <summary>
        /// Create the model for planes.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreatePlaneModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plane>().HasKey(s => s.Id);
            modelBuilder.Entity<Plane>().Property(s => s.Msn).IsRequired().HasMaxLength(64);
            modelBuilder.Entity<Plane>().Property(s => s.IsActive).IsRequired();
            modelBuilder.Entity<Plane>().Property(s => s.FirstFlightDate).IsRequired();
            modelBuilder.Entity<Plane>().Property(s => s.LastFlightDate).IsRequired(false);
            modelBuilder.Entity<Plane>().Property(s => s.Capacity).IsRequired();
        }
    }
}