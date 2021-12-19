using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Command_SetQueenList : Command
    {
        public Building_Beehouse beehouse;

        public Command_SetQueenList()
        {
        }

        public override void ProcessInput(Event ev)
        {
            base.ProcessInput(ev);

            var options = new List<FloatMenuOption>();

            foreach (var species in DefDatabase<BeeSpeciesDef>.AllDefs)
            {
                if (beehouse.Map.listerThings.ThingsOfDef(species.queen).Count > 0)
                {
                    options.Add(new FloatMenuOption(species.queen.LabelCap, delegate
                    {
                        this.TryInsertQueen(species.queen);
                    }, extraPartWidth: 29f));
                }
            }

            if (options.Count > 0)
            {
                options.SortBy(item => item.Label);
            }
            else
            {
                options.Add(new FloatMenuOption("RB_NoQueens".Translate(), null, extraPartWidth: 29f));
            }

            Find.WindowStack.Add(new FloatMenu(options));
        }

        private void TryInsertQueen(ThingDef queen)
        {
            beehouse.BeehouseIsExpectingQueens = true;
            beehouse.theQueenIAmGoingToInsert = queen;
        }
    }
}
