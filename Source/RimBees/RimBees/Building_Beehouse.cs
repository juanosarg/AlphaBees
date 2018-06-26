

using Verse;

using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;


namespace RimBees
{
    class Building_Beehouse : Building, IThingHolder
    {
        //public Thing droneThing;
       // public Thing queenThing;


        protected ThingOwner innerContainerDrones = null;
        protected ThingOwner innerContainerQueens = null;


        protected bool contentsKnown = false;
        protected bool contentsKnownQueens = false;

        public Map map;

        public Building_Beehouse()
        {
            this.innerContainerDrones = new ThingOwner<Thing>(this, false, LookMode.Deep);
            this.innerContainerQueens = new ThingOwner<Thing>(this, false, LookMode.Deep);

        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            map = this.Map;
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }

            if (innerContainerDrones.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetBeeListCommand(this, map);
            } else
            {
                Command_Action RB_Gizmo_Empty_Drones = new Command_Action();
                RB_Gizmo_Empty_Drones.action = delegate
                {
                    this.EjectContents();
                    
                };
                RB_Gizmo_Empty_Drones.defaultLabel = "RB_ExtractBees".Translate();
                RB_Gizmo_Empty_Drones.defaultDesc = "RB_ExtractBeesDesc".Translate();
                RB_Gizmo_Empty_Drones.icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractDrones_FromBeehouse", true);
                yield return RB_Gizmo_Empty_Drones;
            }

            if (innerContainerQueens.NullOrEmpty())
            {
                yield return BeeListSetupUtility.SetQueenListCommand(this, map);
            }
            else
            {
                Command_Action RB_Gizmo_Empty_Queens = new Command_Action();
                RB_Gizmo_Empty_Queens.action = delegate
                {
                    this.EjectContentsQueens();
                    
                };
                RB_Gizmo_Empty_Queens.defaultLabel = "RB_ExtractQueens".Translate();
                RB_Gizmo_Empty_Queens.defaultDesc = "RB_ExtractQueensDesc".Translate();
                RB_Gizmo_Empty_Queens.icon = ContentFinder<Texture2D>.Get("UI/RB_ExtractQueens_FromBeehouse", true);
                yield return RB_Gizmo_Empty_Queens;
            }

        }

        public override string GetInspectString()
        {
            string text = base.GetInspectString();
            string str = "";
            string str2 = "";
            if (!innerContainerDrones.NullOrEmpty())
            {
                str = innerContainerDrones.RandomElement().def.label;
            }
            else { str = "Nothing"; }

            if (!innerContainerQueens.NullOrEmpty())
            {
                str2 = innerContainerQueens.RandomElement().def.label;
            }
            else { str2 = "Nothing"; }


            return text + "CasketContains".Translate() + ": " + str.CapitalizeFirst()
                + "      "+ "CasketContains".Translate() + ": " + str2.CapitalizeFirst();
        }

        public bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            bool result;
           
                bool flag;
                if (thing.holdingOwner != null)
                {
                    thing.holdingOwner.TryTransferToContainer(thing, this.innerContainerDrones, thing.stackCount, true);
                    flag = true;
                }
                else
                {
                    flag = this.innerContainerDrones.TryAdd(thing, true);
                }
                if (flag)
                {
                    if (thing.Faction != null && thing.Faction.IsPlayer)
                    {
                        this.contentsKnown = true;
                    }
                    result = true;
                }
                else
                {
                    result = false;
                }
            
            return result;
        }

        public bool TryAcceptAnyQueen(Thing thing, bool allowSpecialEffects = true)
        {
            bool result;

            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.TryTransferToContainer(thing, this.innerContainerQueens, thing.stackCount, true);
                flag = true;
            }
            else
            {
                flag = this.innerContainerQueens.TryAdd(thing, true);
            }
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    this.contentsKnownQueens = true;
                }
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return this.innerContainerDrones;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public virtual void EjectContents()
        {
            this.innerContainerDrones.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnown = true;
        }

        public virtual void EjectContentsQueens()
        {
            this.innerContainerQueens.TryDropAll(this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null);
            this.contentsKnownQueens = true;
        }


        public override void TickRare()
        {
            base.TickRare();
            if(!innerContainerDrones.NullOrEmpty() && !innerContainerQueens.NullOrEmpty())
            {

                //Log.Message("me siento completo", false);

            }

        }

    }
}
