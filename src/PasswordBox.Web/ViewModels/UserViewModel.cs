using PasswordBox.Core.Resources;
using PasswordBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordBox.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = "اسم المستخدم")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessageResourceName = nameof(ValidationText.Email), ErrorMessageResourceType = typeof(ValidationText))]
        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; }

        //[Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        //[Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Compare(nameof(Password), ErrorMessageResourceName = nameof(ValidationText.Compare), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }

        public string[] Roles { get; set; }

        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            this.Id = user.Id;
            this.Username = user.UserName;
            this.Email = user.Email;
        }

        public User ToDomainModel()
        {
            return new User()
            {
                UserName = this.Username,
                Email = this.Email,

            };
        }
    }
}
