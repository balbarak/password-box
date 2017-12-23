using PasswordBox.Core.Search;
using PasswordBox.Domain.Models;
using PasswordBox.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasswordBox.Application.Services
{
    public class ProductService : ServiceBase<ProductService>
    {
        public ProductService()
        {
            Includes = new[] { nameof(Product.Category), nameof(Product.Items) };
        }

        public Product Add(Product entity)
        {
            Validate(entity);

            return repository.Create(entity);
        }

        public Product Update(Product entity)
        {
            Validate(entity);

            using (UnitOfWork work = new UnitOfWork())
            {

                work.GenericRepository.Update(entity);

                UpdateItems(entity, work);

                work.Commit();
            }

            return entity;
        }
        
        public SearchResult<Product> Search(SearchCriteria<Product> search)
        {
            return repository.Search<Product>(search,Includes);
        }

        public Product GetById(int id)
        {
            return repository.Get<Product>(a => a.Id == id, includeProperties: Includes).FirstOrDefault();
        }

        public void Delete(int id)
        {
            repository.Delete<Product>(id);
        }

        private void UpdateItems(Product entity, UnitOfWork work)
        {
            var ids = entity.Items.Select(a => a.Id).ToList();

            foreach (var item in entity.Items)
            {
                //update exsit
                if (item.Id > 0)
                    work.GenericRepository.Update(item);

                //add
                if (item.Id == 0)
                    work.GenericRepository.Create(item);
            }

            //delete items
            var itemsToDelete = work.GenericRepository.Get<ProductItem>(a => a.ProductId == entity.Id && !ids.Contains(a.Id)).ToList();
            foreach (var item in itemsToDelete)
            {
                work.GenericRepository.Delete(item);
            }

        }


        private void Validate(Product entity)
        {
            ValidateEntity(entity);

            foreach (var item in entity.Items)
            {
                ValidateEntity(item);
            }
        }

    }
}
