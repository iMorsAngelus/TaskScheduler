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
        /// <inheritdoc />
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan?) value;

            return time?.ToString("hh\\:mm");
        }

        /// <inheritdoc />
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