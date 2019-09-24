using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;


namespace RimBees
{
    public class CompBeeResearch : CompUsable
    {
       
        protected override string FloatMenuOptionLabel
        {
            get
            {
                return string.Format(base.Props.useLabel);
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            foreach (Gizmo c in base.CompGetGizmosExtra())
            {
                yield return c;
            }
            Command_Action BR_Gizmo_DestroyThis = new Command_Action();
            BR_Gizmo_DestroyThis.action = delegate
            {
                this.parent.Destroy();
            };
            BR_Gizmo_DestroyThis.defaultLabel = "RB_DestroyResearch".Translate();
            BR_Gizmo_DestroyThis.defaultDesc = "RB_DestroyResearchLabel".Translate();
            BR_Gizmo_DestroyThis.icon = ContentFinder<Texture2D>.Get("ui/commands/Detonate", true);
            yield return BR_Gizmo_DestroyThis;

        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            
        }

        

        
    }
}
