# KittsGlowSystem
*LabAPI Glowing Tool*

[![License](https://img.shields.io/badge/License-AGPL%20v3.0-blue?style=for-the-badge)](https://github.com/Kittscloud/KittsGlowSystem/blob/main/LICENSE) [![Downloads](https://img.shields.io/github/downloads/Kittscloud/KittsGlowSystem/total?style=for-the-badge)](https://github.com/Kittscloud/ServerSpecificsSyncer/releases/latest) [![GitHub release](https://img.shields.io/github/v/release/Kittscloud/KittsGlowSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsGlowSystem/releases/latest) [![](https://img.shields.io/badge/.NET-4.8.1-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481) [![GitHub stars](https://img.shields.io/github/stars/Kittscloud/KittsGlowSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsGlowSystem/stargazers) [![GitHub issues](https://img.shields.io/github/issues/Kittscloud/KittsGlowSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsGlowSystem/issues)

`KittsGlowSystem` is a tool that adds player glowing for `SCP Secret Laboratory` using `LabAPI`.

## Consider Supporting?
If you enjoy this project and would like to support future development, I would greatly appreciate it if you considered donating via my [`Ko-Fi`](https://ko-fi.com/kittscloud).

## Installation
`KittsGlowSystem` is available in two editions:

- **Standard**: Stores all infractions in a local JSON file.
- **MongoDB**: Stores all infractions in a MongoDB database.

All required files can be found in the [`latest release`](https://github.com/Kittscloud/KittsGlowSystem/releases/latest).

### Standard
**Required files**
- `KittsGlowSystem.dll` (latest version)
- `Newtonsoft.Json.dll` (`v13.0.4` or later)

**Installation**
- Place `KittsGlowSystem.dll` in your `plugins` folder.
- Place `Newtonsoft.Json.dll` in your `dependencies` folder.

### MongoDB
**Required files**
- `KittsGlowSystem-MongoDB.dll` (latest version)
- `DnsClient.dll` (`v1.6.1` or later)
- `Microsoft.Extensions.Logging.Abstractions.dll` (`v2.0.0` or later)
- `MongoDB.Bson.dll` (`v3.10.0` or later)
- `MongoDB.Driver.dll` (`v3.10.0` or later)

**Installation**
- Place `KittsGlowSystem-MongoDB.dll` in your `plugins` folder.
- Place all other `.dll` files in your `dependencies` folder.

Run the server and you're set!

### Configurations:
| Parameter                   | Type     | Description                                                            | Default Value                            |
|-----------------------------|----------|------------------------------------------------------------------------|------------------------------------------|
| `Debug`                     | `bool`   | Sends debug logs to console.                                           | `false`                                  |
| `MongoDBURI`                | `string` | MongoDB URI.                                                           | `"mongodb://username:password@ip:port/"` |
| `MongoDBName`               | `string` | MongoDB name.                                                          | `KittsGlowSystem`                        |
| `MongoDBCollectionName`     | `string` | MongoDB collection name if using database.                             | `PlayerGlowData`                         |
| `PlayerGlowPermission`      | `string` | Permission for player badge command.                                   | `kts.glow`                               |
| `AdminGlowPermission`       | `string` | Permission for admin badge command.                                    | `kts.glowadmin`                          |
| `DefaultBadgeFormat`        | `string` | Formatting for badges with no text, {group} = groupName.               | `'{group}'`                              |
| `BadgeFormat`               | `string` | Permission for admin badge command.                                    | `'{group} | {text}'`                     |

### Default YML Config File
```yml
# Sends debug logs to console
debug: false
# Permission for player glow command
player_glow_permission: kts.glow
# Permission for admin glow command
admin_glow_permission: kts.glowadmin
```

### MongoDB YML Config File
```yml
# Sends debug logs to console
debug: false
# MongoDB URI
mongo_d_b_u_r_i: mongodb://username:password@ip:port/
# MongoDB name
mongo_d_b_name: KittsGlowSystem
# MongoDB collection name
mongo_d_b_collection_name: PlayerGlowData
# Permission for player glow command
player_glow_permission: kts.glow
# Permission for admin glow command
admin_glow_permission: kts.glowadmin
```

### Want to use in your own project?
To install in your project, simply reference the `KittsGlowSystem.dll` file, found in the [`latest release`](https://github.com/Kittscloud/KittsGlowSystem/releases/latest).

Alternatively, you can use the `NuGet Package` which can be found here:
- [`KittsGlowSystem`](https://www.nuget.org/packages/KittsGlowSystem)
- [`KittsGlowSystem-MongoDB`](https://www.nuget.org/packages/KittsGlowSystem-MongoDB)

`KittsGlowSystem.dll` is really easy to use, if you would like to change someone's glow data you can do `Player.GetGlowData()` and change what you want.
You can also use the command if you're only using this as a standalone plugin.

A really simple example of using this plugin could be the colour being changed to a random colour when jumping as shown below.

```cssharp
public override void OnPlayerJumped(PlayerJumpedEventArgs ev)
{
    ev.Player.GetGlowData().GlowColour = ColourTypes.Random().EnumColour;

    // If you're not using MonogoDB you will need to update the JsonCache as shown below
    DatabaseJson.SaveJsonCache();
    // If you are using MongoDB it will automatically update to the database
}
```

## Found a bug or have feedback?
If you have found a bug please make an issue on GitHub or the quickest way is to message me on discord at `kittscloud`.

Also message me on discord if you have feedback for me, I'd appreciate it very much. Thank you!
