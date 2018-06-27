using Verse;

namespace RimBees
{
    class CompBees : ThingComp
    {


        public CompProperties_Bees Props
        {
            get
            {
                return (CompProperties_Bees)this.props;
            }
        }

        public string GetSpecies
        {
            get
            {
                return this.Props.species;
            }
        }

        public string GetComb
        {
            get
            {
                return this.Props.comb;
            }
        }


    }
}

