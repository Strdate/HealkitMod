using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HealkitMod
{
    public class ExceptionPanelExt
    {
        private static ExceptionPanelExt _instance;
        public static ExceptionPanelExt instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ExceptionPanelExt();
                    _instance.Init();
                }
                return _instance;
            }
        }

        private ExceptionPanel m_exceptionPanel;
        public UICheckBox m_chbSuppressThis;

        public void Init()
        {
            m_exceptionPanel = UIView.library.Get<ExceptionPanel>(typeof(ExceptionPanel).Name);
            m_chbSuppressThis = UIUtil.CreateCheckBox(m_exceptionPanel.component);
            m_chbSuppressThis.name = "HealkitMod_SuppressThis";
            m_chbSuppressThis.label.text = "Suppress";
            m_chbSuppressThis.tooltip = "[Default tooltip]";
            m_chbSuppressThis.isChecked = false;
            m_chbSuppressThis.width = 250f;
            m_chbSuppressThis.relativePosition = new Vector3(14f, 223f);
        }
    }
}
