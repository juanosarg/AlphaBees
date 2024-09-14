using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimBees;
using RimWorld;
using UnityEngine;
using Verse;
namespace RimBees
{

    public class CompRandomItemSpawner : ThingComp
    {
        public CompProperties_RandomItemSpawner Props => (CompProperties_RandomItemSpawner)this.props;


        public override void CompTick()
        {
            base.CompTick();
            if (this.parent.IsHashIntervalTick(20))
            {

                if (!(this.parent.ParentHolder is Pawn_CarryTracker))
                {
                    SpawnItemAndDelete();
                }

            }
        }

        public void SpawnItemAndDelete()
        {
            Thing thing = null;
            if (!Props.items.NullOrEmpty())
            {
                thing = GenSpawn.Spawn(Props.items.RandomElement(), this.parent.Position, this.parent.Map);              

            }else
            if(!Props.categories.NullOrEmpty())
            {
                List<ThingDef> itemsInCategory = new List<ThingDef>();
                foreach (ThingCategoryDef category in Props.categories)
                {
                    itemsInCategory.AddRange(DefDatabase<ThingDef>.AllDefsListForReading.Where(x => x.thingCategories?.Contains(category)==true && Props.itemsBlacklistedFromCategories?.Contains(x)!=true));
                }
               
                thing = GenSpawn.Spawn(itemsInCategory.RandomElement(), this.parent.Position, this.parent.Map);

            }
            if (thing != null)
            {
                thing.stackCount = Props.amount;
                this.parent.Destroy();
            }
            

        }

    }
}
