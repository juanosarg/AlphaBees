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



namespace RimBees
{
    public class AdditionalBeeEffects_Light : AdditionalBeeEffects
    {

       
        public int rareTickFrequency = 4;
        private int tickCounter = 0;

        public AdditionalBeeEffects_Light()
        {

        }

       

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    bool flagFoundLight = false;
                  
                    foreach (Thing thing in building.Position.GetThingList(building.Map))
                    { 
                        if(thing.def == InternalDefOf.RB_Bee_LightSource)
                        {
                            Building_BeeLightSource thingAsLightSource = thing as Building_BeeLightSource;
                            thingAsLightSource.tickCounter = Building_BeeLightSource.tickCounterToDelete;
                            flagFoundLight = true;
                            break;
                        }
                      
                    
                    }
                    if (!flagFoundLight)
                    {
                        Thing newLight = ThingMaker.MakeThing(InternalDefOf.RB_Bee_LightSource, null);
                        GenPlace.TryPlaceThing(newLight, building.Position, building.Map, ThingPlaceMode.Direct);
                        newLight.SetFaction(Faction.OfPlayer);
                    }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}