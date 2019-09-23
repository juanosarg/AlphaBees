using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using Verse;


namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetQueenList : Command
    {

        public Map map;
        public Building beehouse;
        public Thing queen;



        public Command_SetQueenList()
        {

        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            foreach (BeeListDef element in DefDatabase<BeeListDef>.AllDefs)
            {
                if (map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed(element.beeQueenDef, true)).Count > 0)
                {
                    list.Add(new FloatMenuOption(element.beeQueenTag.Translate(), delegate
                    {
                        queen = map.listerThings.ThingsOfDef(DefDatabase<ThingDef>.GetNamed(element.beeQueenDef, true)).RandomElement();
                        this.TryInsertQueen();
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                }

            }

            if (list.Count > 0) {

            } else {
                list.Add(new FloatMenuOption("RB_NoQueens".Translate(), delegate
                {
                   
                }, MenuOptionPriority.Default, null, null, 29f, null, null));
            }
            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertQueen()
        {
            Building_Beehouse buildingbeehouse = (Building_Beehouse)this.beehouse;
            buildingbeehouse.BeehouseIsExpectingQueens = true;
            buildingbeehouse.theQueenIAmGoingToInsert = queen.def.defName;

        
        }

     


    }


}

