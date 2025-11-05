using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;



namespace RimBees
{
    public class AdditionalBeeEffects_CleanPollution : AdditionalBeeEffects
    {

       
        public int rareTickFrequency = 240;
        private int tickCounter = 0;

        public AdditionalBeeEffects_CleanPollution()
        {

        }

       

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(building.Position, RimBees_Settings.beeEffectRadius, useCenter: true);

                    foreach (IntVec3 c in cells.InRandomOrder())
                    {
                        if (c.InBounds(building.Map) && c.CanUnpollute(building.Map))
                        {

                            building.Map.pollutionGrid.SetPolluted(c, isPolluted: false);
                       
                            InternalDefOf.PollutionExtractedPoluxTree.Spawn(c, building.Map).Cleanup();
                            
                            MoteMaker.MakeAttachedOverlay(building, ThingDefOf.Mote_PollutionPump, Vector3.zero);
                            break;
                        }
                  

                    }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }


       
    }
}