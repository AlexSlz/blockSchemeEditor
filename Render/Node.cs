using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace blockSchemeEditor
{
    internal class Node
    {
        public Point position;
        public Size Size => new Size(30,30);
        public NodePosition nodePosition { get; set; }

        public ElementObject Parent { get; private set; }

        public Node(NodePosition position, ElementObject parent)
        {
            nodePosition = position;
            Parent = parent;
        }
        public void Move(Point pos, Size size)
        {
            int offset = 7;
            switch(nodePosition)
            {
                case NodePosition.Left:
                    position = new Point(pos.X - Size.Width + offset, pos.Y - Size.Height / 2 + size.Height / 2);
                    break;
                case NodePosition.Right:
                    position = new Point(pos.X + size.Width - offset, pos.Y - Size.Height / 2 + size.Height / 2);
                    break;
                case NodePosition.Top:
                    position = new Point(pos.X - Size.Width / 2 + size.Width / 2, pos.Y - Size.Height + offset);
                    break;
                case NodePosition.Bottom:
                    position = new Point(pos.X - Size.Width / 2 + size.Width / 2, pos.Y + size.Height - offset);
                    break;
            }
        }
    }
    enum NodePosition
    {
        Left,
        Right,
        Top,
        Bottom
    }
}