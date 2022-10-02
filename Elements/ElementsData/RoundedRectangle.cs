using System.Drawing;

namespace blockSchemeEditor.Elements
{

    internal class RoundedRectangle : IElement
    {
        public string Name => "Rounded Rectangle";
        public int Angle = 50;
        public Size BaseSize => new Size(200, 100);
        public void Draw(Graphics graphics, Point position)
        {
            using (SolidBrush pen = new SolidBrush(Color.Coral))
            {
                Rectangle rect = new Rectangle(position, BaseSize);
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddArc(rect.X, rect.Y, Angle, Angle, 180f, 90f);
                path.AddArc((rect.Right - Angle), rect.Y, Angle, Angle, 270f, 90f);
                path.AddArc((rect.Right - Angle), (rect.Bottom - Angle), Angle, Angle, 0f, 90f);
                path.AddArc(rect.X, (rect.Bottom - Angle), Angle, Angle, 90f, 90f);
                path.CloseFigure();
                graphics.FillPath(pen, path);
            }
        }

    }
}
