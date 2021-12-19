using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class BeeSpeciesDef : Def
    {
        public ThingDef drone;
        public ThingDef queen;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (var error in base.ConfigErrors())
            {
                yield return error;
            }

            var droneComp = drone?.GetCompProperties<CompProperties_Bees>();
            if (drone == null)
            {
                yield return "drone is missing";
            }
            else if (droneComp == null)
            {
                yield return "drone is missing CompProperties_Bees";
            }
            else if (droneComp.species != this)
            {
                yield return "drone CompProperties_Bees is not this species";
            }

            var queenComp = queen?.GetCompProperties<CompProperties_Bees>();
            if (queen == null)
            {
                yield return "queen is missing";
            }
            else if (queenComp == null)
            {
                yield return "queen is missing CompProperties_Bees";
            }
            else if (queenComp.species != this)
            {
                yield return "queen CompProperties_Bees is not this species";
            }
        }
    }
}

