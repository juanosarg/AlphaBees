using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_TakeThingsOutOfBeehouse : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null);
        }

        public ThingDef DecideRandomComb()
        {
            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
            ThingDef resultingComb;

            if (Rand.Chance(1f / 3f))
            {
                resultingComb = DefDatabase<ThingDef>.GetNamed(buildingbeehouse.innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetComb, true);
            }
            else
            {
                resultingComb = DefDatabase<ThingDef>.GetNamed(buildingbeehouse.innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetComb, true);
            }

            return resultingComb; 
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {

                    Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
                    buildingbeehouse.BeehouseIsFull = false;
                    Thing newComb = ThingMaker.MakeThing(DecideRandomComb());
                    GenSpawn.Spawn(newComb, buildingbeehouse.Position - GenAdj.CardinalDirections[0], buildingbeehouse.Map);
                    
                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(newComb);
                    IntVec3 c;
                    if (StoreUtility.TryFindBestBetterStoreCellFor(newComb, this.pawn, this.Map, currentPriority, this.pawn.Faction, out c, true))
                    {
                        this.job.SetTarget(TargetIndex.C, c);
                        this.job.SetTarget(TargetIndex.B, newComb);
                        this.job.count = newComb.stackCount;
                        buildingbeehouse.tickCounter = 0;
                    }
                    else
                    {
                        this.EndJobWith(JobCondition.Incompletable);
                        buildingbeehouse.BeehouseIsFull = false;
                        buildingbeehouse.tickCounter = 0;


                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
            yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
            Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
            yield return carryToCell;
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
           
        }
    }
}
