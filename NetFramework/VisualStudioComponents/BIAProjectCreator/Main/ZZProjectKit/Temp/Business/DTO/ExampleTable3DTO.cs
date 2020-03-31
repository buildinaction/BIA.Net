namespace $safeprojectname$.DTO
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
    public class ExampleTable3DTO
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Title { get; set; }

        [StringLength(200)]
        [Required]
        public string Description { get; set; }

        [Required]
        public short Value { get; set; }

        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        public decimal? MyDecimal { get; set; }

        public double? MyDouble { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? DateOnly { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public DateTime? DateAndTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public DateTime? TimeOnly { get; set; }

        [DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]
        public TimeSpan? MyTimeSpan { get; set; }

        public long? TimeSpanOver24H { get; set; }

        [Required]
        public int SiteId
        {
            get { return Site != null ? Site.Id : 0; }
            set { Site = new SiteDTO() { Id = value }; }
        }

        [Required]
        public SiteDTO Site { get; set; }
    }

    public class ExampleTable3Mapper : MapperBase<ExampleTable3, ExampleTable3DTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public override Expression<Func<ExampleTable3, ExampleTable3DTO>> SelectorExpression
        {
            get
            {
                return (Expression<Func<ExampleTable3, ExampleTable3DTO>>)(p => new ExampleTable3DTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Value = p.Value,
                    MyDecimal = p.MyDecimal,
                    MyDouble = p.MyDouble,
                    DateOnly = p.DateOnly,
                    DateAndTime = p.DateAndTime,
                    TimeOnly = p.TimeOnly,
                    MyTimeSpan = p.MyTimeSpan,
                    TimeSpanOver24H = p.TimeSpanOver24H,
                    Site = (p.Site == null) ? null : new SiteDTO() { Id = p.Site.Id, Title = p.Site.Title }
                });
            }
        }

        public override void MapToModel(ExampleTable3DTO dto, ExampleTable3 model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.Title = dto.Title;
            model.Description = dto.Description;
            model.Value = dto.Value;
            model.MyDecimal = dto.MyDecimal;
            model.MyDouble = dto.MyDouble;
            model.DateOnly = dto.DateOnly;
            model.DateAndTime = dto.DateAndTime;
            model.TimeOnly = dto.TimeOnly;
            model.MyTimeSpan = dto.MyTimeSpan;
            model.TimeSpanOver24H = dto.TimeSpanOver24H;
            model.Site = (dto.Site == null) ? null : new Site() { Id = dto.Site.Id, Title = dto.Site.Title };
        }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}