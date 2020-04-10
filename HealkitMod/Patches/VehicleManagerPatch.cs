using ColossalFramework.UI;
using Harmony;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HealkitMod.Patches
{
    [HarmonyPatch(typeof(VehicleManager))]
    [HarmonyPatch("SimulationStepImpl")]
    class VehicleManagerPatch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codes)
        {
            var from = typeof(VehicleAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
                new Type[] { typeof(ushort), typeof(Vehicle).MakeByRefType(), typeof(Vector3) }, null);
            var to = typeof(VehicleManagerPatch).GetMethod("SimulationStep", BindingFlags.Static | BindingFlags.NonPublic);

            var list = codes.ToList();
            HarmonyUtil.ReplaceCalls(list, from, to);
            return list;
        }

        static void SimulationStep(VehicleAI _this, ushort vehicleID, ref Vehicle data, Vector3 physicsLodRefPos)
        {
            try
            {
                _this.SimulationStep(vehicleID, ref data, physicsLodRefPos);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during VehicleAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nVehicleID: {vehicleID}\nType: {_this.GetType().Name}\nSeverity: High";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                e2.m_supperessMsg = "Suppress similar exceptions caused by this asset";
                UIView.ForwardException(e2);
            }
        }
    }
}
