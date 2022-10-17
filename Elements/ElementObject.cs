using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements
{
    internal class ElementObject
    {
        public string Id { get; private set; }

        public ElementParameter Parameters = null;

        public List<Node> Nodes;
        private int nodeOffset = 7;
        public IElement elementData { get; private set; }

        public ElementObject(Point startPos, IElement elementData)
        {
            Id = Guid.NewGuid().ToString("N");
            this.elementData = elementData;
            Parameters = elementData.Parameters;
            Parameters.Position = startPos;
            InitNodes();
        }
        public ElementObject(Point startPos ,IElement elementData, string id, ElementParameter parameter) : this(startPos, elementData)
        {
            Id = id;
            this.elementData = elementData;
            Parameters = parameter;
            InitNodes();
        }

        private void InitNodes()
        {
            Nodes = new List<Node>() {
                new Node(NodePosition.Left, this),
                new Node(NodePosition.Right, this),
                new Node(NodePosition.Top, this),
                new Node(NodePosition.Bottom, this)
            };
            Nodes.ForEach(item =>
            {
                item.Move(Parameters.Position, Parameters.CustomSize);
            });
        }

        public void DrawElement(Graphics g, bool selected = false)
        {
            elementData.Draw(g, Parameters);
            if (elementData.Name != "Text")
                DrawText(g);

            if (selected)
                DrawFrame(g);
        }
        private void DrawText(Graphics g)
        {
            using (StringFormat sf = new StringFormat())
            using (SolidBrush TextClr = new SolidBrush(Color.Black))
            using (Font drawFont = new Font("Microsoft Sans Serif", 14))
            {
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                g.DrawString((Parameters.Text == "") ? elementData.Name : Parameters.Text, drawFont, TextClr,
                new Rectangle(Parameters.Position, Parameters.CustomSize), sf);
            }
        }
        private void DrawFrame(Graphics g)
        {
            using(Pen pen = new Pen(Color.Aqua, 3))
            {
                g.DrawRectangle(pen, new Rectangle(Parameters.Position, Parameters.CustomSize));
            }
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
        public bool DetectElementCollision(Point mousePos)
        {
            return DetectCollision(new System.Drawing.Rectangle(Parameters.Position, Parameters.CustomSize), mousePos);
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
}
