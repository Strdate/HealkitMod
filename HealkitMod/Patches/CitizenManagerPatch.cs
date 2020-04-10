using ColossalFramework.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod.Patches
{
    [HarmonyPatch(typeof(CitizenManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class CitizenManagerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var from = typeof(CitizenAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] { typeof(uint), typeof(Citizen).MakeByRefType() }, null);
            var to = typeof(CitizenManagerPatch).GetMethod("SimulationStep", BindingFlags.Static | BindingFlags.NonPublic);

            var list = codes.ToList();
            HarmonyUtil.ReplaceCalls(list, from, to);
            return list;
        }

        static void SimulationStep(CitizenAI _this,uint citizenID, ref Citizen data)
        {
            try
            {
                _this.SimulationStep(citizenID, ref data);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during CitizenAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nCitizenID: {citizenID}\nType: {_this.GetType().Name}\nSeverity: High";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
                UIView.ForwardException(e2);
            }
        }
    }
}
