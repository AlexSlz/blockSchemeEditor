using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal class Canvas
    {
        public List<ElementObject> Elements = new List<ElementObject>();
        public event System.EventHandler ElementsChanged;
        public List<Line> Lines = new List<Line>();
        public ElementObject selectedItem { get; set; }
        public Node selectedNode { get; set; }

        public virtual void OnElementsChanged()
        {
            if (ElementsChanged != null) ElementsChanged(this, EventArgs.Empty);
        }

        public void Click(Point p)
        {
            var temp = Elements.FindAll(item =>
            {
                return item.DetectElementCollision(p);
            });

            if (temp.Count > 0)
                selectedItem = temp.Last();

            if (selectedItem != null)
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
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                gfx.Clear(color);
                Elements.ForEach(item =>
                {
                    item.DrawElement(gfx, item == selectedItem);
                    item.DrawNodes(mousePos, gfx);
                });
                Lines.ForEach(line =>
                {
                    line.Draw(gfx);
                });
                Order();
            }
        }

        public void ClearElements()
        {
            Elements = new List<ElementObject>();
            Lines = new List<Line>();
            OnElementsChanged();
        }

        public void ResetSelected()
        {
            selectedItem = null;
            selectedNode = null;
            secondNode = null;
        }
        public void Order()
        {
            Elements = Elements.OrderBy(x => x.Parameters.Index).ToList();
        }
    }
}
