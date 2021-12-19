using UnityEngine;
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

        public Texture2D GetTexture
        {
            get
            {
                return this.Props.Texture;
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
