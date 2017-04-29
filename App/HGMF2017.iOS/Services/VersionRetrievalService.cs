using System;
using HGMF2017.iOS;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(VersionRetrievalService))]
namespace HGMF2017.iOS
{
	public class VersionRetrievalService : IVersionRetrievalService
	{
		public string Version
		{
			get
			{
				NSObject ver = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
                return ver.ToString();
			}
		}
	}
}
