using PasswordBox.Core.Extensions;
using PasswordBox.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;

namespace PasswordBox.Core.Entities
{
    public class AuditableEntity : BaseEntity
    {
        [Display(Name = nameof(DomainText.AuditableEntity_CreatedDate), ResourceType = typeof(DomainText))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = nameof(DomainText.AuditableEntity_ModifiedDate), ResourceType = typeof(DomainText))]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = nameof(DomainText.AuditableEntity_CreatedBy), ResourceType = typeof(DomainText))]
        public string CreatedByUserId { get; set; }

        public string ModifiedByUserId { get; set; }

        public void InsertAudit()
        {
            this.CreatedDate = DateTime.Now;
            this.CreatedByUserId = Thread.CurrentPrincipal.GetUserId();
        }

        public void UpdateAudit()
        {
            this.ModifiedDate = DateTime.Now;
            this.ModifiedByUserId = Thread.CurrentPrincipal.GetUserId();
        }
    }
}
