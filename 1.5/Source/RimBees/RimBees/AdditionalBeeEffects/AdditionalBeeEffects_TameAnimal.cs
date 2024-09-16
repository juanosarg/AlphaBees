using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;



namespace RimBees
{
    public class AdditionalBeeEffects_TameAnimal : AdditionalBeeEffects
    {

        public int rareTickFrequency = 8;

        private int tickCounter = 0;

        private static readonly SimpleCurve TameChanceFactorCurve_Wildness = new SimpleCurve
    {
        new CurvePoint(1f, 0f),
        new CurvePoint(0.5f, 1f),
        new CurvePoint(0f, 2f)
    };

        public AdditionalBeeEffects_TameAnimal()
        {

        }



        public override void AdditionalEffectTick(Building_Beehouse building)
        {
            if (tickCounter > rareTickFrequency)
            {
                if (building.Map != null)
                {

                    Designation designation = building.Map.designationManager.designationsByDef[DesignationDefOf.Tame]?.ToList()?.RandomElement();

                    if(designation != null)
                    {
                        Pawn pawn = designation.target.Thing as Pawn;
                        if (pawn != null && TameUtility.CanTame(pawn) && !TameUtility.TriedToTameTooRecently(pawn))
                        {
                            float num2 = 0.34f;
                            float x = pawn.IsWildMan() ? 0.75f : pawn.RaceProps.wildness;
                            num2 *= TameChanceFactorCurve_Wildness.Evaluate(x);
                            string letterText = null;
                            string letterLabel = null;
                            LetterDef letterDef = null;
                            LookTargets lookTargets = null;
                          
                            if (Rand.Chance(num2))
                            {
                                InteractionWorker_RecruitAttempt.DoRecruit(null, pawn, out letterLabel, out letterText, useAudiovisualEffects: true, sendLetter: true);
                                if (!letterLabel.NullOrEmpty())
                                {
                                    letterDef = LetterDefOf.PositiveEvent;
                                }
                                lookTargets = new LookTargets(pawn);
                                Messages.Message("RB_HandlerBees_TameSuccess".Translate(pawn.LabelCapNoCount), lookTargets, MessageTypeDefOf.PositiveEvent);

                            }
                            else
                            {
                                TaggedString taggedString = "TextMote_TameFail".Translate(num2.ToStringPercent());
                                MoteMaker.ThrowText(building.DrawPos, building.Map, taggedString, 8f);
                                MoteMaker.ThrowText(pawn.DrawPos, building.Map, taggedString, 8f);
                                pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
                            }
                        }
                    }
                    



                }
                tickCounter = 0;
            }
            tickCounter++;
        }

        public bool CanCut(Thing t)
        {
            if (t.def.category != ThingCategory.Plant)
            {
                return false;
            }
            if (t.IsBurning())
            {
                return false;
            }
            if (t.def.plant?.IsTree != true)
            {
                return false;
            }
            return true;

        }

        public void CutPlant(Thing t)
        {
            Plant plant = t as Plant;
            if (plant.def.plant.harvestedThingDef != null)
            {

                int num = plant.YieldNow();

                if (num > 0)
                {
                    Thing thing = ThingMaker.MakeThing(plant.def.plant.harvestedThingDef);
                    thing.stackCount = num;

                    GenPlace.TryPlaceThing(thing, t.Position, t.Map, ThingPlaceMode.Near);

                }
                if (plant.HarvestableNow)
                {
                    foreach (ThingComp allComp in plant.AllComps)
                    {
                        foreach (ThingDefCountClass item in allComp.GetAdditionalHarvestYield())
                        {
                            Thing thing2 = ThingMaker.MakeThing(item.thingDef);
                            thing2.stackCount = item.count;
                            GenPlace.TryPlaceThing(thing2, t.Position, t.Map, ThingPlaceMode.Near);
                        }
                    }
                }
            }

            plant.def.plant.soundHarvestFinish.PlayOneShot(t);
            plant.Destroy();




        }



    }
}