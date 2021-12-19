using RimWorld;
using Verse;

namespace RimBees
{
    public class Building_TreeHive : Plant
    {
        public override void PlantCollected(Pawn by)
        {
            var replaceWith = this.TryGetComp<CompTreeHive>().GetSpecies;
            var position = this.Position;
            var map = this.Map;

            base.PlantCollected(by);

            if (replaceWith != null)
            {
                var regularPlant = (Plant)ThingMaker.MakeThing(replaceWith);
                GenSpawn.Spawn(regularPlant, position, map);
                regularPlant.Growth = 0.9f;
            }
        }
    }
}
