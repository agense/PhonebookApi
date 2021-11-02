using PhonebookBusiness.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhonebookBusiness.Interfaces
{

    public interface IContactRepository : IRepository<Contact>
    {

        public Task<IEnumerable<Contact>> SearchUsersContacts(string text, int id);

        public Task<IEnumerable<Contact>> GetUsersContacts(int userId);

        public Task<IEnumerable<Contact>> GetUsersOwnedContacts(int userId);

        public Task<bool> AddContactUser(int contactId, int userId);

        public Task<bool> RemoveContactUser(int contactId, int userId);

        public Task<bool> IsContactOwner(int contactId, int userId);


        public Task<bool> ExistsInUsersContacts(int contactId, int userId);

        public Task<bool> ExistsInUsersOwnedContacts(int contactId, int userId);

        public Task<Contact> GetUsersContact(int contactId, int userId);

        public Task<Contact> GetUsersOwnedContact(int contactId, int userId);

        public Task<IEnumerable<ApplicationUser>> GetContactUsers(int contactId);

        public Task<IEnumerable<ApplicationUser>> GetContactsUsersExcludingOwner(int contactId, int ownerId);
    } 
}
