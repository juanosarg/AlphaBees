using System;
using HarmonyLib;
using UnityEngine.XR;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class CompBroodChamber : ThingComp
    {
      

        public override void PostDraw()
        {
            if (RimBees_Settings.RB_Ben_ShowProgress)
            {
                Building_BroodChamber broodchamber = this.parent as Building_BroodChamber;
                var progress = broodchamber.tickCounter / ((float)broodchamber.ticksToDays * RimBees_Settings.broodChamberMultiplier);
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
