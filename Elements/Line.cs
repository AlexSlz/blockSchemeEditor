using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Elements
{
    internal class Line
    {
        public Node FirstNode { get; private set; }
        public Node SecondNode { get; private set; }

        public Line(Node firstNode, Node secondNode)
        {
            FirstNode = firstNode;
            firstNode.Lines.Add(this);
            SecondNode = secondNode;
            secondNode.Lines.Add(this);
        }

        public void Draw(Graphics graphics)
        {
            Point x = new Point(FirstNode.position.X + FirstNode.Size.Height / 2, FirstNode.position.Y + FirstNode.Size.Width / 2);
            Point y = new Point(SecondNode.position.X + SecondNode.Size.Height / 2, SecondNode.position.Y + SecondNode.Size.Width / 2);

            graphics.DrawLine(new Pen(Color.Black, 5), x, y);
        }
    }
}
