using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LinqToTwitter;
using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public class TweetUrlConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = (Status)value;

			if (status?.Entities?.UrlEntities?.Count > 0)
			{
				var url = status?.Entities?.UrlEntities?.FirstOrDefault();

				if (url != null)
					return url.Url;

				// try finding in text with regex
				var m = Regex.Match(status.Text, @"(http|ftp|https)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?");
				if (m.Groups.Count > 0 && !String.IsNullOrWhiteSpace(m.Groups[0].Value))
					return m.Groups[0].Value;

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
