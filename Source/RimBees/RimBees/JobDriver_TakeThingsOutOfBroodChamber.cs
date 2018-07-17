using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;

namespace RimBees
{
    public class JobDriver_TakeThingsOutOfBroodChamber : JobDriver
    {
        private const TargetIndex BarrelInd = TargetIndex.A;


        private Random rand = new Random();

        private const int Duration = 200;

     

        public override bool TryMakePreToilReservations()
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
           
            int randomNumber = rand.Next(1, 5);

            if (randomNumber == 1)
            {
                resultingBee = DefDatabase<ThingDef>.GetNamed(beeDrone.def.defName, true);
            } else if (randomNumber == 2)
            {
                resultingBee = DefDatabase<ThingDef>.GetNamed(getQueenFromDrone(beeDrone), true);
            } else if (randomNumber == 3)
            {
                resultingBee = DefDatabase<ThingDef>.GetNamed(beeQueen.def.defName, true);
            }
            else {
                resultingBee = DefDatabase<ThingDef>.GetNamed(getDroneFromQueen(beeQueen), true);

            }

            return resultingBee; 
        }

        public string getDroneFromQueen(Thing beeQueen)
        {
            string beeSpecies = beeQueen.TryGetComp<CompBees>().GetSpecies;
            return "RB_Bee_" + beeSpecies + "_Drone";
        }

        public string getQueenFromDrone(Thing beeDrone)
        {
            string beeSpecies = beeDrone.TryGetComp<CompBees>().GetSpecies;
            return "RB_Bee_"+ beeSpecies +"_Queen";
            
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
                    Thing newBee = ThingMaker.MakeThing(DecideRandomBee());
                    GenSpawn.Spawn(newBee, buildingBroodChamber.Position - GenAdj.CardinalDirections[0], buildingBroodChamber.Map);
                    buildingBroodChamber.broodChamberFull = false;
                    buildingBroodChamber.tickCounter = 0;
                },
                defaultCompleteMode = ToilCompleteMode.Instant
            };
               
            }
    }
}
