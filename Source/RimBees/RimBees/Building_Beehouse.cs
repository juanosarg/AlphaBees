

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

        public int tickCounter = 0;
        public bool BeehouseIsFull = false;


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

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainerDrones, "innerContainerDrones", new object[]
            {
                this
            });
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainerQueens, "innerContainerQueens", new object[]
            {
                this
            });
           
            Scribe_Values.Look<bool>(ref this.contentsKnown, "contentsKnown", false, false);
            Scribe_Values.Look<bool>(ref this.contentsKnownQueens, "contentsKnownQueens", false, false);
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter", 0, false);
            Scribe_Values.Look<bool>(ref this.BeehouseIsFull, "BeehouseIsFull", false, false);

        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            if (base.Faction != null && base.Faction.IsPlayer)
            {
                this.contentsKnown = true;
            }
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
            string str3 = "";
            if (!innerContainerDrones.NullOrEmpty())
            {
                str = innerContainerDrones.RandomElement().def.label;
            }
            else { str = "RB_BeehouseNonePresent".Translate(); }

            if (!innerContainerQueens.NullOrEmpty())
            {
                str2 = innerContainerQueens.RandomElement().def.label;
            }
            else { str2 = "RB_BeehouseNonePresent".Translate(); }

            if (!innerContainerQueens.NullOrEmpty()&& !innerContainerQueens.NullOrEmpty())
            {
                str3 = tickCounter.ToString();
            }
            else { str3 = "RB_BeehouseCombNoProgress".Translate(); }


            return text + "RB_BeehouseContainsDrone".Translate() + ": " + str.CapitalizeFirst()
                + "      " + "RB_BeehouseContainsQueen".Translate() + ": " + str2.CapitalizeFirst() + "\n" +
                "RB_BeehouseCombProgress".Translate() + ": "+ str3;
        }

        public bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            bool result;
           
                bool flag;
                if (thing.holdingOwner != null)
                {
                Log.Message(this.innerContainerDrones.ToString(), false);
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
                if (!BeehouseIsFull) { 
                    //Log.Message("me siento completo", false);
                    tickCounter++;
                    if (tickCounter > 10)
                    {
                        SignalBeehouseFull();
                    }
                }

        }

        }

        public void SignalBeehouseFull()
        {
            BeehouseIsFull = true;
        }

    }
}
