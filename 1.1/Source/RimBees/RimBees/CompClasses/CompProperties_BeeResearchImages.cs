using RimWorld;

using Verse;

namespace RimBees
{
    public class CompProperties_BeeResearchImages : CompProperties
    {
        public string imageNameInGraphicsCache;
        public string textForTheImage;

        public CompProperties_BeeResearchImages()
        {
            this.compClass = typeof(CompBeeResearchImages);
        }
    }
}