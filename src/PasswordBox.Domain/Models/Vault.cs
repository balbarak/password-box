using PasswordBox.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class Vault : AuditableEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(2048)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public string Website { get; set; }

        [StringLength(2048)]
        public string Note { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
