using ColossalFramework;
using Harmony;
using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HealkitMod
{
    public class ModInfo : IUserMod
    {
        public string Name => "Healkit mod";

        public string Description => "0.0.0";

        public static readonly string SETTINGS_FILE_NAME = "HealkitMod";
        public static readonly string HARMONY_STRING = "strad.healkitMod.v1";

        public HarmonyInstance Harmony = HarmonyInstance.Create(HARMONY_STRING);

        public ModInfo()
        {
            try
            {
                // Creating setting file - from SamsamTS
                if (GameSettings.FindSettingsFileByName(SETTINGS_FILE_NAME) == null)
                {
                    GameSettings.AddSettingsFile(new SettingsFile[] { new SettingsFile() { fileName = SETTINGS_FILE_NAME } });
                }
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't load/create the setting file.");
                Debug.LogException(e);
            }
        }

        public void OnEnabled()
        {
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnDisabled()
        {
            Harmony.UnpatchAll(HARMONY_STRING);
        }


    }
}
