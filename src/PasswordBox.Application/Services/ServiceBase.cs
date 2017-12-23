using PasswordBox.Core.Exceptions;
using PasswordBox.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PasswordBox.Application.Services
{
    public class ServiceBase<TService> where TService : class, new()
    {
        protected static TService instance;

        protected internal GenericRepository repository;

        protected string[] Includes { get; set; }

        public static TService Instance { get { return instance; } }

        protected ServiceBase()
        {
            this.repository = new GenericRepository();
        }

        static ServiceBase()
        {
            instance = new TService();
        }

        public void ValidateEntity(object entity)
        {
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entity, new ValidationContext(entity), validationResults))
            {
                var erros = new List<string>();

                foreach (var item in validationResults)
                    erros.Add(item.ErrorMessage);

                throw new EntityValidationException(erros);
            }
        }

    }
}
