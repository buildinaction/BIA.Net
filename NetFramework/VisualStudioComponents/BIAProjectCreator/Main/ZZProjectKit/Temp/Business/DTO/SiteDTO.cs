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
    public class SiteDTO
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonIgnore]
        public ICollection<int> MembersIds
        {
            get { return Members?.Select(s => s.Id).ToList(); }
            set { Members = value.Select(v => new MemberDTO() { Id = v }).ToList(); }
        }

        [JsonProperty(PropertyName = "members")]
        public ICollection<MemberDTO> Members { get; set; }
    }

    public class SiteMapper : MapperBase<Site, SiteDTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        public override Expression<Func<Site, SiteDTO>> SelectorExpression
        {
            get
            {
                return (Expression<Func<Site, SiteDTO>>)(p => new SiteDTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    Title = p.Title,
                    Members = p.Members.Select(s1 => new MemberDTO() { Id = s1.Id, User = (s1.User == null) ? null : new UserDTO() { Id = s1.User.Id, FirstName = s1.User.FirstName, LastName = s1.User.LastName }, MemberRole = s1.MemberRole.Select(s2 => new MemberRoleDTO() { Id = s2.Id, Title = s2.Title }).ToList() }).ToList()
                });
            }
        }

        public override void MapToModel(SiteDTO dto, Site model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.Title = dto.Title;
            model.Members = dto.Members?.Select(s1 => new Member() { Id = s1.Id, User = (s1.User == null) ? null : new User() { Id = s1.User.Id, FirstName = s1.User.FirstName, LastName = s1.User.LastName }, MemberRole = s1.MemberRole?.Select(s2 => new MemberRole() { Id = s2.Id, Title = s2.Title }).ToList() }).ToList();
        }
    }
}
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class