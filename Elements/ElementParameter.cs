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
        public Point Position;
        public Size CustomSize;
        public Color CustomColor;

        private double _angle = int.MinValue;
        public double Angle
        {
            get { if (_angle >= 110) return 110; if (_angle <= 0 && _angle != int.MinValue) return 1; return _angle; }
            set { _angle = value; }
        }

        private double _size = int.MinValue;
        public double FontSize
        {
            get { if (_size >= 90) return 90; if (_size < 4 && _size != int.MinValue) return 4; return _size; }
            set { _size = value; }
        }
        public List<string> HideList = new List<string>();
    }
}
