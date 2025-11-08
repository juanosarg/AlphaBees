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
    public class AdditionalBeeEffects_Tending : AdditionalBeeEffects
    {


        public int rareTickFrequency = 4;
        private int tickCounter = 0;

        public AdditionalBeeEffects_Tending()
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
                            bool foundPawn = false;
                            HashSet<Thing> thingsInCell = new HashSet<Thing>(current.GetThingList(building.Map));
                            foreach (Thing thingInCell in thingsInCell)
                            {
                                if (thingInCell is Pawn pawn && pawn.Faction == Faction.OfPlayerSilentFail)
                                {
                                    List<Hediff_Injury> injuries = GetInjuries(pawn);
                                    if (injuries.Count > 0)
                                    {
                                        foreach (Hediff_Injury injury in injuries)
                                        {
                                            if (injury.TendableNow()) {
                                                injury.Tended(0.1f, 0.3f);
                                                foundPawn = true;
                                                break;
                                                
                                            }
                                                

                                        }

                                    }
                                }

                            }
                            if (foundPawn)
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

        public List<Hediff_Injury> GetInjuries(Pawn pawn)
        {
            List<Hediff_Injury> injuries = new List<Hediff_Injury>();
            for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
            {
                if (pawn.health.hediffSet.hediffs[i] is Hediff_Injury hediff_Injury)
                {
                    
                   injuries.Add(hediff_Injury);
                       

                }
            }
            return injuries;
        }



    }
}