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

        public void Draw(Graphics graphics, ElementParameter parameters)
        {
            using (SolidBrush pen = new SolidBrush(parameters.CustomColor))
            {
                graphics.FillEllipse(pen, new Rectangle(parameters.Position, parameters.CustomSize));
            }
        }
    }
}
