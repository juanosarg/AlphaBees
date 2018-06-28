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

        public float GetCombtimedays
        {
            get
            {
                return this.Props.combtimedays;
            }
        }

        public bool GetNocturnal
        {
            get
            {
                return this.Props.nocturnal;
            }
        }

        public bool GetPluviophile
        {
            get
            {
                return this.Props.pluviophile;
            }
        }

        public string GetWeirdPlant
        {
            get
            {
                return this.Props.weirdplantneeded;
            }
        }

        public int GetTempMin
        {
            get
            {
                return this.Props.tempMin;
            }
        }

        public int GetTempMax
        {
            get
            {
                return this.Props.tempMax;
            }
        }


    }
}

