
using RimWorld;
using Verse;


namespace RimBees
{
    class Building_TreeHive : Plant
    {


       

     



        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            string strSpecies = this.TryGetComp<CompTreeHive>().GetSpecies;
            IntVec3 thisPosition = this.Position;
            Map map = base.Map;
            base.Destroy(mode);
            if (strSpecies!="None") {
                Plant regularPlant = (Plant)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed(strSpecies, true));
                GenSpawn.Spawn(regularPlant, thisPosition, map);
                regularPlant.Growth = 0.9f;
            }
           

        }


    }
}
