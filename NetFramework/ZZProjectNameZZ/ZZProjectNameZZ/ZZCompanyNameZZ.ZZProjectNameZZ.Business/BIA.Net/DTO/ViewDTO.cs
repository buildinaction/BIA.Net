namespace BIA.Net.Business.DTO
{
#pragma warning disable SA1210 // Using directives must be ordered alphabetically by namespace
    using ZZCompanyNameZZ.ZZProjectNameZZ.Model;
#pragma warning restore SA1210 // Using directives must be ordered alphabetically by namespace

    using BIA.Net.Business.DTO.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;

    /// <summary>
    /// Type of the view in the grid
    /// </summary>
    public enum TypeOfView
    {
        /// <summary>
        /// Default System
        /// </summary>
        Undefined = -1,

        /// <summary>
        /// System view set as default view
        /// </summary>
        SystemDefault = 0,

        /// <summary>
        /// Site view can be manage by site Admin
        /// </summary>
        Site = 1,

        /// <summary>
        /// User view can be manage by the user
        /// </summary>
        User = 2,

        /// <summary>
        /// System view view by every body
        /// </summary>
        System = 3
    }

#pragma warning disable CS1591 // Missing XML Comment
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1402 // File may only contain one single class
    public class ViewDTO
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        public ViewDTO()
        {
        }

        public int SitesAssignedCount { get; set; }

        public bool IsDefaultViewForThisSite { get; set; }

        public bool IsAssignedToThisSite { get; set; }

        ////ECC/ END CUSTOM CODE SECTION

        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string TableId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string Preference { get; set; }

        [Required]
        public TypeOfView ViewType { get; set; }
    }

    public class Preference<FilterAdvanced>
    {
        public FilterAdvanced AdvancedFilterValues { get; set; }

        /*public Dictionary<int, string> headerFilterValues { get; set; }

        public object datatableOption { get; set; }*/
    }

    public class ViewMapper : MapperBase<View, ViewDTO>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public override Expression<Func<View, ViewDTO>> SelectorExpression
        {
            get
            {
                return p => new ViewDTO()
                {
                    ////BCC/ BEGIN CUSTOM CODE SECTION
                    ////ECC/ END CUSTOM CODE SECTION
                    Id = p.Id,
                    TableId = p.TableId,
                    Name = p.Name,
                    Description = p.Description,
                    Preference = p.Preference,
                    ViewType = (TypeOfView)p.ViewType,
                };
            }
        }

        public override void MapToModel(ViewDTO dto, View model)
        {
            ////BCC/ BEGIN CUSTOM CODE SECTION
            ////ECC/ END CUSTOM CODE SECTION
            model.Id = dto.Id;
            model.TableId = dto.TableId;
            model.Name = dto.Name;
            model.Description = dto.Description;
            model.Preference = dto.Preference;
            model.ViewType = (int)dto.ViewType;
        }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}