using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;


namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetBeeList : Command
    {

        public Map map;
        public Building beehouse;
        public Thing drone;

       

        public Command_SetBeeList()
        {
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            foreach (BeeListDef element in DefDatabase<BeeListDef>.AllDefs)
            {
                if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed(element.beeDroneDef, true)).Count > 0)
                {
                    list.Add(new FloatMenuOption(element.beeDroneTag.Translate(), delegate
                    {
                        drone = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed(element.beeDroneDef, true)).RandomElement();
                        this.TryInsertDrone();
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                }

            }



            if (list.Count > 0) {

            } else {
                list.Add(new FloatMenuOption("RB_NoBees".Translate(), delegate
                {
                   
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertDrone()
        {
            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.beehouse;
            buildingbeehouse.BeehouseIsExpectingBees = true;
            buildingbeehouse.theDroneIAmGoingToInsert = drone.def.defName;

  
        }

    


    }


}

