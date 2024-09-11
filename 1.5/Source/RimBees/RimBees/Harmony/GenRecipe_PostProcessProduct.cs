﻿using AlphaImplants;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace RimBees
{
    [HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
    public class AlphaBees_GenRecipe_PostProcessProduct_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(RecipeDef recipeDef, ref Thing __result)
        {
            if(recipeDef.GetModExtension<OutputMultiplierRecipe>() != null)
            {
                float multiplier = recipeDef.GetModExtension<OutputMultiplierRecipe>().multiplier;

                __result.stackCount = (int)(__result.stackCount*multiplier*RimBees_Settings.beeProductionMultiplier);

            }
        }
    }
}