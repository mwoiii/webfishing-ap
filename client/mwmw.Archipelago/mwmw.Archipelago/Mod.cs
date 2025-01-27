using ArchipelagoTweaks;
using GDWeave;

namespace mwmw.Archipelago;

public class Mod : IMod {
    public Config Config;
    public static Serilog.ILogger Logger;

    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        Logger = modInterface.Logger;

        // both gamemodes
        modInterface.RegisterScriptMod(new QuestCheck());
        modInterface.RegisterScriptMod(new SpectralCheck());
        modInterface.RegisterScriptMod(new APMsg());
        modInterface.RegisterScriptMod(new InteractIntercept());
        modInterface.RegisterScriptMod(new HideMenu());
        modInterface.RegisterScriptMod(new LockButtons());
        modInterface.RegisterScriptMod(new GoalRank());
        modInterface.RegisterScriptMod(new GoalCompletion());
        modInterface.RegisterScriptMod(new HandleCamp());

        // needed for alt
        modInterface.RegisterScriptMod(new BaitQualityPatch());
        modInterface.RegisterScriptMod(new BuddyLootTablePatch());
        modInterface.RegisterScriptMod(new EquippedRodPatch());
        modInterface.RegisterScriptMod(new ItemPricePatch());
        modInterface.RegisterScriptMod(new RodLootTablePatch());
        modInterface.RegisterScriptMod(new JournalRequirementPatch());
        modInterface.RegisterScriptMod(new SpectralTagRequire());
    }

    public void Dispose() {
    }
}
