namespace BIA.Net.Authentication.Business.Synchronize
{
    public interface ILinkedProperties
    {
        bool IsValid { get; set; }
        string Login { get; set; }
    }
}