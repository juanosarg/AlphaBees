using System.Collections.Generic;
using Verse;

namespace RimBees
{
    public class GameComponent_HyperlinkInitializer : GameComponent
    {
        public GameComponent_HyperlinkInitializer(Game game)
        {
        }

        public override void FinalizeInit()
        {
            var processedCombs = new HashSet<ThingDef>();
            foreach (var species in DefDatabase<BeeSpeciesDef>.AllDefsListForReading)
            {
                AddBeeHyperlinks(species.queen, species.drone, processedCombs);
                AddBeeHyperlinks(species.drone, species.queen, processedCombs);
            }
        }

        private void AddBeeHyperlinks(ThingDef def, ThingDef other, HashSet<ThingDef> processedCombs)
        {
            if (def.descriptionHyperlinks == null)
            {
                def.descriptionHyperlinks = new List<DefHyperlink>();
            }

            def.descriptionHyperlinks.Add(other);

            var comp = def.GetCompProperties<CompProperties_Bees>();
            if (comp != null)
            {
                var combDef = DefDatabase<ThingDef>.GetNamed(comp.comb);
                def.descriptionHyperlinks.Add(combDef);

                var weirdPlantName = comp.weirdplantneeded;
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
            }
        }
    }
}
