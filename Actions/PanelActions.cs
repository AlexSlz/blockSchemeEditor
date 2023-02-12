﻿using blockSchemeEditor.Elements;
using blockSchemeEditor.Render.MyPanel;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace blockSchemeEditor.Actions
{
    internal class PanelActions
    {
        private Panel _panel;

        public PanelActions(Panel panel)
        {
            _panel = panel;
        }

        public void DisplayElementOnPanel(ElementObject element)
        {
            int y = 10;

            element.Parameters.GetType().GetFields().ToList().ForEach(item =>
            {
                int temp = CreateNewPanelSection(item, element.Parameters, y);
                if (temp != -1)
                    y = temp;
            });
            element.Parameters.GetType().GetProperties().ToList().ForEach(item =>
            {
                int temp = CreateNewPanelSection(item, element.Parameters, y);
                if(temp != -1)
                    y = temp;
            });
            _panel.Controls[0].Controls[_panel.Controls[0].Controls.Count - 1].Focus();
        }

        private bool Valid(string itemName, dynamic value, ElementParameter parameter)
        {
            foreach (var element in parameter.HideList)
            {
                if (itemName.Contains(element))
                    return false;
            }

            double.TryParse(value.ToString(), out double temp);

            return itemName != "HideList" && temp != int.MinValue;
        }

        private int CreateNewPanelSection(MemberInfo item, ElementParameter parameters, int y)
        {
            dynamic value = 0;
            switch (item.MemberType)
            {
                case MemberTypes.Field:
                    value = ((FieldInfo)item).GetValue(parameters);
                    break;
                case MemberTypes.Property:
                    value = ((PropertyInfo)item).GetValue(parameters);
                    break;
            }

            if (!Valid(item.Name, value, parameters))
                return -1;

            PanelSection tempPanel = new PanelSection(value, item.Name);
            tempPanel.Section.Location = new Point(0, y);
            tempPanel.onValueChanged += new Action<dynamic>((data) =>
            {
                UpdateData(item, parameters, data);
            });
            _panel.Controls.Add(tempPanel.Section);
            y += tempPanel.Section.Height;

            return y;
        }

        private void UpdateData(MemberInfo item, ElementParameter parameters, dynamic value)
        {
            switch (item.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)item).SetValue(parameters, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)item).SetValue(parameters, value);
                    break;
                default:
                    MessageBox.Show(item.MemberType.ToString(), this.ToString());
                    break;
            }
        }

        public void DisplaySettingsPanel(PictureBox pictureBox)
        {
            int y = 10;
            PanelSection tempPanel = new PanelSection(pictureBox.BackColor, "Background Color");
            tempPanel.Section.Location = new Point(0, y);

            tempPanel.onValueChanged += new Action<dynamic>((data) =>
            {
                pictureBox.BackColor = data;
            });

            _panel.Controls.Add(tempPanel.Section);
            y += tempPanel.Section.Height;
        }

    }
}
