using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetBeeList : Command
    {
        public Map map;
        public Building_Beehouse beehouse;
        public ThingDef drone;

        public Command_SetBeeList()
        {
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            foreach (BeeSpeciesDef element in DefDatabase<BeeSpeciesDef>.AllDefs)
            {
                if (map.listerThings.ThingsOfDef(element.drone).Count > 0)
                {
                    list.Add(new FloatMenuOption(element.drone.LabelCap, delegate
                    {
                        drone = element.drone;
                        this.TryInsertDrone();
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                }
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(item => item.Label).ToList();
            }
            else
            {
                list.Add(new FloatMenuOption("RB_NoBees".Translate(), null, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertDrone()
        {
            beehouse.BeehouseIsExpectingBees = true;
            beehouse.theDroneIAmGoingToInsert = drone;
        }
    }
}
