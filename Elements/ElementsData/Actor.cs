using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Actor : IElement
    {
        public string Name => "Actor";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.Yellow,
                CustomSize = new Size(100, 100),
            };


        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (Pen pen = new Pen(parameters.CustomColor, 7))
            using (SolidBrush brush = new SolidBrush(parameters.CustomColor))
            {
                int x = parameters.Position.X + parameters.CustomSize.Width / 3;
                graphics.FillEllipse(brush, new Rectangle(new Point(x, parameters.Position.Y),
                    new Size(parameters.CustomSize.Width / 3, parameters.CustomSize.Height / 3)));

                Point bodyStart = new Point(parameters.Position.X + parameters.CustomSize.Width / 2, parameters.Position.Y + parameters.CustomSize.Height / 3);
                Point bodyEnd = new Point(bodyStart.X, bodyStart.Y + parameters.CustomSize.Height / 3);
                graphics.DrawLine(pen, bodyStart, bodyEnd);                
                
                Point endPoint2 = new Point(bodyEnd.X + parameters.CustomSize.Width / 6, bodyEnd.Y + parameters.CustomSize.Height / 3);
                graphics.DrawLine(pen, bodyEnd, endPoint2);                
                
                Point endPoint3 = new Point(bodyEnd.X - parameters.CustomSize.Width / 6, bodyEnd.Y + parameters.CustomSize.Height / 3);
                graphics.DrawLine(pen, bodyEnd, endPoint3);               
                
                Point endPoint4 = new Point(bodyStart.X + parameters.CustomSize.Width / 4, bodyStart.Y + parameters.CustomSize.Height / 3);
                graphics.DrawLine(pen, bodyStart, endPoint4);              
                
                Point endPoint5 = new Point(bodyStart.X - parameters.CustomSize.Width / 4, bodyStart.Y + parameters.CustomSize.Height / 3);
                graphics.DrawLine(pen, bodyStart, endPoint5);
            }
        }
    }
}
