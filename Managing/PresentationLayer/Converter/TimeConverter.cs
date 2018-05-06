using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Managing.PresentationLayer.Converter
{
    /// <summary>
    /// Converter for time.
    /// </summary>
    public class TimeConverter : BaseConvertor<TimeConverter>
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
            var time = (TimeSpan?) value;

            return time?.ToString("hh\\:mm");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeString = (string) value;
            timeString = timeString?.Remove(timeString.IndexOf(' '));
            var format = "g";
            TimeSpan.TryParseExact(timeString, format, culture, out var time);
            /*Validation*/
            var digitArray = timeString?
                .Split(':')
                .Select((v, i) => new { value = short.Parse(v.Replace(":", "")), index = i })
                .ToList();
            var maxValues = new List<int> { 24, 59};

            if (digitArray?.Any(x => x.value>maxValues[x.index]) ?? false) 
                return null;
            /**/

            return time;
        }
    }
}