using ColossalFramework.UI;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod
{
    public class TransportLinePatch
    {
        private static RedirectCallsState _simulationStepState;
        private static MethodInfo _simulationStepOriginal = typeof(TransportLine).GetMethod("SimulationStep", BindingFlags.Public | BindingFlags.Instance, null,
            new Type[] { typeof(ushort) }, null);
        private static MethodInfo _simulationStepPatch = typeof(TransportLinePatch).GetMethod("SimulationStep", BindingFlags.NonPublic | BindingFlags.Static);

        public static void Patch()
        {
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }

        public static void Unpatch()
        {
            RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
        }

        private static void SimulationStep(TransportLine _this, ushort lineID)
        {
            try
            {
                RedirectionHelper.RevertRedirect(_simulationStepOriginal, _simulationStepState);
                _this.SimulationStep(lineID);
            }
            catch (Exception e)
            {
                string info = $"An exception occured during TransportLine simulation step.\n" +
                    $"Line: {Enum.GetName(typeof(TransportInfo.TransportType), _this.Info.m_transportType)} {_this.m_lineNumber}" +
                    $"LineID: {lineID}";
                HealkitException e2 = new HealkitException(info, e);
                UIView.ForwardException(e2);
            }
            _simulationStepState = RedirectionHelper.RedirectCalls(_simulationStepOriginal, _simulationStepPatch);
        }
    }
}
