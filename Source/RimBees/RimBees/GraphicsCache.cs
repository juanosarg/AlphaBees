using System;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {
        public static readonly Texture2D BeeResearchNutTech = ContentFinder<Texture2D>.Get("Research/BeeResearchNutTech", true);
        public static readonly Texture2D BeeResearchNutAdapt = ContentFinder<Texture2D>.Get("Research/BeeResearchNutAdapt", true);
        public static readonly Texture2D BeeResearchAdaptTech = ContentFinder<Texture2D>.Get("Research/BeeResearchAdaptTech", true);
        public static readonly Texture2D BeeResearchStoPsy = ContentFinder<Texture2D>.Get("Research/BeeResearchStoPsy", true);
        public static readonly Texture2D BeeResearchAgriSwamp = ContentFinder<Texture2D>.Get("Research/BeeResearchAgriSwamp", true);
        public static readonly Texture2D BeeResearchPlasAlloy = ContentFinder<Texture2D>.Get("Research/BeeResearchPlasAlloy", true);






    }
}
