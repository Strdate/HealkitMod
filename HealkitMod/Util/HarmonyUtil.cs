using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace HealkitMod
{
    public static class HarmonyUtil
    {
        public static int ReplaceCalls(List<CodeInstruction> codes, MethodInfo from, MethodInfo to)
        {
            bool isFromVirtual = from.IsVirtual;
            bool isToVirtual = to.IsVirtual;
            int count = 0;
            for (var index = 0; index < codes.Count; index++)
            {
                if ((codes[index].opcode == OpCodes.Callvirt && isFromVirtual) || (codes[index].opcode == OpCodes.Call && !isFromVirtual))
                {
                    if (codes[index].operand == from)
                    {
                        codes[index] = codes[index].Clone(isToVirtual ? OpCodes.Callvirt : OpCodes.Call, to );
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
