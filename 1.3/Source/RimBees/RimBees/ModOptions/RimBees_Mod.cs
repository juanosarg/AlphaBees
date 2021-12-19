using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class RimBees_Mod : Mod
    {
        static RimBees_Mod()
        {
            var harmony = new Harmony("com.rimbees");
            harmony.PatchAll();
        }

        public RimBees_Mod(ModContentPack content) : base(content)
        {
            GetSettings<RimBees_Settings>();
        }

        public override string SettingsCategory() => "RimBees";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            RimBees_Settings.DoWindowContents(inRect);
        }
    }
}
