using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Core.Entities
{
    [Serializable]
    public class BaseEntity
    {
        public virtual int Id { get; set; }
    }
}
