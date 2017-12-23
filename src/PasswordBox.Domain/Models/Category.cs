using PasswordBox.Core.Entities;
using PasswordBox.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = nameof(DomainText.Category_Name), ResourceType = typeof(DomainText))]
        public string Name { get; set; }


        public Category Update(Category entity)
        {
            this.Name = entity.Name;

            return this;
        }

    }
}
