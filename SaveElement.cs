using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor
{
    [Serializable]
    internal class SaveElement
    {
        [Serializable]
        public struct StructElement
        {
            public string Name;
            public string Description;
            public Point point;
            public string elementData;
        }
        [Serializable]
        public struct StructLine
        {
            public string firstNodeName;
            public string firstNodePos;
            public string secondNodeName;
            public string secondNodePos;
        }

        public List<StructElement> elements;
        public List<StructLine> lines;

        public SaveElement()
        {
            elements = new List<StructElement>();
            lines = new List<StructLine>();
        }
    }
}
