using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor
{
    internal class Node
    {
        public Point position;
        public Size Size => new Size(30,30);

        public NodePosition nodePosition { get; set; }

        public Node connectNode = null;
        public Node(NodePosition position)
        {
            nodePosition = position;
        }
        public void Move(Point pos, Size size)
        {
            int offset = 7;
            switch(nodePosition)
            {
                case NodePosition.Left:
                    position = new Point(pos.X - Size.Height + offset, pos.Y - Size.Width / 2 + size.Width / 2);
                    break;
                case NodePosition.Right:
                    position = new Point(pos.X + size.Height - offset, pos.Y - Size.Width / 2 + size.Width / 2);
                    break;
                case NodePosition.Top:
                    position = new Point(pos.X - Size.Height / 2 + size.Height / 2, pos.Y - Size.Width + offset);
                    break;
                case NodePosition.Bottom:
                    position = new Point(pos.X - Size.Height / 2 + size.Height / 2, pos.Y + size.Width - offset);
                    break;
            }
        }

/*        public void Swipe(NodePosition position)
        {
            switch (position)
            {
                case NodePosition.Left:
                    nodePosition = NodePosition.Right;
                    break;
                case NodePosition.Right:
                    nodePosition = NodePosition.Left;
                    break;
                case NodePosition.Top:
                    nodePosition = NodePosition.Bottom;
                    break;
                case NodePosition.Bottom:
                    nodePosition = NodePosition.Top;
                    break;
            }
        }*/

    }
    enum NodePosition
    {
        Left,
        Right,
        Top,
        Bottom
    }
}