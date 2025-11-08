using System;
using System.Collections.Generic;
using System.Linq;

using RimWorld;

using Verse;
using Verse.Noise;
using static HarmonyLib.Code;
using static UnityEngine.UI.GridLayoutGroup;



namespace RimBees
{
    public class AdditionalBeeEffects_SlaughterCorpses : AdditionalBeeEffects
    {


        public int rareTickFrequency = 4;
        private int tickCounter = 0;

        public AdditionalBeeEffects_SlaughterCorpses()
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
                                    CompRottable compRottable = corpse.TryGetComp<CompRottable>();
                                    if (compRottable.Stage == RotStage.Fresh)
                                    {
                                        foreach (Thing item in ButcherProducts(corpse.InnerPawn))
                                        {
                                                                                  
                                            GenPlace.TryPlaceThing(item, current, building.Map, ThingPlaceMode.Near);
                                        }
                                    }
                                    for (int i = 0; i < 6; i++)
                                    {
                                        CellFinder.TryFindRandomReachableNearbyCell(current, building.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors), null, null, out var c);
                                        FilthMaker.TryMakeFilth(c, building.Map, corpse.InnerPawn.RaceProps.BloodDef, corpse.InnerPawn.LabelIndefinite());
                                       
                                    }
                                    corpse.Destroy();
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

        public IEnumerable<Thing> ButcherProducts(Pawn def)
        {
            if (def.RaceProps.meatDef != null)
            {
                int num = GenMath.RoundRandom(def.GetStatValue(StatDefOf.MeatAmount));
                if (num > 0)
                {
                    Thing thing = ThingMaker.MakeThing(def.RaceProps.meatDef);
                    thing.stackCount = num;
                    yield return thing;
                }
            }
           
            if (def.RaceProps.leatherDef != null)
            {
                int num2 = GenMath.RoundRandom(def.GetStatValue(StatDefOf.LeatherAmount));
                if (num2 > 0)
                {
                    Thing thing2 = ThingMaker.MakeThing(def.RaceProps.leatherDef);
                    thing2.stackCount = num2;
                    yield return thing2;
                }
            }
            if (def.RaceProps.Humanlike)
            {
                yield break;
            }
            PawnKindLifeStage lifeStage = def.ageTracker.CurKindLifeStage;
            if (lifeStage.butcherBodyPart == null || (def.gender != 0 && (def.gender != Gender.Male || !lifeStage.butcherBodyPart.allowMale) && (def.gender != Gender.Female || !lifeStage.butcherBodyPart.allowFemale)))
            {
                yield break;
            }
            while (true)
            {
                BodyPartRecord bodyPartRecord = def.health.hediffSet.GetNotMissingParts().FirstOrDefault((BodyPartRecord x) => x.IsInGroup(lifeStage.butcherBodyPart.bodyPartGroup));
                if (bodyPartRecord != null)
                {
                    def.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, def, bodyPartRecord));
                    yield return ThingMaker.MakeThing(lifeStage.butcherBodyPart.thing ?? bodyPartRecord.def.spawnThingOnRemoved);
                    continue;
                }
                break;
            }
        }

    }

}