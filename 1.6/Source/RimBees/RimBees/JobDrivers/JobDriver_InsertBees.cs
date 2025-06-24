using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class JobDriver_InsertBees : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null) && this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null);
        }

        public override void Notify_PatherFailed()
        {

            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;

            buildingbeehouse.BeehouseIsExpectingBees = false;

            this.EndJobWith(JobCondition.ErroredPather);

        }

        [DebuggerHidden]
        protected override IEnumerable<Toil> MakeNewToils()
        {
            //Log.Message("I am inside the job now, with "+pawn.ToString(), false);

            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_General.DoAtomic(delegate
            {
                this.job.count = 1;
            });
            Toil reserveBees = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);

            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
            yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveBees, TargetIndex.B, TargetIndex.None, true, null);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {
                    Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
                   // buildingbeehouse.droneThing = this.job.targetB.Thing;
                    buildingbeehouse.TryAcceptThing(this.job.targetB.Thing,true);
                    buildingbeehouse.BeehouseIsExpectingBees = false;
                    //this.job.targetB.Thing.Destroy(); 

                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
            


            


        }
    }
}