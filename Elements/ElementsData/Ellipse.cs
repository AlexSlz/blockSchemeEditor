using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal class Ellipse : IElement
    {
        public string Name => "Ellipse";

        public Size BaseSize => new Size(110, 110);

        public void Draw(Graphics graphics, Point position)
        {
            using (SolidBrush pen = new SolidBrush(Color.Yellow))
            {
                graphics.FillEllipse(pen, new System.Drawing.Rectangle(position.X, position.Y, BaseSize.Width, BaseSize.Height));
            }
        }
    }
}
