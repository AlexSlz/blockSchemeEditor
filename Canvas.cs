using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace blockSchemeEditor
{
    internal class Canvas
    {
        public List<ElementObject> Elements = new List<ElementObject>();
        public List<Line> Lines = new List<Line>();
        public ElementObject selectedItem { get; set; }
        public Node selectedNode { get; set; }

        public void Click(Point p)
        {
            selectedItem = Elements.Find(item =>
            {
                return item.DetectElementCollision(p);
            });
            if(selectedItem != null)
                return;

            foreach (var item in Elements)
            {
                if (selectedNode == null)
                    selectedNode = item.DetectNodeCollision(mousePos);
            }
        }
        private Node secondNode = null;
        public void OnMouseUp()
        {   
            if (selectedNode != null && secondNode != null)
            {
                Lines.Add(new Line(selectedNode, secondNode));
            }
        }

        public void ConnectNode()
        {
            secondNode = null;
            foreach (var item in Elements)
            {
                bool thisElement = false;
                item.Nodes.ForEach(node =>
                {
                    if (node == selectedNode)
                        thisElement = true;
                });
                if (secondNode == null && !thisElement)
                    secondNode = item.DetectNodeCollision(mousePos);
            }
        }


        Point mousePos;
        public void OnMove(Point mousePos)
        {
            this.mousePos = mousePos;
        }

        public void Render(Bitmap bmp, Color color)
        {
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(color);
                Elements.ForEach(item =>
                {
                    item.DrawElement(gfx);
                    item.DrawNodes(mousePos, gfx);
                });
                Lines.ForEach(line =>
                {
                    line.Draw(gfx);
                });
            }
        }

        public void ResetSelected()
        {
            selectedItem = null;
            selectedNode = null;
            secondNode = null;
        }

        public Node FindNode(string nodeDes)
        {
            ElementObject tempObject = Elements.Find(item => item.Name == nodeDes.Split('.')[0]);
            Node node = tempObject.Nodes.Find(item => item.nodePosition == (NodePosition)Enum.Parse(typeof(NodePosition), nodeDes.Split('.')[1]));
            return node;
        }

    }
}
