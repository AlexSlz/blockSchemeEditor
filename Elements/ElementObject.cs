using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements
{
    internal class ElementObject
    {
        public string Id { get; private set; }
        public string Description { get; set; }
        public Point Position;

        public List<Node> Nodes;
        private int nodeOffset = 7;
        public IElement elementData { get; private set; }

        Font drawFont = new Font("Microsoft Sans Serif", 14);
        public ElementObject(Point startPos ,IElement elementData, string description = "", string id = "")
        {
            Id = (id == "") ? Guid.NewGuid().ToString("N") : id;
            this.elementData = elementData;
            Description = (description == "") ? elementData.Name : description;
            Position = new Point(startPos.X, startPos.Y);
            Nodes = new List<Node>() {
                new Node(NodePosition.Left, this),
                new Node(NodePosition.Right, this),
                new Node(NodePosition.Top, this),
                new Node(NodePosition.Bottom, this)
            };
            Nodes.ForEach(item =>
            {
                item.Move(Position, elementData.BaseSize);
            });
        }
        SolidBrush TextClr = new SolidBrush(Color.Black);
        public void DrawElement(Graphics g)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            elementData.Draw(g, Position);
            g.DrawString((Description == "") ? elementData.Name : Description, drawFont, TextClr, 
                new System.Drawing.Rectangle(Position.X, Position.Y,
                elementData.BaseSize.Width, elementData.BaseSize.Height), sf);
            DrawDebugPanel(g);
        }
        public void DrawNodes(Point mousePos, Graphics g)
        {
            Nodes.ForEach(node =>
            {
                if (DetectCollision(new System.Drawing.Rectangle(node.position, node.Size), mousePos, nodeOffset))
                {
                    g.FillEllipse(new SolidBrush(Color.Red), new System.Drawing.Rectangle(node.position, node.Size));
                }
            });
        }
        private void DrawDebugPanel(Graphics g)
        {
            string text = $"{Position.X}|{Position.Y}";
            Point p = new Point(Position.X, Position.Y - 20);
            g.FillRectangle(new SolidBrush(Color.White), new System.Drawing.Rectangle(p, TextRenderer.MeasureText(text, drawFont)));
            g.DrawString(text, drawFont, TextClr, p);
        }
        public bool DetectElementCollision(Point mousePos)
        {
            return DetectCollision(new System.Drawing.Rectangle(Position, elementData.BaseSize), mousePos);
        }
        public Node DetectNodeCollision(Point mousePos)
        {
            foreach (var node in Nodes)
            {
                if (DetectCollision(new System.Drawing.Rectangle(node.position, node.Size), mousePos, nodeOffset))
                {
                    return node;
                }
            }
            return null;
        }
        private bool DetectCollision(System.Drawing.Rectangle element, Point mousePos, int offset = 0)
        {
            int x = element.Width + element.X;
            int y = element.Height + element.Y;

            return ((element.X <= mousePos.X + offset && x >= mousePos.X - offset) && (element.Y <= mousePos.Y + offset && y >= mousePos.Y - offset));
        }

    }
/*    internal static class ElementObjectExtension
    {
        public static void Add(this List<ElementObject> elements, ElementObject element, Canvas canvas)
        {
            elements.Add(element);
            canvas.Render();
        }
    }*/
}
