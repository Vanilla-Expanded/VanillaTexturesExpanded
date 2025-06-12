using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using RimWorld;
using ModSettingsFramework;
using System.Diagnostics;

namespace VanillaTexturesExpanded
{
    public class VanillaTexturesExpandedSettings : PatchOperationWorker
    {
        public MainButtonRenderMode mainButtonMode = MainButtonRenderMode.IconsAndText;

        public bool MainButtonsHaveIcons => mainButtonMode == MainButtonRenderMode.IconsAndText || mainButtonMode == MainButtonRenderMode.IconsThenText;

        public override void CopyFrom(PatchOperationWorker savedWorker)
        {
            var copy = savedWorker as VanillaTexturesExpandedSettings;
            mainButtonMode = copy.mainButtonMode;
        }

        public override void DoSettings(ModSettingsContainer container, Listing_Standard list)
        {
            var labelRect = list.Label("VanillaTexturesExpanded.MainButtonRenderMode".Translate());
            scrollHeight += labelRect.height;
            var renderModeOpts = Enum.GetValues(typeof(MainButtonRenderMode)).Cast<MainButtonRenderMode>().ToList();
            for (int i = 0; i < renderModeOpts.Count; i++)
            {
                var renderOpt = renderModeOpts[i];
                var prevHeight = list.CurHeight;
                if (list.RadioButton($"VanillaTexturesExpanded.MainButtonRenderMode_{renderOpt}".Translate(), 
                    mainButtonMode == renderOpt, 12))
                {
                    mainButtonMode = renderOpt;
                }
                var height = list.CurHeight - prevHeight;
                scrollHeight += height;
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref mainButtonMode, "mainButtonMode", MainButtonRenderMode.IconsAndText);
        }

        public override void Reset()
        {
            mainButtonMode = MainButtonRenderMode.IconsAndText;
        }
    }

}
