using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TogoFogo.Filters
{
    public class DateTimeModelBinder: IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = valueProviderResult.AttemptedValue;

            if (String.IsNullOrEmpty(date))
            {
                return null;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            try
            {
                // Parse DateTimeusing current culture.
                return DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, String.Format("\"{0}\" is invalid.", bindingContext.ModelName));
                return null;
            }
        }
    }
}