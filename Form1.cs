using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Process = blockSchemeEditor.Elements.Process;

namespace blockSchemeEditor
{
    public partial class Form1 : Form
    {
        List<IElement> elements = new List<IElement>()
        {
            new Process()
        };
        private Canvas _canvas;
        public Form1()
        {
            InitializeComponent();
            InitListBox();
            _canvas = new Canvas();

            //_render = new Render(new Size(pictureBox1.Width, pictureBox1.Height), _canvas);

        }

        private void InitListBox()
        {
            elements.ForEach(item => {
                listBox1.Items.Add(item.Name);
            });
        }

        private bool _mouseDownOnCanvas = false;
        private Point _oldPos;
        private Point _elementOldPos;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _canvas.ResetSelected();
            textBox1.Hide();
            _oldPos = e.Location;
            _mouseDownOnCanvas = true;
            _canvas.Click(new Point(e.X, e.Y));

            if (_canvas.selectedItem != null)
                _elementOldPos = _canvas.selectedItem.Position;
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _canvas.OnMove(e.Location, label1);
            if (_mouseDownOnCanvas && _canvas.selectedItem != null)
            {
                Cursor.Current = Cursors.SizeAll;
                _canvas.selectedItem.Move(e.Location, _oldPos, _elementOldPos);
            }

            if (_mouseDownOnCanvas && _canvas.selectedNode != null)
            {
                _canvas.ConnectNode();
            }

            if (_mouseDownOnListBox && listBox1.SelectedIndex != -1)
            {
                ElementObject newElement = new ElementObject(_canvas.Elements.Count, e.Location, elements[listBox1.SelectedIndex]);
                _canvas.Elements.Add(newElement);
                _mouseDownOnListBox = false;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDownOnCanvas = false;
            _canvas.OnMouseUp();
        }

        private bool _mouseDownOnListBox = false;
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
            _mouseDownOnListBox = true;
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _canvas.Click(new Point(e.X, e.Y));
            if (_canvas.selectedItem != null)
            {
                textBox1.Text = (textBox1.Text != "") ? _canvas.selectedItem.Description : _canvas.selectedItem.Name;
                textBox1.Show();
                textBox1.Focus();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_canvas.selectedItem != null)
            {
                _canvas.selectedItem.Description = textBox1.Text;
            }
        }

        Stopwatch stopwatch = new Stopwatch();
        private void timer1_Tick(object sender, EventArgs e)
        {
            stopwatch.Restart();
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _canvas.Render(bmp, pictureBox1.BackColor);
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = bmp;
            double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
            Text = $"A - {elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} FPS)";
        }
    }
}
