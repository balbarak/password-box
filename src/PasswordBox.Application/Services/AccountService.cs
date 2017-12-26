using PasswordBox.Core.Exceptions;
using PasswordBox.Core.Extensions;
using PasswordBox.Core.Search;
using PasswordBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PasswordBox.Application.Services
{
    public class AccountService : ServiceBase<AccountService>
    {

        public Account Add(Account account)
        {
            var userId = Thread.CurrentPrincipal.GetUserId();

            if (String.IsNullOrWhiteSpace(userId))
                throw new PermissionException();

            var user = UserService.Instance.GetByID(userId).Result;

            ValidateEntity(account);

            account.Password = EncryptionService.Instance.Encrypt(account.Password,user.PasswordHash);
            account.UserId = user.Id;

            return repository.Create(account);
        }

        public void Delete(int id)
        {
            repository.Delete<Account>(id);
        }

        public SearchResult<Account> Search(SearchCriteria<Account> search)
        {
            var userId = Thread.CurrentPrincipal.GetUserId();

            if (search.FilterExpression == null)
                search.FilterExpression = a => true;


            search.FilterExpression = search.FilterExpression.And(p => p.UserId == userId);

            return repository.Search(search);
        }
    }
}
