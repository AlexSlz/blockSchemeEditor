using System.Collections.Generic;
using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal class MyRectangle : IElement
    {
        public string Name => "Rectangle";

        public ElementParameter Parameters => 
            new ElementParameter { Text = this.Name,
                                    CustomColor = Color.Aquamarine, 
                                    CustomSize = new Size(200,100) };

        public void Draw(Graphics graphics, ElementParameter parameter)
        {
            using(SolidBrush pen = new SolidBrush(parameter.CustomColor))
            {
                graphics.FillRectangle(pen, new Rectangle(parameter.Position, parameter.CustomSize));
            }
        }
    }
}
