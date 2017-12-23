using PasswordBox.Application.Identity;
using PasswordBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordBox.Application.Services
{
    public class RoleService : ServiceBase<RoleService>
    {
        public List<Role> GetAll()
        {
            return repository.Get<Role>().ToList();
        }

        public async Task<List<string>> GetRolesByUserIdAsync(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
                return new List<string>();

            List<string> result = new List<string>();

            using (PasswordBoxUserManager manager = PasswordBoxUserManager.Create())
            {
                var user = await manager.FindByIdAsync(id);

                if (user != null)
                {
                    var roles = await manager.GetRolesAsync(user);

                    result = roles.ToList();
                }
            }

            return result;
        }
    }
}
