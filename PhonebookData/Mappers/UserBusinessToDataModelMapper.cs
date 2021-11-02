using PhonebookData.Interfaces;
using PhonebookData.Models;
using System.Collections.Generic;
using System.Linq;

namespace PhonebookData.Mappers
{
    public class UserBusinessToDataModelMapper: IUserBusinessToDataModelMapper
    {
        private readonly IPhonebookBusinessToDataContactListItemMapper _contactListMapper;

        public UserBusinessToDataModelMapper(IPhonebookBusinessToDataContactListItemMapper contactListMapper)
        {
            _contactListMapper = contactListMapper;
        }

        public ApplicationUser Map(PhonebookBusiness.Models.ApplicationUser user) { 
            return new ApplicationUser()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserContactList = new List<UserContactList>()
            };
        }

        public ApplicationUser MapWithRelations(PhonebookBusiness.Models.ApplicationUser user)
        {
            var userEntity = Map(user);

            if (user.UserContactListItems != null && user.UserContactListItems.Count() > 0)
            {   
                foreach (var contactListItem in user.UserContactListItems)
                {
                    userEntity.UserContactList.Add(_contactListMapper.Map(contactListItem));
                }
            }
            return userEntity;
        }

        public IEnumerable<ApplicationUser> MapList(
            IEnumerable<PhonebookBusiness.Models.ApplicationUser> userList
        )
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (var user in userList)
            {
                users.Add(Map(user));
            }
            return users;
        }
    }
}
