<h1 align="center">EmbedBot</h1>
<p align="center"><img src="icon.png" width="128"></p>
<p align="center"><i>A Discord bot for managing infractions against misbehaving users.</i></p>
<p align="center">
<a href="https://github.com/BrackeysBot/EmbedBot/releases"><img src="https://img.shields.io/github/v/release/BrackeysBot/EmbedBot?include_prereleases&style=flat-square"></a>
<a href="https://github.com/BrackeysBot/EmbedBot/actions/workflows/dotnet.yml"><img src="https://img.shields.io/github/actions/workflow/status/BrackeysBot/EmbedBot/dotnet.yml?branch=main&style=flat-square" alt="GitHub Workflow Status" title="GitHub Workflow Status"></a>
<a href="https://github.com/BrackeysBot/EmbedBot/issues"><img src="https://img.shields.io/github/issues/BrackeysBot/EmbedBot?style=flat-square" alt="GitHub Issues" title="GitHub Issues"></a>
<a href="https://github.com/BrackeysBot/EmbedBot/blob/main/LICENSE.md"><img src="https://img.shields.io/github/license/BrackeysBot/EmbedBot?style=flat-square" alt="MIT License" title="MIT License"></a>
</p>

## About
EmbedBot is a Discord bot which can create and send embeds in text channels.

## Installing and configuring EmbedBot 
EmbedBot runs in a Docker container, and there is a [docker-compose.yaml](docker-compose.yaml) file which simplifies this process.

### Clone the repository
To start off, clone the repository into your desired directory:
```bash
git clone https://github.com/BrackeysBot/EmbedBot.git
```
Step into the EmbedBot directory using `cd EmbedBot`, and continue with the steps below.

### Setting things up
The bot's token is passed to the container using the `DISCORD_TOKEN` environment variable. Create a file named `.env`, and add the following line:
```
DISCORD_TOKEN=your_token_here
```

Two directories are required to exist for Docker compose to mount as container volumes, `data` and `logs`:
```bash
mkdir data
mkdir logs
```
Copy the example `config.example.json` to `data/config.json`, and assign the necessary config keys. Below is breakdown of the config.json layout:
```json
{
  "GUILD_ID": {
    "logChannel": /* The ID of the log channel */,
    "primaryColor": /* The primary branding colour, as a 24-bit RGB integer. Defaults to #7837FF */,
    "secondaryColor": /* The secondary branding colour, as a 24-bit RGB integer. Defaults to #E33C6C */,
    "tertiaryColor": /* The tertiary branding colour, as a 24-bit RGB integer. Defaults to #FFE056 */,
    "urgentReportThreshold": /* How many message reports until the bot uses @ everyone instead of @ here. Defaults to 5  */,
    "mute": {
      "gagDuration": /* The duration of a gag, in milliseconds. Defaults to 5 minutes */,
      "maxModeratorMuteDuration": /* The maximum duration that a Moderator is allowed to mute, in milliseconds. Defaults to 14 days */
    },
    "reactions": {
      "deleteMessageReaction": /* The fallback reaction to delete messages, in Discord format. Defautls to 🗑️ (:wastebasket:) */,
      "gagReaction": /* The fallback reaction to gag users, in Discord format. Defautls to 🔇 (:mute:) */,
      "historyReaction": /* The fallback reaction to view infraction history, in Discord format. Defautls to 🕓 (:clock4:) */,
      "reportReaction": /* The fallback reaction to report messages, in Discord format. Defautls to 🚩 (:triangular_flag_on_post:) */
    },
    "roles": {
      "administratorRoleId": /* The ID of the Admin role */,
      "guruRoleId": /* The ID of the Guru role */,
      "moderatorRoleId": /* The ID of the Mod role */,
      "mutedRoleId": /* The ID of the muted role */
    }
  }
}
```
The `logs` directory is used to store logs in a format similar to that of a Minecraft server. `latest.log` will contain the log for the current day and current execution. All past logs are archived.

The `data` directory is used to store persistent state of the bot, such as config values and the infraction database.

### Launch EmbedBot
To launch EmbedBot, simply run the following commands:
```bash
sudo docker-compose build
sudo docker-compose up --detach
```

## Updating EmbedBot
To update EmbedBot, simply pull the latest changes from the repo and restart the container:
```bash
git pull
sudo docker-compose stop
sudo docker-compose build
sudo docker-compose up --detach
```

## Using EmbedBot
For further usage breakdown and explanation of commands, see [USAGE.md](USAGE.md).

## License
This bot is under the [MIT License](LICENSE.md).

## Disclaimer
This bot is tailored for use within the [Brackeys Discord server](https://discord.gg/brackeys). While this bot is open source and you are free to use it in your own servers, you accept responsibility for any mishaps which may arise from the use of this software. Use at your own risk.
