using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace HealkitMod
{
    public static class HarmonyUtil
    {
        public static int ReplaceCalls(List<CodeInstruction> codes, MethodInfo from, MethodInfo to, bool debug = false)
        {
            string debugStr = "";
            debugStr += "ReplaceCalls start\n";
            //bool isFromVirtual = from.IsVirtual;
            bool isToVirtual = !to.IsStatic;
            int count = 0;
            for (var index = 0; index < codes.Count; index++)
            {
                debugStr +=  codes[index] + "\n";
                if (codes[index].opcode == OpCodes.Callvirt  || codes[index].opcode == OpCodes.Call)
                {
                    if (codes[index].operand == from)
                    {
                        var newIns = new CodeInstruction(codes[index])
                        {
                            opcode = isToVirtual ? OpCodes.Callvirt : OpCodes.Call,
                            operand = to,
                        };
                        debugStr += "Replacing with " + newIns + "\n";
                        codes[index] = newIns;
                        count++;
                    }
                }
            }
            if (debug) Debug.Log(debugStr);
            return count;
        }
    }
}
