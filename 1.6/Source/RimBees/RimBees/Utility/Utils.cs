
using UnityEngine;
using Verse;

namespace RimBees
{
    public static class Utils
    {

        public static string getDroneFromQueen(Thing beeQueen)
        {
            string beeSpecies = beeQueen.TryGetComp<CompBees>()?.GetSpecies;
            return "RB_Bee_" + beeSpecies + "_Drone";
        }

        public static string getQueenFromDrone(Thing beeDrone)
        {
            string beeSpecies = beeDrone.TryGetComp<CompBees>()?.GetSpecies;
            return "RB_Bee_" + beeSpecies + "_Queen";

        }

        public static string getDroneFromQueen(ThingDef beeQueen)
        {
            string beeSpecies = beeQueen.GetCompProperties<CompProperties_Bees>()?.species;
            return "RB_Bee_" + beeSpecies + "_Drone";
        }

        public static string getQueenFromDrone(ThingDef beeDrone)
        {
            string beeSpecies = beeDrone.GetCompProperties<CompProperties_Bees>()?.species;
            return "RB_Bee_" + beeSpecies + "_Queen";

        }

        



    }
}
