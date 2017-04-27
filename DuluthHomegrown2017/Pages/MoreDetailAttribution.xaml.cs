using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DuluthHomegrown2017
{
	public partial class MoreDetailAttribution : ContentPage
	{
		public MoreDetailAttribution()
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
