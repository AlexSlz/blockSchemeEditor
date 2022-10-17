using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Hexagon : IElement
    {
        public string Name => "Hexagon";

        public ElementParameter Parameters => new ElementParameter
                                            {
                                                Text = this.Name,
                                                CustomColor = Color.MediumPurple,
                                                CustomSize = new Size(110, 110),
                                                VerticesCount = 6,
                                            };

        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (SolidBrush pen = new SolidBrush(parameters.CustomColor))
            {
                graphics.FillPolygon(pen, CalculatePoints(parameters));
            }
        }

        private PointF[] CalculatePoints(ElementParameter parameters)
        {
            PointF[] points = new PointF[parameters.VerticesCount];
            int cx = parameters.CustomSize.Width / 2;
            int cy = parameters.CustomSize.Height / 2;
            double theta = 0;
            double dtheta = 2 * Math.PI / points.Length;
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X = (int)(cx + cx * Math.Cos(theta)) + parameters.Position.X;
                points[i].Y = (int)(cy + cy * Math.Sin(theta)) + parameters.Position.Y;
                theta += dtheta;
            }
            return points;
        }

    }
}
