using blockSchemeEditor.Elements;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace blockSchemeEditor.Actions
{
    internal class PanelActions
    {
        private Panel _panel;
        private Canvas _canvas;
        public PanelActions(Panel panel, Canvas canvas)
        {
            _panel = panel;
            _canvas = canvas;
        }
        private ElementObject activeElement;
        public void InitPanel(ElementObject element)
        {
            activeElement = element;
            int y = 10;

            element.Parameters.GetType().GetFields().ToList().ForEach(item =>
            {
                int temp = Init(item, element.Parameters, y);
                if(temp != -1)
                    y = temp;
            });
            element.Parameters.GetType().GetProperties().ToList().ForEach(item =>
            {
                int temp = Init(item, element.Parameters, y);
                if (temp != -1)
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

        private int Init(MemberInfo item, ElementParameter parameter, int y)
        {
            dynamic value = 0;
            switch (item.MemberType)
            {
                case MemberTypes.Field:
                    value = ((FieldInfo)item).GetValue(parameter);
                    break;
                case MemberTypes.Property:
                    value = ((PropertyInfo)item).GetValue(parameter);
                    break;
            }

            if (!Valid(item.Name, value, parameter))
                return -1;

            Panel tempPanel = CreatePanel(item.Name, value, new Point(0, y));
            _panel.Controls.Add(tempPanel);
            y += tempPanel.Height;

            return y;
        }

        private Panel CreatePanel(string name, dynamic value, Point pos)
        {
            Panel panel = new Panel();
            
            panel.Font = new Font(FontFamily.GenericSansSerif, 14);

            Label label = new Label();
            label.Text = name;
            label.Location = new Point(0,0);
            label.Width = 150;
            panel.Controls.Add(label);
            switch (value.GetType().Name)
            {
                case ("String"):
                    panel.Controls.Add(CreateTextBox(value, new Point(5, label.Size.Height + 5)));
                    break;
                case ("Double"):
                    panel.Controls.Add(CreateNumeric(value, new Point(5, label.Size.Height + 5), 3, -100));
                    break;
                case ("Int32"):
                    panel.Controls.Add(CreateNumeric(value, new Point(5, label.Size.Height + 5)));
                    break;
                case ("Point"):
                    Point point = (Point)value;
                    panel.Controls.Add(CreateNumeric(point.X, new Point(5, label.Size.Height + 5)));
                    panel.Controls.Add(CreateNumeric(point.Y, new Point(5, label.Size.Height + 40)));
                    break;
                case ("Size"):
                    Size size = (Size)value;
                    panel.Controls.Add(CreateNumeric(size.Width, new Point(5, label.Size.Height + 5)));
                    panel.Controls.Add(CreateNumeric(size.Height, new Point(5, label.Size.Height + 40)));
                    break;
                case ("Color"):
                    panel.Controls.Add(CreateColorButton(value, new Point(5, label.Size.Height + 5)));
                    break;
                case ("Boolean"):
                    panel.Controls.Add(CreateCheckbox(value, new Point(5, label.Size.Height + 5)));
                    break;
                default:
                    MessageBox.Show(value.GetType().Name);
                    break;
            }
            panel.Location = pos;

            int width = 0;
            int height = 0;

            foreach (Control control in panel.Controls)
            {
                if(width <= control.Width)
                   width = control.Width + 5;
                height += control.Height + 7;
            }

            panel.Size = new Size(width, height);
            return panel;
        }

        private Button CreateColorButton(object value, Point pos)
        {
            Button button = new Button();
            button.Text = "";
            button.Width = 20;
            button.Height = 20;
            button.Location = pos;
            button.BackColor = (Color)value;
            button.Click += (object sender, EventArgs e) =>
            {
                ColorDialog colorDialog = new ColorDialog();
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    button.BackColor = colorDialog.Color;
            };
            button.BackColorChanged += ControlChanged;
            return button;
        }

        private CheckBox CreateCheckbox(object value, Point pos)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Text = "";
            checkBox.Width = 20;
            checkBox.Height = 20;
            checkBox.Location = pos;
            checkBox.Checked = (bool)value;
            checkBox.CheckedChanged += ControlChanged;
            return checkBox;
        }

        private TextBox CreateTextBox(dynamic value, Point pos, bool onlyNum = false)
        {
            TextBox textBox = new TextBox();
            textBox.Text = $"{value}";
            textBox.Location = pos;
            textBox.MaxLength = 64;
            textBox.Width = 150;
            textBox.TextChanged += (object sender, EventArgs e) =>
            {
                TextBox t = (sender as TextBox);

                if (ValidateText(t.Text, onlyNum))
                {
                    ControlChanged(this, EventArgs.Empty);
                }
            };
            if (onlyNum)
                textBox.KeyPress += (object sender, KeyPressEventArgs e) =>
                {
                    TextBox t = sender as TextBox;
                    e.Handled = checkKey(e);
                };
            return textBox;
        }


        private NumericUpDown CreateNumeric(dynamic value, Point pos, int decimalPlaces = 0, int minimum = 0)
        {
            NumericUpDown numeric = new NumericUpDown();

            numeric.Location = pos;
            numeric.Maximum = 9999;
            numeric.Width = 150;
            numeric.DecimalPlaces = decimalPlaces;
            numeric.Minimum = minimum;

            numeric.Value = decimal.Parse(value.ToString());

            numeric.ValueChanged += (object sender, EventArgs e) =>
            {
                ControlChanged(this, EventArgs.Empty);
            };

            return numeric;
        }

        private bool ValidateText(string text, bool onlyNum)
        {
            bool validate = text != "";
            if (onlyNum)
            {
                int max = int.MaxValue / 9999;

                double.TryParse(text, out double outValue);
                validate = validate && outValue >= -max;
                validate = validate && outValue <= max;
            }
            validate = validate && (!onlyNum || text.Count(char.IsDigit) > 0);
            return validate;
        }

        private bool checkKey(KeyPressEventArgs e)
        {
            return (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }

        private void ControlChanged(object sender, EventArgs e)
        {
            activeElement?.Parameters.GetType().GetFields().ToList().ForEach(item => {
                GetControl(item.Name, (control) =>
                {
                    item.SetValue(activeElement.Parameters, GetValue(item.FieldType.Name, control));
                });
            });
            activeElement?.Parameters.GetType().GetProperties().ToList().ForEach(item =>
            {
                GetControl(item.Name, (control) =>
                {
                    item.SetValue(activeElement.Parameters, GetValue(item.PropertyType.Name, control));
                });
            });
        }

        private void GetControl(string name, Action<Control> Callback)
        {
            foreach (Control control in _panel.Controls)
            {
                if (name.Contains(control.Controls[0].Text))
                {
                    Callback(control);
                }
            }
        }

        private dynamic GetValue(string type, Control control)
        {
            switch (type)
            {
                case ("String"):
                    return control.Controls[1].Text;
                case ("Double"):
                    return double.Parse(control.Controls[1].Text);
                case ("Int32"):
                    return int.Parse(control.Controls[1].Text);
                case ("Point"):
                    return new Point(int.Parse(control.Controls[1].Text), int.Parse(control.Controls[2].Text));
                case ("Size"):
                    return new Size(int.Parse(control.Controls[1].Text), int.Parse(control.Controls[2].Text));
                case ("Color"):
                    return control.Controls[1].BackColor;
                case ("Boolean"):
                    return ((CheckBox)(control.Controls[1])).Checked;
                default:
                    return 0;
            }
        }
    }
}
