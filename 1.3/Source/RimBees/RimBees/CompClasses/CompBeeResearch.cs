using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace RimBees
{
    public class CompBeeResearch : CompUsable
    {
        protected override string FloatMenuOptionLabel(Pawn pawn)
        {
            return this.Props.useLabel;
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (var gizmo in base.CompGetGizmosExtra())
            {
                yield return gizmo;
            }

            yield return new Command_Action
            {
                action = delegate
                {
                    this.parent.Destroy();
                },
                defaultLabel = "RB_DestroyResearch".Translate(),
                defaultDesc = "RB_DestroyResearchLabel".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Commands/Detonate", true)
            };
        }
    }
}
