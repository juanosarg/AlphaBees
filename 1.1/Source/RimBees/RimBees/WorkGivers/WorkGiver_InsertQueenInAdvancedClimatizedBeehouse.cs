using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_InsertQueenInAdvancedClimatizedBeehouse : WorkGiver_InsertQueenInBeehouse
    {
       

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(ThingDef.Named("RB_AdvancedClimatizedBeehouse"));
            }
        }

       
    }
}

