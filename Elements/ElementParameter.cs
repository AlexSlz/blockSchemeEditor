using System;
using System.Drawing;
using System.Windows.Forms;

namespace blockSchemeEditor.Elements
{
    [Serializable]
    internal class ElementParameter
    {
        public ElementParameter() { }

        public string Text;
        public Point Position;
        public Size CustomSize;
        public Color CustomColor;

        private double _angle = int.MinValue;
        [CustomParams(Optional = true)]
        public double Angle
        {
            get { if (_angle >= 110) return 110; if (_angle <= 0 && _angle != int.MinValue) return 1; return _angle; }
            set { _angle = value; }
        }

    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomParams : Attribute
    {
        public bool Optional { get; set; }
    }
}
