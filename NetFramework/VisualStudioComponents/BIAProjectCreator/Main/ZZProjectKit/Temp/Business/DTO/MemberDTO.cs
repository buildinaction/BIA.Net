namespace $safeprojectname$.DTO
{
    using BIA.Net.Business.DTO.Infrastructure;
    using Model;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;

#pragma warning disable CS1591 // Missing XML Comment
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1402 // File may only contain one single class
    public class MemberDTO
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName
        {
            get { return this.User?.DisplayName; }
        }

        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [JsonIgnore]
        public int UserId
        {
            get { return User != null ? User.Id : 0; }
            set { User = new UserDTO() { Id = value }; }
        }

        [Required]
        [JsonProperty(PropertyName = "user")]
        public UserDTO User { get; set; }

        [JsonIgnore]
        public ICollection<int> MemberRoleIds
        {
            get { return MemberRole?.Select(s => s.Id).ToList(); }
            set { MemberRole = value.Select(v => new MemberRoleDTO() { Id = v }).ToList(); }
        }

        [JsonProperty(PropertyName = "memberRoles")]
        public ICollection<MemberRoleDTO> MemberRole { get; set; }

        [Required]
        [JsonIgnore]
        public int SiteId
        {
            get { return Site != null ? Site.Id : 0; }
            set { Site = new SiteDTO() { Id = value }; }
        }

        [Required]
        [JsonProperty(PropertyName = "site")]
        public SiteDTO Site { get; set; }
    }

    public class MemberMapper : MapperBase<Member, MemberDTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        public override Expression<Func<Member, MemberDTO>> SelectorExpression
        {
            get
            {
                return (Expression<Func<Member, MemberDTO>>)(p => new MemberDTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    User = (p.User == null) ? null : new UserDTO() { Id = p.User.Id, Email = p.User.Email, FirstName = p.User.FirstName, LastName = p.User.LastName, Login = p.User.Login, DistinguishedName = p.User.DistinguishedName, IsEmployee = p.User.IsEmployee, IsExternal = p.User.IsExternal, ExternalCompany = p.User.ExternalCompany, Company = p.User.Company, Site = p.User.Site, Manager = p.User.Manager, Department = p.User.Department, SubDepartment = p.User.SubDepartment, Office = p.User.Office, Country = p.User.Country, DAIEnable = p.User.DAIEnable, DAIDate = p.User.DAIDate },
                    MemberRole = p.MemberRole.Select(s1 => new MemberRoleDTO() { Id = s1.Id, Title = s1.Title }).ToList(),
                    Site = (p.Site == null) ? null : new SiteDTO() { Id = p.Site.Id, Title = p.Site.Title }
                });
            }
        }

        public override void MapToModel(MemberDTO dto, Member model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.User = (dto.User == null) ? null : new User() { Id = dto.User.Id, Email = dto.User.Email, FirstName = dto.User.FirstName, LastName = dto.User.LastName, Login = dto.User.Login, DistinguishedName = dto.User.DistinguishedName, IsEmployee = dto.User.IsEmployee, IsExternal = dto.User.IsExternal, ExternalCompany = dto.User.ExternalCompany, Company = dto.User.Company, Site = dto.User.Site, Manager = dto.User.Manager, Department = dto.User.Department, SubDepartment = dto.User.SubDepartment, Office = dto.User.Office, Country = dto.User.Country, DAIEnable = dto.User.DAIEnable, DAIDate = dto.User.DAIDate };
            model.MemberRole = (dto.MemberRole == null) ? null : dto.MemberRole.Select(s1 => new MemberRole() { Id = s1.Id, Title = s1.Title }).ToList();
            model.Site = (dto.Site == null) ? null : new Site() { Id = dto.Site.Id, Title = dto.Site.Title };
        }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}