using PasswordBox.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PasswordBox.Domain.Models
{
    public class Account : AuditableEntity
    {
        [Required]
        [StringLength(2048)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Website { get; set; }

        [StringLength(2048)]
        public string Note { get; set; }
    }
}
