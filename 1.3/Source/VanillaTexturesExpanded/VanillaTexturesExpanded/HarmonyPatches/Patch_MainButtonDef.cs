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

    public static class Patch_MainButtonDef
    {

        [HarmonyPatch(typeof(MainButtonDef), nameof(MainButtonDef.Icon), MethodType.Getter)]
        public static class get_Icon
        {

            public static void Postfix(ref Texture2D __result)
            {
                // Don't return an icon if settings are configured that way
                if (!VanillaTexturesExpandedSettings.MainButtonsHaveIcons)
                    __result = null;
            }

        }

    }

}
