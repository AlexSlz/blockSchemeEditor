using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements
{
    internal class Process : IElement
    {
        public string Name => "Proccess";
        public Size BaseSize => new Size(100, 200);
        public void Draw(Graphics graphics, Point position)
        {
            SolidBrush _pen = new SolidBrush(Color.Aquamarine);
            graphics.FillRectangle(_pen, new Rectangle(position.X, position.Y, BaseSize.Height, BaseSize.Width));
        }
    }
}
