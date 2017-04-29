# HGMF2017
The unofficial app of the 2017 Duluth Homegrown Music Festival

## Get the apps now

### [HGMF2017 app for iOS in the Apple App Store](https://itunes.apple.com/us/app/hgmf2017-unofficial/id1229131015)

### [HGMF2017 app for Android in the Google Play Store](https://play.google.com/store/apps/details?id=com.joesauve.duluthhomegrown2017)

## Screenshots

![](Screenshots/featureimage.png)

<hr>

![](Screenshots/featureimage-IOS.png)

<hr>

## Setup (compiling the code for yourself)

### App
1. Install one of the following: 
  - [Visual Studio 2017](https://www.visualstudio.com/downloads/) (free Community Edition works great) + the optional Xamarin tools during installation
  - [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/)
  - [Xamarin Studio](https://www.xamarin.com/download) (currently my preference, until VS for Mac stabilizies a little more)
2. Open `App/HGMF2017.sln` usng one of the above.
2. Rename the [`App/HGMF2017/SettingsTemplate.cs`](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017/SettingsTemplate.cs) file to `App/HGMF2017/Settings.cs`. Note that `SettingsTemplate.cs` is excluded from the solution intentionally, so you'll need to rename this file via your file system. When you do this, the `Settings.cs` file that Visual Studio may have complained is missing will now be present in the solution.
3. Replace all the values in `Settings.cs` with your various API keys. 

#### NOTE
You may not need all the API keys that you see in `Settings.cs`. If you poke around in the code, you'll see that I'm using some services for crash reporting and analytics, like Pyze and Azure Mobile Center. If you don't need these, then just remove the few lines of code that enable these things:
- [Mobile Center in iOS](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017.iOS/AppDelegate.cs#L19)
- [Mobile Center in Android](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017.Droid/MainActivity.cs#L21)
- [Pyze in iOS](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017.iOS/AppDelegate.cs#L38)
- [Pyze in Android](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017.Droid/MainActivity.cs#L22)

If you want the app to function properly, at the bare minimum you'll need an Azure account (for hosting the Azure Function code) and a Twitter developer account (for pulling in the tweets). Enter the relevant API keys for these services in the `Settings.cs` file.


### Backend
1. Deploy the files from the `Backend` folder to an [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) instance.
2. Update the URL of your Azure Function in the [`App/HGMF2017/Data/DayDataSource.cs`](https://github.com/jsauve/HGMF2017/blob/6a3a991056ab4ac1f7f03732ad21a6a800f48dfb/App/HGMF2017/Data/DayDataSource.cs) file.
