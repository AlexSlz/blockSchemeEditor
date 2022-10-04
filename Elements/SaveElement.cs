using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace blockSchemeEditor
{
    [Serializable]
    internal class SaveElement
    {
        [Serializable]
        public struct StructElement
        {
            public string Id;
            public ElementParameter parameter;
            public string elementData;
        }
        [Serializable]
        public struct StructLine
        {
            public string firstNodeId;
            public string firstNodePos;
            public string secondNodeId;
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
