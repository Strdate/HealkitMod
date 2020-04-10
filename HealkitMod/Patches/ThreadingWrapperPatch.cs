using ColossalFramework;
using ColossalFramework.Plugins;
using ColossalFramework.UI;
using Harmony;
using ICities;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HealkitMod.Patches
{
    public class ThreadingWrapperExceptionHandler
    {
        public static void Handle(Exception e, IThreadingExtension ext, string method)
        {
            Assembly assembly = ext.GetType().Assembly;
            Type modInfoType = null;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface("IUserMod") != null)
                {
                    modInfoType = type;
                    break;
                }
            }

            string modName = null;
            IUserMod modInfo = null;
            foreach (PluginManager.PluginInfo pluginInfo in Singleton<PluginManager>.instance.GetPluginsInfo())
            {
                try
                {
                    EntryData entryData = new EntryData(pluginInfo);
                    if (!entryData.pluginInfo.isBuiltin)
                    {
                        IUserMod[] instances = entryData.pluginInfo.GetInstances<IUserMod>();
                        if (instances.Length == 1)
                        {
                            modInfo = instances[0];
                        }
                        if(modInfo.GetType() == modInfoType)
                        {
                            modName = modInfo.Name + " - " + modInfo.Description;
                            break;
                        }
                    }
                }
                catch
                { }
            }

            string info = $"An error has occured in mod's {method} method.\nMod name: {modName ?? "<Unknown>"}\nAssembly: {assembly.FullName}\nSeverity: Medium";
            HealkitException e2 = new HealkitException(info, e);
            e2.m_uniqueData = modName;
            e2.m_supperessMsg = "Suppress similar exceptions caused by this mod";
            UIView.ForwardException(e2);
        }
    }

    [HarmonyPatch(typeof(ThreadingWrapper))]
    [HarmonyPatch("OnUpdate")]
    public class ThreadingWrapperOnUpdatePatch
    {
        public static bool Prefix(float realTimeDelta, float simulationTimeDelta, List<IThreadingExtension> ___m_ThreadingExtensions)
        {
            for (int i = 0; i < ___m_ThreadingExtensions.Count; i++)
            {
                try
                {
                    ___m_ThreadingExtensions[i].OnUpdate(realTimeDelta, simulationTimeDelta);
                } catch(Exception e)
                {
                    ThreadingWrapperExceptionHandler.Handle(e, ___m_ThreadingExtensions[i], "OnUpdate");
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ThreadingWrapper))]
    [HarmonyPatch("OnBeforeSimulationTick")]
    public class ThreadingWrapperOnBeforeSimulationTickPatch
    {
        public static bool Prefix(List<IThreadingExtension> ___m_ThreadingExtensions)
        {
            for (int i = 0; i < ___m_ThreadingExtensions.Count; i++)
            {
                try
                {
                    ___m_ThreadingExtensions[i].OnBeforeSimulationTick();
                }
                catch (Exception e)
                {
                    ThreadingWrapperExceptionHandler.Handle(e, ___m_ThreadingExtensions[i], "OnBeforeSimulationTick");
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ThreadingWrapper))]
    [HarmonyPatch("OnBeforeSimulationFrame")]
    public class ThreadingWrapperOnBeforeSimulationFramePatch
    {
        public static bool Prefix(List<IThreadingExtension> ___m_ThreadingExtensions)
        {
            for (int i = 0; i < ___m_ThreadingExtensions.Count; i++)
            {
                try
                {
                    ___m_ThreadingExtensions[i].OnBeforeSimulationFrame();
                }
                catch (Exception e)
                {
                    ThreadingWrapperExceptionHandler.Handle(e, ___m_ThreadingExtensions[i], "OnBeforeSimulationFrame");
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ThreadingWrapper))]
    [HarmonyPatch("OnAfterSimulationFrame")]
    public class ThreadingWrapperOnAfterSimulationFramePatch
    {
        public static bool Prefix(List<IThreadingExtension> ___m_ThreadingExtensions)
        {
            for (int i = 0; i < ___m_ThreadingExtensions.Count; i++)
            {
                try
                {
                    ___m_ThreadingExtensions[i].OnAfterSimulationFrame();
                }
                catch (Exception e)
                {
                    ThreadingWrapperExceptionHandler.Handle(e, ___m_ThreadingExtensions[i], "OnAfterSimulationFrame");
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(ThreadingWrapper))]
    [HarmonyPatch("OnAfterSimulationTick")]
    public class ThreadingWrapperOnAfterSimulationTickPatch
    {
        public static bool Prefix(List<IThreadingExtension> ___m_ThreadingExtensions)
        {
            for (int i = 0; i < ___m_ThreadingExtensions.Count; i++)
            {
                try
                {
                    ___m_ThreadingExtensions[i].OnAfterSimulationTick();
                }
                catch (Exception e)
                {
                    ThreadingWrapperExceptionHandler.Handle(e, ___m_ThreadingExtensions[i], "OnAfterSimulationTick");
                }
            }
            return false;
        }
    }
}
