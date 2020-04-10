using ColossalFramework;
using ColossalFramework.UI;
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
        public string Name => "Extended Error Reporting";

        public string Description => "[" + VERSION + "]";

        public static readonly string VERSION = "0.1.0";
        public static readonly string SETTINGS_FILE_NAME = "HealkitMod";
        public static readonly string HARMONY_STRING = "strad.healkitMod.v1";

        public static HarmonyInstance Harmony = HarmonyInstance.Create(HARMONY_STRING);

        public static readonly SettingsBool sb_SuppressAllExceptions = new SettingsBool("Suppress all exceptions", "Not recommended", "suppressAllExceptions", false);

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

        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {
                UIHelper group = helper.AddGroup(Name) as UIHelper;
                UIPanel panel = group.self as UIPanel;

                sb_SuppressAllExceptions.Draw(group);

                group.AddSpace(10);

                group.AddButton("Clear list of suppressed exceptions", () =>
                {
                    ExceptionTemplate.ResetSuppressing();
                });

                group.AddSpace(10);

                group.AddButton("Open log folder", () =>
                {
                    Utils.OpenInFileBrowser(Application.dataPath);
                });

            }
            catch (Exception e)
            {
                Debug.Log("OnSettingsUI failed");
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
