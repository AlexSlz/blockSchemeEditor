using blockSchemeEditor;
using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace blockSchemeEditor.Elements
{
    internal class ElementObject
    {
        public string Name { get; private set; }
        public string Description { get; set; }
        public Point Position;

        public List<Node> Nodes = new List<Node>()
        {
            new Node(NodePosition.Left),
            new Node(NodePosition.Right),
            new Node(NodePosition.Top),
            new Node(NodePosition.Bottom)
        };

        private int nodeOffset = 7;

        public IElement elementData { get; private set; }
        Font drawFont = new Font("Microsoft Sans Serif", 14);
        public ElementObject(Point startPos ,IElement elementData, string description = "")
        {
            Name = elementData.Name;
            this.elementData = elementData;
            Description = (description == "") ? Name : description;
            Position = new Point(startPos.X, startPos.Y);
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
            g.DrawString((Description == "") ? Name : Description, drawFont, TextClr, 
                new Rectangle(Position.X, Position.Y,
                elementData.BaseSize.Height, elementData.BaseSize.Width), sf);
            DrawDebugPanel(g);
        }


        public void DrawNodes(Point mousePos, Graphics g)
        {
            Nodes.ForEach(node =>
            {
                if (node.connectNode != null && node.connectNode.connectNode == node)
                    DrawLine(g, node, node.connectNode);

                if (DetectCollision(new Rectangle(node.position, node.Size), mousePos, nodeOffset))
                {
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(node.position, node.Size));
                }
            });
        }

        private void DrawLine(Graphics g, Node first, Node second)
        {
            Point x = new Point(first.position.X + first.Size.Height / 2, first.position.Y + first.Size.Width / 2);
            Point y = new Point(second.position.X + second.Size.Height / 2, second.position.Y + second.Size.Width / 2);

            g.DrawLine(new Pen(Color.Black, 5), x, y);
        }

        private void DrawDebugPanel(Graphics g)
        {
            string text = $"{Position.X}|{Position.Y}";
            Point p = new Point(Position.X, Position.Y - 20);
            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(p, TextRenderer.MeasureText(text, drawFont)));
            g.DrawString(text, drawFont, TextClr, p);
        }

        public bool DetectElementCollision(Point mousePos)
        {
            return DetectCollision(new Rectangle(Position, elementData.BaseSize), mousePos);
        }

        public Node DetectNodeCollision(Point mousePos)
        {
            foreach (var node in Nodes)
            {
                if (DetectCollision(new Rectangle(node.position, node.Size), mousePos, nodeOffset))
                {
                    return node;
                }
            }
            return null;
        }
        
        private bool DetectCollision(Rectangle element, Point mousePos, int offset = 0)
        {
            int x = element.Height + element.X;
            int y = element.Width + element.Y;

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
