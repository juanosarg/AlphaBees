﻿using Verse;

namespace RimBees
{
    public class CompTreeHive : ThingComp
    {


        public CompProperties_TreeHive Props
        {
            get
            {
                return (CompProperties_TreeHive)this.props;
            }
        }

        public string GetSpecies
        {
            get
            {
                return this.Props.species;
            }
        }

     

    }
}

