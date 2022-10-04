using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements
{

    internal class RoundedRectangle : IElement
    {
        public string Name => "Rounded Rectangle";
        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = "Rounded Rectangle",
                CustomColor = Color.Coral,
                CustomSize = new Size(200, 100),
                Angle = 50,
            };
        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (SolidBrush pen = new SolidBrush(parameters.CustomColor))
            {
                float Angle = float.Parse(parameters.Angle.ToString());
                Rectangle rect = new Rectangle(parameters.Position, parameters.CustomSize);
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
