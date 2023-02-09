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
        public List<ElementObject> selectedItems = new List<ElementObject>();
        public ElementObject lastSelectedElement = null;
        public Node selectedNode { get; set; }

        public virtual void OnElementsChanged()
        {
            if (ElementsChanged != null) ElementsChanged(this, EventArgs.Empty);
        }

        public bool Click(Point p)
        {
            var temp = Elements.FindAll(item =>
            {
                item.lastPosition = item.Parameters.Position;
                return item.DetectElementCollision(p);
            });

            if (temp.Count > 0)
            {
                SelectElement(temp.Last());
            }

            SelectNode();

            return temp.Count > 0 || selectedNode != null;
        }

        private void SelectElement(ElementObject element)
        {
            lastSelectedElement = element;
            var lastElement = selectedItems.Find(item => item == element);
            if (lastElement == null)
            {
                if (Control.ModifierKeys == Keys.Shift && selectedItems.Count > 0)
                {
                    selectedItems.Add(element);
                }
                else
                {
                    selectedItems = new List<ElementObject> { element };
                }
            }
            else
            {
                lastSelectedElement = lastElement;
            }
        }

        private bool SelectNode()
        {
            foreach (var item in Elements)
            {
                selectedNode = item.DetectNodeCollision(mousePos);
                if (selectedNode != null)
                {
                    selectedItems.Clear();
                    return true;
                }
            }
            return false;
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
                {
                    secondNode = item.DetectNodeCollision(mousePos);
                }
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
                    var find = selectedItems.Find(i => item == i) != null;
                    item.DrawElement(gfx, find);
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

        public void ClearSelection()
        {
            selectedItems.Clear();
            lastSelectedElement = null;
            selectedNode = null;
            secondNode = null;
        }


        public void Order()
        {
            Elements = Elements.OrderBy(x => x.Parameters.Index).ToList();
        }
    }
}
