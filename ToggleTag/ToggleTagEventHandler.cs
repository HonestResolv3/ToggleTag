
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using RemoteAdmin;

namespace ToggleTag
{
    public class ToggleTagEventHandler
    {
        public enum PropertyType
        {
            Tag,
            Overwatch
        }

        public ToggleTag Plugin;

        public ToggleTagEventHandler(ToggleTag Plugin) => this.Plugin = Plugin;

        public void RunWhenPlayerJoins(JoinedEventArgs JoinEv)
        {
            Timing.CallDelayed(0.1f, () => ShowOrHideFeatures(JoinEv.Player));
        }

        public void RunWhenConsoleCommandEndered(SendingConsoleCommandEventArgs SendComEv)
        {
            if (SendComEv.Name.Contains("showtag"))
            {
                SendComEv.Allow = false;
                SendComEv.Color = "green";
                SendComEv.ReturnMessage = "Your tag (if you have one) will permanently be shown on this server now";
                SendComEv.Player.BadgeHidden = false;
                Plugin.TagManager.AddPlayer(SendComEv.Player.UserId, 0, PropertyType.Tag);
            }
            else if (SendComEv.Name.Contains("hidetag"))
            {
                SendComEv.Allow = false;
                SendComEv.Color = "red";
                SendComEv.ReturnMessage = "Your tag (if you have one) will permanently be hidden on this server now";
                SendComEv.Player.BadgeHidden = true;
                Plugin.TagManager.AddPlayer(SendComEv.Player.UserId, 1, PropertyType.Tag);
            }
            else if (SendComEv.Name.Contains("overwatch"))
            {
                SendComEv.Allow = false;
                if (!(CommandProcessor.CheckPermissions(SendComEv.Player.CommandSender, "overwatch", PlayerPermissions.Overwatch, "ToggleTag", false)))
                {
                    SendComEv.Color = "yellow";
                    SendComEv.ReturnMessage = "You do not have permission to use this command!";
                    return;
                }

                if (!SendComEv.Player.IsOverwatchEnabled)
                {
                    SendComEv.Player.IsOverwatchEnabled = true;
                    Plugin.TagManager.AddPlayer(SendComEv.Player.UserId, 1, ToggleTagEventHandler.PropertyType.Overwatch);
                    SendComEv.Color = "red";
                    SendComEv.ReturnMessage = "You will stay in overwatch by default when you join now";
                }
                else
                {
                    SendComEv.Player.IsOverwatchEnabled = false;
                    Plugin.TagManager.AddPlayer(SendComEv.Player.UserId, 0, ToggleTagEventHandler.PropertyType.Overwatch);
                    SendComEv.Color = "green";
                    SendComEv.ReturnMessage = "You will no longer stay in overwatch by default when you join now";
                }
            }
        }

        public void ShowOrHideFeatures(Player Ply)
        {
            if (Plugin.TagManager.TagPlayers.ContainsKey(Ply.UserId))
            {
                switch (Plugin.TagManager.TagPlayers[Ply.UserId])
                {
                    case 0:
                        Ply.BadgeHidden = false;
                        break;
                    case 1:
                        Ply.BadgeHidden = true;
                        break;
                    default:
                        Ply.BadgeHidden = false;
                        break;
                }
            }
            if (Plugin.TagManager.OwPlayers.ContainsKey(Ply.UserId))
            {
                switch (Plugin.TagManager.OwPlayers[Ply.UserId])
                {
                    case 0:
                        Ply.IsOverwatchEnabled = false;
                        break;
                    case 1:
                        Ply.IsOverwatchEnabled = true;
                        break;
                    default:
                        Ply.IsOverwatchEnabled = false;
                        break;
                }
            }
        }
    }
}
