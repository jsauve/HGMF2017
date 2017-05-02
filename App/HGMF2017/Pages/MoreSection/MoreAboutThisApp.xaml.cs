using Xamarin.Forms;

namespace HGMF2017
{
	public partial class MoreAboutThisApp : ContentPage
	{
		public MoreAboutThisApp()
		{
			BindingContext = this;

			InitializeComponent();
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
