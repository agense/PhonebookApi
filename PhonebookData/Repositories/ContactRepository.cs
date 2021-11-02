using Microsoft.EntityFrameworkCore;
using PhonebookBusiness.Interfaces;
using PhonebookData.Interfaces;
using PhonebookData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhonebookData.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly AppDbContext _context;
        private readonly IContactBusinessToDataModelMapper _bussinessToDataMapper;
        private readonly IContactDataToBusinessModelMapper _dataToBusinessMapper;
        private readonly IUserDataToBusinessModelMapper _userDataToBusinessMapper;

        public ContactRepository(
            AppDbContext context,
            IContactBusinessToDataModelMapper bussinessToDataMapper,
            IContactDataToBusinessModelMapper dataToBusinessMapper,
            IUserDataToBusinessModelMapper userDataToBusinessMapper
         )
        {
            _context = context;
            _bussinessToDataMapper = bussinessToDataMapper;
            _dataToBusinessMapper = dataToBusinessMapper;
            _userDataToBusinessMapper = userDataToBusinessMapper;
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.Contact>> GetAll()
        {
            return _dataToBusinessMapper.MapList(await _context.Contacts.ToListAsync());
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.Contact>> Search(string text)
        {
            var result = await _context.Contacts.Where(
                 c => c.FirstName.ToLower().StartsWith(text.ToLower())
                 || c.LastName.ToLower().StartsWith(text.ToLower())
                 || (c.FirstName + " " + c.LastName).ToLower().Contains(text.ToLower()))
                .ToListAsync();
            return _dataToBusinessMapper.MapList(result);
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.Contact>> SearchUsersContacts(string text, int id)
        {
            var result = await _context.UserContacts
                .Where(uc => uc.UserId == id)
                .OrderBy(uc => uc.Contact.LastName)
                .Select(uc => new Contact()
                {
                    FirstName = uc.Contact.FirstName,
                    LastName = uc.Contact.LastName,
                    CountryCode = uc.Contact.CountryCode,
                    Phone = uc.Contact.Phone,
                    Id = uc.Contact.Id,
                    OwnerId = uc.OwnerId
                })
                .Distinct()
                .Where(
                 c => c.FirstName.ToLower().StartsWith(text.ToLower())
                 || c.LastName.ToLower().StartsWith(text.ToLower())
                 || (c.FirstName + " " + c.LastName).ToLower().StartsWith(text.ToLower()))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
            return _dataToBusinessMapper.MapList(result);
        }

        public async Task<bool> Exists(int contactId)
        {
            return await _context.Contacts.Where(c => c.Id == contactId).AnyAsync();
        }

        public async Task<PhonebookBusiness.Models.Contact> GetOneById(int id)
        {
            return _dataToBusinessMapper.Map(await GetContactById(id));
        }

        public async Task<PhonebookBusiness.Models.Contact> Create(PhonebookBusiness.Models.Contact contact)
        {
            var newContact = _bussinessToDataMapper.MapWithRelations(contact);
            await _context.AddAsync(newContact);

            newContact.UserContactList = new List<UserContactList>();
            newContact.UserContactList.Add(new UserContactList()
            {
                UserId = (int)newContact.OwnerId,
                OwnerId = (int)newContact.OwnerId,
            });

            await _context.SaveChangesAsync();
            return _dataToBusinessMapper.Map(newContact);
        }

        public async Task<PhonebookBusiness.Models.Contact> Update(int contactId, PhonebookBusiness.Models.Contact updatedContact)
        {
            var contact = await GetContactById(contactId);

            contact.FirstName = updatedContact.FirstName;
            contact.LastName = updatedContact.LastName;
            contact.CountryCode = updatedContact.CountryCode;
            contact.Phone = updatedContact.Phone;

            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return _dataToBusinessMapper.Map(contact);
        }

        public async Task<bool> Delete(int id)
        {
            var contact = await GetContactById(id);

            _context.Contacts.Remove(contact);

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<IEnumerable<PhonebookBusiness.Models.Contact>> GetUsersContacts(int userId) 
        {
            var userContacts = await _context.UserContacts
                .Where(uc => uc.UserId == userId)
                .OrderBy(uc => uc.Contact.LastName)
                .Select(uc => new Contact() { 
                    FirstName = uc.Contact.FirstName, 
                    LastName = uc.Contact.LastName,
                    CountryCode = uc.Contact.CountryCode,
                    Phone = uc.Contact.Phone,
                    Id = uc.Contact.Id,
                    OwnerId = uc.OwnerId
                })
                .Distinct()
                .ToListAsync();
            return _dataToBusinessMapper.MapList(userContacts);
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.Contact>> GetUsersOwnedContacts(int userId) 
        {
            var ownedContacts = await _context.UserContacts
            .Where(uc => uc.OwnerId == userId)
            .OrderBy(uc => uc.Contact.LastName)
            .Select(uc => new Contact()
            {
                FirstName = uc.Contact.FirstName,
                LastName = uc.Contact.LastName,
                CountryCode = uc.Contact.CountryCode,
                Phone = uc.Contact.Phone,
                Id = uc.Contact.Id,
                OwnerId = uc.OwnerId
            })
            .Distinct()
            .ToListAsync();
            return _dataToBusinessMapper.MapList(ownedContacts);
        }

        public async Task<bool> AddContactUser(int contactId, int userId)
        {
            var contact = await GetContactById(contactId);

            if (contact.OwnerId == null) return false;

            await _context.UserContacts.AddAsync(
                new UserContactList()
                {
                    ContactId = contact.Id,
                    UserId = userId,
                    OwnerId = (int)contact.OwnerId,
                }
                );
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveContactUser(int contactId, int userId)
        {
            var contactListItem = await _context.UserContacts
                .Where(uc => uc.Contact.Id == contactId && uc.UserId == userId)
                .SingleOrDefaultAsync();

            _context.UserContacts.Remove(contactListItem);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsContactOwner(int contactId, int userId) 
        {
            var contact = await GetOneById(contactId);
            if (contact == null || contact.OwnerId == null)
            {
                return false;
            }
            else {
                return contact.OwnerId == userId;
            }
        }


        //Check if contact exists and is in the contact list of specific user
        public async Task<bool> ExistsInUsersContacts(int contactId, int userId)
        {
            return await _context.UserContacts
            .Where(uc => uc.UserId == userId && uc.Contact.Id == contactId)
            .AnyAsync();
        }

        //Check if contact exists and is owned by specific user
        public async Task<bool> ExistsInUsersOwnedContacts(int contactId, int userId)
        {
            return await _context.UserContacts
            .Where(uc => uc.OwnerId == userId && uc.Contact.Id == contactId)
            .AnyAsync();
        }

        public async Task<PhonebookBusiness.Models.Contact> GetUsersContact(int contactId, int userId)
        {
            var contact = await _context.UserContacts
            .Where(uc => uc.UserId == userId && uc.Contact.Id == contactId)
            .Select(uc => uc.Contact).FirstOrDefaultAsync();
            return _dataToBusinessMapper.Map(contact);
        }

        public async Task<PhonebookBusiness.Models.Contact> GetUsersOwnedContact(int contactId, int userId)
        {
            var contact = await _context.UserContacts
                .Where(uc => uc.OwnerId == userId && uc.Contact.Id == contactId)
                .Select(uc => uc.Contact).FirstOrDefaultAsync();
            return _dataToBusinessMapper.Map(contact);
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.ApplicationUser>> GetContactUsers(int contactId)
        {
            var contactUsers = await _context.UserContacts
           .Where(uc => uc.Contact.Id == contactId).Select(c => c.User)
           .ToListAsync();

            return _userDataToBusinessMapper.MapList(contactUsers);
        }

        //Get all users with whom the contact is being shared
        public async Task<IEnumerable<PhonebookBusiness.Models.ApplicationUser>> GetContactsUsersExcludingOwner(
            int contactId, 
            int ownerId
        )
        {
            var contactUsers =  await _context.UserContacts
            .Where(uc => uc.Contact.Id == contactId)
            .Select(c => c.User).Where(u => u.Id != ownerId)
            .ToListAsync();
            return _userDataToBusinessMapper.MapList(contactUsers);
        }

        
        //PRIVATE METHODS
        private async Task<Contact> GetContactById(int id)
        {
            return await _context.UserContacts
                .Where(uc => uc.Contact.Id == id)
                .Select(uc => new Contact()
                {
                    Id = uc.Contact.Id,
                    FirstName = uc.Contact.FirstName,
                    LastName = uc.Contact.LastName,
                    CountryCode = uc.Contact.CountryCode,
                    Phone = uc.Contact.Phone,
                    OwnerId = uc.OwnerId
                }).
                FirstOrDefaultAsync();
        }
    }
}
