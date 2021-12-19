using UnityEngine;
using Verse;

namespace RimBees
{
    public class RimBees_Settings : ModSettings
    {
        public static bool RB_IgnoreNight = false;
        public static bool RB_IgnoreRain = false;
        public static bool RB_IgnoreTemperature = false;
        public static bool RB_IgnorePlants = false;
        public static bool RB_GreenhouseBees = false;
        public static bool RB_DisableMessages = false;
        public static bool RB_Ben_ShowMissingBees = true; 
        public static bool RB_Ben_ShowProgress = true;
        public static bool RB_Ben_KnownBees = true;
        public static bool RB_Ben_BeeDangerAlert = true;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref RB_IgnoreNight, "RB_IgnoreNight", false, true);
            Scribe_Values.Look(ref RB_IgnoreRain, "RB_IgnoreRain", false, true);
            Scribe_Values.Look(ref RB_IgnoreTemperature, "RB_IgnoreTemperature", false, true);
            Scribe_Values.Look(ref RB_IgnorePlants, "RB_IgnorePlants", false, true);
            Scribe_Values.Look(ref RB_GreenhouseBees, "RB_GreenhouseBees", false, true);
            Scribe_Values.Look(ref RB_DisableMessages, "RB_DisableMessages", false, true);
            Scribe_Values.Look(ref RB_Ben_ShowMissingBees, "RB_Ben_ShowMissingBees", true, true);
            Scribe_Values.Look(ref RB_Ben_ShowProgress, "RB_Ben_ShowProgress", true, true);
            Scribe_Values.Look(ref RB_Ben_KnownBees, "RB_Ben_KnownBees", true, true);
            Scribe_Values.Look(ref RB_Ben_BeeDangerAlert, "RB_Ben_BeeDangerAlert", true, true);
        }

        public static void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();

            ls.Begin(inRect);
            ls.ColumnWidth = inRect.width / 1f;

            ls.CheckboxLabeled("RB_IgnoreNight".Translate(), ref RB_IgnoreNight, null);
            ls.CheckboxLabeled("RB_IgnoreRain".Translate(), ref RB_IgnoreRain, null);
            ls.CheckboxLabeled("RB_IgnoreTemperature".Translate(), ref RB_IgnoreTemperature, null);
            ls.CheckboxLabeled("RB_IgnorePlants".Translate(), ref RB_IgnorePlants, null);
            ls.CheckboxLabeled("RB_GreenhouseBees".Translate(), ref RB_GreenhouseBees, null);
            ls.CheckboxLabeled("RB_DisableMessages".Translate(), ref RB_DisableMessages, null);
            ls.CheckboxLabeled("BenLubarsRimBeesPatches_showMissingBees_title".Translate(), ref RB_Ben_ShowMissingBees, "BenLubarsRimBeesPatches_showMissingBees_desc".Translate());
            ls.CheckboxLabeled("BenLubarsRimBeesPatches_showProgress_title".Translate(), ref RB_Ben_ShowProgress, "BenLubarsRimBeesPatches_showProgress_desc".Translate());
            ls.CheckboxLabeled("BenLubarsRimBeesPatches_knownBees_title".Translate(), ref RB_Ben_KnownBees, "BenLubarsRimBeesPatches_knownBees_desc".Translate());
            ls.CheckboxLabeled("BenLubarsRimBeesPatches_beeDangerAlert_title".Translate(), ref RB_Ben_BeeDangerAlert, "BenLubarsRimBeesPatches_beeDangerAlert_desc".Translate());
            ls.End();
        }
    }
}
