using ColossalFramework.UI;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HealkitMod
{
    public class VehicleAIPatch
    {
        private static RedirectCallsState _simulationStepState;
        private static MethodInfo _simulationStepOriginal = typeof(VehicleAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { typeof(ushort), typeof(Vehicle).MakeByRefType(), typeof(Vector3) }, null);
        private static MethodInfo _simulationStepPatch = typeof(VehicleAIPatch).GetMethod("SimulationStep", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Patch()
        {
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }

        public static void Unpatch()
        {
            RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
        }

        private static void SimulationStep(VehicleAI _this, ushort vehicleID, ref Vehicle data, Vector3 physicsLodRefPos)
        {
            try
            {
                RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
                _this.SimulationStep(vehicleID, ref data, physicsLodRefPos);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during VehicleAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nVehicleID: {vehicleID}\nType: {_this.GetType().Name}";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                UIView.ForwardException(e2);
            }
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }
    }
}
