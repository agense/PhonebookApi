using PhonebookBusiness.Models;
using System.Threading.Tasks;

namespace PhonebookBusiness.Interfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {

        public Task<bool> IsAccountOwner(int accountId, int userId);

        public Task<ApplicationUser> GetOneByEmail(string email);

        public Task<ApplicationUser> GetOneWithContactList(int id);

        public Task<bool> UserEmailExists(string email);

        public Task<bool> CredentialsValid(string email, string password);

        public Task<bool> UpdatePasword(int id, string password, string newPassword);

        public Task<ApplicationUser> Update(int id, ApplicationUser model, string newPassword);

    }
}
