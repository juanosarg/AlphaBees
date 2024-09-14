using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;



namespace RimBees
{
    public class AdditionalBeeEffects_DryTerrain : AdditionalBeeEffects
    {

       
        public int rareTickFrequency = 240;
        private int tickCounter = 0;

        public AdditionalBeeEffects_DryTerrain()
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
                        bool convertedTerrain = false;
                        TerrainDef terrain = c.GetTerrain(building.Map);
                        TerrainDef terrainToDryTo = GetTerrainToDryTo(building.Map, terrain);
                        if (terrainToDryTo != null)
                        {
                            building.Map.terrainGrid.SetTerrain(c, terrainToDryTo);
                            convertedTerrain = true;
                        }
                        TerrainDef terrainDef = building.Map.terrainGrid.UnderTerrainAt(c);
                        if (terrainDef != null)
                        {
                            TerrainDef terrainToDryTo2 = GetTerrainToDryTo(building.Map, terrainDef);
                            if (terrainToDryTo2 != null)
                            {
                                building.Map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
                                convertedTerrain = true;
                            }
                        }
                        if (convertedTerrain) {
                            break;
                        }

                    }

                }
                tickCounter = 0;
            }
            tickCounter++;
        }


        private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
        {
            if (terrainDef.driesTo == null)
            {
                return null;
            }
            if (map.Biome == BiomeDefOf.SeaIce)
            {
                return TerrainDefOf.Ice;
            }
            return terrainDef.driesTo;
        }
    }
}