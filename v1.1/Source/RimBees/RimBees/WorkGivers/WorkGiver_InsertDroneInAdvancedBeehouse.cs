using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_InsertDroneInAdvancedBeehouse : WorkGiver_InsertDroneInBeehouse
    {
      

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(ThingDef.Named("RB_AdvancedBeehouse"));
            }
        }

     
    }
}

