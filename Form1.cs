using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Process = blockSchemeEditor.Elements.Process;

namespace blockSchemeEditor
{
    public partial class Form1 : Form
    {
        internal static List<IElement> elements = new List<IElement>()
        {
            new Process(),
            new Ellipse()
        };
        private Canvas _canvas;
        private MyFileSystem _fileSystem;

        public Form1()
        {
            InitializeComponent();
            InitListBox();
            _canvas = new Canvas();
            _fileSystem = new MyFileSystem(_canvas);
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
            _canvas.OnMove(e.Location);
            if (_mouseDownOnCanvas && _canvas.selectedItem != null)
            {
                Cursor.Current = Cursors.SizeAll;
                _canvas.selectedItem.Move(e.Location, _oldPos, _elementOldPos);
            }

            if (_mouseDownOnCanvas && _canvas.selectedNode != null)
            {
                _canvas.ConnectNode();
            }

            if (_listBoxSelectIndex != -1 && listBox1.SelectedIndex != -1)
            {
                ElementObject newElement = new ElementObject(e.Location, elements[_listBoxSelectIndex], "", _canvas.Elements.Count);
                _canvas.Elements.Add(newElement);
                _listBoxSelectIndex = -1;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDownOnCanvas = false;
            _canvas.OnMouseUp();
        }

        private int _listBoxSelectIndex = -1;
        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.SizeAll;
            _listBoxSelectIndex = listBox1.SelectedIndex;
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
            if (pictureBox1.Width + pictureBox1.Height >= 100)
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            DialogResult dialogResult = DialogResult.No;
            if (_canvas.selectedNode != null || _canvas.selectedItem != null)
                dialogResult = MessageBox.Show($"Delete, {_canvas.selectedItem?.Description}?", ":)", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (_canvas.selectedNode != null && dialogResult == DialogResult.Yes)
                ElementActions.DeleteNode(_canvas, _canvas.selectedNode);
            if (_canvas.selectedItem != null && dialogResult == DialogResult.Yes)
                _canvas.DeleteElement(_canvas.selectedItem);
        }

        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = SaveDialog("Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf");
            if(filename != null)
                try
                {
                    pictureBox1.Image.Save(filename);
                    MessageBox.Show($"File created\n{filename}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "blockSheme (.block)|*.block";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            _fileSystem.Import(filename);
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = SaveDialog("blockSheme (.block)|*.block");
            if (filename != null)
                try
                {
                    _fileSystem.CreateFile(filename);
                    MessageBox.Show($"File created\n{filename}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private string SaveDialog(string filter)
        {
            saveFileDialog1.Filter = filter;
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return null;
            return saveFileDialog1.FileName;
        }
    }
}
