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
    [HarmonyPatch(typeof(TransportManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class TransportManagerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var from = typeof(TransportLine).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] { typeof(ushort) }, null);
            var to = typeof(TransportManagerPatch).GetMethod("SimulationStep", BindingFlags.Static | BindingFlags.NonPublic);

            var list = codes.ToList();
            HarmonyUtil.ReplaceCalls(list, from, to, true);
            return list;
        }

        static void SimulationStep(ref TransportLine _this, ushort lineID)
        {
            try
            {
                _this.SimulationStep(lineID);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during TransportLine simulation step.\n" +
                    $"Line: {Enum.GetName(typeof(TransportInfo.TransportType), _this.Info.m_transportType)} {_this.m_lineNumber}\n" +
                    $"LineID: {lineID}\nSeverity: High";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_supperessMsg = "Suppress this exception";
                UIView.ForwardException(e2);
            }
        }
    }
}
