

using Verse;
using RimWorld;
using System.Linq;
using Verse.AI;


namespace RimBees
{
    class Building_BroodChamber : Building
    {

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
                return "My nearest beehouse is " + GetAdjacentBeehouse().ToString() + " and it is "+ GetAdjacentBeehouse().BeehouseIsRunning.ToString();
            }
            else return "GU_NoAdjacentBeehouse".Translate();
        }


    }
}
