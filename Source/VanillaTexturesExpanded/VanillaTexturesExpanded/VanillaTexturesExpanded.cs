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
            
            harmonyInstance = new Harmony("OskarPotocki.VanillaTexturesExpanded");
        }

        public static Harmony harmonyInstance;

    }

}
