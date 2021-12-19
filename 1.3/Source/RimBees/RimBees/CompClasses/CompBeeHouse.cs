using Verse;
using System;
using RimWorld;
using UnityEngine;


namespace RimBees
{
    [StaticConstructorOnStartup]
    class CompBeeHouse : ThingComp
    {

        private static readonly Material DroneMat = MaterialPool.MatFrom("Things/Item/Bees/RB_Bee_Temperate_Drone", ShaderDatabase.MetaOverlay);
        private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

        internal static readonly Vector3 ProgressBarOffset = new Vector3(0f, 0.1f, -0.45f);
        internal static readonly Vector2 ProgressBarSize = new Vector2(0.55f, 0.1f);
        internal static readonly Material UnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));
        internal static readonly Material FilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f));

        public CompPowerTrader compPower;

        public CompProperties_BeeHouse Props
        {
            get
            {
                return (CompProperties_BeeHouse)this.props;
            }
        }

        public bool GetIsBeehouse
        {
            get
            {
                return this.Props.isBeehouse;
            }
        }

        public bool GetIsElectricBeehouse
        {
            get
            {
                return this.Props.electricBeehouse;
            }
        }

        public bool GetIsClimatizedBeehouse
        {
            get
            {
                return this.Props.climatizedBeehouse;
            }
        }

        public float GetBeehouseRate
        {
            get
            {
                return this.Props.beehouseRate;
            }
        }

        public ThingOwner GetInnerContainerQueens
        {
            get
            {
                Building_Beehouse beehouse = this.parent as Building_Beehouse;
                return beehouse.innerContainerQueens;
            }
        }

        public ThingOwner GetInnerContainerDrones
        {
            get
            {
                Building_Beehouse beehouse = this.parent as Building_Beehouse;
                return beehouse.innerContainerDrones;
            }
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            compPower = this.parent.TryGetComp<CompPowerTrader>();
            Beehouses_MapComponent mapComp = this.parent.Map.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.AddBeehouseToMap(this.parent);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            Beehouses_MapComponent mapComp = map.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveBeehouseFromMap(this.parent);
            }
        }

        public override void PostDestroy(DestroyMode mode, Map previousMap)
        {

            Beehouses_MapComponent mapComp = previousMap.GetComponent<Beehouses_MapComponent>();
            if (mapComp != null)
            {
                mapComp.RemoveBeehouseFromMap(this.parent);
            }
        }

        public override void PostDrawExtraSelectionOverlays()
        {

            GenDraw.DrawRadiusRing(this.parent.Position, 6f);

        }

        public override void PostDraw()
        {
            if (RimBees_Settings.RB_Ben_ShowMissingBees)
            {
                Building_Beehouse beehouse = this.parent as Building_Beehouse;
                var expectingQueens = beehouse.BeehouseIsExpectingQueens;
                var expectingDrones = beehouse.BeehouseIsExpectingBees;
                var queenContainer = beehouse.innerContainerQueens;
                var droneContainer = beehouse.innerContainerDrones;
                var missingPlant = !beehouse.flagPlants;

                int offset = 0;
                if ((queenContainer.NullOrEmpty() && !expectingQueens) || (droneContainer.NullOrEmpty() && !expectingDrones))
                {
                    RenderMissingFuelOverlay(DroneMat, ref offset);
                }
                else if (compPower != null && !compPower.PowerOn)
                {
                    var matNoPower = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);
                    RenderMissingFuelOverlay(matNoPower, ref offset);
                }
                else if (missingPlant)
                {
                    // only one "weird plant" is needed even if both bees require different plants, so if we got here, both are missing.
                    var queenPlant = queenContainer.FirstOrFallback()?.TryGetComp<CompBees>()?.GetWeirdPlant;
                    var dronePlant = droneContainer.FirstOrFallback()?.TryGetComp<CompBees>()?.GetWeirdPlant;
                    var weirdPlant = queenPlant ?? dronePlant;
                    if (weirdPlant != null)
                    {
                        var mat = MaterialPool.MatFrom(new MaterialRequest(weirdPlant.uiIcon, ShaderDatabase.MetaOverlay));
                        RenderMissingFuelOverlay(mat, ref offset);
                    }
                }
            }

            if (RimBees_Settings.RB_Ben_ShowProgress)
            {
                Building_Beehouse beehouse = this.parent as Building_Beehouse;
                var averageDays = (float)beehouse.CalculateTheTicksAverage();
                if (averageDays > 0f)
                {
                    var progress = beehouse.tickCounter / (beehouse.ticksToDays * averageDays);
                    GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
                    {
                        center = parent.DrawPos + ProgressBarOffset,
                        size = ProgressBarSize,
                        fillPercent = progress,
                        margin = 0.1f,
                        rotation = Rot4.North,
                        filledMat = FilledMat,
                        unfilledMat = UnfilledMat,
                    });
                }
            }

        }
        private void RenderMissingFuelOverlay(Material mat, ref int offset)
        {
            var mesh = MeshPool.plane08;
            var drawPos = parent.TrueCenter();
            drawPos.x += offset * parent.RotatedSize.x * 0.25f;
            drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor(5f);
            offset++;

            var fade = (Mathf.Sin((Time.realtimeSinceStartup + 397f * (parent.thingIDNumber % 571)) * 4f) + 1f) * 0.5f;
            fade = 0.3f + fade * 0.7f;

            var fadedMat1 = FadedMaterialPool.FadedVersionOf(mat, fade);
            Graphics.DrawMesh(mesh, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), fadedMat1, 0);

            var fadedMat2 = FadedMaterialPool.FadedVersionOf(OutOfFuelMat, fade);
            drawPos.y += 3f / 74f;
            Graphics.DrawMesh(mesh, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), fadedMat2, 0);
        }


    }
}

