using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace RimBees
{
    public class ThingSetMaker_BeeResourcePod : ThingSetMaker
    {
        protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
        {
            ThingDef thingDef = ThingSetMaker_BeeResourcePod.RandomPodContentsDef(false);
            float num = Rand.Range(150f, 600f);
            do
            {
                Thing thing = ThingMaker.MakeThing(thingDef, null);
                int num2 = Rand.Range(20, 40);
                if (num2 > thing.def.stackLimit)
                {
                    num2 = thing.def.stackLimit;
                }
                if ((float)num2 * thing.def.BaseMarketValue > num)
                {
                    num2 = Mathf.FloorToInt(num / thing.def.BaseMarketValue);
                }
                if (num2 == 0)
                {
                    num2 = 1;
                }
                thing.stackCount = num2;
                outThings.Add(thing);
                num -= (float)num2 * thingDef.BaseMarketValue;
            }
            while (outThings.Count < 7 && num > thingDef.BaseMarketValue);
        }

        private static IEnumerable<ThingDef> PossiblePodContentsDefs()
        {
            return from d in DefDatabase<ThingDef>.AllDefs
                   where d.category == ThingCategory.Item && d.defName.Contains("RB_") && !d.defName.Contains("Research") && d.equipmentType == EquipmentType.None && d.BaseMarketValue >= 1f && d.BaseMarketValue < 100f && !d.HasComp(typeof(CompHatcher))
                   select d;
        }

        public static ThingDef RandomPodContentsDef(bool mustBeResource = false)
        {
            IEnumerable<ThingDef> source = ThingSetMaker_BeeResourcePod.PossiblePodContentsDefs();
            if (mustBeResource)
            {
                source = from x in source
                         where x.stackLimit > 1
                         select x;
            }
            int numMeats = (from x in source
                            where x.IsMeat
                            select x).Count<ThingDef>();
            int numLeathers = (from x in source
                               where x.IsLeather
                               select x).Count<ThingDef>();
            return source.RandomElementByWeight((ThingDef d) => ThingSetMakerUtility.AdjustedBigCategoriesSelectionWeight(d, numMeats, numLeathers));
        }

        

        protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
        {
            return ThingSetMaker_BeeResourcePod.PossiblePodContentsDefs();
        }

        private const int MaxStacks = 7;

        private const float MaxMarketValue = 40f;

        private const float MinMoney = 150f;

        private const float MaxMoney = 600f;
    }
}
