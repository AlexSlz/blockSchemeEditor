using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor
{
    internal class Line
    {
        List<Point> points;

        public Line(Point first, Point second)
        {
            points = new List<Point>()
            {
                first, second
            };
        }


        public void Draw(Graphics g)
        {
            Point[] ptarray = points.ToArray();

            Pen pengraph = new Pen(Color.Black, 1);
            g.DrawLines(pengraph, ptarray);
        }

    }
}
