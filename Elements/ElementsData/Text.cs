using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Text : IElement
    {
        public string Name => "Text";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.Black,
                FontSize = 14,
                CustomSize = new Size(63, 43),
                HideList = { "CustomSize" }
            };

        public void Draw(Graphics graphics, ElementParameter parameter)
        {
            using (Font drawFont = new Font("Microsoft Sans Serif", (float)parameter.FontSize))
            using (SolidBrush color = new SolidBrush(parameter.CustomColor))
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                string text = (parameter.Text == "") ? Parameters.Text : parameter.Text;

                SizeF fontSize = graphics.MeasureString(text, drawFont);

                parameter.CustomSize = new Size((int)fontSize.Width + 20, (int)fontSize.Height + 20);
                graphics.DrawString(text, drawFont, color, new Rectangle(parameter.Position, parameter.CustomSize), sf);
            }
        }
    }
}
