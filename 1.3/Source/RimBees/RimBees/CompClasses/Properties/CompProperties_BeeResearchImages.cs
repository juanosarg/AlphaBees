using UnityEngine;
using Verse;

namespace RimBees
{
    public class CompProperties_BeeResearchImages : CompProperties
    {
        [NoTranslate]
        public string image;
        [MustTranslate]
        public string textForTheImage;

        [Unsaved(false)]
        private Texture2D texture;
        public Texture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = ContentFinder<Texture2D>.Get(image);
                }

                return texture;
            }
        }

        public CompProperties_BeeResearchImages() : base(typeof(CompBeeResearchImages))
        {
        }
    }
}