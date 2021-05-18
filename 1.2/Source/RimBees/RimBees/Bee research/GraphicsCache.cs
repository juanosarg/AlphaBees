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
        public static readonly Texture2D BeeResearchElectronicSynthetic = ContentFinder<Texture2D>.Get("Research/BeeResearchElectronicSynthetic", true);


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

        // Alpha Animals Tier research graphics
        public static readonly Texture2D BeeResearchAgriElec = ContentFinder<Texture2D>.Get("Research/BeeResearchAgriElec", true);
        public static readonly Texture2D BeeResearchAlloyPrec = ContentFinder<Texture2D>.Get("Research/BeeResearchAlloyPrec", true);
        public static readonly Texture2D BeeResearchArcticLactic = ContentFinder<Texture2D>.Get("Research/BeeResearchArcticLactic", true);
        public static readonly Texture2D BeeResearchInsectoidDesert = ContentFinder<Texture2D>.Get("Research/BeeResearchInsectoidDesert", true);
        public static readonly Texture2D BeeResearchNeutroWood = ContentFinder<Texture2D>.Get("Research/BeeResearchNeutroWood", true);
        public static readonly Texture2D BeeResearchNoctInsectoid = ContentFinder<Texture2D>.Get("Research/BeeResearchNoctInsectoid", true);
        public static readonly Texture2D BeeResearchNoctTanner = ContentFinder<Texture2D>.Get("Research/BeeResearchNoctTanner", true);
        public static readonly Texture2D BeeResearchRadiWood = ContentFinder<Texture2D>.Get("Research/BeeResearchRadiWood", true);
        public static readonly Texture2D BeeResearchSwampDesert = ContentFinder<Texture2D>.Get("Research/BeeResearchSwampDesert", true);
        public static readonly Texture2D BeeResearchSwampNutri = ContentFinder<Texture2D>.Get("Research/BeeResearchSwampNutri", true);
        public static readonly Texture2D BeeResearchSwampWeaving = ContentFinder<Texture2D>.Get("Research/BeeResearchSwampWeaving", true);
        public static readonly Texture2D BeeResearchWoolyTanner = ContentFinder<Texture2D>.Get("Research/BeeResearchWoolyTanner", true);

        // Rim-Effect Tier research graphics
        public static readonly Texture2D BeeResearchElectronicPlastic = ContentFinder<Texture2D>.Get("Research/BeeResearchElectronicPlastic", true);
        public static readonly Texture2D BeeResearchPrefabSynthetic = ContentFinder<Texture2D>.Get("Research/BeeResearchPrefabSynthetic", true);

        // Dice of Destiny Tier research graphics
        public static readonly Texture2D BeeResearchAdaptiveAurum = ContentFinder<Texture2D>.Get("Research/BeeResearchAdaptiveAurum", true);
        public static readonly Texture2D BeeResearchCasinoOily = ContentFinder<Texture2D>.Get("Research/BeeResearchCasinoOily", true);
        public static readonly Texture2D BeeResearchCasinoPrecious = ContentFinder<Texture2D>.Get("Research/BeeResearchCasinoPrecious", true);

        // More Archotech Garbage Tier research graphics
        public static readonly Texture2D BeeResearchFabricatedHyper = ContentFinder<Texture2D>.Get("Research/BeeResearchFabricatedHyper", true);



    }
}
