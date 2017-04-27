using System;
using System.Linq;
using System.Globalization;
using LinqToTwitter;
using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public class TweetImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = (Status)value;

			if (status?.Entities?.MediaEntities?.Count > 0)
			{
				var image = status?.Entities?.MediaEntities?.FirstOrDefault(x => x.Type == "photo");

				if (image != null)
					return image.MediaUrl;

				return null;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
