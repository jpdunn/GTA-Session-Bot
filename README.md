# GTA-Session-Bot

## Overview
This bot is designed to help manage a machine that is used to host a Grand Theft Auto V Online Session.

## Usage
TODO

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
