﻿using AlphaImplants;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimBees
{
    [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
    public class AlphaBees_GenRecipe_PostProcessProduct_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(Pawn worker, RecipeDef recipeDef, ref Thing __result)
        {
            if(recipeDef.GetModExtension<OutputMultiplierRecipe>() != null)
            {
                float multiplier = recipeDef.GetModExtension<OutputMultiplierRecipe>().multiplier;
                int resultingStack = (int)(__result.stackCount * multiplier * RimBees_Settings.beeProductionMultiplier * worker.GetStatValue(InternalDefOf.AB_CombYieldFactor));
                if(resultingStack == 0)
                {
                    resultingStack = 1;
                }


                __result.stackCount = resultingStack;

            }
        }
    }
}