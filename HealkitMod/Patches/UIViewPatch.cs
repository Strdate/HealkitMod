using ColossalFramework.UI;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealkitMod
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
                var template = ExceptionTemplate.RegisterException(exception);
                template.RaisedCount++;
                if (!template.Suppressed)
                {
                    ExceptionPanelExt.instance.m_chbSuppressThis.isVisible = true;
                    ExceptionPanelExt.instance.m_chbSuppressThis.isChecked = false;
                    message = message ?? "";
                    if(exception is HealkitException)
                    {
                        message += exception.Message + "\n\n" + exception.InnerException.ToString().Replace('&', '+');
                    }
                    else
                    {
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
            ExceptionTemplate.RegisterException(e).Suppressed = ExceptionPanelExt.instance.m_chbSuppressThis.isChecked;
            ExceptionPanelExt.instance.m_chbSuppressThis.isVisible = false;
            return true;
        }
    }
}
