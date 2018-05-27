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
        /// <inheritdoc />
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