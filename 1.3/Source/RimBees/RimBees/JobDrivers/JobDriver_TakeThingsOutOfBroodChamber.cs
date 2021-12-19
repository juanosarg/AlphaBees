using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_TakeThingsOutOfBroodChamber : JobDriver
    {
        private const TargetIndex BarrelInd = TargetIndex.A;

        private const int Duration = 200;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null);
        }

        public ThingDef DecideRandomBee()
        {
            Building_BroodChamber buildingbroodchamber = (Building_BroodChamber)this.job.GetTarget(TargetIndex.A).Thing;
            Building_Beehouse buildingbeehouse = buildingbroodchamber.GetAdjacentBeehouse();
            Thing beeDrone = buildingbeehouse.innerContainerDrones.FirstOrFallback();
            Thing beeQueen = buildingbeehouse.innerContainerQueens.FirstOrFallback();
            ThingDef resultingBee;

            int randomNumber = Rand.Range(1, 14);

            if (randomNumber >= 1 && randomNumber <= 5)
            {
                resultingBee = beeDrone.def;
            }
            else if (randomNumber == 6)
            {
                resultingBee = getQueenFromDrone(beeDrone);
            }
            else if (randomNumber == 7)
            {
                resultingBee = beeQueen.def;
            }
            else
            {
                resultingBee = getDroneFromQueen(beeQueen);
            }

            return resultingBee;
        }

        public ThingDef getDroneFromQueen(Thing beeQueen)
        {
            var beeSpecies = beeQueen.TryGetComp<CompBees>().GetSpecies;
            return beeSpecies.drone;
        }

        public ThingDef getQueenFromDrone(Thing beeDrone)
        {
            var beeSpecies = beeDrone.TryGetComp<CompBees>().GetSpecies;
            return beeSpecies.queen;
        }

        [DebuggerHidden]
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
                    Building_BroodChamber buildingBroodChamber = (Building_BroodChamber)this.job.GetTarget(TargetIndex.A).Thing;
                    buildingBroodChamber.broodChamberFull = false;
                    Thing newBee = ThingMaker.MakeThing(DecideRandomBee());
                    GenSpawn.Spawn(newBee, buildingBroodChamber.Position - GenAdj.CardinalDirections[0], buildingBroodChamber.Map);
                    StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(newBee);
                    IntVec3 c;
                    if (StoreUtility.TryFindBestBetterStoreCellFor(newBee, this.pawn, this.Map, currentPriority, this.pawn.Faction, out c, true))
                    {
                        this.job.SetTarget(TargetIndex.C, c);
                        this.job.SetTarget(TargetIndex.B, newBee);
                        this.job.count = newBee.stackCount;
                        buildingBroodChamber.tickCounter = 0;
                    }
                    else
                    {
                        this.EndJobWith(JobCondition.Incompletable);
                        buildingBroodChamber.broodChamberFull = false;
                        buildingBroodChamber.tickCounter = 0;


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
