using ColossalFramework.UI;
using Harmony;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod.Patches
{
    [HarmonyPatch(typeof(NetManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class NetManagerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var from = typeof(NetAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType() }, null);
            var to = typeof(NetManagerPatch).GetMethod("SimulationStep", BindingFlags.Static | BindingFlags.NonPublic);

            var list = codes.ToList();
            HarmonyUtil.ReplaceCalls(list, from, to);
            return list;
        }

        static void SimulationStep(NetAI _this, ushort segmentID, ref NetSegment data)
        {
            try
            {
                _this.SimulationStep(segmentID, ref data);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during NetAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nSegmentID: {segmentID}\nType: {_this.GetType().Name}\nSeverity: High";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
                UIView.ForwardException(e2);
            }
        }
    }
}
