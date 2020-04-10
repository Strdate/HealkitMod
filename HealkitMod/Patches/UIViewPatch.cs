using ColossalFramework.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealkitMod.Patches
{
    [HarmonyPatch(typeof(UIView))]
    [HarmonyPatch("HandleException")]
    public class UIViewHandleExceptionPatch
    {
        static bool Prefix(ref string message,ref Exception exception, Queue<Exception> ___sLastException)
        {
            UIComponent uicomponent = UIView.library.Get("ExceptionPanel");
            if (uicomponent != null && UIView.GetModalComponent() != uicomponent)
            {
                if(exception == null)
                {
                    return ModInfo.sb_SuppressAllExceptions.value ? false : true;
                }
                var template = ExceptionTemplate.RegisterException(exception);
                template.RaisedCount++;
                if (!template.Suppressed && !ModInfo.sb_SuppressAllExceptions.value)
                {
                    ExceptionPanelExt.instance.m_chbSuppressThis.isChecked = false;
                    message = message ?? "";
                    HealkitException hke = exception as HealkitException;
                    if(hke != null)
                    {
                        message += exception.Message + "\n\n" + exception.InnerException.ToString().Replace('&', '+');
                        if(hke.m_supperessMsg != null)
                        {
                            ExceptionPanelExt.instance.m_chbSuppressThis.isVisible = true;
                            ExceptionPanelExt.instance.m_chbSuppressThis.tooltip = hke.m_supperessMsg;
                        }
                    }
                    else
                    {
                        ExceptionPanelExt.instance.m_chbSuppressThis.isVisible = false;
                        message += exception.ToString().Replace('&', '+');
                    }
                    exception = null;
                    return true;
                }
                else
                {
                    if (___sLastException.Count > 0)
                    {
                        ___sLastException.Dequeue();
                    }
                }
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(UIView))]
    [HarmonyPatch("OnExceptionClosed")]
    public class UIViewOnExceptionClosedPatch
    {
        static bool Prefix(Queue<Exception> ___sLastException)
        {
            Exception e = ___sLastException.Peek();
            if(e != null)
            {
                ExceptionTemplate.RegisterException(e).Suppressed = ExceptionPanelExt.instance.m_chbSuppressThis.isChecked;
            }
            ExceptionPanelExt.instance.m_chbSuppressThis.isVisible = false;
            return true;
        }
    }
}
