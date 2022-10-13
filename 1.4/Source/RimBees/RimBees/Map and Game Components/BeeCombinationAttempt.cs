using System;
using Verse;

namespace RimBees
{
    public class BeeCombinationAttempt : IExposable, IEquatable<BeeCombinationAttempt>
    {
        public string first;
        public string second;
        public string result;

        public void ExposeData()
        {
            Scribe_Values.Look(ref first, "first");
            Scribe_Values.Look(ref second, "second");
            Scribe_Values.Look(ref result, "result");
        }

        public override bool Equals(object obj)
        {
            return obj is BeeCombinationAttempt a && Equals(a);
        }

        public bool Equals(BeeCombinationAttempt other)
        {
            return !(other is null) &&
                ((first == other.first && second == other.second) ||
                (second == other.first && first == other.second)) &&
                result == other.result;
        }

        public override int GetHashCode()
        {
            var hash1 = first?.GetHashCode() ?? 0;
            var hash2 = second?.GetHashCode() ?? 0;
            var hash = Gen.HashOrderless(hash1, hash2);
            hash = Gen.HashCombine(hash, result);

            return hash;
        }
    }
}
