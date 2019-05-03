

using Verse;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;



namespace RimBees
{
    class Building_BroodChamber : Building
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;
        public int daysTotal = 3;
        public bool broodChamberFull = false;

        private Graphic BroodChamberIsFullGraphic = null;

        /// <summary>
        /// Returns the graphic of the object.
        /// The renderer will draw the needed object graphic from here.
        /// </summary>
        public override Graphic Graphic
        {
            get
            {
                if (!broodChamberFull)
                    return base.Graphic;

                if (BroodChamberIsFullGraphic == null)
                {
                    string graphicRealPath = def.graphicData.texPath + "_NeedRecharge";
                    BroodChamberIsFullGraphic = GraphicDatabase.Get(def.graphicData.graphicClass,
                        graphicRealPath,
                        def.graphic.Shader,
                        def.graphicData.drawSize,
                        def.graphic.Color,
                        def.graphic.ColorTwo);
                }

                return BroodChamberIsFullGraphic;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
       
            Scribe_Values.Look<bool>(ref this.broodChamberFull, "broodChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
        }

       

        public Building_Beehouse GetAdjacentBeehouse()
        {
            Building_Beehouse result;
           
                
                IntVec3 c = this.Position+ GenAdj.CardinalDirections[3];
                Building_Beehouse edifice = (Building_Beehouse)c.GetEdifice(base.Map);
                if (edifice != null && ((edifice.def == DefDatabase<ThingDef>.GetNamed("RB_Beehouse", true))|| (edifice.def == DefDatabase<ThingDef>.GetNamed("RB_ClimatizedBeehouse", true)) || (edifice.def == DefDatabase<ThingDef>.GetNamed("RB_AdvancedBeehouse", true))))
                {
                    result = edifice;
                    return result;
                }
            
            result = null;
            return result;
        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();

            if (GetAdjacentBeehouse() != null)
            {
                string strPercentProgress = ((float)tickCounter / ((ticksToDays) * daysTotal)).ToStringPercent();

                if (GetAdjacentBeehouse().BeehouseIsRunning) {

                    return text + "GU_AdjacentBeehouseRunning".Translate() + "\n" + "GU_BroodChamberProgress".Translate()+" "+ strPercentProgress;

                } else return text + "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + strPercentProgress +" (stopped)";

            }
            else return text+"GU_NoAdjacentBeehouse".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();
            if (GetAdjacentBeehouse().BeehouseIsRunning && !broodChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    SignalBroodChamberFull();
                }
            }

        }

        public void SignalBroodChamberFull()
        {

            broodChamberFull = true;
        }


    }
}
