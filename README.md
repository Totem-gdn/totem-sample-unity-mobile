# Totem plugin build sample Android/IOS
Sample Unity project for Android and iOS. Contains the demonstration of the functionality available in Totem plugin for Unity.

# Build instructions
To build your game with the Totem plugin in it you need to perform this simple steps:
1) Add totem plugin to your projects from the git page https://github.com/Totem-gdn/TotemGeneratorUnity
2) Before building the application you need to generate a deep link which can be easily done by going to **Window > Totem Generator > Generate Deep Link** and typing in your gameId
3) Set your **Api Compatibility Level**, this can be done by going to **Edit > Project Settings > Player > Android/IOS tab > Configuration Section > Api Compatibility Level** and pick **.NET Framework**

# Plugin Instructions
1) User login. To login into plugin create a new TotemCore instance with the _gameId parameter. After that you can call AuthenticateCurrentUser method with the callback function as a parameter.
```C#
[Header("Demo")]
public string _gameId = "TotemDemo"; 

/// <summary>
/// Initializing TotemCore
/// </summary>
void Start()
{
  totemCore = new TotemCore(_gameId);
}

public void OnLoginButtonClick()
{
  //Login user
  totemCore.AuthenticateCurrentUser(OnUserLoggedIn);
}
```
2) Avatars initialization. After you successfully loged in, inside your callback method you can get a list of TotemAvatars by calling the GetUserAvatars method. You must define a TotemFilter and to pass it as a parameter with the previously obtained User object.
```C#
private List<TotemDNADefaultAvatar> _userAvatars;

private void OnUserLoggedIn(TotemUser user)
{
  //Using default filter with a default avatar model. You can implement your own filters and/or models
  totemCore.GetUserAvatars<TotemDNADefaultAvatar>(user, TotemDNAFilter.DefaultAvatarFilter, (avatars) =>
  {
    //Initialize additional user data
    profileNameText.SetText(user.Name);
    profilePublicKey.SetText(user.PublicKey);

    if (_userAvatars.Count > 0)
    {
      //Build your UI representation of avatars
      BuildAvatarList();
    }
  });
}
```
3) Items initialization. After you successfully loged in, inside your callback method you can get a list of TotemItems by calling the GetUserItems method. You must define a TotemFilter and to pass it as a parameter with the previously obtained User object.
```C#
private List<TotemDNADefaultItem> _userItems;

private void OnUserLoggedIn(TotemUser user)
{
  totemCore.GetUserItems<TotemDNADefaultItem>(user, TotemDNAFilter.DefaultItemFilter, (items) =>
  {
    _userItems = items;
    
    //Build your UI representation of avatars
    if (_userItems.Count > 0)
    {
      BuildItemList();
    }
  });
}
```
4) Add Legacy Record to a specific Asset. In order to add a legacy record you must call AddLegacyRecord method and pass the asset of your choise (avatar or item), assetType which matches to the asset object, and data which you wish to add to the legacy.
```C#
/// <summary>
/// Add a new Legacy Record to a specific Totem Asset.
/// </summary>
public void AddLegacyRecord(object asset, TotemAssetType assetType, int data)
{
  totemCore.AddLegacyRecord(asset, assetType, data.ToString(), (record) =>
  {
    Debug.Log($"Legacy record added: {record}");
  });
}
```
5) Get Legacy Records of a specific Asset. In order to get legacy records you must call GetLegacyRecords method with the folowing parameters: asset of your choise (avatar or item), assetType which matches to the asset object, yours game id and the onSuccess callback.
```C#
public void GetLegacyRecords(object asset, TotemAssetType assetType, UnityAction<List<TotemLegacyRecord>> onSuccess)
{
  totemCore.GetLegacyRecords(asset, assetType, onSuccess, _gameId);
}
```
