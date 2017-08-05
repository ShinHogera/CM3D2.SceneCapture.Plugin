using System;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CM3D2.SceneCapture.Plugin
{
    internal static class GUIUtil {
        private static GUIStyle gsLabel;

        static GUIUtil() {
            gsLabel = new GUIStyle("label");
            gsLabel.fontSize = 11;
            gsLabel.alignment = TextAnchor.MiddleLeft;
        }

        public static void AddGUICheckbox(ControlBase parent, ControlBase elem)
        {
            float lastSize = parent.LastElementSize;
            elem.Left = parent.Left + ControlBase.FixedMargin;
            elem.Top = lastSize + ControlBase.FixedMargin;
            elem.Width = parent.Width - ControlBase.FixedMargin * 4;
            elem.Height = ControlHeight(parent);
            elem.FontSize = parent.FontSize;
            elem.OnGUI();
            elem.Visible = true;
        }

        public static void AddGUICheckbox(ControlBase parent, ControlBase elem, ControlBase reference)
        {
            elem.Left = parent.Left + ControlBase.FixedMargin;
            elem.Top = reference.Top + reference.Height + ControlBase.FixedMargin;
            elem.Width = parent.Width - ControlBase.FixedMargin * 4;
            elem.Height = ControlHeight(parent);
            elem.OnGUI();
            elem.Visible = true;
        }

        public static void AddGUISlider(ControlBase parent, ControlBase elem)
        {
            float lastSize = parent.LastElementSize;
            elem.Left = parent.Left + ControlBase.FixedMargin;
            elem.Top = lastSize + ControlBase.FixedMargin;
            elem.Width = parent.Width - ControlBase.FixedMargin * 4;
            elem.Height = ControlHeight(parent) * 2;
            elem.FontSize = parent.FontSize;
            elem.OnGUI();
            elem.Visible = true;
        }

        public static void AddGUISlider(ControlBase parent, ControlBase elem, string label)
        {
            float lastSize = parent.LastElementSize;
            Rect area = new Rect(parent.Left + ControlBase.FixedMargin,
                                 lastSize + ControlBase.FixedMargin,
                                 parent.Width - ControlBase.FixedMargin * 4,
                                 ControlHeight(parent));
            GUI.Label(area, label, gsLabel);
            area.y += area.height;
            elem.Left = area.x;
            elem.Top = area.y;
            elem.Width = area.width;
            elem.Height = area.height;
            elem.OnGUI();
            elem.Visible = true;
        }

        public static void AddGUISlider(ControlBase parent, ControlBase elem, ControlBase reference, string label)
        {
            Rect area = new Rect(parent.Left + ControlBase.FixedMargin,
                                 reference.Top + reference.Height + ControlBase.FixedMargin,
                                 parent.Width - ControlBase.FixedMargin * 4,
                                 ControlHeight(parent));
            GUI.Label(area, label, gsLabel);
            area.y += area.height;
            elem.Left = area.x;
            elem.Top = area.y;
            elem.Width = area.width;
            elem.Height = area.height;
            elem.OnGUI();
            elem.Visible = true;
        }

        public static void AddResetButton(ControlBase parent, ControlBase elem) {
                elem.Left = parent.Left + parent.Width - elem.Width - ControlBase.FixedMargin;
                elem.Top = parent.Top + ControlBase.FixedMargin;
                elem.Width = parent.FontSize * 2;
                elem.Height = ControlHeight(parent);
            elem.OnGUI();
            elem.Visible = true;
        }

        public static float GetHeightForParent(ControlBase parent, ControlBase lastElem)
        {
            return lastElem.Top + lastElem.Height + ControlBase.FixedMargin - parent.Top;
        }

        public static float GetHeightForParent(ControlBase parent)
        {
            float lastSize = parent.LastElementSize;
            return lastSize + ControlBase.FixedMargin - parent.Top;
        }

        public static float ControlHeight(ControlBase parent)
        {
            return parent.FixedFontSize + ControlBase.FixedMargin * 1;
        }
    }
}
