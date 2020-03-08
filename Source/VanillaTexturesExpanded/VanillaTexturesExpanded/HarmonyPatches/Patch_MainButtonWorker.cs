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

    public static class Patch_MainButtonWorker
    {

        [HarmonyPatch(typeof(MainButtonWorker), nameof(MainButtonWorker.DoButton))]
        public static class DoButton
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method, ILGenerator ilGen)
            {
                #if DEBUG
                    Log.Message("Transpiler start: Patch_MainButtonWorker.DoButton (3 matches)");
                #endif

                var instructionList = instructions.ToList();

                var locals = method.GetMethodBody().LocalVariables;
                int? textLocalIndex = null;
                int? vectorLocalIndex = null;

                // Sort out local indexes
                for (int i = 0; i < locals.Count; i++)
                {
                    var local = locals[i];

                    if (local.LocalType == typeof(string) && !textLocalIndex.HasValue)
                        textLocalIndex = local.LocalIndex;
                    else if (local.LocalType == typeof(Vector2) && !vectorLocalIndex.HasValue)
                        vectorLocalIndex = local.LocalIndex + 1; // Returns 4 without modifying for some reason (as of 1.1.2263)
                }

                var adjustedLeftMarginInfo = AccessTools.Method(typeof(DoButton), nameof(AdjustedLeftMargin));
                var noBlankTextInfo = AccessTools.Method(typeof(DoButton), nameof(NoBlankText));
                var adjustedLogoPosInfo = AccessTools.Method(typeof(DoButton), nameof(AdjustedLogoPos));

                bool labelDone = false;
                bool textLeftMarginDone = false;
                bool vectorDone = false;

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    // Look for the first empty string (only 1 by default)
                    if (!labelDone && instruction.opcode == OpCodes.Ldstr && instruction.OperandIs(""))
                    {
                        #if DEBUG
                            Log.Message("Patch_MainButtonWorker.DoButton match 1 of 3");
                        #endif

                        yield return instruction; // ""
                        yield return new CodeInstruction(OpCodes.Ldloc_S, textLocalIndex.Value); // label
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // rect
                        instruction = new CodeInstruction(OpCodes.Call, noBlankTextInfo); // NoBlankText("", label, this, rect)
                        labelDone = true;
                    }

                    // Look for assignment to textLeftMargin local var (local index is 3 as of 1.1.2263)
                    else if (!textLeftMarginDone && instruction.opcode == OpCodes.Stloc_3)
                    {
                        #if DEBUG
                            Log.Message("Patch_MainButtonWorker.DoButton match 2 of 3");
                        #endif

                        yield return instruction; // float textLeftMargin = flag ? 2f : -1f;
                        yield return new CodeInstruction(OpCodes.Ldloc_3); // textLeftMargin
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // rect
                        yield return new CodeInstruction(OpCodes.Call, adjustedLeftMarginInfo); // AdjustedLeftMargin(textLeftMargin, this, rect)
                        instruction = instruction.Clone(); // textLeftMargin = AdjustedLeftMargin(textLeftMargin, this, rect)
                        textLeftMarginDone = true;
                    }


                    // Look for first assignment to vector local var
                    else if (!vectorDone && instruction.opcode == OpCodes.Stloc_S && ((LocalBuilder)instruction.operand).LocalIndex == vectorLocalIndex.Value)
                    {
                        #if DEBUG
                            Log.Message("Patch_MainButtonWorker.DoButton match 3 of 3");
                        #endif

                        yield return instruction; // Vector2 vector = rect.center
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldloc_S, vectorLocalIndex.Value); // vector
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // rect
                        yield return new CodeInstruction(OpCodes.Call, adjustedLogoPosInfo); // AdjustedLogoPos(this, vector, rect)
                        instruction = instruction.Clone(); // vector = AdjustedLogoPos(this, vector, rect)
                        vectorDone = true;
                    }

                    yield return instruction;
                }
            }

            private static float AdjustedLeftMargin(float margin, MainButtonWorker instance, Rect rect)
            {
                // Modify rect if appropriate
                if (VanillaTexturesExpandedUtility.CanDrawIconAndLabel(rect.width, instance.def))
                {
                    margin += VanillaTexturesExpandedUtility.CalculatedMainButtonIconMargin(rect.width) + VanillaTexturesExpandedUtility.MainButtonWorker_IconSize;
                }

                return margin;
            }

            private static string NoBlankText(string blank, string label, MainButtonWorker instance, Rect rect)
            {
                // Unblank label if appropriate
                if (VanillaTexturesExpandedSettings.mainButtonMode == MainButtonRenderMode.TextOnly || VanillaTexturesExpandedUtility.CanDrawIconAndLabel(rect.width, instance.def))
                    return label;

                return blank;
            }

            private static Vector2 AdjustedLogoPos(MainButtonWorker instance, Vector2 logoPos, Rect rect)
            {
                // Move logo to the left if appropriate
                if (VanillaTexturesExpandedUtility.CanDrawIconAndLabel(rect.width, instance.def))
                {
                    logoPos.x = rect.xMin + VanillaTexturesExpandedUtility.CalculatedMainButtonIconMargin(rect.width);
                }

                return logoPos;
            }

        }

    }

}
