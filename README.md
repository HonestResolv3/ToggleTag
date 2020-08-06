# ToggleTag
A remake of a ServerMod plugin that automatically shows/hides your tag in the server and sets/removes overwatch when you join

### Commands
```
.showtag - Shows the users tag (if they have one) permanently on your server
.hidetag - Hides the users tag (if they have one) permanently on your server
.overwatch - Sets/Removes someone to/from overwatch when they join by default permanently (Requires Overwatch permission in config_remoteadmin.txt)
```

### Notes
```
- One folder and two text files will be generated if they do not exist in %appdata% (or .config) / EXILED / Configs
- You can manually add in your own UserID's and values (Just remember to only do this while the server is not online or else you could encounter errors)
- (0) Enables Tags and Overwatch in their respective text files, (1) Disables Tags and Overwatch in their respective text files
```

### Example In Text File
```
NorthwoodUserHere@northwood (1)
Steam64IDHere@steam (1)
DiscordIDHere@discord (0)
```

### Config
```yaml
toggle_tag:
  is_enabled: true
```
