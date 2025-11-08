using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Noise;
using static HarmonyLib.Code;



namespace RimBees
{
    public class AdditionalBeeEffects_RotCorpses : AdditionalBeeEffects
    {


        public int rareTickFrequency = 4;
        private int tickCounter = 0;

        public AdditionalBeeEffects_RotCorpses()
        {

        }



        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    IEnumerable<IntVec3> cells = GenRadial.RadialCellsAround(building.Position, RimBees_Settings.beeEffectRadius, useCenter: true);

                    foreach (IntVec3 current in cells)
                    {
                        if (current.InBounds(building.Map))
                        {
                            bool foundCorpse = false;
                            HashSet<Thing> thingsInCell = new HashSet<Thing>(current.GetThingList(building.Map));
                            foreach (Thing thingInCell in thingsInCell)
                            {
                                if (thingInCell is Corpse corpse && corpse.InnerPawn.def.race.IsFlesh)
                                {
                                    corpse.HitPoints -= 5;
                                    CompRottable compRottable = corpse.TryGetComp<CompRottable>();
                                    if (compRottable.Stage == RotStage.Fresh)
                                    {
                                        compRottable.RotProgress += 100000;
                                    }
                                    if (corpse.HitPoints < 0)
                                    {
                                        corpse.Destroy();

                                    }
                                    FilthMaker.TryMakeFilth(current, building.Map, ThingDefOf.Filth_CorpseBile);
                                    foundCorpse = true;
                                    break;
                                }

                            }
                            if (foundCorpse)
                            {
                                break;
                            }
                        }


                    }



                }


                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}