using RimWorld;

using Verse;

namespace RimBees
{
    public class CompBeeResearchImages : ThingComp
    {

        private CompProperties_BeeResearchImages Props
        {
            get
            {
                return (CompProperties_BeeResearchImages)this.props;
            }
        }

        public string GetImage
        {
            get
            {
                return this.Props.imageNameInGraphicsCache;
            }
        }

        public string GetText
        {
            get
            {
                return this.Props.textForTheImage;
            }
        }
    }
}
