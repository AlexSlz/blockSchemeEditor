using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Star : IElement
    {
        public string Name => "Star";

        public ElementParameter Parameters =>
                        new ElementParameter
                        {
                            Text = this.Name,
                            CustomColor = Color.LightYellow,
                            CustomSize = new Size(110, 110),
                            VerticesCount = 5
                        };

        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (SolidBrush brush = new SolidBrush(parameters.CustomColor))
            {
                PointF[] points = new PointF[2 * parameters.VerticesCount];

                double rx1 = parameters.CustomSize.Width / 2;
                double ry1 = parameters.CustomSize.Height / 2;
                double rx2 = rx1 * 0.5;
                double ry2 = ry1 * 0.5;
                double x = parameters.Position.X + rx1;
                double y = parameters.Position.Y + ry1;

                double theta = -Math.PI / 2;
                double dtheta = Math.PI / parameters.VerticesCount;
                for (int i = 0; i < 2 * parameters.VerticesCount; i += 2)
                {
                    points[i] = new PointF(
                        (float)(x + rx1 * Math.Cos(theta)),
                        (float)(y + ry1 * Math.Sin(theta)));
                    theta += dtheta;

                    points[i + 1] = new PointF(
                        (float)(x + rx2 * Math.Cos(theta)),
                        (float)(y + ry2 * Math.Sin(theta)));
                    theta += dtheta;
                }

                graphics.FillPolygon(brush, points);
            }
        }
    }
}
