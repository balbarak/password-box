using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordBox.Web.Helpers
{
    public class DateTimeBinder : IModelBinder
    {
        SimpleTypeModelBinder _baseBinder;

        public DateTimeBinder()
        {

        }

        public DateTimeBinder(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            _baseBinder = new SimpleTypeModelBinder(type);
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            string hijriPattern = @"\d{4}-\d{1,2}-\d{1,2}";
            string monthPattern = @"\d{4}-\d{1,2}";

            // Check the value sent in
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                // Attempt to scrub the input value
                var valueAsString = valueProviderResult.FirstValue;

                object result = new DateTime();

                if (valueAsString.Contains("-"))
                {
                    bool isHijriDate = Regex.Match(valueAsString, hijriPattern).Success;
                    bool isMonth = Regex.Match(valueAsString, monthPattern).Success;

                    if (isHijriDate)
                        result = DateTime.ParseExact(valueAsString, "yyyy-MM-dd", new CultureInfo("ar-SA"));
                    else if (isMonth)
                        result = DateTime.ParseExact(valueAsString, "yyyy-MM", new CultureInfo("en-US"));
                    else
                        result = DateTime.ParseExact(valueAsString, "dd-MM-yyyy", new CultureInfo("en-US"));
                }

                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
            // If we haven't handled it, then we'll let the base SimpleTypeModelBinder handle it
            return _baseBinder.BindModelAsync(bindingContext);
        }
    }
}