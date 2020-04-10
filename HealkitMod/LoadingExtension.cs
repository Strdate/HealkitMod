using ColossalFramework.UI;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            ModInfo.Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnLevelUnloading()
        {
            
        }

        public void OnReleased()
        {
            
        }
    }
}
