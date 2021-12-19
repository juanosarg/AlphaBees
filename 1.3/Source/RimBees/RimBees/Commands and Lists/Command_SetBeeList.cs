using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetBeeList : Command
    {
        public Building_Beehouse beehouse;

        public Command_SetBeeList()
        {
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            var options = new List<FloatMenuOption>();

            foreach (var species in DefDatabase<BeeSpeciesDef>.AllDefs)
            {
                if (beehouse.Map.listerThings.ThingsOfDef(species.drone).Count > 0)
                {
                    options.Add(new FloatMenuOption(species.drone.LabelCap, delegate
                    {
                        this.TryInsertDrone(species.drone);
                    }, extraPartWidth: 29f));
                }
            }

            if (options.Count > 0)
            {
                options.SortBy(item => item.Label);
            }
            else
            {
                options.Add(new FloatMenuOption("RB_NoBees".Translate(), null, extraPartWidth: 29f));
            }

            Find.WindowStack.Add(new FloatMenu(options));
        }

        private void TryInsertDrone(ThingDef drone)
        {
            beehouse.BeehouseIsExpectingBees = true;
            beehouse.theDroneIAmGoingToInsert = drone;
        }
    }
}
