using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;


namespace Verse
{
    [StaticConstructorOnStartup]
    public class Command_SetQueenList : Command
    {

        public Map map;
        public Building beehouse;
        public Thing queen;



        public Command_SetQueenList()
        {

        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Temperate_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Mild_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Mild_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Mild_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hybrid_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Hybrid_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hybrid_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Amalgam_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Amalgam_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Amalgam_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (list.Count > 0) {

            } else {
                list.Add(new FloatMenuOption("RB_NoQueens".Translate(), delegate
                {
                   
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertQueen()
        {

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
            //Log.Message(pawn.ToString(), false);

            bool flag4 = (pawn != null);

            if (flag4)
            {
                if (pawn.CanReach(beehouse, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
                {

                    // pawn.jobs.StopAll(false);

                    Job job = new Job(DefDatabase<JobDef>.GetNamed("RB_InsertingQueenBees", true), beehouse, queen);
                    job.count = 1;
                    if (TryToReserveThings(pawn, job))
                    {
                        pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);

                    }
                    else
                    {
                        pawn.ClearAllReservations();
                        //Messages.Message("Can't reserve things", MessageTypeDefOf.RejectInput);
                    }


                }
            }
            else
            {
                Messages.Message("No colonists available to take care of the bees", MessageTypeDefOf.RejectInput);
            }
        }

        public bool TryToReserveThings(Pawn pawn, Job job)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null) && pawn.Reserve(job.targetB, job, 1, -1, null);
        }


    }


}

