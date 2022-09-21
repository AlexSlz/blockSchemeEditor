using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
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

        public List<Line> lines = new List<Line>();

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

        public Node secondNode = null;
        public void OnMouseUp()
        {
            if (selectedNode != null && secondNode != null)
            {
                selectedNode.connectNode = secondNode;
                secondNode.connectNode = selectedNode;
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
        public void OnMove(Point mousePos, Label text)
        {
            this.mousePos = mousePos;

            string temp = "";

            if (selectedNode != null)
                temp += $"+{selectedNode}\n";
            if(secondNode != null)
            {
                temp += $"-{secondNode}\n";
            }
            text.Text = temp;

        }

        public void Render(Bitmap bmp, Color color)
        {
            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                gfx.Clear(color);
                lines.ForEach(item =>
                {
                    item.Draw(gfx);
                });
                Elements.ForEach(item =>
                {
                    item.Draw(gfx);
                    item.DrawNodes(mousePos, gfx, selectedNode);
                });
            }
        }

        public void ResetSelected()
        {
            selectedItem = null;
            selectedNode = null;
            secondNode = null;
        }

    }
}
