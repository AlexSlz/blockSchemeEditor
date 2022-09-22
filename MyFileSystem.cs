using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    internal class MyFileSystem
    {

        private Canvas _canvas;
        public MyFileSystem(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void CreateFile(string path)
        {
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    _canvas.Elements.ForEach(item =>
                    {
                        sw.Write($"Name-{item.Description},PositionX-{item.Position.X},PositionY-{item.Position.Y},ElementData-{item.elementData.Name};\n");
                    });
                }
            }
        }

        public void Import(string path)
        {
            try
            {
                string readText = File.ReadAllText(path);
                readText = readText.Remove(readText.Length - 2);
                _canvas.Elements = new List<ElementObject>();
                Parse(readText);
            }
            catch (Exception)
            {
                MessageBox.Show("Import Error!");
            }
        }

        private void Parse(string text)
        {
            List<string> Lines = text.Split(';').ToList();
            Lines.ForEach(line =>
            {
                List<string> param = line.Split(',').ToList();

                string name = FindNeedParam(param, "Name");
                Point pos = new Point(int.Parse(FindNeedParam(param, "PositionX")), int.Parse(FindNeedParam(param, "PositionY")));

                IElement elementBase = Form1.elements.Find(item => item.Name == FindNeedParam(param, "ElementData"));

                _canvas.Elements.Add(new ElementObject(pos, elementBase, name));

            });
        }

        private static string FindNeedParam(List<string> param, string name)
        {
            return param.ToList().Find(item => item.Contains(name)).Split('-')[1];
        }

    }
}
