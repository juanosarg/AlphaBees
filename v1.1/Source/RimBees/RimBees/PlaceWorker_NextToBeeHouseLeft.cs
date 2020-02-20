using System;
using System.Collections.Generic;
using Verse;
using RimWorld;

namespace RimBees
{
    public class PlaceWorker_NextToBeeHouseLeft : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            for (int i = 0; i < 4; i++)
            {
                //IntVec3 c = loc;
                IntVec3 c = loc + GenAdj.CardinalDirections[i];
                 if (i == 0)
                 {
                     c = loc;
                 }
                 else if (i == 1)
                 {
                    c = loc + GenAdj.CardinalDirections[1];

                }
                 else if (i == 2)
                 {
                     c = loc;
                 }
                 else if (i == 3)
                 {
                    c = loc;
                }
                if (c.InBounds(map))
                {
                    List<Thing> thingList = c.GetThingList(map);
                    for (int j = 0; j < thingList.Count; j++)
                    {
                        Thing ParticularThing = thingList[j];
                        ThingDef thingDef = GenConstruct.BuiltDefOf(ParticularThing.def) as ThingDef;

                        if (thingDef != null && thingDef.building != null)
                        {

                            if (thingDef.building.wantsHopperAdjacent)
                            {
                                CompBeeHouse comp = ParticularThing.TryGetComp<CompBeeHouse>();
                                if (comp != null)
                                {
                                    if (comp.GetIsBeehouse)
                                    {
                                        return true;
                                    }

                                }
                                else return "RB_BeehouseNotYetBuilt".Translate();


                            }
                        }
                    }
                }
            }
            return "GU_PlaceNextToBeeHouseLeft".Translate();
        }
    }
}
