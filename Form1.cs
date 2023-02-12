using blockSchemeEditor.Actions;
using blockSchemeEditor.Elements;
using blockSchemeEditor.Elements.ElementsData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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
            new Hexagon(),
            new Actor(),
            new Triangle(),
            new Star(),
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
            _panelActions = new PanelActions(panel1);
            InitListBox();

            if (args.Length > 0)
            {
                _fileSystem.Import(args[0]);
                this.FormClosed += Form1_FormClosed;
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

            _canvas.Elements.Add(new ElementObject(new Point(500, 200), new Star()));


            /*            Random rand = new Random();
                        for (int i = 0; i < 10; i++)
                        {
                            _canvas.Elements.Add(new ElementObject(new Point(rand.Next(200, 500), rand.Next(200, 500)), elements[rand.Next(0, elements.Count - 1)]));
                        }*/

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show($"Save File, {_fileSystem.FileName}?", "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question); ;

            if (dialogResult == DialogResult.Yes)
                _fileSystem.CreateFile(_fileSystem.FilePath);
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
                //_panelActions.DisplayElementOnPanel(_canvas.lastSelectedElement);
                _panelActions.DisplayElementsOnPanel(_canvas.selectedItems);
            }
            else {
                panel1.Show();
                _panelActions.DisplaySettingsPanel(pictureBox1);
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
                Text = $"{((_fileSystem.FilePath == null) ? "blockShemeEditor" : _fileSystem.FileName)} - {elapsedSec * 1000:0.00} ms ({1 / elapsedSec:0.00} FPS)";
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
            
            DialogResult dialogResult = MessageBox.Show($"Delete, {selectedItemText}?", ":)", MessageBoxButtons.YesNo, MessageBoxIcon.Question); ;

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
            this.FormClosed += Form1_FormClosed;
        }
        private void pictureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDialog("Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf", (fileName) =>
            {
                Rectangle crop = new Rectangle(_canvas.Elements[0].Parameters.Position, _canvas.Elements[0].Parameters.CustomSize);

                _canvas.Elements.ForEach(element =>
                {
                    crop.X = Math.Min(crop.X, element.Parameters.Position.X);
                    crop.Y = Math.Min(crop.Y, element.Parameters.Position.Y);
                    crop.Width = Math.Max(crop.Width, element.Parameters.Position.X);
                    crop.Height = Math.Max(crop.Height, element.Parameters.Position.Y);
                });

                var result = new Bitmap(crop.Width, crop.Height);
                using (var gr = Graphics.FromImage(result))
                {
                    gr.DrawImage(pictureBox1.Image, new Rectangle(0, 0, crop.Width, crop.Height), crop, GraphicsUnit.Pixel);
                }
                result.Save(fileName);
                MessageBox.Show($"File created\n{fileName}");
            });
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDialog("blockSheme (.block)|*.block", (fileName) =>
            {
                _fileSystem.CreateFile(fileName);
                MessageBox.Show($"File created\n{fileName}");
                this.FormClosed += Form1_FormClosed;
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
