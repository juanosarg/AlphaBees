using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_TakeThingsOutOfBeehouse : JobDriver
    {
        private const TargetIndex BarrelInd = TargetIndex.A;

      

        private const int Duration = 200;

     

        public override bool TryMakePreToilReservations()
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null);
        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
            Thing newComb = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("RB_Temperate_Honeycomb", true));

            GenSpawn.Spawn(newComb, buildingbeehouse.Position - GenAdj.CardinalDirections[0], buildingbeehouse.Map);
            buildingbeehouse.BeehouseIsFull = false;
            buildingbeehouse.tickCounter = 0;
                /* yield return Toils_General.Wait(200).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).FailOn(() => !this.$this.Barrel.Fermented).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {
                    Thing thing = this.$this.Barrel.TakeOutBeer();
                    GenPlace.TryPlaceThing(thing, this.$this.pawn.Position, this.$this.Map, ThingPlaceMode.Near, null, null);
                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(thing);
                    IntVec3 c;
                    if (StoreUtility.TryFindBestBetterStoreCellFor(thing, this.$this.pawn, this.$this.Map, currentPriority, this.$this.pawn.Faction, out c, true))
                    {
                        this.$this.job.SetTarget(TargetIndex.C, c);
                        this.$this.job.SetTarget(TargetIndex.B, thing);
                        this.$this.job.count = thing.stackCount;
                    }
                    else
                    {
                        this.$this.EndJobWith(JobCondition.Incompletable);
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
            yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true);*/
        }
    }
}
