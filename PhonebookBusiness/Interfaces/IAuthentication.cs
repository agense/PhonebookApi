using PhonebookBusiness.Models;

namespace PhonebookBusiness.Interfaces
{
    public interface IAuthentication
    {
        public string Authenticate(ApplicationUser user);
    }
}
