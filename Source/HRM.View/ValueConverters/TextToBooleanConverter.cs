using System;
using System.Globalization;

namespace HRM.View
{
    /// <summary>
    /// A converter that takes in text and checks if it is null
    /// </summary>
    public class TextToBooleanConverter : BaseValueConverter<TextToBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return string.IsNullOrEmpty(value.ToString()) ? true : false;
            else
                return string.IsNullOrEmpty(value.ToString()) ? false : true;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
