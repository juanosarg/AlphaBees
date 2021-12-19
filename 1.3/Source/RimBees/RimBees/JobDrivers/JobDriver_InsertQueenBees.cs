using System.Collections.Generic;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_InsertQueenBees : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null) &&
                this.pawn.Reserve(this.job.targetB, this.job, 1, -1, null);
        }

        public override void Notify_PatherFailed()
        {
            var beehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
            beehouse.BeehouseIsExpectingQueens = false;
            this.EndJobWith(JobCondition.ErroredPather);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_General.DoAtomic(delegate
            {
                this.job.count = 1;
            });

            var reserveBees = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
            yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch)
                .FailOnDespawnedNullOrForbidden(TargetIndex.B)
                .FailOnSomeonePhysicallyInteracting(TargetIndex.B);
            yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false)
                .FailOnDestroyedNullOrForbidden(TargetIndex.B);
            yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveBees, TargetIndex.B, TargetIndex.None, true, null);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(240)
                .FailOnDestroyedNullOrForbidden(TargetIndex.B)
                .FailOnDestroyedNullOrForbidden(TargetIndex.A)
                .FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
                .WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            yield return new Toil
            {
                initAction = delegate
                {
                    Building_Beehouse buildingbeehouse = (Building_Beehouse)this.job.GetTarget(TargetIndex.A).Thing;
                    buildingbeehouse.TryAcceptAnyQueen(this.job.targetB.Thing,true);
                    buildingbeehouse.BeehouseIsExpectingQueens = false;
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
        }
    }
}
