# GTA-Session-Bot

## Overview
This bot is designed to help manage a machine that is used to host a Grand Theft Auto V Online Session.

## Usage
### Installation
Grab a copy of the installer from the releases page and install the bot to a location of your choosing.
Create a new .json file named `_configurations.json`.
Fill out the `_configurations.json` file with the required settings that are detailed below.


### Settings
The bot can be configured to help with running on different machines and under different Discord accounts. The settings are as follows:
* `Prefix` - The command prefix that the bot will listen to (`string`).
* `DiscordToken` - The token used to log in with the Discord bot account (`string`).
* `IsSteamGame` - Boolean flag stating if the game is a Steam game or not (`boolean`).
* `DebugLogging` - Boolean flag to turn on or off debug logging (`boolean`).
* `ListWidth` - The width of the player list in pixels (`integer`).
* `ListHeight` - The height of the player list in pixels (`integer`).
* `ListYOffset` - The Y axis offset of the player list in pixels (`integer`).
* `ListXOffset` - The X axis offset of the player list in pixels (`integer`).
* `AdminRoleList` - The list of admin role IDs which controls who can use commands that require admin rights (`integer array`).

An example of a complete settings file:
```json
{
  "Prefix": ".",  
  "DiscordToken": "abcdefghijkmnopqrstuvwxyz",
  "IsSteamGame": true,
  "DebugLogging": true,
  "ListWidth": 280,
  "ListHeight": 601,
  "ListXOffset": 50,
  "ListYOffset": 40,
  "AdminRoleList": [123456789, 897654321]
}

```

### Commands
There are a relatively large number of commands that this bot supports, and as such they are broken down into the following modules. Each command in a module _must_ include the module name as part of the command (e.g. the `exit` command is in the `game` module, so if the prefix is `.`, the command would be `.game exit`). Some commands also have aliases to make them easier to type and remember, these will be listed in brackets next to each command if there are any. 

The commands for managing the machine that the bot is running on are not under any module. As these are the most commonly used (and more verbose commands), they live at the root to try and cut down on the time needed to enter these commands.

#### Management Commannds
* `newsession (split, solo)` - Splits the GTA Online session and places the character into a new session.
* `foreground (front)` - Attempts to bring the GTA process to the foreground.
* `start idle` - Starts the process that allows the online character to idle in game and record screenshots of the player list.
* `stop idle` - Stops the process that allows the online character to idle in game and record screenshots of the player list.
* `status` - Uploads a screenshot of the current desktop window of the machine.
* `version` - Gets the version of the bot.
* `desktop` - Attempts to set the desktop to be the active window.
* `cookie` - Gives a cookie to the user who invoked the command. Useful for testing if the bot is online.
* `cookie <<@USERNAME>>` - Gives a cookie to the tagged user. Who doesn't love cookies?

#### Game Module
* `exit` - Exits the GTA game.
* `start` - Starts the GTA game.
* `restart` - Restarts the GTA game.
* `kill launcher` - Kills the GTA launcher process if it exists.
* `kill error` - Kills the Windows error dialog process if it exists.
* `list` - Grabs the player list from the game and embeds in into a Discord message.

#### Help Module
* `help` - Provides basic help.
* `help <<MODULE_NAME>>` - Provides help about the given module name.

#### Keyboard Module (experimental use only)
* `enter` - Presses the enter key using an AutoHotKey script.
* `online` - Presses the required keys to enter GTA Online from story mode.

#### System Module
* `cpu` - Gets some basic CPU information.
* `gpu` - Gets some basic GPU information.
* `ram` - Gets some basic RAM information.
* `shutdown` - Shuts down the machine.
* `restart` - Restarts the machine.
* `latency (ping)` - Returns the latency from the machine to the Discord gateway server.
* `network (net)` - Returns some basic UDP throughput information.

#### Splitting to new session
1. Use the `newsession` command to split the host into a new session. This command will automatically do the following:
	* Stop the bot script if it is currently running.
	* Suspend the GTA process for 8 seconds, causing the game to force the host into a solo session.
	* Start the bot script again.

#### Restarting the game
1. Use the `game restart` command to restart the game. This command will automatically do the following:
	* Stop the bot script if it is currently running.
	* Exit GTA, and kill the associated launcher process.
	* Start GTA again after a small delay.
2. From this point on it is basically a waiting game, the game is loading into online mode and can take anywhere between 1 minute and 10 minutes depending on the internet connection. If you want to know if the game has loaded yet, you can use the `status` command to get a screenshot of the game to see if it has loaded in yet.
3. Once the game has loaded into online, use the `newsession` command to split the host into a new session. Using this command will automatically start the bot script.


## Known Issues
#### GTA Launcher Failed To Exit
I have only seen this happen once, so it might not occur often but it is possible for the GTA Launcher process to hang around and prevent the game from starting up properly again.
The launcher process is the pesky thing that prompts you asking if you wish to start the game in safe mode because it didn't shut down properly last time - in short, it's a pain in the ass.
To handle this I have added a separate `game kill launcher` command to kill specifically the game launcher.
