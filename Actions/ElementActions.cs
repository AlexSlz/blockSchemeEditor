using blockSchemeEditor.Commands;
using blockSchemeEditor.Elements;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal static class ElementActions
    {
        public static void DeleteElements(this Canvas canvas, List<ElementObject> elements)
        {
            elements.ForEach(element =>
            {
                canvas.DeleteElement(element);
            });
            canvas.OnElementsChanged();
        }

        public static void DeleteElement(this Canvas canvas, ElementObject element)
        {
            element.Nodes.ForEach(node =>
            {
                DeleteNode(canvas, node);
            });
            canvas.Elements.Remove(element);
            canvas.OnElementsChanged();
        }

        public static void AddElement(this Canvas canvas, ElementObject element)
        {
            canvas.Elements.Add(element);
            canvas.OnElementsChanged();
        }
        public static List<Line> DeleteNode(this Canvas canvas, Node node)
        {
            var currentLines = canvas.Lines.FindAll(line => line.FirstNode == node || line.SecondNode == node);

            currentLines.ForEach(line =>
            {
                canvas.Lines.Remove(line);
            });
            return currentLines;
        }
        public static void Move(this ElementObject element, Point pos, Point oldPos)
        {
            element.Parameters.Position.X = element.lastPosition.X + (pos.X - oldPos.X);
            element.Parameters.Position.Y = element.lastPosition.Y + (pos.Y - oldPos.Y);
            MoveNodes(element);
        }

        public static void Move(this List<ElementObject> elements, Point pos, Point oldPos)
        {
            elements.ForEach(element =>
            {
                element.Move(pos, oldPos);
            });
        }

        private static void MoveNodes(ElementObject element)
        {
            element.Nodes.ForEach(node =>
            {
                node.Move(element.Parameters.Position, element.Parameters.CustomSize);
            });
        }


        public static string AddCommand(this List<ICommand> commandList, ICommand command)
        {
            commandList.Add(command);
            command.Execute();
            return command.CommandName;
        }
    }
}
