using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class CompBeeResearchTable : ThingComp
    {
        private static readonly Texture2D GizmoIcon = ContentFinder<Texture2D>.Get("Achievements/RB_AchievementPursuingTenure");

        private static string labelCached;
        private static string descCached;

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            if (RimBees_Settings.RB_Ben_KnownBees)
            {
                if (labelCached == null)
                {
                    labelCached = "BenLubarsRimBeesPatches_ResearchButton_title".Translate();
                    descCached = "BenLubarsRimBeesPatches_ResearchButton_desc".Translate();
                }

                yield return new Command_Action
                {
                    defaultLabel = labelCached,
                    defaultDesc = descCached,
                    tutorTag = "KnownBees",
                    hotKey = KeyBindingDefOf.Misc1,
                    icon = GizmoIcon,
                    action = delegate
                    {
                        GameComponent_KnownBees.Instance.BackfillOwnedBees(this.parent.Map);

                        Find.WindowStack.Add(new Dialog_KnownBees());
                    },
                };
            }
        }
    }
}
