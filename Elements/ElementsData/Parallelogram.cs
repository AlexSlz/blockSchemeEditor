using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Parallelogram : IElement
    {
        public string Name => "Parallelogram";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.IndianRed,
                CustomSize = new Size(200, 100),
                Angle = 50
            };

        public void Draw(Graphics graphics, ElementParameter parameter)
        {
            using (SolidBrush pen = new SolidBrush(parameter.CustomColor))
            {
                int x = parameter.Position.X;
                int y = parameter.Position.Y;
                int angle = (int)parameter.Angle;
                Point[] points = new Point[]
                {
                    new Point(x + angle, y),
                    new Point(x, y + parameter.CustomSize.Height),
                    new Point(x - angle + parameter.CustomSize.Width, y + parameter.CustomSize.Height),
                    new Point(x + parameter.CustomSize.Width, y)
                };
                graphics.FillPolygon(pen, points);
            }
        }
    }
}
