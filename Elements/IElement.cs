using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements
{
    internal interface IElement
    {
        string Name { get; }

        Size BaseSize { get; }

        void Draw(Graphics graphics, Point position);
    }
}
