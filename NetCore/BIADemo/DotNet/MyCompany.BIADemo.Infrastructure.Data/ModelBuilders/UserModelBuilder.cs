// <copyright file="UserModelBuilder.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Infrastructure.Data.ModelBuilders
{
    using Microsoft.EntityFrameworkCore;
    using MyCompany.BIADemo.Domain.UserModule.Aggregate;

    /// <summary>
    /// Class used to update the model builder for user domain.
    /// </summary>
    public static class UserModelBuilder
    {
        /// <summary>
        /// Create the user model.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void CreateModel(ModelBuilder modelBuilder)
        {
            CreateMemberModel(modelBuilder);
            CreateUserModel(modelBuilder);
            CreateRoleModel(modelBuilder);
            CreateMemberRoleModel(modelBuilder);
        }

        /// <summary>
        /// Create the model for members.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMemberModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>().HasKey(m => m.Id);
            modelBuilder.Entity<Member>().Property(m => m.SiteId).IsRequired();
            modelBuilder.Entity<Member>().Property(m => m.UserId).IsRequired();

            modelBuilder.Entity<Member>().HasOne(m => m.Site).WithMany(s => s.Members).HasForeignKey(m => m.SiteId);
            modelBuilder.Entity<Member>().HasOne(m => m.User).WithMany(u => u.Members).HasForeignKey(m => m.UserId);
        }

        /// <summary>
        /// Create the model for users.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<User>().Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Login).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.DistinguishedName).IsRequired().HasMaxLength(250);
            modelBuilder.Entity<User>().Property(u => u.IsEmployee).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsExternal).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.ExternalCompany).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Company).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Site).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Manager).HasMaxLength(250);
            modelBuilder.Entity<User>().Property(u => u.Department).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.SubDepartment).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Office).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<User>().Property(u => u.Country).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<User>().Property(u => u.DaiDate).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Guid).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.IsActive).IsRequired();
        }

        /// <summary>
        /// Create the model for roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasKey(r => r.Id);
            modelBuilder.Entity<Role>().Property(r => r.Label).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Label = "Site Admin", Code = "Site_Admin" });
            modelBuilder.Entity<Role>().HasData(new Role { Id = 2, Label = "Site Member", Code = "Site_Member" });
        }

        /// <summary>
        /// Create the model for member roles.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private static void CreateMemberRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberRole>().HasKey(mr => new { MemberId = mr.MemberId, RoleId = mr.RoleId });
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Member).WithMany(m => m.MemberRoles)
                .HasForeignKey(mr => mr.MemberId);
            modelBuilder.Entity<MemberRole>().HasOne(mr => mr.Role).WithMany(m => m.MemberRoles)
                .HasForeignKey(mr => mr.RoleId);
        }
    }
}