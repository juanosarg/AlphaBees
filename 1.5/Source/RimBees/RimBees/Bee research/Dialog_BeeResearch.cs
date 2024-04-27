using Verse;
using RimWorld;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Dialog_BeeResearch: Window
    {
        public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);
      
   
        public ThingDef firstBee;
        public ThingDef secondBee;
        public List<ThingDef> resultBees;

        public string firstSpecies;
        public string secondSpecies;
        public List<string> resultSpecies = new List<string>();


        public Dialog_BeeResearch(ThingDef firstBee, ThingDef secondBee, List<ThingDef> resultBees)
        {

            this.firstBee = firstBee;
            this.secondBee = secondBee;
            this.resultBees = resultBees;

            firstSpecies = firstBee.GetCompProperties<CompProperties_Bees>()?.species;
            secondSpecies = secondBee.GetCompProperties<CompProperties_Bees>()?.species;
            foreach(ThingDef bee in resultBees)
            {
                resultSpecies.Add(bee.GetCompProperties<CompProperties_Bees>()?.species);
            }

            draggable = true;
        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
          
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect position = new Rect(inRect.x, inRect.y, inRect.width - 40f, 20f).ContractedBy(2f);
            GUI.DragWindow(position);
            if (Widgets.ButtonImage(new Rect(inRect.xMax - 20f, inRect.y, 20f, 20f).ContractedBy(2f), Dialog_BeeResearch.CloseXSmall))
            {
                this.Close(true);
            }
            if(resultBees.Count == 1)
            {
                Widgets.DrawTextureFitted(inRect.ContractedBy(3f), GraphicsCache.BeeResearchBG, 1f);
            }
            else
            {
                Widgets.DrawTextureFitted(inRect.ContractedBy(3f), GraphicsCache.BeeResearchBGMulti, 1f);
            }


            Rect rectIconFirstQueen = new Rect(90, 58, 57f, 57f);
            GUI.DrawTexture(rectIconFirstQueen, firstBee.graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);         
            TooltipHandler.TipRegion(rectIconFirstQueen, firstBee.LabelCap);
            ThingDef firstDrone = DefDatabase<ThingDef>.GetNamedSilentFail(Utils.getDroneFromQueen(firstBee));
            if (firstDrone != null)
            {
                Rect rectIconFirstDrone = new Rect(120, 145, 57f, 57f);
                GUI.DrawTexture(rectIconFirstDrone, firstDrone.graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                TooltipHandler.TipRegion(rectIconFirstDrone, firstDrone.LabelCap);
            }

            Rect rectIconSecondQueen = new Rect(320, 58, 57f, 57f);
            GUI.DrawTexture(rectIconSecondQueen,secondBee.graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
            TooltipHandler.TipRegion(rectIconSecondQueen, secondBee.LabelCap);
            ThingDef secondDrone = DefDatabase<ThingDef>.GetNamedSilentFail(Utils.getDroneFromQueen(secondBee));
            if (secondDrone != null)
            {
                Rect rectIconSecondDrone = new Rect(290, 145, 57f, 57f);
                GUI.DrawTexture(rectIconSecondDrone, secondDrone.graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                TooltipHandler.TipRegion(rectIconSecondDrone, secondDrone.LabelCap);
            }

            string descriptionText ="";

            if (resultBees.Count == 1)
            {
                Rect rectIconResultQueen = new Rect(135, 350, 57f, 57f);
                GUI.DrawTexture(rectIconResultQueen, resultBees[0].graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                TooltipHandler.TipRegion(rectIconResultQueen, resultBees[0].LabelCap);
                ThingDef resultDrone = DefDatabase<ThingDef>.GetNamedSilentFail(Utils.getDroneFromQueen(resultBees[0]));
                if (resultDrone != null)
                {
                    Rect rectIconResultDrone = new Rect(270, 350, 57f, 57f);
                    GUI.DrawTexture(rectIconResultDrone, resultDrone.graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                    TooltipHandler.TipRegion(rectIconResultDrone, resultDrone.LabelCap);
                }

                descriptionText = firstSpecies + " + " + secondSpecies + " = " + resultSpecies[0];

            }
            else
            {
                descriptionText = firstSpecies + " + " + secondSpecies + " = ";
                for(int i = 0; i < resultBees.Count; i++)
                {
                    Rect rectIconResultQueen = new Rect(135+57*i, 350, 57f, 57f);
                    GUI.DrawTexture(rectIconResultQueen, resultBees[i].graphic.MatSingle.mainTexture, ScaleMode.StretchToFill, alphaBlend: true, 0f, Color.white, 0f, 0f);
                    TooltipHandler.TipRegion(rectIconResultQueen, resultBees[i].LabelCap);
                    if (i!= resultBees.Count-1)
                    {
                        descriptionText += resultSpecies[i] + " - ";
                    }
                    else
                    {
                        descriptionText += resultSpecies[i];
                    }
                }
            }
            Widgets.Label(new Rect(inRect.x, inRect.y+445, inRect.width, 30), descriptionText);
        }

       
    }

}
