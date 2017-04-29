using System;
using System.Globalization;
using Xamarin.Forms;

namespace HGMF2017
{
	public class TweetTimestampConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var createdAt = (DateTime)value;

			return createdAt.ToTweetTimestampFormat();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
