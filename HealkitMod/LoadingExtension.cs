using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HealkitMod
{
    public class LoadingExtension : ILoadingExtension
    {
        public void OnCreated(ILoading loading)
        {
            
        }

        public void OnLevelLoaded(LoadMode mode)
        {
            BuildingAIPatch.Patch();
            NetAIPatch.Patch();
            TransportLinePatch.Patch();
            VehicleAIPatch.Patch();
        }

        public void OnLevelUnloading()
        {
            VehicleAIPatch.Unpatch();
            TransportLinePatch.Unpatch();
            NetAIPatch.Unpatch();
            BuildingAIPatch.Unpatch();
        }

        public void OnReleased()
        {
            
        }
    }
}
