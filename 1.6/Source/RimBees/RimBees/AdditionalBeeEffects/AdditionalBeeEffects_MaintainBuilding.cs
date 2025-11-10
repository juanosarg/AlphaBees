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
    public class AdditionalBeeEffects_MaintainBuilding : AdditionalBeeEffects
    {
      
        public int rareTickFrequency = 4;
        private int tickCounter = 0;

        public ThingDef maintainedBuilding;

        public AdditionalBeeEffects_MaintainBuilding()
        {

        }

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {
                    bool flagFoundBuilding = false;
                  
                    foreach (Thing thing in building.Position.GetThingList(building.Map))
                    { 
                        if(thing.def == maintainedBuilding)
                        {
                            Building_BeeMaintainable thingAsMaintainedBuilding = thing as Building_BeeMaintainable;
                            thingAsMaintainedBuilding.tickCounter = Building_BeeMaintainable.tickCounterToDelete;
                            flagFoundBuilding = true;
                            break;
                        }
                      
                    
                    }
                    if (!flagFoundBuilding)
                    {
                        Thing thingAsMaintainedBuilding = ThingMaker.MakeThing(maintainedBuilding, null);
                        GenPlace.TryPlaceThing(thingAsMaintainedBuilding, building.Position, building.Map, ThingPlaceMode.Direct);
                        thingAsMaintainedBuilding.SetFaction(Faction.OfPlayer);
                    }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}