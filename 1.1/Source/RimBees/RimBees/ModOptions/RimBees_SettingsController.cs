using RimWorld;
using UnityEngine;
using Verse;

namespace RimBees
{
    public class RimBees_Mod : Mod
    {


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
