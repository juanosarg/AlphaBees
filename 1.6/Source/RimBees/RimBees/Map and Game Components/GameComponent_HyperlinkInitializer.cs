using System;
using System.Collections.Generic;
using System.Linq;

using Verse;

namespace RimBees
{
    class GameComponent_HyperlinkInitializer: GameComponent
    {

        internal static readonly Type CompProperties_Bees = Type.GetType("RimBees.CompProperties_Bees,RimBees");

        public GameComponent_HyperlinkInitializer(Game game)
        {
        }

        public override void FinalizeInit()
        {
            var processedCombs = new HashSet<ThingDef>();
            foreach (var list in DefDatabase<BeeListDef>.AllDefsListForReading)
            {
                var queenDef = DefDatabase<ThingDef>.GetNamed(list.beeQueenDef);
                var droneDef = DefDatabase<ThingDef>.GetNamed(list.beeDroneDef);
                AddBeeHyperlinks(queenDef, droneDef, processedCombs);
                AddBeeHyperlinks(droneDef, queenDef, processedCombs);
            }
        }

        private void AddBeeHyperlinks(ThingDef def, ThingDef other, HashSet<ThingDef> processedCombs)
        {
            if (def.descriptionHyperlinks == null)
            {
                def.descriptionHyperlinks = new List<DefHyperlink>();
            }

            def.descriptionHyperlinks.Add(other);

            foreach (var comp in def.comps)
            {
                if (CompProperties_Bees.IsInstanceOfType(comp))
                {
                    var combDef = DefDatabase<ThingDef>.GetNamed(BenLubarsRimBeesPatches.comb(comp));
                    def.descriptionHyperlinks.Add(combDef);

                    var weirdPlantName = BenLubarsRimBeesPatches.weirdplantneeded(comp);
                    if (weirdPlantName != "no")
                    {
                        def.descriptionHyperlinks.Add(DefDatabase<ThingDef>.GetNamed(weirdPlantName));
                    }

                    if (processedCombs.Add(combDef))
                    {
                        if (combDef.descriptionHyperlinks == null)
                        {
                            combDef.descriptionHyperlinks = new List<DefHyperlink>();
                        }

                        foreach (var product in combDef.butcherProducts)
                        {
                            combDef.descriptionHyperlinks.Add(product.thingDef);
                        }
                    }

                    break;
                }
            }
        }

    }
}
