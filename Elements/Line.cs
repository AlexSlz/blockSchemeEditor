using System.Drawing;
using System.Drawing.Drawing2D;

namespace blockSchemeEditor.Elements
{
    internal class Line
    {
        public Node FirstNode { get; private set; }
        public Node SecondNode { get; private set; }

        public Line(Node firstNode, Node secondNode)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
        }

        public void Draw(Graphics graphics)
        {
            using(AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4))
            using (Pen pen = new Pen(Color.Black, 5))
            {
                Point first = new Point(FirstNode.position.X + FirstNode.Size.Height / 2, FirstNode.position.Y + FirstNode.Size.Width / 2);
                Point second = new Point(SecondNode.position.X + SecondNode.Size.Height / 2, SecondNode.position.Y + SecondNode.Size.Width / 2);

                pen.CustomEndCap = bigArrow;

                graphics.DrawLine(pen, first, second);
            }
        }
    }
}
