using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_TakeThingsOutOfHybridizationChamber : JobDriver
    {
        private const TargetIndex BarrelInd = TargetIndex.A;


      

        private const int Duration = 200;

     

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A).Thing, this.job, 1, -1, null);
        }

        public ThingDef GetHybridBee()
        {
            Building_HybridizationChamber buildinghybridizationchamber = (Building_HybridizationChamber)this.job.GetTarget(TargetIndex.A).Thing;
            return DefDatabase<ThingDef>.GetNamed("RB_Bee_" + buildinghybridizationchamber.hybridizedBee + "_Queen", true);
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
                    Building_HybridizationChamber buildingHybridizationChamber = (Building_HybridizationChamber)this.job.GetTarget(TargetIndex.A).Thing;
                    Thing newBee = ThingMaker.MakeThing(GetHybridBee());
                    GenSpawn.Spawn(newBee, buildingHybridizationChamber.Position - GenAdj.CardinalDirections[0], buildingHybridizationChamber.Map);
                    buildingHybridizationChamber.hybridizationChamberFull = false;
                    buildingHybridizationChamber.tickCounter = 0;
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
               
            }
    }
}
