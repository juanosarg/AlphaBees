using Verse;

namespace RimBees
{
    class CompBeeHouse : ThingComp
    {


        public CompProperties_BeeHouse Props
        {
            get
            {
                return (CompProperties_BeeHouse)this.props;
            }
        }

        public bool GetIsBeehouse
        {
            get
            {
                return this.Props.isBeehouse;
            }
        }

        public bool GetIsElectricBeehouse
        {
            get
            {
                return this.Props.electricBeehouse;
            }
        }

        public bool GetIsClimatizedBeehouse
        {
            get
            {
                return this.Props.climatizedBeehouse;
            }
        }

        public float GetBeehouseRate
        {
            get
            {
                return this.Props.beehouseRate;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            Beehouses_MapComponent mapComp = this.parent.Map.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddBeehouseToMap(this.parent);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            Beehouses_MapComponent mapComp = map.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveBeehouseFromMap(this.parent);
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {

            Beehouses_MapComponent mapComp = previousMap.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveBeehouseFromMap(this.parent);
            }
        }


    }
}

