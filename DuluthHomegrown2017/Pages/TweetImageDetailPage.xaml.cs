using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public partial class TweetImageDetailPage : ContentPage
	{
		protected TweetsViewModel ViewModel => BindingContext as TweetsViewModel;

		public TweetImageDetailPage()
		{
			InitializeComponent();
		}
	}
}
