using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Dialog_KnownBees : Window
    {
        private static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall");

        private readonly GameComponent_KnownBees knownBees = GameComponent_KnownBees.Instance;

        private readonly string selectBothCached = "BenLubarsRimBeesPatches_KnownBees_SelectBoth".Translate();
        private readonly string unknownResultCached = "BenLubarsRimBeesPatches_KnownBees_UnknownResult".Translate();
        private readonly string noResultCached = "BenLubarsRimBeesPatches_KnownBees_NoResult".Translate();
        private readonly string selectFirstCached = "BenLubarsRimBeesPatches_KnownBees_SelectFirst".Translate();
        private readonly string selectSecondCached = "BenLubarsRimBeesPatches_KnownBees_SelectSecond".Translate();
        private readonly string discoveredTextCached;
        private readonly List<GameComponent_KnownBees.BeeSpeciesData> cachedBees = new List<GameComponent_KnownBees.BeeSpeciesData>();
        private string additionalUndiscovered = null;
        private string selectionError = null;

        private bool debugShowAll = false;
        private Vector2 scrollPosition;
        private int parent1 = -1;
        private int parent2 = -1;

        public Dialog_KnownBees()
        {
            preventCameraMotion = false;
            resizeable = true;

            var totalDiscovered = knownBees.TotalDiscovered;
            discoveredTextCached = "BenLubarsRimBeesPatches_KnownBees_Discovered".Translate(
                totalDiscovered.Named("CUR"),
                knownBees.BeeSpecies.Count.Named("MAX"),
                ((float)totalDiscovered / (float)knownBees.BeeSpecies.Count).ToStringPercent().Named("PERCENT")
            );

            UpdateCachedBees();
        }

        private void UpdateCachedBees()
        {
            cachedBees.Clear();
            additionalUndiscovered = null;
            selectionError = null;

            if (parent1 != -1 && parent2 != -1)
            {
                var species1 = knownBees.BeeSpecies[parent1];
                var species2 = knownBees.BeeSpecies[parent2];
                if (!knownBees.Attempted(species1.Species, species2.Species))
                {
                    selectionError = unknownResultCached;

                    return;
                }

                var undiscovered = knownBees.Undiscovered(species1.Species, species2.Species);
                if (undiscovered != 0)
                {
                    additionalUndiscovered = "BenLubarsRimBeesPatches_KnownBees_Additional".Translate(undiscovered.Named("NUM"));
                }
            }
            else if (parent1 != -1 || parent2 != -1)
            {
                selectionError = selectBothCached;

                return;
            }

            var anyAtThisDepth = true;

            for (int depth = 1; anyAtThisDepth; depth++)
            {
                anyAtThisDepth = false;

                foreach (var species in knownBees.BeeSpecies)
                {
                    if (species.Depth != depth)
                    {
                        continue;
                    }

                    anyAtThisDepth = true;

                    if (SpeciesVisible(species.Species))
                    {
                        cachedBees.Add(species);
                    }
                }
            }

            if (cachedBees.Count == 0)
            {
                selectionError = noResultCached;
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            var prevFont = Text.Font;
            try
            {
                Text.Font = GameFont.Small;

                var dragBar = new Rect(inRect.x, inRect.y, inRect.width - 40f, 20f).ContractedBy(2f);
                GUI.DragWindow(dragBar);
                Widgets.DrawLine(new Vector2(dragBar.x, dragBar.y + dragBar.height * 0.25f), new Vector2(dragBar.xMax, dragBar.y + dragBar.height * 0.25f), Color.gray, 1f);
                Widgets.DrawLine(new Vector2(dragBar.x, dragBar.y + dragBar.height * 0.75f), new Vector2(dragBar.xMax, dragBar.y + dragBar.height * 0.75f), Color.gray, 1f);
                if (Widgets.ButtonImage(new Rect(inRect.xMax - 20f, inRect.y, 20f, 20f).ContractedBy(2f), CloseXSmall))
                {
                    Close();
                }

                inRect.yMin += 20f;

                var buttonRow = new Rect(inRect.x, inRect.y, inRect.width, 32f).ContractedBy(32f, 0f);

                if (Widgets.ButtonText(buttonRow.LeftHalf().ContractedBy(3f), parent1 == -1 ? selectFirstCached : knownBees.BeeSpecies[parent1].QueenName))
                {
                    var options = new List<FloatMenuOption>
                {
                    new FloatMenuOption("NoneLower".Translate(), delegate { parent1 = -1; UpdateCachedBees(); }),
                };

                    options.AddRange(knownBees.BeeSpecies.Where(s => SpeciesVisible(s.Species, false))
                        .Select(s => new FloatMenuOption(s.QueenName, delegate { parent1 = knownBees.BeeSpeciesInv[s.Species]; UpdateCachedBees(); }, s.QueenDef)));

                    Find.WindowStack.Add(new FloatMenu(options));
                }

                if (Widgets.ButtonText(buttonRow.RightHalf().ContractedBy(3f), parent2 == -1 ? selectSecondCached : knownBees.BeeSpecies[parent2].DroneName))
                {
                    var options = new List<FloatMenuOption>
                {
                    new FloatMenuOption("NoneLower".Translate(), delegate { parent2 = -1; UpdateCachedBees(); }),
                };

                    options.AddRange(knownBees.BeeSpecies.Where(s => SpeciesVisible(s.Species, false))
                        .Select(s => new FloatMenuOption(s.DroneName, delegate { parent2 = knownBees.BeeSpeciesInv[s.Species]; UpdateCachedBees(); }, s.DroneDef)));

                    Find.WindowStack.Add(new FloatMenu(options));
                }

                if (parent1 != -1)
                {
                    if (Widgets.ButtonImage(new Rect(inRect.x, inRect.y, 32f, 32f).ContractedBy(4f), knownBees.BeeSpecies[parent1].QueenDef.uiIcon))
                    {
                        new Dialog_InfoCard.Hyperlink(knownBees.BeeSpecies[parent1].QueenDef).ActivateHyperlink();
                    }
                }

                if (parent2 != -1)
                {
                    if (Widgets.ButtonImage(new Rect(inRect.xMax - 32f, inRect.y, 32f, 32f).ContractedBy(4f), knownBees.BeeSpecies[parent2].DroneDef.uiIcon))
                    {
                        new Dialog_InfoCard.Hyperlink(knownBees.BeeSpecies[parent2].DroneDef).ActivateHyperlink();
                    }
                }

                var scrollOutRect = inRect.ContractedBy(3f, 35f);

                if (selectionError != null)
                {
                    var midY = scrollOutRect.y + scrollOutRect.height / 2f;
                    GUI.Label(new Rect(scrollOutRect.x, midY - 16f, scrollOutRect.width, 32f), selectionError);
                }
                else
                {
                    DoBeeList(scrollOutRect);
                }

                Widgets.Label(new Rect(inRect.x, inRect.yMax - 24f, inRect.width, 24f), discoveredTextCached);
            }
            finally
            {
                Text.Font = prevFont;
            }
        }

        private void DoBeeList(Rect scrollOutRect)
        {
            var scrollViewRect = scrollOutRect;
            scrollViewRect.width -= 16f;
            scrollViewRect.height = cachedBees.Count * 32f;

            if (unknownResultCached != null)
            {
                scrollViewRect.height += 32f;
            }

            Widgets.BeginScrollView(scrollOutRect, ref scrollPosition, scrollViewRect);

            var row = new Rect(scrollViewRect.x + 2f, scrollViewRect.y, scrollViewRect.width - 4f, 32f);
            float minY = scrollPosition.y - 32f;
            float maxY = scrollPosition.y + scrollOutRect.height + 32f;

            foreach (var species in cachedBees)
            {
                if (row.y > minY && row.y < maxY)
                {
                    Widgets.HyperlinkWithIcon(row, new Dialog_InfoCard.Hyperlink(species.QueenDef), species.QueenName);
                }

                row.y += 32f;
            }

            if (additionalUndiscovered != null && row.y > minY && row.y < maxY)
            {
                Widgets.Label(row, additionalUndiscovered);
            }

            Widgets.EndScrollView();
        }

        private bool SpeciesVisible(string species, bool checkParents = true)
        {
            if (checkParents && parent1 != -1 && parent2 != -1)
            {
                var species1 = knownBees.BeeSpecies[parent1].Species;
                var species2 = knownBees.BeeSpecies[parent2].Species;

                var data = knownBees.BeeSpecies[knownBees.BeeSpeciesInv[species]];
                if ((data.Parent1 != species1 || data.Parent2 != species2) && (data.Parent1 != species2 || data.Parent2 != species1))
                {
                    return false;
                }
            }

            return debugShowAll || knownBees.Discovered(species);
        }
    }
}
