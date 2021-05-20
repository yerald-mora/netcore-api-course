using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCoreMoviesAPI.Helpers
{
    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var propertyName = bindingContext.ModelName;
            var valuesProvider = bindingContext.ValueProvider.GetValue(propertyName);

            if (valuesProvider == ValueProviderResult.None)
                return Task.CompletedTask;

            try
            {
                var deserializedVal = JsonConvert.DeserializeObject<T>(valuesProvider.FirstValue);

                bindingContext.Result = ModelBindingResult.Success(deserializedVal);
            }
            catch (Exception)
            {

                bindingContext.ModelState.TryAddModelError(propertyName, $"Invalid value for type {typeof(T)}");
            }

            return Task.CompletedTask;
        }
    }
}
