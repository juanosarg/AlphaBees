using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RimBees
{


    public class CompProperties_RandomItemSpawner : CompProperties
    {
        public List<ThingDef> items;
        public List<ThingCategoryDef> categories;
        public List<ThingDef> itemsBlacklistedFromCategories;
        public int amount = 1;

        public CompProperties_RandomItemSpawner()
        {
            this.compClass = typeof(CompRandomItemSpawner);
        }
    }
}
