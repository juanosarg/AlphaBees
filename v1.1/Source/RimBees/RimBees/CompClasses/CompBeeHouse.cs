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

       


    }
}

