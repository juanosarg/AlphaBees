using RimWorld;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Building_MeadFermentingBarrel : Building, IThingHolder
    {
        private int wortCount;

        private ThingOwner ingredients;

        private float progressInt;

        private Material barFilledCachedMat;

        public const int MaxCapacity = 25;

        private const int BaseFermentationDuration = 360000;

        public const float MinIdealTemperature = 7f;

        private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

        private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

        private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

        private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);

        public float Progress
        {
            get
            {
                return this.progressInt;
            }
            set
            {
                if (value == this.progressInt)
                {
                    return;
                }
                this.progressInt = value;
                this.barFilledCachedMat = null;
            }
        }

        private Material BarFilledMat
        {
            get
            {
                if (this.barFilledCachedMat == null)
                {
                    this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_MeadFermentingBarrel.BarZeroProgressColor, Building_MeadFermentingBarrel.BarFermentedColor, this.Progress), false);
                }
                return this.barFilledCachedMat;
            }
        }

        public int SpaceLeftForWort
        {
            get
            {
                if (this.Fermented)
                {
                    return 0;
                }
                return 25 - this.wortCount;
            }
        }

        private bool Empty
        {
            get
            {
                return this.wortCount <= 0;
            }
        }

        public bool Fermented
        {
            get
            {
                return !this.Empty && this.Progress >= 1f;
            }
        }

        private float CurrentTempProgressSpeedFactor
        {
            get
            {
                CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
                float ambientTemperature = base.AmbientTemperature;
                if (ambientTemperature < compProperties.minSafeTemperature)
                {
                    return 0.1f;
                }
                if (ambientTemperature < 7f)
                {
                    return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
                }
                return 1f;
            }
        }

        private float ProgressPerTickAtCurrentTemp
        {
            get
            {
                return 2.77777781E-06f * this.CurrentTempProgressSpeedFactor;
            }
        }

        private int EstimatedTicksLeft
        {
            get
            {
                return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
            }
        }

        public Building_MeadFermentingBarrel()
        {
            this.ingredients = new ThingOwner<Thing>(this, false);
        }
        
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
            Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
            Scribe_Deep.Look<ThingOwner>(ref this.ingredients, "ingredients", this);
        }
        
        public override void TickRare()
        {
            base.TickRare();
            if (!this.Empty)
            {
                this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
            }
        }

        protected override void ReceiveCompSignal(string signal)
        {
            if (signal == "RuinedByTemperature")
            {
                this.Reset();
            }
        }

        private void Reset()
        {
            this.wortCount = 0;
            this.Progress = 0f;
            if (!ingredients.NullOrEmpty())
            {
                this.ingredients.ClearAndDestroyContents();
            }
        }

        public void AddWort(Thing wort)
        {
            int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
            if (num > 0)
            {
                if (ingredients == null)
                {
                    ingredients = new ThingOwner<Thing>(this, false);
                }
                num = ingredients.TryAddOrTransfer(wort, 25 - this.wortCount);
                GetComp<CompTemperatureRuinable>().Reset();
                if (Fermented)
                {
                    Log.Warning("Tried to add mead must to a barrel full of mead. Colonists should take the mead first.", false);
                    return;
                }
                Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
                wortCount += num;
            }
        }

        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.GetInspectString());
            if (stringBuilder.Length != 0)
            {
                stringBuilder.AppendLine();
            }
            CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
            if (!this.Empty && !comp.Ruined)
            {
                if (this.Fermented)
                {
                    stringBuilder.AppendLine("RB_ContainsMead".Translate(this.wortCount, 25));
                }
                else
                {
                    stringBuilder.AppendLine("RB_ContainsMeadMust".Translate(this.wortCount, 25));
                }
            }
            if (!this.Empty)
            {
                if (this.Fermented)
                {
                    stringBuilder.AppendLine("Fermented".Translate());
                }
                else
                {
                    stringBuilder.AppendLine("FermentationProgress".Translate(this.Progress.ToStringPercent(), this.EstimatedTicksLeft.ToStringTicksToPeriod()));
                    if (this.CurrentTempProgressSpeedFactor != 1f)
                    {
                        stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(this.CurrentTempProgressSpeedFactor.ToStringPercent()));
                    }
                }
            }
            stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
            stringBuilder.AppendLine(string.Concat(new string[]
            {
                "IdealFermentingTemperature".Translate(),
                ": ",
                7f.ToStringTemperature("F0"),
                " ~ ",
                comp.Props.maxSafeTemperature.ToStringTemperature("F0")
            }));
            return stringBuilder.ToString().TrimEndNewlines();
        }

        public Thing TakeOutBeer()
        {
            if (!this.Fermented)
            {
                Log.Warning("Tried to get mead but it's not yet fermented.", false);
                return null;
            }

            var mead = ThingMaker.MakeThing(ThingDef.Named("RB_Mead"), null);
            var meadIngredients = mead.TryGetComp<CompIngredients>();
            if (!ingredients.NullOrEmpty())
            {
                foreach (var meadMust in ingredients)
                {
                    var mustIngredientDefs = meadMust.TryGetComp<CompIngredients>();
                    mustIngredientDefs?.ingredients.ForEach(meadIngredients.RegisterIngredient);
                }
            }

            mead.stackCount = this.wortCount;
            this.Reset();
            return mead;
        }

        public override void Draw()
        {
            base.Draw();
            if (!this.Empty)
            {
                Vector3 drawPos = this.DrawPos;
                drawPos.y += 0.046875f;
                drawPos.z += 0.25f;
                GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
                {
                    center = drawPos,
                    size = Building_MeadFermentingBarrel.BarSize,
                    fillPercent = (float)this.wortCount / 25f,
                    filledMat = this.BarFilledMat,
                    unfilledMat = Building_MeadFermentingBarrel.BarUnfilledMat,
                    margin = 0.1f,
                    rotation = Rot4.North
                });
            }
        }

        [DebuggerHidden]
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo g in base.GetGizmos())
            {
                yield return g;
            }
            if (Prefs.DevMode && !this.Empty)
            {
                yield return new Command_Action
                {
                    defaultLabel = "Debug: Set progress to 1",
                    action = delegate
                    {
                        this.Progress = 1f;
                    }
                };
            }
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return ingredients;
        }
    }
}
