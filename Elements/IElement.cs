using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal interface IElement
    {
        string Name { get; }

        Size BaseSize { get; }

        void Draw(Graphics graphics, Point position);
    }
}
