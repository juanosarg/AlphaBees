using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;


namespace RimBees
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

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Alloy_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Alloy_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Alloy_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Adaptive_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Adaptive_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Adaptive_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Neutro_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Neutro_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Neutro_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nutritious_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Nutritious_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nutritious_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Technological_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Technological_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Technological_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Arctic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Arctic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Arctic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Desert_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Desert_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Desert_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Swamp_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Swamp_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Swamp_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nocturnal_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Nocturnal_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Nocturnal_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Argent_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Argent_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Argent_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Aurum_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Aurum_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Aurum_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Precious_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Precious_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Precious_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Radioactive_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Radioactive_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Radioactive_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Tipsy_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Tipsy_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Tipsy_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Luxurious_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Luxurious_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Luxurious_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Psychic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Psychic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Psychic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Stoner_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Stoner_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Stoner_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Agricultural_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Agricultural_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Agricultural_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Bittersweet_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Bittersweet_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Bittersweet_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lactic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Lactic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lactic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Egglaying_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Egglaying_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Egglaying_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Electronic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Electronic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Electronic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Oily_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Oily_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Oily_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Plastic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Plastic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Plastic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_High_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_High_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_High_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lucifer_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Lucifer_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Lucifer_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Culinary_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Culinary_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Culinary_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Chef_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Chef_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Chef_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Medicinal_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Medicinal_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Medicinal_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Surgeon_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Surgeon_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Surgeon_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Synthetic_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Synthetic_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Synthetic_Queen", true)).RandomElement();
                    this.TryInsertQueen();
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hyper_Queen", true)).Count > 0)
            {
                list.Add(new FloatMenuOption("RB_Hyper_Queen_Tag".Translate(), delegate
                {
                    queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed("RB_Bee_Hyper_Queen", true)).RandomElement();
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
            }
        }

        public bool TryToReserveThings(Pawn pawn, Job job)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null) && pawn.Reserve(job.targetB, job, 1, -1, null);
        }


    }


}

