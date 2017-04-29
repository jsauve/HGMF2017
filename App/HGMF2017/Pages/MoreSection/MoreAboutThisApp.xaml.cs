using Xamarin.Forms;

namespace HGMF2017
{
	public partial class MoreAboutThisApp : ContentPage
	{
		public MoreAboutThisApp()
		{
			BindingContext = this;

			InitializeComponent();
			
			IconLabel.IsEnabled = false;
			IconLabel.IsVisible = false;
			IconLabelText.IsEnabled = false;
			IconLabelText.IsVisible = false;
		}

		public string Version 
		{
			get 
			{
				return DependencyService.Get<IVersionRetrievalService>().Version;
			}
		}
	}
}
