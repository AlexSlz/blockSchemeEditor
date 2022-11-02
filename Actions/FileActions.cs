using blockSchemeEditor.Elements;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal class FileActions
    {
        private Canvas _canvas;
        private BinaryFormatter _formatter;
        private SaveElement save;
        public FileActions(Canvas canvas)
        {
            _formatter = new BinaryFormatter();
            _canvas = canvas;
        }

        public void CreateFile(string path)
        {
            save = new SaveElement();
            ExportElements();
            ExportLines();

            using(FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                _formatter.Serialize(fs, save);
                fs.Close();
            }
        }
        private void ExportElements()
        {
            _canvas.Elements.ForEach(element =>
            {
                save.elements.Add(new SaveElement.StructElement
                {
                    Id = element.Id,
                    parameter = element.Parameters,
                    elementData = element.elementData.Name
                });
            });
        }
        private void ExportLines()
        {
            _canvas.Lines.ForEach(line =>
            {
                save.lines.Add(new SaveElement.StructLine
                {
                    firstNodeId = line.FirstNode.Parent.Id,
                    firstNodePos = line.FirstNode.nodePosition.ToString(),
                    secondNodeId = line.SecondNode.Parent.Id,
                    secondNodePos = line.SecondNode.nodePosition.ToString()
                });
            });
        }

        public void Import(string path)
        {
            save = new SaveElement();
            if (File.Exists(path))
            {
                _canvas.ClearElements();
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
                _canvas.AddElement(new ElementObject(item.parameter.Position, Form1.elements.Find(element => item.elementData == element.Name), item.Id, item.parameter));
            });
        }

        private void LoadLines()
        {
            save.lines.ForEach(line =>
            {
                Node firstNode = FindNode(line.firstNodeId, line.firstNodePos);
                Node secondNode = FindNode(line.secondNodeId, line.secondNodePos);

                if(firstNode != null && secondNode != null)
                    _canvas.Lines.Add(new Line(firstNode, secondNode));
            });
        }

        private Node FindNode(string elementId, string nodePos)
        {
            foreach (var item in _canvas.Elements)
            {
                if(elementId == item.Id)
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
