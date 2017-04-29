using System.Collections.Generic;
using System.Text;
using LinqToTwitter;
using MvvmHelpers;

namespace HGMF2017
{
	public class TweetWrapper : ObservableObject
	{
		public TweetWrapper() { }

		public TweetWrapper(Status status, string imageUrl)
		{
			status.Text = System.Net.WebUtility.HtmlDecode(status.Text);
			Status = status;
			ImageUrl = imageUrl;
		}

		public Status Status { get; private set; }

		public string ImageUrl { get; private set; }

		public string StatusUrl
		{
			get { return $"https://twitter.com/{Status.User.ScreenNameResponse}/status/{Status.StatusID}"; }
		}

		public string ScreenNameResponse => $"@{Status.User.ScreenNameResponse}";

		public bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl);

		public bool HasUrl => !string.IsNullOrWhiteSpace(StatusUrl);
	}

	public class TweetImageWrapper
	{
		public TweetImageWrapper(string imageUrl, string text, string handle, List<string> tags)
		{
			ImageUrl = imageUrl;

			Text = System.Net.WebUtility.UrlDecode(text);

			Handle = handle;

			Tags = tags;
		}

		public string Handle { get; private set; }

		public string Text { get; private set; }

		public List<string> Tags { get; private set; }

		public string TagsAsString 
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				foreach (var t in Tags)
				{
					builder.AppendFormat("#{0} ", t);
				}

				return builder.ToString().TrimEnd(' ');
			}
		}

		public string ImageUrl { get; private set;}
	}
}
