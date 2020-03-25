namespace $safeprojectname$.ViewModel.Member
{
    using Business.DTO;
    using System.Collections.Generic;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class MemberIndexVM
    {
        /// <summary>
        /// Gets or sets filter
        /// </summary>
        public MemberFilterVM Filter { get; set; }

        /// <summary>
        /// Gets or sets the Model
        /// </summary>
        public List<MemberDTO> ListMemberDTO { get; set; }
    }
}