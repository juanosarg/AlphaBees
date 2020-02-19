using Verse;

namespace RimBees
{
    class CompProperties_BeeHouse : CompProperties
    {
        public bool isBeehouse = true;
        public bool electricBeehouse = false;
        public bool climatizedBeehouse = false;
        public float beehouseRate = 1f;
       





        public CompProperties_BeeHouse()
        {
            this.compClass = typeof(CompBeeHouse);
        }
    }
}
