using System.Drawing;

namespace blockSchemeEditor.Elements.ElementsData
{
    internal class Rhombus : IElement
    {
        public string Name => "Rhombus";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.IndianRed,
                CustomSize = new Size(130, 130),
            };

        public void Draw(Graphics graphics, ElementParameter parameter)
        {
            using (SolidBrush pen = new SolidBrush(parameter.CustomColor))
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

                path.AddLines(new[]
                {
      new Point(parameter.Position.X, parameter.Position.Y + (parameter.CustomSize.Height / 2)),
      new Point(parameter.Position.X + (parameter.CustomSize.Width / 2), parameter.Position.Y),
      new Point(parameter.Position.X + parameter.CustomSize.Width, parameter.Position.Y + (parameter.CustomSize.Height / 2)),
      new Point(parameter.Position.X + (parameter.CustomSize.Width / 2), parameter.Position.Y + parameter.CustomSize.Height),
      new Point(parameter.Position.X, parameter.Position.Y + (parameter.CustomSize.Height / 2))
                });
                graphics.FillPath(pen, path);
            }
        }
    }
}
