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
            var beehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;

            return (Rand.Chance(1f / 3f) ? beehouse.innerContainerDrones : beehouse.innerContainerQueens).FirstOrFallback().TryGetComp<CompBees>().GetComb;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(240)
                .FailOnDestroyedNullOrForbidden(TargetIndex.A)
                .FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
                .WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {
                    var beehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
                    beehouse.BeehouseIsFull = false;
                    var newComb = ThingMaker.MakeThing(DecideRandomComb());
                    GenSpawn.Spawn(newComb, beehouse.InteractionCell, beehouse.Map);

                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(newComb);
                    if (StoreUtility.TryFindBestBetterStoreCellFor(newComb, this.pawn, this.Map, currentPriority, this.pawn.Faction, out var c))
                    {
                        this.job.SetTarget(TargetIndex.C, c);
                        this.job.SetTarget(TargetIndex.B, newComb);
                        this.job.count = newComb.stackCount;
                        beehouse.tickCounter = 0;
                    }
                    else
                    {
                        this.EndJobWith(JobCondition.Incompletable);
                        beehouse.BeehouseIsFull = false;
                        beehouse.tickCounter = 0;
                    }
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
            yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
            var carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
            yield return carryToCell;
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);
        }
    }
}
