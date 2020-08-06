using Exiled.API.Features;

namespace ToggleTag
{
    public class ToggleTag : Plugin<Config>
    {
        public UserManager TagManager;
        public ToggleTagEventHandler Handler;
        public override string Name => "ToggleTag";
        public override string Author => "KoukoCocoa";

        public override void OnEnabled()
        {
            TagManager = new UserManager();
            Handler = new ToggleTagEventHandler(this);
            Exiled.Events.Handlers.Player.Joined += Handler.RunWhenPlayerJoins;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += Handler.RunWhenConsoleCommandEndered;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= Handler.RunWhenConsoleCommandEndered;
            Exiled.Events.Handlers.Player.Joined -= Handler.RunWhenPlayerJoins;
            Handler = null;
            TagManager = null;
        }

        public override void OnReloaded() { }
    }
}
