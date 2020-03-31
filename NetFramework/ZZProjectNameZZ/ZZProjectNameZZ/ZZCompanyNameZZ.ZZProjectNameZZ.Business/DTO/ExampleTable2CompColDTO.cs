namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO
{
    using BIA.Net.Business.DTO.Infrastructure;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;


#pragma warning disable CS1591 // Missing XML Comment
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1402 // File may only contain one single class
    public class ExampleTable2CompColDTO
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        public int Id { get; set; }

        [StringLength(10)]
        [Required]
        public string Title { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }

        public string ComputedCol { get { return Title + "(" + Site.Title + ") - " + Description; } }

        [Required]
        public int SiteId
        {
            get { return Site != null ? Site.Id : 0; }
            set { Site = new SiteDTO() { Id = value }; }
        }

        [Required]
        public SiteDTO Site { get; set; }
    }

    public class ExampleTable2CompColMapper : MapperBase<ExampleTable2, ExampleTable2CompColDTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        public override Expression<Func<ExampleTable2, ExampleTable2CompColDTO>> SelectorExpression
        {
            get
            {
                return (Expression<Func<ExampleTable2, ExampleTable2CompColDTO>>)(p => new ExampleTable2CompColDTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Site = (p.Site == null) ? null : new SiteDTO() { Id = p.Site.Id, Title = p.Site.Title }
                });
            }
        }

        public override void MapToModel(ExampleTable2CompColDTO dto, ExampleTable2 model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.Title = dto.Title;
            model.Description = dto.Description;
            model.Site = (dto.Site == null) ? null : new Site() { Id = dto.Site.Id, Title = dto.Site.Title };
        }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}