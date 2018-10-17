using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;


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

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Temperate_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Temperate_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Mild_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Mild_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Mild_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hybrid_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Hybrid_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hybrid_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Amalgam_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Amalgam_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Amalgam_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Alloy_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Alloy_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Alloy_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Adaptive_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Adaptive_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Adaptive_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Neutro_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Neutro_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Neutro_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nutritious_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Nutritious_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nutritious_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Technological_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Technological_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Technological_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Arctic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Arctic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Arctic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Desert_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Desert_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Desert_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Swamp_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Swamp_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Swamp_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nocturnal_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Nocturnal_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nocturnal_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Argent_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Argent_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Argent_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Aurum_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Aurum_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Aurum_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Precious_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Precious_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Precious_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Radioactive_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Radioactive_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Radioactive_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Tipsy_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Tipsy_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Tipsy_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Luxurious_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Luxurious_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Luxurious_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Psychic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Psychic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Psychic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Stoner_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Stoner_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Stoner_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (list.Count > 0) {

            } else {
                list.Add(new FloatMenuOption("RB_NoBees".Translate(), delegate
                {
                   
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertDrone()
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

                    Job job = new Job(DefDatabase<JobDef>.GetNamed("RB_InsertingBees", true), beehouse, drone);
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

