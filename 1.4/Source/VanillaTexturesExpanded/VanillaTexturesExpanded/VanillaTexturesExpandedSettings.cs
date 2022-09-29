using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaTexturesExpanded
{

    public class VanillaTexturesExpandedSettings : ModSettings
    {

        public static MainButtonRenderMode mainButtonMode = MainButtonRenderMode.IconsAndText;

        public static bool MainButtonsHaveIcons => mainButtonMode == MainButtonRenderMode.IconsAndText || mainButtonMode == MainButtonRenderMode.IconsThenText;

        public void DoWindowContents(Rect wrect)
        {
            var options = new Listing_Standard();
            var defaultColor = GUI.color;
            options.Begin(wrect);

            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            options.Gap();

            // Menu button render mode
            options.Label("VanillaTexturesExpanded.MainButtonRenderMode".Translate());
            var renderModeOpts = Enum.GetValues(typeof(MainButtonRenderMode)).Cast<MainButtonRenderMode>().ToList();
            for (int i = 0; i < renderModeOpts.Count; i++)
            {
                var renderOpt = renderModeOpts[i];
                if (options.RadioButton($"VanillaTexturesExpanded.MainButtonRenderMode_{renderOpt}".Translate(), mainButtonMode == renderOpt, 12))
                    mainButtonMode = renderOpt;
            }

            options.End();

            VanillaTexturesExpanded.settings.Write();

        }


        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref mainButtonMode, "mainButtonMode", MainButtonRenderMode.IconsAndText);
        }

    }

}
