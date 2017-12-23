using PasswordBox.Core.Entities;
using PasswordBox.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class ProductItem : BaseEntity
    {
        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = nameof(DomainText.ProductItem_Name), ResourceType = typeof(DomainText))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = nameof(DomainText.ProductItem_Description), ResourceType = typeof(DomainText))]
        public string Description { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public ProductItem Update(ProductItem entity)
        {
            this.Name = entity.Name;
            this.Description = entity.Description;

            return this;
        }
    }
}
