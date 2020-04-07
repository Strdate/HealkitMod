using ColossalFramework.UI;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod
{
    public class NetAIPatch
    {
        private static RedirectCallsState _simulationStepState;
        private static MethodInfo _simulationStepOriginal = typeof(NetAI).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { typeof(ushort), typeof(NetSegment).MakeByRefType() }, null);
        private static MethodInfo _simulationStepPatch = typeof(VehicleAIPatch).GetMethod("SimulationStep", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Patch()
        {
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }

        public static void Unpatch()
        {
            RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
        }

        private static void SimulationStep(NetAI _this, ushort segmentID, ref NetSegment data)
        {
            try
            {
                RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
                _this.SimulationStep(segmentID, ref data);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during NetAI simulation step.\nAsset: {_this.m_info.name}" +
                    $"\nSegmentID: {segmentID}\nType: {_this.GetType().Name}";
                HealkitException e2 = new HealkitException(info, e);
                e2.m_uniqueData = _this.m_info.name;
                UIView.ForwardException(e2);
            }
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }
    }
}
