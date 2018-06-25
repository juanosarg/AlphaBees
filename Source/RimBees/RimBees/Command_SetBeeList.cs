using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld.Planet;

namespace Verse
{
    [StaticConstructorOnStartup]
    public class Command_SetBeeList : Command
    {

        public Map map;
        public Building beehouse;
        public Thing drone;



        public Command_SetBeeList()
        {
        
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            list.Add(new FloatMenuOption("Caraculo", delegate
            {
                drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Drone", true)).RandomElement();
                //drone.def = DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Drone", true);
                this.TryInsertDrone();
            }, MenuOptionPriority.Default, null, null, 29f, null, null));

            list.Add(new FloatMenuOption("Caraculo2", delegate
            {

            }, MenuOptionPriority.Default, null, null, 29f, null, null));

         
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertDrone(){

            Pawn pawn = null;
            foreach (Pawn current in map.mapPawns.FreeColonistsSpawned)
            {

                bool flag = !current.Dead;

                if (flag)
                {
                    bool flag2 = current.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && current.health.capacities.CapableOf(PawnCapacityDefOf.Moving);

                    if (flag2)
                    {
                        pawn = current;
                            break;
                        
                    }
                }
            }
            Log.Message(pawn.ToString(), false);

            bool flag4 = (pawn != null);

            if (flag4)
            {
                if (pawn.CanReach(beehouse, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
                {

                   // pawn.jobs.StopAll(false);

                    Job job = new Job(DefDatabase<JobDef>.GetNamed("RB_InsertingBees", true), beehouse,drone);
                    pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);


                }






            }
            else
            {
                Messages.Message("No colonists available to take care of the bees", MessageTypeDefOf.RejectInput);
            }
        }

     




    }
}

