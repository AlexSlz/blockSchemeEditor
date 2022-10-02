using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal class MyRectangle : IElement
    {
        public string Name => "Rectangle";
        public Size BaseSize => new Size(200, 100);
        public void Draw(Graphics graphics, Point position)
        {
            using(SolidBrush pen = new SolidBrush(Color.Aquamarine))
            {
                graphics.FillRectangle(pen, new System.Drawing.Rectangle(position.X, position.Y, BaseSize.Width, BaseSize.Height));
            }
        }
    }
}
