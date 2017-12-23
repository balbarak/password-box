using PasswordBox.Core.Entities;
using PasswordBox.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class Product : AuditableEntity
    {
        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = nameof(DomainText.Product_Name), ResourceType = typeof(DomainText))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        [Display(Name = nameof(DomainText.Product_Descriptoin), ResourceType = typeof(DomainText))]
        public string Description { get; set; }
        
        [Display(Name = nameof(DomainText.Product_Category), ResourceType = typeof(DomainText))]
        [Required(ErrorMessageResourceName = nameof(ValidationText.Required), ErrorMessageResourceType = typeof(ValidationText))]
        public int CategoryID { get; set; }

        [ForeignKey(nameof(CategoryID))]
        public Category Category { get; set; }

        public List<ProductItem> Items { get; set; }

        public Product()
        {
            Items = new List<ProductItem>();
        }

        public Product Update(Product entity)
        {
            this.Name = entity.Name;
            this.Description = entity.Description;
            this.CategoryID = entity.CategoryID;

            UpdateItems(entity.Items);

            return this;
        }


        private void UpdateItems(List<ProductItem> options)
        {
            if (this.Items == null)
                this.Items = new List<ProductItem>();

            if (options == null)
            {
                this.Items.Clear();
                return;
            }

            //Delete Not Exist items
            this.Items.RemoveAll(a => !options.Where(p => p.Id == a.Id).Any());

            //AddOrUpdate
            foreach (var item in options)
            {
                if (item.Id == 0)
                    this.Items.Add(item);
                else
                {
                    var orginal = this.Items.Where(a => a.Id == item.Id).FirstOrDefault();

                    if (orginal != null)
                        orginal.Update(item);
                }
            }
        }
    }
}
