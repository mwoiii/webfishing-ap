using GDWeave;

namespace mwmw.Archipelago;

public class Mod : IMod {
    public Config Config;
    public static Serilog.ILogger Logger;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        Logger = modInterface.Logger;
        modInterface.RegisterScriptMod(new APCheck());
        modInterface.RegisterScriptMod(new APMsg());
        modInterface.RegisterScriptMod(new InteractIntercept());
        modInterface.RegisterScriptMod(new HideMenu());
        modInterface.RegisterScriptMod(new LockButtons());
        modInterface.RegisterScriptMod(new GoalRank());
        modInterface.RegisterScriptMod(new GoalCompletion());
    }

    public void Dispose() {
    }
}
