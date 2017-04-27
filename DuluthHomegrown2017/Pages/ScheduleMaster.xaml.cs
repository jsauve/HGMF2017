using System;
using System.Collections.Generic;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public partial class ScheduleMaster : ContentPage
	{
		bool IsAppearing;

		protected ScheduleMasterViewModel ViewModel => BindingContext as ScheduleMasterViewModel;

		public ScheduleMaster()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ViewModel.NoNetworkDetected += async (sender, e) => {
				await App.DisplayNoNetworkAlert(this);
				DaysListView.EndRefresh();
			};

			ViewModel.OnError += async(sender, e) => {
				await App.DisplayErrorAlert(this);
				DaysListView.EndRefresh();
			};
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (IsAppearing)
				return;

			if (ViewModel.IsInitialized)
				return;

			IsAppearing = true;
			
			await ViewModel.ExecuteLoadDaysCommand();

			IsAppearing = false;
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			Navigation.PushAsync(new ScheduleDetail() { BindingContext = new ScheduleDetailViewModel(ViewModel.Days, (Day)e.Item)});

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}
	}
}
