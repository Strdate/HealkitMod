using ColossalFramework.UI;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod
{
    public class BuildingAIPatch
    {
        private static RedirectCallsState _simulationStepState;
        private static MethodInfo _simulationStepOriginal = typeof(BuildingAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { typeof(ushort), typeof(Building).MakeByRefType() }, null);
        private static MethodInfo _simulationStepPatch = typeof(BuildingAIPatch).GetMethod("SimulationStep", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Patch()
        {
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }

        public static void Unpatch()
        {
            RedirectionHelper.RevertRedirect(_simulationStepOriginal,_simulationStepState);
        }

        private static void SimulationStep(BuildingAI _this, ushort buildingID, ref Building data)
        {
            try
            {
                RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
                _this.SimulationStep(buildingID, ref data);
            }
            catch(Exception e)
            {
                string info = $"An exception occured during BuildingAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nBuildingID: {buildingID}\nType: {_this.GetType().Name}";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                UIView.ForwardException(e2);
            }
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }
    }
}
