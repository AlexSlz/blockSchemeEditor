using System;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Render.MyPanel
{
    internal class PanelSection
    {
        public dynamic value { get ; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public Panel Section { get; private set; }

        public Action<dynamic> onValueChanged;


        public PanelSection(dynamic value, string name)
        {
            this.value = value;
            Type = value.GetType().Name;
            Name = name;
            Section = CreateSection();
        }

        public void UpdateValue()
        {
            dynamic temp = GetData();
            this.onValueChanged?.Invoke(temp);
        }
        private dynamic GetData()
        {
            switch (Type)
            {
                case ("String"):
                    return Section.Controls[1].Text;
                case ("Double"):
                    return double.Parse(Section.Controls[1].Text);
                case ("Int32"):
                    return int.Parse(Section.Controls[1].Text);
                case ("Point"):
                    return new Point(int.Parse(Section.Controls[1].Text), int.Parse(Section.Controls[2].Text));
                case ("Size"):
                    return new Size(int.Parse(Section.Controls[1].Text), int.Parse(Section.Controls[2].Text));
                case ("Color"):
                    return Section.Controls[1].BackColor;
                case ("Boolean"):
                    return ((CheckBox)(Section.Controls[1])).Checked;
                default:
                    return 0;
            }
        }

        private Panel CreateSection()
        {
            Panel panel = new Panel();

            panel.Font = new Font(FontFamily.GenericSansSerif, 14);

            Label label = new Label();
            label.Text = Name;
            label.Location = new Point(0, 0);
            label.Width = 170;
            panel.Controls.Add(label);
            switch (Type)
            {
                case ("String"):
                    panel.Controls.Add(PanelElements.CreateTextBox(value, new Point(5, label.Size.Height + 5), new Action(()=> UpdateValue())));
                    break;
                case ("Double"):
                    panel.Controls.Add(PanelElements.CreateNumeric(value, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue()), 3, -100));
                    break;
                case ("Int32"):
                    panel.Controls.Add(PanelElements.CreateNumeric(value, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue())));
                    break;
                case ("Point"):
                    Point point = (Point)value;
                    panel.Controls.Add(PanelElements.CreateNumeric(point.X, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue())));
                    panel.Controls.Add(PanelElements.CreateNumeric(point.Y, new Point(5, label.Size.Height + 40), new Action(() => UpdateValue())));
                    break;
                case ("Size"):
                    Size size = (Size)value;
                    panel.Controls.Add(PanelElements.CreateNumeric(size.Width, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue())));
                    panel.Controls.Add(PanelElements.CreateNumeric(size.Height, new Point(5, label.Size.Height + 40), new Action(() => UpdateValue())));
                    break;
                case ("Color"):
                    panel.Controls.Add(PanelElements.CreateColorButton(value, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue())));
                    break;
                case ("Boolean"):
                    panel.Controls.Add(PanelElements.CreateCheckbox(value, new Point(5, label.Size.Height + 5), new Action(() => UpdateValue())));
                    break;
                default:
                    MessageBox.Show(value.GetType().Name, "A");
                    break;
            }
            
            int width = 0;
            int height = 0;

            foreach (Control control in panel.Controls)
            {
                if (width <= control.Width)
                    width = control.Width + 5;
                height += control.Height + 7;
            }

            panel.Size = new Size(width, height);
            return panel;
        }
    }
}
