namespace BIA.Net.Authentication.Business
{
#pragma warning disable CS1591 // Missing XML Comment
#pragma warning disable SA1600 // Elements must be documented
#pragma warning disable SA1402 // File may only contain one single class
    public interface IUserPropertiesInDB
    {
        /*
        int Id { get; set; }
        
        string Email { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }
        
        string Login { get; set; }
        
        string DistinguishedName { get; set; }

        bool IsEmployee { get; set; }

        bool IsExternal { get; set; }

        string ExternalCompany { get; set; }

        string Company { get; set; }

        string Site { get; set; }

        string Manager { get; set; }

        string Department { get; set; }

        string SubDepartment { get; set; }

        string Office { get; set; }

        string Language { get; set; }
        
        bool DAIEnable { get; set; }
        
        System.DateTime DAIDate { get; set; }*/

        bool IsValid { get; set; }
        string BusinessID { get; set; }
    }
#pragma warning restore CS1591 // Missing XML Comment
#pragma warning restore SA1600 // Elements must be documented
#pragma warning restore SA1402 // File may only contain one single class
}