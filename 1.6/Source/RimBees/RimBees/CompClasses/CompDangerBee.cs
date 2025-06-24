using Verse;

namespace RimBees
{
    public class CompDangerBee : ThingComp
    {
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            parent.Map.GetComponent<BeeDangerManager_MapComponent>().Track(parent);
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            map.GetComponent<BeeDangerManager_MapComponent>().Untrack(parent);
        }
    }
}
