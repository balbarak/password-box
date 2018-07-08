using PasswordBox.Core.Exceptions;
using PasswordBox.Core.Extensions;
using PasswordBox.Core.Search;
using PasswordBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PasswordBox.Application.Services
{
    public class VaultService : ServiceBase<VaultService>
    {
        public Vault Add(Vault entity)
        {
            var userId = Thread.CurrentPrincipal.GetUserId();

            if (String.IsNullOrWhiteSpace(userId))
                throw new PermissionException();

            var user = UserService.Instance.GetByID(userId).GetAwaiter().GetResult();

            ValidateEntity(entity);

            entity = entity.Encrypt(user.PasswordHash);

            entity.UserId = user.Id;

            return repository.Create(entity);
        }

        public void Delete(int id)
        {
            repository.Delete<Vault>(id);
        }

        public Vault GetById(int id)
        {
            var userId = Thread.CurrentPrincipal.GetUserId();

            var user = UserService.Instance.GetByID(userId).GetAwaiter().GetResult();

            if (user == null)
                throw new PermissionException();

            var result = repository.Get<Vault>(a => a.Id == id).FirstOrDefault();

            if (result != null && !result.CreatedByUserId.Equals(userId))
                throw new PermissionException();

            if (result != null)
                result = result.Decrypt(user.PasswordHash);

            return result;
        }

        public SearchResult<Vault> Search(SearchCriteria<Vault> search)
        {
            var userId = Thread.CurrentPrincipal.GetUserId();

            if (search.FilterExpression == null)
                search.FilterExpression = a => true;


            search.FilterExpression = search.FilterExpression.And(p => p.UserId == userId);

            return repository.Search(search);
        }
    }
}
