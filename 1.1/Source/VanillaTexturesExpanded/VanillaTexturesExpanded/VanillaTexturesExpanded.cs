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

    public class VanillaTexturesExpanded : Mod
    {

        public VanillaTexturesExpanded(ModContentPack content) : base(content)
        {
            #if DEBUG
                Log.Error("Somebody left debugging enabled in Vanilla Textures Expanded - please let the team know!");
            #endif

            settings = GetSettings<VanillaTexturesExpandedSettings>();
            harmonyInstance = new Harmony("OskarPotocki.VanillaTexturesExpanded");
        }

        public override string SettingsCategory() => "VanillaTexturesExpanded.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }

        public static VanillaTexturesExpandedSettings settings;
        public static Harmony harmonyInstance;

    }

}
