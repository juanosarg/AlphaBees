

using Verse;
using UnityEngine;
using RimWorld;

using System;


namespace RimBees
{
    class Building_HybridizationChamber : Building
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;//240;
        public int daysTotal = 3;
        public string hybridizedBee = "";
        public bool hybridizationChamberFull = false;
        private System.Random rand = new System.Random();
        private System.Random beeRandomizer = new System.Random();

        /// <summary>
        /// Returns the graphic of the object.
        /// The renderer will draw the needed object graphic from here.
        /// </summary>
        public override Graphic Graphic
        {
            get
            {
                var customSuffix = "";

                if (hybridizationChamberFull)
                    customSuffix = "_NeedRecharge";
                else if (string.IsNullOrEmpty(HybridizationChecker()))
                    customSuffix = "_WrongBees";
                else if (GetAdjacentBeehouse() == null
                     || !GetAdjacentBeehouse().BeehouseIsRunning)
                    customSuffix = "_Stopped";

                if (string.IsNullOrEmpty(customSuffix))
                    return base.Graphic;

                return GraphicDatabase.Get(def.graphicData.graphicClass,
                    def.graphicData.texPath + customSuffix,
                    def.graphic.Shader,
                    def.graphicData.drawSize,
                    def.graphic.Color,
                    def.graphic.ColorTwo);
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            daysTotal =  rand.Next(1, 4);
        }

        public void RandomizeDays()
        {
            daysTotal = rand.Next(1, 4);
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look<bool>(ref this.hybridizationChamberFull, "hybridizationChamberFull", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look<string>(ref this.hybridizedBee, "hybridizedBee", "", false);

        }


        public Building_Beehouse GetAdjacentBeehouse()
        {
            Building_Beehouse result;
           
                
                IntVec3 c = this.Position+ GenAdj.CardinalDirections[1];
                Building_Beehouse edifice = (Building_Beehouse)c.GetEdifice(base.Map);
                if (edifice != null && ((edifice.def == DefDatabase<ThingDef>.GetNamed("RB_Beehouse", true)) || (edifice.def == DefDatabase<ThingDef>.GetNamed("RB_ClimatizedBeehouse", true)) || (edifice.def == DefDatabase<ThingDef>.GetNamed("RB_AdvancedBeehouse", true))))
                {
                    result = edifice;
                    return result;
                }
            
            result = null;
            return result;
        }

        public override string GetInspectString()
        {

            if (GetAdjacentBeehouse() != null)
            {
                if (GetAdjacentBeehouse().BeehouseIsRunning) {
                    return "GU_AdjacentBeehouseRunningHybridization".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsRunning".Translate();
                } else return "GU_AdjacentBeehouseInactive".Translate() + "\n" + "GU_BroodChamberProgress".Translate() + " " + "GU_HybridMutationsStopped".Translate();

            }
            else return "GU_NoAdjacentBeehouseHybridization".Translate();
        }

        public override void TickRare()
        {
            base.TickRare();

            if (GetAdjacentBeehouse() == null)
                return;

            if (GetAdjacentBeehouse().BeehouseIsRunning && !hybridizationChamberFull)
            {
                tickCounter++;
                if (tickCounter > ((ticksToDays * daysTotal) - 1))
                {
                    hybridizedBee = HybridizationChecker();
                    //Log.Message(hybridizedBee);
                    if (hybridizedBee != "")
                    {
                        if(hybridizedBee=="Neutro" || hybridizedBee == "Nutritious" || hybridizedBee == "Technological"|| hybridizedBee == "Adaptive" || hybridizedBee == "Alloy") {
                            Messages.Message("RB_FiveHybrids".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        }
                        else if (hybridizedBee == "Arctic" || hybridizedBee == "Desert" || hybridizedBee == "Swamp" || hybridizedBee == "Nocturnal" || hybridizedBee == "Argent" || hybridizedBee == "Aurum" || hybridizedBee == "Precious" || hybridizedBee == "Radioactive"
                            || hybridizedBee == "Tipsy" || hybridizedBee == "Luxurious" || hybridizedBee == "Psychic" || hybridizedBee == "Stoner"
                            || hybridizedBee == "Agricultural" || hybridizedBee == "Bittersweet" || hybridizedBee == "Lactic" || hybridizedBee == "Egglaying")
                        {
                            Messages.Message("RB_FourHybrids".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        }
                        else if (hybridizedBee == "Electronic" || hybridizedBee == "Oily" || hybridizedBee == "Plastic")
                        {
                            Messages.Message("RB_ThreeHybrids".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        }
                        else {
                            Messages.Message("RB_Hybrid".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        }
                        SignalHybridizationChamberFull();
                        RandomizeDays();

                    }
                    else {
                        Messages.Message("RB_NoHybrid".Translate(), this, MessageTypeDefOf.NegativeEvent);
                        tickCounter = 0;
                        RandomizeDays();
                    }
                }
            }

        }

        public void SignalHybridizationChamberFull()
        {

            hybridizationChamberFull = true;
        }

        public string HybridizationChecker()
        {
            string beeDrone = "";
            string beeQueen = "";

            if (GetAdjacentBeehouse() == null)
                return string.Empty;

            if ((this.GetAdjacentBeehouse().innerContainerDrones.TotalStackCount>0)&& (this.GetAdjacentBeehouse().innerContainerQueens.TotalStackCount > 0)){
                beeDrone = this.GetAdjacentBeehouse().innerContainerDrones.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
                beeQueen = this.GetAdjacentBeehouse().innerContainerQueens.FirstOrFallback().TryGetComp<CompBees>().GetSpecies;
            }
            

            if (beeDrone == "Temperate")
            {
                
                if (beeQueen == "Mild")
                {
                    return "Hybrid";
                }
                if (beeQueen == "Hybrid")
                {
                    return "Amalgam";
                }
            }

            if (beeDrone == "Mild") {
                if (beeQueen == "Temperate")
                {
                    return "Hybrid";
                }

                if (beeQueen == "Hybrid")
                {
                    return "Amalgam";
                }
            }

            if (beeDrone == "Hybrid")
            {
                if (beeQueen == "Temperate")
                {
                    return "Amalgam";
                }
                if (beeQueen == "Mild")
                {
                    return "Amalgam";
                }
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 6))
                    {
                        case 1:
                            return "Neutro";
                        case 2:
                            return "Nutritious";
                        case 3:
                            return "Technological";
                        case 4:
                            return "Adaptive";
                        case 5:
                            return "Alloy";                   
                        default:
                            break;
                    }
                }

            }

            if (beeDrone == "Amalgam")
            {
               
                if (beeQueen == "Hybrid")
                {
                    switch (beeRandomizer.Next(1, 6))
                    {
                        case 1:
                            return "Neutro";
                        case 2:
                            return "Nutritious";
                        case 3:
                            return "Technological";
                        case 4:
                            return "Adaptive";
                        case 5:
                            return "Alloy";
                        default:
                            break;
                    }
                }

                if (beeQueen == "Adaptive")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Arctic";
                        case 2:
                            return "Desert";
                        case 3:
                            return "Swamp";
                        case 4:
                            return "Nocturnal";
                      
                        default:
                            break;
                    }
                }

                if (beeQueen == "Alloy")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Argent";
                        case 2:
                            return "Aurum";
                        case 3:
                            return "Precious";
                        case 4:
                            return "Radioactive";

                        default:
                            break;
                    }
                }
                if (beeQueen == "Neutro")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Tipsy";
                        case 2:
                            return "Luxurious";
                        case 3:
                            return "Psychic";
                        case 4:
                            return "Stoner";

                        default:
                            break;
                    }
                }
                if (beeQueen == "Nutritious")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Agricultural";
                        case 2:
                            return "Bittersweet";
                        case 3:
                            return "Lactic";
                        case 4:
                            return "Egglaying";

                        default:
                            break;
                    }
                }
                if (beeQueen == "Technological")
                {
                    switch (beeRandomizer.Next(1, 4))
                    {
                        case 1:
                            return "Electronic";
                        case 2:
                            return "Oily";
                        case 3:
                            return "Plastic";


                        default:
                            break;
                    }
                }



            }

            if (beeDrone == "Adaptive")
            {
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Arctic";
                        case 2:
                            return "Desert";
                        case 3:
                            return "Swamp";
                        case 4:
                            return "Nocturnal";

                        default:
                            break;
                    }
                }


            }

            if (beeDrone == "Alloy")
            {
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Argent";
                        case 2:
                            return "Aurum";
                        case 3:
                            return "Precious";
                        case 4:
                            return "Radioactive";

                        default:
                            break;
                    }
                }
                if (beeQueen == "Plastic")
                {
                    return "Synthetic";
                }


            }

            if (beeDrone == "Neutro")
            {
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Tipsy";
                        case 2:
                            return "Luxurious";
                        case 3:
                            return "Psychic";
                        case 4:
                            return "Stoner";

                        default:
                            break;
                    }
                }


            }

            if (beeDrone == "Nutritious")
            {
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 5))
                    {
                        case 1:
                            return "Agricultural";
                        case 2:
                            return "Bittersweet";
                        case 3:
                            return "Lactic";
                        case 4:
                            return "Egglaying";

                        default:
                            break;
                    }
                }
                if (beeQueen == "Technological")
                {
                    return "Culinary";
                }

            }

            if (beeDrone == "Technological")
            {
                if (beeQueen == "Amalgam")
                {
                    switch (beeRandomizer.Next(1, 4))
                    {
                        case 1:
                            return "Electronic";
                        case 2:
                            return "Oily";
                        case 3:
                            return "Plastic";
                       

                        default:
                            break;
                    }
                }
                if (beeQueen == "Nutritious")
                {
                    return "Culinary";
                }
                if (beeQueen == "Medicinal")
                {
                    return "Surgeon";
                }
                if (beeQueen == "Synthetic")
                {
                    return "Hyper";
                }


            }

            if (beeDrone == "Stoner")
            {
                if (beeQueen == "Psychic")
                {
                    return "High";
                }
            }
            if (beeDrone == "Psychic")
            {
                if (beeQueen == "Stoner")
                {
                    return "High";
                }
            }

            if (beeDrone == "High")
            {
                if (beeQueen == "Luxurious")
                {
                    return "Lucifer";
                }
            }
            if (beeDrone == "Luxurious")
            {
                if (beeQueen == "High")
                {
                    return "Lucifer";
                }
            }
            if (beeDrone == "Culinary")
            {
                if (beeQueen == "Agricultural")
                {
                    return "Chef";
                }
            }
            if (beeDrone == "Agricultural")
            {
                if (beeQueen == "Culinary")
                {
                    return "Chef";
                }
                if (beeQueen == "Swamp")
                {
                    return "Medicinal";
                }
            }
            if (beeDrone == "Swamp")
            {
                if (beeQueen == "Agricultural")
                {
                    return "Medicinal";
                }
            }
            if (beeDrone == "Medicinal")
            {
                if (beeQueen == "Technological")
                {
                    return "Surgeon";
                }
            }
            if (beeDrone == "Plastic")
            {
                if (beeQueen == "Alloy")
                {
                    return "Synthetic";
                }
            }
            if (beeDrone == "Synthetic")
            {
                if (beeQueen == "Technological")
                {
                    return "Hyper";
                }
            }


            return "";
         }
    }
}
