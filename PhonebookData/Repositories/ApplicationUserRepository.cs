using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhonebookBusiness.Interfaces;
using PhonebookData.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ApplicationUser = PhonebookData.Models.ApplicationUser;

namespace PhonebookData.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserDataToBusinessModelMapper _dataToBusinessMapper;
        private readonly IUserBusinessToDataModelMapper _businessToDataMapper;

        public ApplicationUserRepository(
            UserManager<ApplicationUser> userManager,
            IUserDataToBusinessModelMapper dataToBusinessMapper,
            IUserBusinessToDataModelMapper businessToDataMapper
        )
        {
            _userManager = userManager;
            _dataToBusinessMapper = dataToBusinessMapper;
            _businessToDataMapper = businessToDataMapper;
        }

        public async Task<bool> Exists(int id)
        {
            return await _userManager.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> UserEmailExists(string email) {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        public async Task<PhonebookBusiness.Models.ApplicationUser> GetOneByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return _dataToBusinessMapper.Map(user);
        }

        public async Task<bool> CredentialsValid(string email, string password ) {

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) 
                return false;
            else 
                return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.ApplicationUser>> GetAll()
        {
            return _dataToBusinessMapper.MapList(
                await _userManager.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToListAsync()
            );
        }

        public async Task<IEnumerable<PhonebookBusiness.Models.ApplicationUser>> Search(string text)
        {
            var result = await _userManager.Users.Where(
                 u => u.FirstName.ToLower().StartsWith(text.ToLower()) 
                 || u.LastName.ToLower().StartsWith(text.ToLower()) 
                 || (u.FirstName+ " " + u.LastName).ToLower().StartsWith(text.ToLower()))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
            return _dataToBusinessMapper.MapList(result);
        }

        public async Task<PhonebookBusiness.Models.ApplicationUser> GetOneById(int id)
        {
            return _dataToBusinessMapper.Map(await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id));
        }

        public async Task<PhonebookBusiness.Models.ApplicationUser> GetOneWithContactList(int id)
        {
            return _dataToBusinessMapper.Map(await GetUserWithContactList(id));
        }

        public async Task<PhonebookBusiness.Models.ApplicationUser> Create(PhonebookBusiness.Models.ApplicationUser model)
        {
            var newUser = new ApplicationUser()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            };
            var isCreated = await _userManager.CreateAsync(newUser, model.Password);
            if (!isCreated.Succeeded)
            {
                return null;
            }
            return _dataToBusinessMapper.Map(newUser);
        }

        public async Task<PhonebookBusiness.Models.ApplicationUser> Update(int id, PhonebookBusiness.Models.ApplicationUser model)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var updated = await _userManager.UpdateAsync(user);

            if (!updated.Succeeded) throw new Exception("User update failed");

            return _dataToBusinessMapper.Map(user);
        }

       public async Task<PhonebookBusiness.Models.ApplicationUser> Update(
            int id, PhonebookBusiness.Models.ApplicationUser model, string newPassword)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            var updated = await _userManager.UpdateAsync(user);
            if (!updated.Succeeded) throw new Exception("User update failed");

            if (model.Password != newPassword)
            {
                var updatedPassword = await _userManager.ChangePasswordAsync(user, model.Password, newPassword);
                if (!updatedPassword.Succeeded) throw new Exception("Password update failed");
            }
            return _dataToBusinessMapper.Map(user);
        }

        public async Task<bool> UpdatePasword(int id, string password, string newPassword)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();
            if (user == null) return false;
           
            var result = _userManager.ChangePasswordAsync(user, password, newPassword);
            return result.IsCompletedSuccessfully;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).SingleOrDefaultAsync();

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> IsAccountOwner(int accountId, int userId) {
            var accountUser = await _userManager.Users.Where(u => u.Id == accountId).SingleOrDefaultAsync();
            if (accountUser == null)
            {
                return false;
            }
            return accountUser.Id == userId;
        }

        private async Task<ApplicationUser> GetUserWithContactList(int id)
        {
            return await _userManager.Users
                    .Include(u => u.UserContactList)
                    .ThenInclude(uc => uc.Contact)
                    .FirstOrDefaultAsync(u => u.Id == id);
        }
    }

}
