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
                def.descriptionHyperlinks.Add(comp.comb);

                if (comp.weirdplantneeded != null)
                {
                    def.descriptionHyperlinks.Add(comp.weirdplantneeded);
                }

                if (processedCombs.Add(comp.comb))
                {
                    if (comp.comb.descriptionHyperlinks == null)
                    {
                        comp.comb.descriptionHyperlinks = new List<DefHyperlink>();
                    }

                    foreach (var product in comp.comb.butcherProducts)
                    {
                        comp.comb.descriptionHyperlinks.Add(product.thingDef);
                    }
                }
            }
        }
    }
}
