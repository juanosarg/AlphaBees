using System.Collections.Generic;
using System.Linq;
using RimBees;
using RimWorld;
using Verse;

namespace RimBees
{
    public class Alert_BeeDanger : Alert
    {
        public Alert_BeeDanger()
        {
            defaultPriority = AlertPriority.High;
            defaultLabel = "BenLubarsRimBeesPatches_Alert_BeeDanger_title".Translate();
            defaultExplanation = "BenLubarsRimBeesPatches_Alert_BeeDanger_desc".Translate();
        }

        public override AlertReport GetReport()
        {
            if (!RimBees_Settings.RB_Ben_BeeDangerAlert)
            {
                return AlertReport.Inactive;
            }

            var map = Find.CurrentMap;
            if (map == null)
            {
                return AlertReport.Inactive;
            }

            return AlertReport.CulpritsAre(GetBeesInDanger(map).ToList());
        }

        public static IEnumerable<Thing> GetBeesInDanger(Map map)
        {
            var bees = map.GetComponent<BeeDangerManager_MapComponent>().bees;

            foreach (var bee in bees)
            {
                var temp = bee.TryGetComp<CompTempRuinableAndDestroy>();
                if (temp.Ruined)
                {
                    // don't care about bees that can't be saved
                    continue;
                }

                if (SteadyEnvironmentEffects.FinalDeteriorationRate(bee) > 0f)
                {
                    yield return bee;
                }
                else
                {
                    var ambient = bee.AmbientTemperature;
                    if (ambient < temp.Props.minSafeTemperature || ambient > temp.Props.maxSafeTemperature)
                    {
                        yield return bee;
                    }
                }
            }
        }
    }
}
