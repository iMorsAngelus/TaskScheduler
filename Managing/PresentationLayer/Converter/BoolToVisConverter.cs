using System;
using System.Globalization;
using System.Windows;

namespace Managing.PresentationLayer.Converter
{
    /// <summary>
    /// Converter for visibly.
    /// </summary>
    public class BoolToVisConverter : BaseConvertor<BoolToVisConverter>
    {
        /// <summary>
        /// This method converts value from terminal info to title.
        /// </summary>
        /// <param name="value">Value for convert.</param>
        /// <param name="targetType">Value type.</param>
        /// <param name="parameter">Parametr for converting.</param>
        /// <param name="culture">Regional standart info.</param>
        /// <returns>Char array of string to display.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibly = (bool)(value?? false);

            return visibly ? Visibility.Visible:Visibility.Hidden;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}