using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BIA.Net.MVC.Utility
{
    public abstract class FloatingPointModelBinderBase<T> : DefaultModelBinder
    {
        protected abstract Func<string, IFormatProvider, T> ConvertFunc { get; }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == null || string.IsNullOrEmpty(valueProviderResult.AttemptedValue)) return base.BindModel(controllerContext, bindingContext);
            try
            {
                return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.CurrentUICulture);
            }
            catch (FormatException)
            {
                // If format error then fallback to InvariantCulture instead of current UI culture
                return ConvertFunc.Invoke(valueProviderResult.AttemptedValue, CultureInfo.InvariantCulture);
            }
        }
    }

    public class DecimalModelBinder : FloatingPointModelBinderBase<decimal>
    {
        protected override Func<string, IFormatProvider, decimal> ConvertFunc => Convert.ToDecimal;
    }

    public class DoubleModelBinder : FloatingPointModelBinderBase<double>
    {
        protected override Func<string, IFormatProvider, double> ConvertFunc => Convert.ToDouble;
    }

    public class SingleModelBinder : FloatingPointModelBinderBase<float>
    {
        protected override Func<string, IFormatProvider, float> ConvertFunc => Convert.ToSingle;
    }
}
