using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Triangle : IElement
    {
        public string Name => "Triangle";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.GreenYellow,
                CustomSize = new Size(110, 110),
            };

        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (SolidBrush brush = new SolidBrush(parameters.CustomColor))
            {
                Point[] points = { new Point(parameters.Position.X, parameters.Position.Y + parameters.CustomSize.Height), 
                    new Point(parameters.Position.X + parameters.CustomSize.Width, parameters.Position.Y + parameters.CustomSize.Height), 
                    new Point(parameters.Position.X + parameters.CustomSize.Width / 2, parameters.Position.Y)
                };
                graphics.FillPolygon(brush, points);
            }
        }
    }
}

