# HGMF2017
The unofficial app of the 2017 Duluth Homegrown Music Festival

## Setup

### App
1. Rename the `SettingsTemplate.cs` file (not included to the solution) to `Settings.cs`.
2. Replace all the values in `Settings.cs` with your various API keys.

### Backend
1. Deploy the files from the `Backend" folder to an Azure Functions instance.
2. Update the URL of your Azure Function in the `DuluthHomegrown2017/Data/AzureFunctionDayDataSource.cs` file.
