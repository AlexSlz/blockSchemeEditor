using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements
{
    internal class Ellipse : IElement
    {
        public string Name => "Ellipse";

        public Size BaseSize => new Size(100, 100);

        public void Draw(Graphics graphics, Point position)
        {
            using (SolidBrush pen = new SolidBrush(Color.GreenYellow))
            {
                graphics.FillEllipse(pen, new Rectangle(position.X, position.Y, BaseSize.Height, BaseSize.Width));
            }
        }
    }
}
