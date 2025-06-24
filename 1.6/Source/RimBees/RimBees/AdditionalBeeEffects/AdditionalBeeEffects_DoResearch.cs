using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using HarmonyLib;
using RimWorld;

using Verse;


namespace RimBees
{
    public class AdditionalBeeEffects_DoResearch : AdditionalBeeEffects
    {

        public int pointsPerHour;

        private int tickCounter = 0;

        public AdditionalBeeEffects_DoResearch()
        {

        }

       

        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > 9)
            {
                if (building.Map != null)
                {

                    ResearchProjectDef proj = Find.ResearchManager.GetProject();
                    if (proj != null)
                    {
                        FieldInfo fieldInfo = AccessTools.Field(typeof(ResearchManager), "progress");
                        Dictionary<ResearchProjectDef, float> dictionary = fieldInfo.GetValue(Find.ResearchManager) as Dictionary<ResearchProjectDef, float>;
                        if (dictionary.ContainsKey(proj))
                        {
                            dictionary[proj] += pointsPerHour*RimBees_Settings.workerBeeEffectMultiplier;
                        }
                        if (proj.IsFinished)
                        {
                            Find.ResearchManager.FinishProject(proj, doCompletionDialog: true);
                        }
                    }
                }
                tickCounter = 0;
            }
            tickCounter++;
        }



    }
}