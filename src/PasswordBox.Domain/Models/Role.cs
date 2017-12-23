using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class Role : IdentityRole
    {
        public Role Update(Role entity)
        {
            this.Name = entity.Name;
            this.NormalizedName = entity.NormalizedName;
            this.Id = entity.Id;
            this.ConcurrencyStamp = entity.ConcurrencyStamp;

            return this;
        }
    }
}
