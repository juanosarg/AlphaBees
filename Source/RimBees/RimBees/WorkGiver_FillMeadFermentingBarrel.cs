using System;
using Verse;
using Verse.AI;
using RimWorld;

namespace RimBees
{
    public class WorkGiver_FillMeadFermentingBarrel : WorkGiver_Scanner
    {
        private static string TemperatureTrans;

        private static string NoWortTrans;

        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForDef(ThingDef.Named("RB_MeadFermentingBarrel"));
            }
        }

        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }

        public static void ResetStaticData()
        {
            WorkGiver_FillMeadFermentingBarrel.TemperatureTrans = "BadTemperature".Translate().ToLower();
            WorkGiver_FillMeadFermentingBarrel.NoWortTrans = "RB_NoMeadMust".Translate();
        }

        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_MeadFermentingBarrel building_FermentingBarrel = t as Building_MeadFermentingBarrel;
            if (building_FermentingBarrel == null || building_FermentingBarrel.Fermented || building_FermentingBarrel.SpaceLeftForWort <= 0)
            {
                return false;
            }
            float ambientTemperature = building_FermentingBarrel.AmbientTemperature;
            CompProperties_TemperatureRuinable compProperties = building_FermentingBarrel.def.GetCompProperties<CompProperties_TemperatureRuinable>();
            if (ambientTemperature < compProperties.minSafeTemperature + 2f || ambientTemperature > compProperties.maxSafeTemperature - 2f)
            {
                JobFailReason.Is(WorkGiver_FillMeadFermentingBarrel.TemperatureTrans, null);
                return false;
            }
            if (!t.IsForbidden(pawn))
            {
                LocalTargetInfo target = t;
                if (pawn.CanReserve(target, 1, -1, null, forced))
                {
                    if (pawn.Map.designationManager.DesignationOn(t, DesignationDefOf.Deconstruct) != null)
                    {
                        return false;
                    }
                    if (this.FindWort(pawn, building_FermentingBarrel) == null)
                    {
                        JobFailReason.Is(WorkGiver_FillMeadFermentingBarrel.NoWortTrans, null);
                        return false;
                    }
                    return !t.IsBurning();
                }
            }
            return false;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Building_MeadFermentingBarrel barrel = (Building_MeadFermentingBarrel)t;
            Thing t2 = this.FindWort(pawn, barrel);
            return new Job(DefDatabase<JobDef>.GetNamed("RB_FillMeadFermentingBarrelJob", true), t, t2);
        }

        private Thing FindWort(Pawn pawn, Building_MeadFermentingBarrel barrel)
        {
            Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false);
            IntVec3 position = pawn.Position;
            Map map = pawn.Map;
            ThingRequest thingReq = ThingRequest.ForDef(ThingDef.Named("RB_Must"));
            PathEndMode peMode = PathEndMode.ClosestTouch;
            TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
            Predicate<Thing> validator = predicate;
            return GenClosest.ClosestThingReachable(position, map, thingReq, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
        }
    }
}
