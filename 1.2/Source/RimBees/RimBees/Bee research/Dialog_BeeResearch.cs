using Verse;
using RimWorld;
using System.Text;
using UnityEngine;

namespace RimBees
{
    [StaticConstructorOnStartup]
    public class Dialog_BeeResearch: Window
    {
        public static readonly Texture2D CloseXSmall = ContentFinder<Texture2D>.Get("UI/Widgets/CloseXSmall", true);
        public Texture2D theImage;
        public string theText;


        public Dialog_BeeResearch(string BeePicture, string BeeText)
        {
           
            theImage = (Texture2D)typeof(GraphicsCache).GetField(BeePicture).GetValue(theImage);
            theText = BeeText;

        }

        public override void Close(bool doCloseSound = true)
        {
            base.Close(doCloseSound);
          
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect position = new Rect(inRect.x, inRect.y, inRect.width - 40f, 20f).ContractedBy(2f);
            GUI.DragWindow(position);
            Widgets.DrawLine(new Vector2(position.x, position.y + position.height * 0.25f), new Vector2(position.xMax, position.y + position.height * 0.25f), Color.gray, 1f);
            Widgets.DrawLine(new Vector2(position.x, position.y + position.height * 0.75f), new Vector2(position.xMax, position.y + position.height * 0.75f), Color.gray, 1f);
            if (Widgets.ButtonImage(new Rect(inRect.xMax - 20f, inRect.y, 20f, 20f).ContractedBy(2f), Dialog_BeeResearch.CloseXSmall))
            {
                this.Close(true);
            }
            Widgets.DrawTextureFitted(inRect.ContractedBy(3f), theImage, 1f);
            //Text.Font = GameFont.Tiny;
            //Text.Anchor = TextAnchor.MiddleCenter;
           
            Widgets.Label(new Rect(inRect.x, inRect.y+445, inRect.width, 30), theText);
        }

       
    }

}
