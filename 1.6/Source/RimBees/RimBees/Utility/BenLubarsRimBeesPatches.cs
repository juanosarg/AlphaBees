using System;
using System.Collections.Generic;
using HarmonyLib;

using RimBees;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class BenLubarsRimBeesPatches
    {
        internal static readonly Type CompBees = Type.GetType("RimBees.CompBees,RimBees");
        internal static readonly Type CompProperties_Bees = Type.GetType("RimBees.CompProperties_Bees,RimBees");
        internal static readonly AccessTools.FieldRef<object, string> species = AccessTools.FieldRefAccess<string>(CompProperties_Bees, "species");
        internal static readonly AccessTools.FieldRef<object, string> comb = AccessTools.FieldRefAccess<string>(CompProperties_Bees, "comb");
        internal static readonly AccessTools.FieldRef<object, string> weirdplantneeded = AccessTools.FieldRefAccess<string>(CompProperties_Bees, "weirdplantneeded");

        private static ThingComp GetCompBees(Thing bee)
        {
            var withComps = (ThingWithComps)bee;
            var comps = withComps?.AllComps;
            if (comps != null) {
                for (var i = 0; i < comps.Count; i++)
                {
                    var c = comps[i];
                    if (CompBees.IsInstanceOfType(c))
                    {
                        return c;
                    }
                }
            }
            

            return null;
        }

        public static string GetBeeSpecies(Thing bee)
        {
            return species(GetCompBees(bee).props);
        }


        public static ThingDef GetWeirdPlantDef(Thing bee)
        {
            CompBees comp = bee?.TryGetComp<CompBees>();
            var weirdPlantName = comp?.GetWeirdPlant;
            if (weirdPlantName == "no")
            {
                return null;
            }

            if (weirdPlantName != null)
            {
                return DefDatabase<ThingDef>.GetNamedSilentFail(weirdPlantName);
            }else return null;
        }

    }

}
