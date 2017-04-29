using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace HGMF2017
{
	public partial class MoreMaster : ContentPage
	{
		public ObservableCollection<MoreItem> Names { get; set; }

		public MoreMaster()
		{
			BindingContext = this;

			Names = new ObservableCollection<MoreItem>() {
				new MoreItem("Tickets"),
				new MoreItem("About Homegrown"),
				new MoreItem("Contact"),
				new MoreItem("News"),
				new MoreItem("About this app")
			};

			InitializeComponent();
		}

		/// <summary>
		/// The action to take when a list item is tapped.
		/// </summary>
		/// <param name="sender"> The sender.</param>
		/// <param name="e">The ItemTappedEventArgs</param>
		void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var name = ((MoreItem)e.Item).Name;

			switch (name)
			{
			case "Tickets":
				this.Navigation.PushAsync(new MoreDetailTickets());
				break;
			case "About Homegrown":
				this.Navigation.PushAsync(new MoreDetailAbout());
				break;
			case "Contact":
				this.Navigation.PushAsync(new MoreDetailContact());
				break;
			case "News":
				this.Navigation.PushAsync(new News());
				break;
			case "About this app":
				this.Navigation.PushAsync(new MoreDetailAttribution());
				break;
			}

			// prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
		}
	}

	public class MoreItem
	{
		public string Name { get; set; }

		public MoreItem(string name)
		{
			Name = name;
		}
	}
}
