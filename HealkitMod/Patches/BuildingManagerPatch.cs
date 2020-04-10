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
    [HarmonyPatch(typeof(BuildingManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class BuildingManagerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var from = typeof(BuildingAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] { typeof(ushort), typeof(Building).MakeByRefType() }, null);
            var to = typeof(BuildingManagerPatch).GetMethod("SimulationStep", BindingFlags.Static | BindingFlags.NonPublic);

            var list = codes.ToList();
            HarmonyUtil.ReplaceCalls(list,from,to);
            return list;
        }

        static void SimulationStep(BuildingAI _this, ushort buildingID, ref Building data)
        {
            try
            {
                _this.SimulationStep(buildingID, ref data);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during BuildingAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nBuildingID: {buildingID}\nType: {_this.GetType().Name}\nSeverity: High";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
                UIView.ForwardException(e2);
            }
        }
    }
}
