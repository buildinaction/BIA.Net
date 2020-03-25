namespace BIA.Net.ImageManager.ViewModel
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    /// <summary>
    /// UploadFile ViewModel
    /// </summary>
    public class UploadFileVM
    {
        /// <summary>
        /// Gets or sets Entity Id
        /// </summary>
        [Required]
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets Entity name
        /// </summary>
        [Required]
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets upload File
        /// </summary>
        public HttpPostedFileBase UploadFile { get; set; }
    }
}
