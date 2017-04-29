using System;
using System.Globalization;
using Xamarin.Forms;
namespace HGMF2017
{
	public class TweetConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((string)value).TransformToTweetFormattedText();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
