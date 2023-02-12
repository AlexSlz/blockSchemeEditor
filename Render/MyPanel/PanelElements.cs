using System;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Render.MyPanel
{
    internal static class PanelElements
    {
        public static Button CreateColorButton(dynamic value, Point pos, Action onClick = null)
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
            button.BackColorChanged += (object sender, EventArgs e) =>
            {
                onClick?.Invoke();
            };
            return button;
        }

        public static CheckBox CreateCheckbox(dynamic value, Point pos, Action onClick = null)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Text = "";
            checkBox.Width = 20;
            checkBox.Height = 20;
            checkBox.Location = pos;
            checkBox.Checked = (bool)value;
            checkBox.CheckedChanged += (object sender, EventArgs e) =>
            {
                onClick?.Invoke();
            };
            return checkBox;
        }

        public static TextBox CreateTextBox(dynamic value, Point pos, Action onClick = null)
        {
            TextBox textBox = new TextBox();
            textBox.Text = $"{value}";
            textBox.Location = pos;
            textBox.MaxLength = 64;
            textBox.Width = 150;
            textBox.TextChanged += (object sender, EventArgs e) =>
            {
                onClick?.Invoke();
            };

            return textBox;
        }


        public static NumericUpDown CreateNumeric(dynamic value, Point pos, int decimalPlaces = 0, int minimum = 0)
        {
            NumericUpDown numeric = new NumericUpDown();

            numeric.Location = pos;
            numeric.Maximum = 9999;
            numeric.Width = 150;
            numeric.DecimalPlaces = decimalPlaces;
            numeric.Minimum = minimum;

            numeric.Value = decimal.Parse(value.ToString());

            return numeric;
        }

        public static NumericUpDown CreateNumeric(dynamic value, Point pos, Action onClick, int decimalPlaces = 0, int minimum = 0) 
        {
            NumericUpDown numeric = CreateNumeric(value, pos, decimalPlaces, minimum);
            numeric.ValueChanged += (object sender, EventArgs e) =>
            {
                onClick?.Invoke();
            };
            return numeric;
        }


    }
}
