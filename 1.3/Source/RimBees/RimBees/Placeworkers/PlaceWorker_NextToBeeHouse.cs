using RimWorld;
using Verse;

namespace RimBees
{
    public abstract class PlaceWorker_NextToBeeHouse : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            var c = loc - GetOffsetFromBeehouse(rot);
            if (!c.InBounds(map))
            {
                return GetFailureMessage();
            }

            var edifice = c.GetEdifice(map);
            if (edifice is Building_Beehouse beehouse)
            {
                if (beehouse.Rotation == rot)
                {
                    return true;
                }

                return GetFailureMessage();
            }

            if (edifice == null)
            {
                return GetFailureMessage();
            }

            if (GenConstruct.BuiltDefOf(edifice.def) is ThingDef thingDef)
            {
                if (thingDef.GetCompProperties<CompProperties_BeeHouse>() != null && edifice.Rotation == rot)
                {
                    return "RB_BeehouseNotYetBuilt".Translate();
                }
            }

            return GetFailureMessage();
        }

        protected abstract IntVec3 GetOffsetFromBeehouse(Rot4 rot);
        protected abstract TaggedString GetFailureMessage();
    }
}
