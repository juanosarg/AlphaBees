using System;
using HarmonyLib;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class CompHybridizationChamber: ThingComp
    {
      
        public override void PostDraw()
        {
            if (RimBees_Settings.RB_Ben_ShowProgress)
            {
                Building_HybridizationChamber hybridizationchamber = this.parent as Building_HybridizationChamber;
                var progress = hybridizationchamber.tickCounter / (hybridizationchamber.ticksToDays * 3f);
                if (hybridizationchamber.hybridizationChamberFull)
                {
                    progress = 1f;
                }

                GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
                {
                    center = parent.DrawPos + CompBeeHouse.ProgressBarOffset,
                    size = CompBeeHouse.ProgressBarSize,
                    fillPercent = progress,
                    margin = 0.1f,
                    rotation = Rot4.North,
                    filledMat = CompBeeHouse.FilledMat,
                    unfilledMat = CompBeeHouse.UnfilledMat,
                });
            }
        }


    }
}
