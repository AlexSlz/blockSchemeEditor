using blockSchemeEditor.Elements;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal static class ElementActions
    {

        public static void DeleteElement(this Canvas canvas, ElementObject element)
        {
            element.Nodes.ForEach(node =>
            {
                DeleteNode(canvas, node);
            });
            canvas.Elements.Remove(element);
        }
        public static void DeleteNode(Canvas canvas, Node node)
        {
            var temp = canvas.Lines.FindAll(line => line.FirstNode == node || line.SecondNode == node);

            temp.ForEach(line =>
            {
                canvas.Lines.Remove(line);
            });
        }
        public static void Move(this ElementObject element, Point pos, Point oldPos, Point elementOldPos)
        {
            element.Parameters.Position.X = elementOldPos.X + (pos.X - oldPos.X);
            element.Parameters.Position.Y = elementOldPos.Y + (pos.Y - oldPos.Y);
            MoveNodes(element);
        }
        private static void MoveNodes(ElementObject element)
        {
            element.Nodes.ForEach(node =>
            {
                node.Move(element.Parameters.Position, element.Parameters.CustomSize);
            });
        }

    }
}
