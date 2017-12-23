using PasswordBox.Core.Search;
using PasswordBox.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBox.Application.Services
{
    public class CategoryService : ServiceBase<CategoryService>
    {
        public Category Add(Category entity)
        {
            ValidateEntity(entity);

            return repository.Create(entity);
        }

        public Category Update(Category entity)
        {
            ValidateEntity(entity);

            return repository.Update(entity);
        }

        public Category GetById(int id)
        {
            return repository.Get<Category>(a => a.Id == id).FirstOrDefault();
        }

        public SearchResult<Category> Search(SearchCriteria<Category> search)
        {
            return repository.Search<Category>(search);
        }

        public void Delete(int id)
        {
            repository.Delete<Category>(id);
        }

        public List<Category> GetAll()
        {
            return repository.Get<Category>().ToList();
        }
    }
}
