using System;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {

        public static readonly Texture2D BeeResearchBG = ContentFinder<Texture2D>.Get("Research/RB_BeeResearchBackground", true);
        public static readonly Texture2D BeeResearchBGMulti = ContentFinder<Texture2D>.Get("Research/RB_BeeResearchBackgroundMulti", true);

        


    }
}
