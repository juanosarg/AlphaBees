using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;


namespace RimBees
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
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Agricultural_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Agricultural_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Agricultural_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Bittersweet_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Bittersweet_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Bittersweet_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lactic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Lactic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lactic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Egglaying_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Egglaying_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Egglaying_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Electronic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Electronic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Electronic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Oily_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Oily_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Oily_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Plastic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Plastic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Plastic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_High_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_High_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_High_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lucifer_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Lucifer_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lucifer_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Culinary_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Culinary_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Culinary_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Chef_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Chef_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Chef_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Medicinal_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Medicinal_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Medicinal_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Surgeon_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Surgeon_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Surgeon_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Synthetic_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Synthetic_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Synthetic_Drone", true)).RandomElement();
                    this.TryInsertDrone();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hyper_Drone", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Hyper_Drone_Tag".Translate(), delegate
                {
                    drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hyper_Drone", true)).RandomElement();
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
            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.beehouse;
            buildingbeehouse.BeehouseIsExpectingBees = true;
            buildingbeehouse.theDroneIAmGoingToInsert = drone.def.defName;

            /* Pawn pawn = null;
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
                         Building_Beehouse buildingbeehouse = (Building_Beehouse)this.beehouse;
                         buildingbeehouse.BeehouseIsExpectingBees = true;
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
             }*/
        }

       /* public bool TryToReserveThings(Pawn pawn, Job job)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null) && pawn.Reserve(job.targetB, job, 1, -1, null);
        }*/


    }


}

