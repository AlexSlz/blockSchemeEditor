using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal class Ellipse : IElement
    {
        public string Name => "Ellipse";

        public ElementParameter Parameters =>
            new ElementParameter
            {
                Text = this.Name,
                CustomColor = Color.Yellow,
                CustomSize = new Size(110, 110),
            };

        public void Draw(Graphics graphics, ElementParameter parameter)
        {
            using (SolidBrush pen = new SolidBrush(parameter.CustomColor))
            {
                graphics.FillEllipse(pen, new Rectangle(parameter.Position, parameter.CustomSize));
            }
        }
    }
}
