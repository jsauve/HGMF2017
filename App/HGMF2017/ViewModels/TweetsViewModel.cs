using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LinqToTwitter;
using MvvmHelpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace HGMF2017
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

				statuses.AddRange(await SearchTweets("#hgmf17 OR @dhgmf OR from:dhgmf -filter:retweets"));

				if (statuses.Count > 0)
				{
					Tweets = new ObservableRangeCollection<TweetWrapper>();

					ImageUrls = new ObservableRangeCollection<TweetImageWrapper>();

					foreach (var s in statuses)
					{
						var statusUrl = $"{_HttpClient.BaseAddress.ToString()}{s.User.ScreenNameResponse}/status/{s.StatusID}";

						var imageUrl = (string)null;

						imageUrl = s?.Entities?.MediaEntities?.FirstOrDefault(x => x.Type == "photo")?.MediaUrl;

						if (!string.IsNullOrWhiteSpace(imageUrl))
							ImageUrls.Add(new TweetImageWrapper(imageUrl, s?.Text, s?.User?.ScreenNameResponse, s?.Entities?.HashTagEntities?.Select(x => x.Tag)?.ToList()));

						Tweets.Add(new TweetWrapper(s, imageUrl));
					}
				}
			}
			catch (Exception ex)
			{
				ex.ReportError();
				RaiseOnErrorEvent();
			}
			finally
			{
				IsBusy = false;
			}
		}

		async Task<List<Status>> SearchTweets(string query)
		{
			var result = new List<Status>();

			var auth = new ApplicationOnlyAuthorizer
			{
				CredentialStore = new InMemoryCredentialStore()
				{
					ConsumerKey = HGMF2017.Settings.TWITTER_API_CONSUMER_KEY,
					ConsumerSecret = HGMF2017.Settings.TWITTER_API_CONSUMER_SECRET
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