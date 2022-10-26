# About ShiverBot
## **There was a recent banwave around the new update Ver1.2.0, so please refrain from using the tool for now.**
This is a Splatoon 3 memory modifier that connects to a Nintendo Switch console that has sys-botbase running.
Because of the recent banwave on Ver1.2.0 of the game, any cheating options will be removed in a future update.

Features returning:
- Search for your gear's RNG seed (only view, NOT change)
- Post printer

Features that will be removed:
Edit money
- Edit ability chunks
- Edit food and drink tickets.
- Cheat in tableturf against AI.

<details>
  <summary>Screenshots</summary>

![Screenshot showing the gear seed finder](https://i.imgur.com/8ItDfs1.png)
</details>


# Read this
There is a ban risk associated to anything you modify in this game because every action you do sends telemetry.

We [thought] ability chunks and money were safe. The recent banwave proved us wrong.

# Preparations
- You need to have a Nintendo Switch console running Atmosphere CFW.
- Get sys-botbase [from here](https://github.com/olliz0r/sys-botbase/releases/latest). You just need the .zip. Extract the contents in the root of your SD card.
- Delete any active Splatoon 3 cheats files you may have in atmosphere/contents/... in your SD card. This is a very important step or sys-botbase won't be able to read memory.
- Do not run Edizon homebrew or anything else that makes modifications to the RAM (having the overlay is fine, just don't have any cheat files for the game).

# Tutorial
1. Enter your Nintendo Switch IP and click connect.
2. You must be in the game's lobby, as making modifications elsewhere may cause an error in the game.
3. Click "save" to inject the amount of money you inputted. Ability chunks and tickets get injected automatically when you change the values.
4. To store your changes in-game, make a transaction in a place **that is not a shop** (buy a gacha capsule, get a food/drink, use your chunks via Murch etc.)

# Troubleshooting
- If you can't open the program because you're missing frameworks for .NET, [download and install this](https://dotnet.microsoft.com/en-us/...u/runtime-desktop-6.0.9-windows-x64-installer).
- Make sure you're running the game in English (US or EU). Other languages are known to have differences in memory addresses.
- If you can't connect to sys-botbase, follow this [external guide](https://github.com/kwsch/SysBot.NET/wiki/Troubleshooting-Connection-Errors) for advice.
