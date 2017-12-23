using PasswordBox.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordBox.Web.ViewModels
{
    public class SearchViewModelBase<TModel> : SearchCriteria<TModel> where TModel : class
    {
        public virtual SearchCriteria<TModel> ToSearchModel()
        {
            if (FilterExpression == null)
                this.FilterExpression = a => true;

            return this;
        }

    }
}
