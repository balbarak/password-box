using PasswordBox.Application.Identity;
using PasswordBox.Core.Exceptions;
using PasswordBox.Core.Resources;
using PasswordBox.Core.Search;
using PasswordBox.Domain.Models;
using PasswordBox.Persistance;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBox.Application.Services
{
    public class UserService : ServiceBase<UserService>
    {
        public async Task<User> Add(User entity, string password, string[] roles = null)
        {

            if (String.IsNullOrWhiteSpace(password))
                throw new BusinessException(MessageText.PleaseWritePassword);

            using (PasswordBoxUserManager manager = PasswordBoxUserManager.Create())
            {
                IdentityResult result = await manager.CreateAsync(entity, password);

                if (!result.Succeeded)
                    throw new BusinessException(result.Errors.Select(a => a.Description).ToList());

                if (roles != null)
                    await manager.AddToRolesAsync(entity, roles);
            }

            return entity;
        }

        public async Task<User> UpdateAsync(User entity, string[] roles = null)
        {

            using (PasswordBoxUserManager manager = PasswordBoxUserManager.Create())
            {
                var user = await manager.FindByIdAsync(entity.Id);

                user = user.Update(entity);

                var result = await manager.UpdateAsync(user);

                if (!result.Succeeded)
                    throw new BusinessException(result.Errors.Select(a => a.Description).ToList());

                await UpdateRolesAsync(entity.Id, roles,manager);
            }
                

            return entity;
        }

        public SearchResult<User> Search(SearchCriteria<User> search)
        {
            return repository.Search<User>(search);
        }

        public async Task<User> GetByID(string id)
        {
            User result = null;

            using (PasswordBoxUserManager manager = PasswordBoxUserManager.Create())
            {
                result = await manager.FindByIdAsync(id);
            }

            return result;
        }

        public async Task DeleteAsync(string id)
        {
            using (PasswordBoxUserManager manager = PasswordBoxUserManager.Create())
            {
                var user = await manager.FindByIdAsync(id);

                if (user != null)
                {
                    var result = await manager.DeleteAsync(user);

                    if (!result.Succeeded)
                        throw new BusinessException(result.Errors.Select(a => a.Description).ToList());
                }
            }
        }

        private async Task UpdateRolesAsync(string userId, string[] roles, PasswordBoxUserManager manager = null)
        {
            if (manager == null)
                manager = PasswordBoxUserManager.Create();

            var user = await manager.FindByIdAsync(userId);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var userRoles = await manager.GetRolesAsync(user);

            if (userRoles != null && userRoles.Count > 0)
                await manager.RemoveFromRolesAsync(user, userRoles);

            if (roles != null)
                await manager.AddToRolesAsync(user, roles);

            
        }

        private async Task<IdentityResult> RemoveUserRoles(User entity, PasswordBoxUserManager manager)
        {
            var userRoles = await manager.GetRolesAsync(entity);
            var result = await manager.RemoveFromRolesAsync(entity, userRoles);
            return result;
        }

    }
}
