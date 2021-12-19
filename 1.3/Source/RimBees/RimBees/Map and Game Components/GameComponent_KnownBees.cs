using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimBees
{
    public class GameComponent_KnownBees : GameComponent
    {
        public static GameComponent_KnownBees Instance => Current.Game.GetComponent<GameComponent_KnownBees>();

        public readonly Dictionary<BeeSpeciesDef, int> BeeSpeciesInv = new Dictionary<BeeSpeciesDef, int>();
        public readonly List<BeeSpeciesData> BeeSpecies = new List<BeeSpeciesData>();

        public struct BeeSpeciesData
        {
            public BeeSpeciesDef Species;
            public ThingDef QueenDef => Species.queen;
            public ThingDef DroneDef => Species.drone;
            public string QueenName => QueenDef.LabelCap;
            public string DroneName => DroneDef.LabelCap;
            public BeeSpeciesDef Parent1;
            public BeeSpeciesDef Parent2;
            public int Depth;
        }

        public HashSet<BeeCombinationAttempt> attempts = new HashSet<BeeCombinationAttempt>();

        public GameComponent_KnownBees(Game game)
        {
        }

        public override void FinalizeInit()
        {
            BeeSpecies.Clear();
            BeeSpeciesInv.Clear();

            foreach (var species in DefDatabase<BeeSpeciesDef>.AllDefsListForReading)
            {
                BeeSpeciesInv[species] = BeeSpecies.Count;

                BeeSpecies.Add(new BeeSpeciesData
                {
                    Species = species,
                });
            }

            foreach (var combo in DefDatabase<BeeCombinationDef>.AllDefsListForReading)
            {
                foreach (var result in combo.result)
                {
                    var index = BeeSpeciesInv[result];
                    var species = BeeSpecies[index];
                    if (species.Parent1 != null)
                    {
                        if (result == RimBeesDefOf.Amalgam)
                        {
                            continue;
                        }

                        Log.Error($"Did not expect RimBees species {result} to have two paths ({species.Parent1}/{species.Parent2} and {combo.bee1}/{combo.bee2})");
                        continue;
                    }

                    species.Parent1 = combo.bee1;
                    species.Parent2 = combo.bee2;
                    BeeSpecies[index] = species;
                }
            }

            bool assignedDepth = true;
            for (int i = 0; i < BeeSpecies.Count; i++)
            {
                var species = BeeSpecies[i];
                if (species.Parent1 == null)
                {
                    species.Depth = 1;
                    BeeSpecies[i] = species;
                }
            }

            while (assignedDepth)
            {
                assignedDepth = false;

                for (int i = 0; i < BeeSpecies.Count; i++)
                {
                    var species = BeeSpecies[i];
                    if (species.Depth != 0)
                    {
                        continue;
                    }

                    var depth1 = BeeSpecies[BeeSpeciesInv[species.Parent1]].Depth;
                    var depth2 = BeeSpecies[BeeSpeciesInv[species.Parent2]].Depth;
                    if (depth1 != 0 && depth2 != 0)
                    {
                        species.Depth = Math.Max(depth1, depth2) + 1;
                        BeeSpecies[i] = species;
                        assignedDepth = true;
                    }
                }
            }

            foreach (var species in BeeSpecies)
            {
                if (species.Depth == 0)
                {
                    Log.Error($"Could not determine hybridization tree location of RimBees species {species.Species}");
                }
            }
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref attempts, "attempts", LookMode.Deep);
        }

        public void BackfillOwnedBees(Map map)
        {
            foreach (var bee in map.GetComponent<BeeDangerManager_MapComponent>().bees)
            {
                BackfillSpecies(bee);
            }

            foreach (var house in map.GetComponent<Beehouses_MapComponent>().beehouses_InMap)
            {
                if (!house.Faction.IsPlayer)
                {
                    continue;
                }

                var queen = house.TryGetComp<CompBeeHouse>()?.GetInnerContainerQueens?.FirstOrFallback();
                var drone = house.TryGetComp<CompBeeHouse>()?.GetInnerContainerDrones?.FirstOrFallback();

                if (queen != null)
                {
                    BackfillSpecies(queen);
                }

                if (drone != null)
                {
                    BackfillSpecies(drone);
                }
            }
        }

        private void BackfillSpecies(Thing thing)
        {
            var species = thing.TryGetComp<CompBees>().GetSpecies;

            if (!BeeSpeciesInv.TryGetValue(species, out var index))
            {
                return;
            }

            var data = BeeSpecies[index];
            if (data.Parent1 == null)
            {
                return;
            }

            if (species == RimBeesDefOf.Amalgam)
            {
                LogAttempt(RimBeesDefOf.Temperate, RimBeesDefOf.Hybrid, RimBeesDefOf.Amalgam);
                LogAttempt(RimBeesDefOf.Mild, RimBeesDefOf.Hybrid, RimBeesDefOf.Amalgam);
            }
            else
            {
                LogAttempt(data.Parent1, data.Parent2, species);
            }
        }

        public void LogAttempt(BeeSpeciesDef first, BeeSpeciesDef second, BeeSpeciesDef result)
        {
            attempts.Add(new BeeCombinationAttempt
            {
                first = first,
                second = second,
                result = result,
            });
        }

        public int TotalDiscovered => BeeSpecies.Count(s => s.Parent1 == null) + attempts.Select(a => a.result).Where(r => r != null).Distinct().Count();

        public bool Discovered(BeeSpeciesDef species)
        {
            var index = BeeSpeciesInv[species];
            if (BeeSpecies[index].Parent1 == null)
            {
                return true;
            }

            foreach (var attempted in attempts)
            {
                if (attempted.result == species)
                {
                    return true;
                }
            }

            return false;
        }

        public int Undiscovered(BeeSpeciesDef first, BeeSpeciesDef second)
        {
            foreach (var combo in DefDatabase<BeeCombinationDef>.AllDefsListForReading)
            {
                if ((combo.bee1 == first && combo.bee2 == second) || (combo.bee1 == second && combo.bee2 == first))
                {
                    var count = combo.result.Count;

                    foreach (var result in combo.result)
                    {
                        if (attempts.Contains(new BeeCombinationAttempt { first = first, second = second, result = result }))
                        {
                            count--;
                        }
                    }

                    return count;
                }
            }

            return 0;
        }

        public bool Attempted(BeeSpeciesDef first, BeeSpeciesDef second)
        {
            foreach (var attempt in attempts)
            {
                if (attempt.first == first && attempt.second == second)
                {
                    return true;
                }

                if (attempt.first == second && attempt.second == first)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
