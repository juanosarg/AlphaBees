
using RimWorld;
using Verse;
using Verse.Sound;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Verse;

namespace RimBees
{
    class Building_Beehouse : Building
    {

        public Map map;
      
        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            map = this.Map;
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }
            yield return BeeListSetupUtility.SetBeeListCommand(this,map);
        }

    }
}
