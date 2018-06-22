
using RimWorld;
using Verse;
using Verse.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using RimWorld.Planet;

namespace RimBees
{
    class Building_TreeOakHive : Plant
    {


       

     

        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (myPawn.RaceProps.Humanlike && base.Faction == Faction.OfPlayer)
            {
                if (myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
                {
                    Action command_Action = delegate
                    {
                        Job job = new Job(DefDatabase<JobDef>.GetNamed("GU_ExtractBeesJob", true), this);
                        myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);

                    };
                    yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RB_ExtractBees".Translate(), command_Action, MenuOptionPriority.Default, null, null, 0f, null, null), myPawn, this, "ReservedBy");

                }



            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            Map map = base.Map;
            base.Destroy(mode);
            IntVec3 thisPosition = this.Position;
            Plant regularOak = (Plant)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Plant_TreeOak", true));
            GenSpawn.Spawn(regularOak, thisPosition, map);
            regularOak.Growth = 0.9f;

        }


    }
}
