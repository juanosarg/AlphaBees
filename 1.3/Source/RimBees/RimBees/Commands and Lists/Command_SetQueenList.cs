using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetQueenList : Command
    {
        public Map map;
        public Building_Beehouse beehouse;
        public ThingDef queen;

        public Command_SetQueenList()
        {

        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);
            List<FloatMenuOption> list = new List<FloatMenuOption>();

            foreach (BeeSpeciesDef element in DefDatabase<BeeSpeciesDef>.AllDefs)
            {
                if (map.listerThings.ThingsOfDef(element.queen).Count > 0)
                {
                    list.Add(new FloatMenuOption(element.queen.LabelCap, delegate
                    {
                        queen = element.queen;
                        this.TryInsertQueen();
                    }, MenuOptionPriority.Default, null, null, 29f, null, null));
                }

            }

            if (list.Count > 0)
            {
                list = list.OrderBy(item => item.Label).ToList();
            }
            else
            {
                list.Add(new FloatMenuOption("RB_NoQueens".Translate(), null, MenuOptionPriority.Default, null, null, 29f, null, null));
            }

            Find.WindowStack.Add(new FloatMenu(list));
        }

        private void TryInsertQueen()
        {
            beehouse.BeehouseIsExpectingQueens = true;
            beehouse.theQueenIAmGoingToInsert = queen;
        }
    }
}
