using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using MvvmHelpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public class TweetsViewModel : BaseNavigationViewModel
	{
		HttpClient _HttpClient = new HttpClient() { BaseAddress = new Uri("https://twitter.com/") };

		public event EventHandler NoNetworkDetected;

		protected virtual void RaiseNoNetworkDetectedEvent()
		{
			EventHandler handler = NoNetworkDetected;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public event EventHandler OnError;

		protected virtual void RaiseOnErrorEvent()
		{
			EventHandler handler = OnError;

			if (handler != null)
				handler(this, new EventArgs());
		}

		public bool IsInitialized { get { return Tweets.Count > 0; } }

		ObservableRangeCollection<TweetWrapper> _Tweets;
		public ObservableRangeCollection<TweetWrapper> Tweets
		{
			get { return _Tweets ?? (_Tweets = new ObservableRangeCollection<TweetWrapper>()); }
			set
			{
				_Tweets = value;
				OnPropertyChanged("Tweets");
			}
		}

		ObservableRangeCollection<TweetImageWrapper> _ImageUrls;
		public ObservableRangeCollection<TweetImageWrapper> ImageUrls
		{
			get { return _ImageUrls ?? (_ImageUrls = new ObservableRangeCollection<TweetImageWrapper>()); }
			set
			{
				_ImageUrls = value;
				OnPropertyChanged("ImageUrls");
			}
		}

		Command _LoadTweetsCommand;
		public Command LoadTweetsCommand
		{
			get { return _LoadTweetsCommand ?? (_LoadTweetsCommand = new Command(async () => await ExecuteLoadTweetsCommand())); }
		}

		public async Task ExecuteLoadTweetsCommand()
		{
			LoadTweetsCommand.ChangeCanExecute();

			await FetchTweets();

			LoadTweetsCommand.ChangeCanExecute();
		}

		Command _RefreshTweetsCommand;
		public Command RefreshTweetsCommand
		{
			get { return _RefreshTweetsCommand ?? (_RefreshTweetsCommand = new Command(async () => await ExecuteRefreshTweetsCommand())); }
		}

		async Task ExecuteRefreshTweetsCommand()
		{
			RefreshTweetsCommand.ChangeCanExecute();

			await FetchTweets();

			RefreshTweetsCommand.ChangeCanExecute();
		}

		async Task FetchTweets()
		{
			if (!CrossConnectivity.Current.IsConnected)
			{
				RaiseNoNetworkDetectedEvent();
				return;
			}

			if (IsBusy)
				return;

			IsBusy = true;

			try
			{
				var statuses = new List<Status>();

				statuses.AddRange(await Search("#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets"));

				if (statuses.Count > 0)
				{
					Tweets = new ObservableRangeCollection<TweetWrapper>();

					ImageUrls = new ObservableRangeCollection<TweetImageWrapper>();

					foreach (var s in statuses)
					{
						var statusUrl = $"{_HttpClient.BaseAddress.ToString()}{s.User.ScreenNameResponse}/status/{s.StatusID}";

						var imageUrl = (string)null;

						imageUrl = s?.Entities?.MediaEntities?.FirstOrDefault(x => x.Type == "photo")?.MediaUrl;

						if (!String.IsNullOrWhiteSpace(imageUrl))
							ImageUrls.Add(new TweetImageWrapper(imageUrl, s?.Text, s?.User?.ScreenNameResponse, s?.Entities?.HashTagEntities?.Select(x => x.Tag)?.ToList()));

						Tweets.Add(new TweetWrapper(s, imageUrl));
					}

#if DEBUG
					//if (Tweets.Count > 0)
					//{
					//	foreach (var t in Tweets)
					//	{
					//		System.Diagnostics.Debug.WriteLine($"ScreenNameResponse: {t.Status?.User?.ScreenNameResponse}");
					//		System.Diagnostics.Debug.WriteLine($"Text: {t.Status.Text}");
					//		System.Diagnostics.Debug.WriteLine($"Retweeted: {t.Status.Retweeted}");
					//		System.Diagnostics.Debug.WriteLine($"CurrentUserRetweet: {t.Status.CurrentUserRetweet}");
					//		System.Diagnostics.Debug.WriteLine($"Favorited: {t.Status.Favorited}");
					//		System.Diagnostics.Debug.WriteLine($"FavoriteCount: {t.Status.FavoriteCount}");
					//		System.Diagnostics.Debug.WriteLine($"CreatedAt: {t.Status.CreatedAt}");
					//		System.Diagnostics.Debug.WriteLine($"OEmbedUrl: {t.Status.OEmbedUrl}");
					//		WriteLineEntities(t.Status.Entities.MediaEntities);

					//	}
					//}
#endif
				}
			}
			catch (Exception ex)
			{
				ex.ReportToHockeyApp("TweetsViewModel-FetchTweets");
				RaiseOnErrorEvent();
			}
			finally
			{
				IsBusy = false;
			}
		}

		void WriteLineEntities(List<MediaEntity> mediaEntities)
		{
			if (mediaEntities.Count > 0)
			{
				foreach (var me in mediaEntities)
				{
					System.Diagnostics.Debug.WriteLine($"ID: {me.ID}");
					System.Diagnostics.Debug.WriteLine($"Type: {me.Type}");
					System.Diagnostics.Debug.WriteLine($"Url: {me.Url}");
					System.Diagnostics.Debug.WriteLine($"DisplayUrl: {me.DisplayUrl}");
					System.Diagnostics.Debug.WriteLine($"ExpandedUrl: {me.ExpandedUrl}");
					System.Diagnostics.Debug.WriteLine($"MediaUrl: {me.MediaUrl}");
					System.Diagnostics.Debug.WriteLine($"MediaUrlHttps: {me.MediaUrlHttps}");

					foreach (var size in me.Sizes)
					{
						System.Diagnostics.Debug.WriteLine($"Type: {size.Type}");
						System.Diagnostics.Debug.WriteLine($"Width: {size.Width}");
						System.Diagnostics.Debug.WriteLine($"Height: {size.Height}");
						System.Diagnostics.Debug.WriteLine($"Resize: {size.Resize}");
					}
				}
			}
		}

		async Task<List<Status>> Search(string query)
		{
			var result = new List<Status>();

			var auth = new ApplicationOnlyAuthorizer
			{
				CredentialStore = new InMemoryCredentialStore()
				{
					ConsumerKey = Settings.TWITTER_API_CONSUMER_KEY,
					ConsumerSecret = Settings.TWITTER_API_CONSUMER_SECRET
				}
			};

			await auth.AuthorizeAsync();

			var twitterCtx = new TwitterContext(auth);

			var searchResponse =
				await
				(from search in twitterCtx.Search
				 where 
				 search.Type == SearchType.Search &&
				 search.Count == 1000 &&
				 search.ResultType == ResultType.Mixed &&
				 search.IncludeEntities == true &&
				 search.Query == query
				 select search)
				.SingleOrDefaultAsync();

			var statuses = new List<Status>();

			if (searchResponse?.Statuses != null)
			{
				result = searchResponse.Statuses;
			}

			return result;
		}
	}
}
