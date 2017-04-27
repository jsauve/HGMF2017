# HGMF2017
The unofficial app of the 2017 Duluth Homegrown Music Festival

## Setup

### App
1. Install Visual Studio 2017 + the optional Xamarin tools during installation OR install Visual Studio for Mac.
2. Open `DuluthHomegrown2017.sln` in Visual Studio (either version).
2. Rename the `DuluthHomegrown2017/SettingsTemplate.cs` file to `DuluthHomegrown2017/Settings.cs`. Note that `SettingsTemplate.cs` is excluded from the solution intentionally (to protect my API keys), so you'll need to rename this file via your file sytem.
3. Replace all the values in `Settings.cs` with your various API keys.

### Backend
1. Deploy the files from the `Backend` folder to an Azure Functions instance (https://azure.microsoft.com/en-us/services/functions/).
2. Update the URL of your Azure Function in the `DuluthHomegrown2017/Data/AzureFunctionDayDataSource.cs` file.
