using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal class MyFileSystem
    {
        private Canvas _canvas;
        private BinaryFormatter _formatter;
        private SaveElement save;
        public MyFileSystem(Canvas canvas)
        {
            _formatter = new BinaryFormatter();
            _canvas = canvas;
        }


        public void CreateFile(string path)
        {
            save = new SaveElement();
            _canvas.Elements.ForEach(element =>
            {
                save.elements.Add(new SaveElement.StructElement { Name = element.Name, Description = element.Description, 
                                 elementData = element.elementData.Name, point = element.Position });
            });
            _canvas.Lines.ForEach(line =>
            {
                save.lines.Add(new SaveElement.StructLine { firstNodeName = line.FirstNode.Parent.Name, 
                                                            firstNodePos = line.FirstNode.nodePosition.ToString(), 
                                                            secondNodeName = line.SecondNode.Parent.Name, 
                                                            secondNodePos = line.SecondNode.nodePosition.ToString() });
            });


            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                _formatter.Serialize(fs, save);
                fs.Close();
            }
        }

        public void Import(string path)
        {
            save = new SaveElement();
            if (File.Exists(path))
            {
                _canvas.Elements = new List<ElementObject>();
                _canvas.Lines = new List<Line>();
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    save = (SaveElement)_formatter.Deserialize(fs);
                    fs.Close();
                    LoadElements();
                    LoadLines();
                }
            }
        }


        private void LoadElements()
        {
            save.elements.ForEach(item =>
            {
                _canvas.Elements.Add(new ElementObject(item.point, Form1.elements.Find(element => item.Name.Contains(element.Name)), item.Description, 0, item.Name));
            });
        }

        private void LoadLines()
        {
            save.lines.ForEach(line =>
            {
                Node firstNode = FindNode(line.firstNodeName, line.firstNodePos);
                Node secondNode = FindNode(line.secondNodeName, line.secondNodePos);

                if(firstNode != null && secondNode != null)
                    _canvas.Lines.Add(new Line(firstNode, secondNode));
            });
        }

        private Node FindNode(string elementName, string nodePos)
        {
            foreach (var item in _canvas.Elements)
            {
                if(elementName == item.Name)
                {
                    foreach (var node in item.Nodes)
                    {
                        if (node.nodePosition == (NodePosition)Enum.Parse(typeof(NodePosition), nodePos))
                            return node;
                    }
                }
            }
            return null;
        }
    }
}
