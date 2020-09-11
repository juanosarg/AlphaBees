using System;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public static class GraphicsCache
    {
        // Tier 4 research graphics
        public static readonly Texture2D BeeResearchNutTech = ContentFinder<Texture2D>.Get("Research/BeeResearchNutTech", true);
        public static readonly Texture2D BeeResearchNutAdapt = ContentFinder<Texture2D>.Get("Research/BeeResearchNutAdapt", true);
        public static readonly Texture2D BeeResearchAdaptTech = ContentFinder<Texture2D>.Get("Research/BeeResearchAdaptTech", true);
        public static readonly Texture2D BeeResearchStoPsy = ContentFinder<Texture2D>.Get("Research/BeeResearchStoPsy", true);
        public static readonly Texture2D BeeResearchAgriSwamp = ContentFinder<Texture2D>.Get("Research/BeeResearchAgriSwamp", true);
        public static readonly Texture2D BeeResearchPlasAlloy = ContentFinder<Texture2D>.Get("Research/BeeResearchPlasAlloy", true);
        public static readonly Texture2D BeeResearchAgriWood = ContentFinder<Texture2D>.Get("Research/BeeResearchAgriWood", true);
        // Magical Menagerie research graphics (considered tier 4 since MM doesn't have materials)
        public static readonly Texture2D BeeResearchPsyAur = ContentFinder<Texture2D>.Get("Research/BeeResearchPsyAur", true);
        public static readonly Texture2D BeeResearchPsyLightMagical = ContentFinder<Texture2D>.Get("Research/BeeResearchPsyLightMagical", true);



        // Tier 5 research graphics
        public static readonly Texture2D BeeResearchAgriCul = ContentFinder<Texture2D>.Get("Research/BeeResearchAgriCul", true);
        public static readonly Texture2D BeeResearchLuxHigh = ContentFinder<Texture2D>.Get("Research/BeeResearchLuxHigh", true);
        public static readonly Texture2D BeeResearchMedTech = ContentFinder<Texture2D>.Get("Research/BeeResearchMedTech", true);
        public static readonly Texture2D BeeResearchSynthTech = ContentFinder<Texture2D>.Get("Research/BeeResearchSynthTech", true);
        public static readonly Texture2D BeeResearchSynthOily = ContentFinder<Texture2D>.Get("Research/BeeResearchSynthOily", true);

        // Genetic Tier research graphics
        public static readonly Texture2D BeeResearchOrgWood = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgWood", true);
        public static readonly Texture2D BeeResearchOrgWool = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgWool", true);
        public static readonly Texture2D BeeResearchOrgEgg = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgEgg", true);
        public static readonly Texture2D BeeResearchOrgOily = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgOily", true);
        public static readonly Texture2D BeeResearchOrgPsy = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgPsy", true);
        public static readonly Texture2D BeeResearchOrgPet = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgPet", true);
        public static readonly Texture2D BeeResearchOrgElec = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgElec", true);
        public static readonly Texture2D BeeResearchOrgInsect = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgInsect", true);
        public static readonly Texture2D BeeResearchOrgAgri = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgAgri", true);
        public static readonly Texture2D BeeResearchOrgDesert = ContentFinder<Texture2D>.Get("Research/BeeResearchOrgDesert", true);

        // Pawnmorpher Tier research graphics
        public static readonly Texture2D BeeResearchRadiAdapt = ContentFinder<Texture2D>.Get("Research/BeeResearchRadiAdapt", true);
        public static readonly Texture2D BeeResearchMutaPrec = ContentFinder<Texture2D>.Get("Research/BeeResearchMutaPrec", true);
        public static readonly Texture2D BeeResearchMutaWeave = ContentFinder<Texture2D>.Get("Research/BeeResearchMutaWeave", true);




    }
}
