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

    public static class VanillaTexturesExpandedUtility
    {

        public static bool CanDrawIconAndLabel(this MainButtonDef def) => !def.minimized && def.Icon != null && !def.label.NullOrEmpty();

    }

}
