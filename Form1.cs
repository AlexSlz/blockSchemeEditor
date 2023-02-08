using blockSchemeEditor.Actions;
using blockSchemeEditor.Elements;
using blockSchemeEditor.Elements.ElementsData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace blockSchemeEditor
{
    public partial class Form1 : Form
    {
        internal static List<IElement> elements = new List<IElement>()
        {
            new Text(),
            new MyRectangle(),
            new Ellipse(),
            new RoundedRectangle(),
            new Parallelogram(),
            new Hexagon()
        };
        private Canvas _canvas;
        private FileActions _fileSystem;
        private PanelActions _panelActions;

        public Form1(string[] args)
        {
            InitializeComponent();
            panel1.Hide();

            _canvas = new Canvas();
            _fileSystem = new FileActions(_canvas);
            _panelActions = new PanelActions(panel1, _canvas);
            InitListBox();

            if (args.Length > 0)
            {
                _fileSystem.Import(args[0]);
            }


            if (SystemActions.InitRegEdit())
            {
                ToolStripMenuItem deleteRegEditButton = new ToolStripMenuItem();
                deleteRegEditButton.Text = "Delete RegEdit Items.";
                deleteRegEditButton.Click += (object sender, EventArgs e) =>
                {
                    SystemActions.DeleteRegEdit();
                    contextMenuStrip1.Items.Remove(deleteRegEditButton);
                };
                contextMenuStrip1.Items.Add(deleteRegEditButton);
            }

        }

        private void InitListBox()
        {
            elements.ForEach(item => {
                listBox1.Items.Add(item.Name);
            });
            _canvas.ElementsChanged += UpdateElementList;
        }

        private bool _mouseDownOnCanvas = false;
        private Point _oldPos;
        private List<Point> _elementsOldPos = new List<Point>();
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _oldPos = e.Location;
            _mouseDownOnCanvas = true;

            var detect = _canvas.Click(new Point(e.X, e.Y));

            if (!detect)
            {
                _canvas.ClearSelection();
                listBox2.ClearSelected();
                panel1.Hide();
                panel1.Controls.Clear();
            }
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            _canvas.OnMove(e.Location);
            if (_mouseDownOnCanvas && _canvas.selectedItems.Count > 0)
            {
                Cursor.Current = Cursors.SizeAll;
                _canvas.selectedItems.Move(e.Location, _oldPos);
            }

            if (_mouseDownOnCanvas && _canvas.selectedNode != null)
            {
                _canvas.ConnectNode();
            }
            if (_listBoxSelectIndex != -1)
            {
                ElementObject newElement = new ElementObject(e.Location, elements[_listBoxSelectIndex]);
                newElement.Parameters.Index = _canvas.Elements.Count;
                _canvas.AddElement(newElement);
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
        private void listBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if(listBox1.Size.Width >= e.Location.X)
            {
                listBox1.ClearSelected();
                _listBoxSelectIndex = -1;
            }
        }

        private void OpenParametersPanel()
        {
            if (panel1.Visible)
            {
                panel1.Hide();
                panel1.Controls.Clear();
            }
            if (_canvas.lastSelectedElement != null)
            {
                panel1.Show();
                _panelActions.InitPanel(_canvas.lastSelectedElement.Parameters);
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenParametersPanel();
        }

        Stopwatch stopwatch = new Stopwatch();
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pictureBox1.Width + pictureBox1.Height > 0 && _canvas != null)
            {
                stopwatch.Restart();
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                _canvas.Render(bmp, pictureBox1.BackColor);
                pictureBox1.Image?.Dispose();
                pictureBox1.Image = bmp;
                double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
                Text = $"blockShemeEditor - {elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} FPS)";
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            string selectedItemText = "All";
            List<ElementObject> elementTODelete = _canvas.selectedItems;

            if (elementTODelete.Count > 0)
                selectedItemText = elementTODelete.Count + " Elements";
            if (_canvas.selectedNode != null)
                selectedItemText = $"Node{_canvas.selectedNode.nodePosition} Line";
            
            DialogResult dialogResult = MessageBox.Show($"Delete, {selectedItemText}? {elementTODelete.Count} | {_canvas.selectedNode == null}", ":)", MessageBoxButtons.YesNo, MessageBoxIcon.Question); ;

            if (_canvas.selectedNode == null && elementTODelete.Count <= 0 && dialogResult == DialogResult.Yes)
                _canvas.ClearElements();

            if (_canvas.selectedNode != null && dialogResult == DialogResult.Yes)
                ElementActions.DeleteNode(_canvas, _canvas.selectedNode);
            if (elementTODelete != null && dialogResult == DialogResult.Yes)
                _canvas.DeleteElements(elementTODelete);


            panel1.Hide();
            panel1.Controls.Clear();
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "blockSheme (.block)|*.block";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;
            _fileSystem.Import(filename);
        }
        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDialog("Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf", (fileName) =>
            {
                _fileSystem.CreateFile(fileName);
                MessageBox.Show($"File created\n{fileName}");
            });
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDialog("blockSheme (.block)|*.block", (fileName) =>
            {
                _fileSystem.CreateFile(fileName);
                MessageBox.Show($"File created\n{fileName}");
            });
        }

        private void SaveDialog(string filter, Action<string> callBack)
        {
            saveFileDialog1.Filter = filter;
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                try
                {
                    callBack(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateElementList(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            _canvas.Elements.ForEach(item => {
                listBox2.Items.Add($"{item.elementData.Name}-{item.Id}");
            });
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.Items.Count > 0)
                OpenParametersPanel();
        }
        private void listBox2_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count > 0)
                _canvas.selectedItems = new List<ElementObject>() { (_canvas.Elements.Find(item => item.Id == listBox2.Items[listBox2.SelectedIndex].ToString().Split('-')[1])) };
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            List<string> files = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();
            string currentFile = files.Find(item => item.Contains(".block"));
            if (currentFile != null)
                _fileSystem.Import(currentFile);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
    }
}
