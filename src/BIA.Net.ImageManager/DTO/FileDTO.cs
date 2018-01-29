namespace BIA.Net.ImageManager.DTO
{
    /// <summary>
    /// File DTO
    /// </summary>
    public class FileDTO
    {
        /// <summary>
        /// Gets or sets the binary content
        /// </summary>
        public byte[] Binary { get; set; }

        /// <summary>
        /// Gets or sets the content type (MIME type).
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string Name { get; set; }
    }
}