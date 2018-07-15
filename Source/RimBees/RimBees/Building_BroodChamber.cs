

using Verse;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;



namespace RimBees
{
    class Building_BroodChamber : Building, IThingHolder
    {

        public int tickCounter = 0;
        public int ticksToDays = 240;

        public ThingOwner innerContainerBees = null;
        protected bool contentsKnown = false;

        public Building_BroodChamber()
        {
            this.innerContainerBees = new ThingOwner<Thing>(this, false, LookMode.Deep);

        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainerBees, "innerContainerDrones", new object[]
            {
                this
            });           
            Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (base.Faction != null && base.Faction.IsPlayer)
            {
                this.contentsKnown = true;
            }
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.innerContainerBees;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public virtual void EjectContents()
        {
            this.innerContainerBees.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnown = true;
        }

        public Building_Beehouse GetAdjacentBeehouse()
        {
            Building_Beehouse result;
           
                
                IntVec3 c = this.Position+ GenAdj.CardinalDirections[3];
                Building_Beehouse edifice = (Building_Beehouse)c.GetEdifice(base.Map);
                if (edifice != null && edifice.def == DefDatabase<ThingDef>.GetNamed("RB_Beehouse", true))
                {
                    result = edifice;
                    return result;
                }
            
            result = null;
            return result;
        }

        public override string GetInspectString()
        {

            if (GetAdjacentBeehouse() != null)
            {
                if (GetAdjacentBeehouse().BeehouseIsRunning) {
                    return "GU_AdjacentBeehouseRunning".Translate();

                } else return "GU_AdjacentBeehouseInactive".Translate();

            }
            else return "GU_NoAdjacentBeehouse".Translate();
        }


    }
}
