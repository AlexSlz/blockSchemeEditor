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

        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using(SolidBrush pen = new SolidBrush(parameters.CustomColor))
            {
                graphics.FillRectangle(pen, new Rectangle(parameters.Position, parameters.CustomSize));
            }
        }
    }
}
