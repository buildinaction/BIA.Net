namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO
{
    using BIA.Net.Authentication.Business;
    using BIA.Net.Authentication.Business.Synchronize;
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
    public class UserDTO : IUserProperties, ILinkedProperties
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        [JsonProperty(PropertyName = "displayFullName")]
        public string DisplayFullName
        {
            get { return LastName + " " + FirstName + " (" + Login + ")"; }
        }

        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName
        {
            get { return LastName + " " + FirstName; }
        }

        [JsonProperty(PropertyName = "isValid")]
        public bool IsValid
        {
            get { return DAIEnable; }
            set { DAIEnable = value; }
        }

        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [StringLength(256)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "login")]
        public string Login { get; set; }

        [StringLength(250)]
        [Required]
        [JsonProperty(PropertyName = "distinguishedName")]
        public string DistinguishedName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "isEmployee")]
        public bool IsEmployee { get; set; }

        [Required]
        [JsonProperty(PropertyName = "isExternal")]
        public bool IsExternal { get; set; }

        [StringLength(50)]
        [JsonProperty(PropertyName = "externalCompany")]
        public string ExternalCompany { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "company")]
        public string Company { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "site")]
        public string Site { get; set; }

        [StringLength(250)]
        [JsonProperty(PropertyName = "manager")]
        public string Manager { get; set; }

        [StringLength(50)]
        [Required]
        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }

        [StringLength(50)]
        [JsonProperty(PropertyName = "subDepartment")]
        public string SubDepartment { get; set; }

        [StringLength(20)]
        [Required]
        [JsonProperty(PropertyName = "office")]
        public string Office { get; set; }

        [StringLength(10)]
        [Required]
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [Required]
        public bool DAIEnable { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public System.DateTime DAIDate { get; set; }

        [JsonIgnore]
        public ICollection<int> MembersIds
        {
            get { return Members?.Select(s => s.Id).ToList(); }
            set { Members = value.Select(v => new MemberDTO() { Id = v }).ToList(); }
        }

        [JsonProperty(PropertyName = "members")]
        public ICollection<MemberDTO> Members { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserDTO dTO &&
                   Id == dTO.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }

    public class UserMapper : MapperBase<User, UserDTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        public override Expression<Func<User, UserDTO>> SelectorExpression
        {
            get
            {
                return p => new UserDTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    Email = p.Email,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Login = p.Login,
                    DistinguishedName = p.DistinguishedName,
                    IsEmployee = p.IsEmployee,
                    IsExternal = p.IsExternal,
                    ExternalCompany = p.ExternalCompany,
                    Company = p.Company,
                    Site = p.Site,
                    Manager = p.Manager,
                    Department = p.Department,
                    SubDepartment = p.SubDepartment,
                    Office = p.Office,
                    Country = p.Country,
                    DAIEnable = p.DAIEnable,
                    DAIDate = p.DAIDate,
                    Members = p.Members.Select(s1 => new MemberDTO()
                    {
                        Id = s1.Id,
                        Site = (p.Site == null) ? null : new SiteDTO()
                        {
                            Id = s1.Site.Id,
                            Title = s1.Site.Title
                        },
                        MemberRole = s1.MemberRole.Select(s2 => new MemberRoleDTO()
                        {
                            Id = s2.Id,
                            Title = s2.Title
                        }).ToList()
                    }).ToList()
                };
            }
        }

        public override void MapToModel(UserDTO dto, User model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.Email = dto.Email;
            model.FirstName = dto.FirstName;
            model.LastName = dto.LastName;
            model.Login = dto.Login;
            model.DistinguishedName = dto.DistinguishedName;
            model.IsEmployee = dto.IsEmployee;
            model.IsExternal = dto.IsExternal;
            model.ExternalCompany = dto.ExternalCompany;
            model.Company = dto.Company;
            model.Site = dto.Site;
            model.Manager = dto.Manager;
            model.Department = dto.Department;
            model.SubDepartment = dto.SubDepartment;
            model.Office = dto.Office;
            model.Country = dto.Country;
            model.DAIEnable = dto.DAIEnable;
            model.DAIDate = dto.DAIDate;
            model.Members = dto.Members?.Select(s1 => new Member()
            {
                Id = s1.Id,
                MemberRole = s1.MemberRole?.Select(s2 => new MemberRole()
                {
                    Id = s2.Id,
                    Title = s2.Title
                }).ToList()
            }).ToList();
        }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}