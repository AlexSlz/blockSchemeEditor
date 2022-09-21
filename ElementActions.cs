using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal static class ElementActions
    {
        public static void Move(this ElementObject element, Point pos, Point oldPos, Point elementOldPos)
        {
            element.Position.X = elementOldPos.X + (pos.X - oldPos.X);
            element.Position.Y = elementOldPos.Y + (pos.Y - oldPos.Y);
            MoveNodes(element);
        }

        private static void MoveNodes(ElementObject element)
        {
            element.Nodes.ForEach(node =>
            {
                node.Move(element.Position, element.elementData.BaseSize);
            });
        }

    }
}
