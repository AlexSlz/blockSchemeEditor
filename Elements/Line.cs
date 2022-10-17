using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

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
            using (AdjustableArrowCap bigArrow = new AdjustableArrowCap(4, 4))
            using (Pen pen = new Pen(Color.Black, 5))
            {
                Point first = CalculateNodePosition(FirstNode);
                Point second = CalculateNodePosition(SecondNode);

                var (middle, distance) = CalculateMidpointAndDistance(first, second);

                List<Point> points = new List<Point>();

                points.Add(first);
                if (middle.X != 0 && distance > 90)
                {
                    points.Add(middle);
                }
                points.Add(second);

                pen.CustomEndCap = bigArrow;

                graphics.DrawLines(pen, points.ToArray());
            }
        }

        private (Point, double) CalculateMidpointAndDistance(Point first, Point second)
        {
            Point middle = new Point(0, 0);

            if (FirstNode.Parent.Parameters.PolyLine)
            {
                if ((FirstNode.nodePosition == NodePosition.Bottom || FirstNode.nodePosition == NodePosition.Top))
                {
                    middle.X = first.X;
                    middle.Y = second.Y;
                }
                if ((FirstNode.nodePosition == NodePosition.Left || FirstNode.nodePosition == NodePosition.Right))
                {
                    middle.X = second.X;
                    middle.Y = first.Y;
                }
            }

            double distance = Math.Sqrt(Math.Pow(second.X - middle.X, 2) + Math.Pow(second.Y - middle.Y, 2));
            return (middle, distance);
        }

        private Point CalculateNodePosition(Node node)
        {
            return new Point(node.position.X + node.Size.Height / 2, node.position.Y + node.Size.Width / 2);
        }

    }
}
