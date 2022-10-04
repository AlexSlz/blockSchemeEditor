using System.Collections.Generic;
using System.Drawing;

namespace blockSchemeEditor.Elements
{
    internal interface IElement
    {
        string Name { get; }

        ElementParameter Parameters { get; }

        void Draw(Graphics graphics, ElementParameter parameter);
    }
}
