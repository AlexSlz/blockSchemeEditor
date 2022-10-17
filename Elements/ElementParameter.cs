using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements
{
    [Serializable]
    internal class ElementParameter
    {
        public ElementParameter() { }

        public string Text;
        private int _index = 0;
        public int Index { get { return _index; } set { _index = value; } }
        public bool PolyLine = true;
        public Point Position;
        public Size CustomSize;
        public Color CustomColor;

        private double _angle = int.MinValue;
        public double Angle
        {
            get { return MinMax(1, 110, _angle); }
            set { _angle = value; }
        }

        private double _size = int.MinValue;
        public double FontSize
        {
            get { return MinMax(4, 90, _size); }
            set { _size = value; }
        }
        private int _verticesCount = int.MinValue;
        public int VerticesCount { get { return (int)MinMax(4, 9, _verticesCount); } 
                                   set { _verticesCount = value; } }

        public List<string> HideList = new List<string>();

        private double MinMax(int min, int max, double baseValue)
        {
            if (baseValue >= max) 
                return max; 
            if (baseValue < min && baseValue != int.MinValue) 
                return min; 
            return baseValue;
        }
    }
}
