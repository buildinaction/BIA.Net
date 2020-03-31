namespace ZZCompanyNameZZ.ZZProjectNameZZ.MVC.ViewModel.Member
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// WindowsService VM
    /// </summary>
    public class MemberFilterVM
    {
        /// <summary>
        /// Gets or sets Member id dto list
        /// </summary>
        public MultiSelectList MslMember { get; set; }

        /// <summary>
        /// Gets or sets Member Role id dto list
        /// </summary>
        public MultiSelectList MslMemberRole { get; set; }
    }
}