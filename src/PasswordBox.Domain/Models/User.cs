using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class User : IdentityUser
    {
        public DateTime? CreatedDate { get; set; }

        public User()
        {
            CreatedDate = DateTime.Now;
        }

        public User Update(User entity)
        {
            this.UserName = entity.UserName;
            this.Email = entity.Email;

            return this;
        }
    }
}
