using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace webApi.Helpers
{
    //model binder for array from route
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            //get the string
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
            }
            //get the first Type of the InputElementType
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            
            //create a converter
            var converter = TypeDescriptor.GetConverter(elementType);
            //the second parameter is remove empty ,if it exists after ","
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(text: x.Trim())).ToArray();

            var typeValues = Array.CreateInstance(elementType, values.Length);//create a new ArrayInstance
            values.CopyTo(typeValues, index: 0);
            //now! Model is a Guid class
            bindingContext.Model = typeValues;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
