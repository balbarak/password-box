using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Core.Exceptions
{
    public class EntityValidationException : BusinessException
    {
        public EntityValidationException(List<string> errors) : base(errors)
        {

        }
    }
}
