using System;
using Windows.UI.Xaml.Data;

namespace Spp.Presentation.User.Client.Converters
{
    public class ObjectExistsToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            return (value == null) ? Windows.UI.Xaml.Visibility.Collapsed
                : Windows.UI.Xaml.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, string culture)
        {
            throw new NotImplementedException();
        }
    }
}
