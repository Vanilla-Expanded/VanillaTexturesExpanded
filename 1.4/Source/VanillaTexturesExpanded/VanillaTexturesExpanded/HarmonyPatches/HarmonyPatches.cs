using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace VanillaTexturesExpanded
{

    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {

        static HarmonyPatches()
        {
            VanillaTexturesExpanded.harmonyInstance.PatchAll();
            var field = typeof(OverlayDrawer).GetField("NeedsPowerMat", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay));
        }

    }

}
