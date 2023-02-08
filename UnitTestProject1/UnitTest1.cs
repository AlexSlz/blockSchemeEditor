using Microsoft.VisualStudio.TestTools.UnitTesting;
using blockSchemeEditor;
using blockSchemeEditor.Elements;
using System.Drawing;
using blockSchemeEditor.Elements.ElementsData;
using System.IO;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        ElementObject TestElement = new ElementObject(new Point(0, 0), new MyRectangle());
        Canvas canvas = new Canvas();
        private ElementObject CreateElement(IElement prefab)
        {
            ElementObject element = new ElementObject(new Point(0, 0), prefab);
            Assert.AreEqual(element.Parameters.Text, prefab.Name);
            return element;
        }
        private void TestAddElementToCanvas(ElementObject element)
        {
            canvas.Elements.Add(element);
            Assert.AreEqual(canvas.Elements.Find(item => item == element).Id, element.Id);
        }
        [TestMethod]
        public void CreateMyRectangle()
        {
            var element = CreateElement(new MyRectangle());
            TestAddElementToCanvas(element);
        }
        [TestMethod]
        public void CreateEllipse()
        {
            var element = CreateElement(new Ellipse());
            TestAddElementToCanvas(element);
        }
        [TestMethod]
        public void CreateRoundedRectangle()
        {
            var element = CreateElement(new RoundedRectangle());
            TestAddElementToCanvas(element);
        }
        [TestMethod]
        public void CreateHexagon()
        {
            var element = CreateElement(new Hexagon());
            TestAddElementToCanvas(element);
        }
        [TestMethod]
        public void CreateParallelogram()
        {
            var element = CreateElement(new Parallelogram());
            TestAddElementToCanvas(element);
        }
        [TestMethod]
        public void CreateLine()
        {
            var SecondElement = new ElementObject(new Point(0, 0), new MyRectangle());
            Line line = new Line(TestElement.Nodes[0], SecondElement.Nodes[0]);
            Assert.AreEqual(line.FirstNode.Parent, TestElement);
            Assert.AreEqual(line.SecondNode.Parent, SecondElement);
        }
        [TestMethod]
        public void TestAddLineToCanvas()
        {
            var SecondElement = new ElementObject(new Point(0, 0), new MyRectangle());
            Line line = new Line(TestElement.Nodes[0], SecondElement.Nodes[0]);
            canvas.Lines.Add(line);
            Assert.AreEqual(canvas.Lines[0].FirstNode.Parent, TestElement);
            Assert.AreEqual(canvas.Lines[0].SecondNode.Parent, SecondElement);
        }
        [TestMethod]
        public void TestMoveElement()
        {
            Assert.AreEqual(TestElement.Parameters.Position, new Point(0, 0));
            TestElement.Move(new Point(10, 10), new Point(0,0));
            Assert.AreEqual(TestElement.Parameters.Position, new Point(10, 10));
        }
        [TestMethod]
        public void TestDeleteElement()
        {
            var SecondElement = new ElementObject(new Point(0, 0), new MyRectangle());
            canvas.Elements.Add(TestElement);
            canvas.Elements.Add(SecondElement);
            canvas.Lines.Add(new Line(TestElement.Nodes[0], SecondElement.Nodes[0]));
            Assert.AreEqual(canvas.Elements.Count, 2);
            Assert.AreEqual(canvas.Lines.Count, 1);
            canvas.DeleteElement(TestElement);
            Assert.AreEqual(canvas.Elements.Count, 1);
            Assert.AreEqual(canvas.Lines.Count, 0);
        }
        [TestMethod]
        public void TestRenameElement()
        {
            Assert.AreEqual(TestElement.Parameters.Text, new MyRectangle().Name);
            TestElement.Parameters.Text = "Test";
            Assert.AreEqual(TestElement.Parameters.Text, "Test");
        }
        [TestMethod]
        public void TestFileActions()
        {
            var SecondElement = new ElementObject(new Point(0, 0), new MyRectangle());
            canvas.Elements.Add(TestElement);
            canvas.Elements.Add(SecondElement);
            canvas.Lines.Add(new Line(TestElement.Nodes[0], SecondElement.Nodes[0]));

            FileActions fileActions = new FileActions(canvas);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.block");

            fileActions.CreateFile(path);

            Assert.AreEqual(canvas.Elements.Count, 2);
            Assert.AreEqual(canvas.Lines.Count, 1);

            canvas.DeleteElement(TestElement);
            canvas.DeleteElement(SecondElement);

            Assert.AreEqual(canvas.Elements.Count, 0);
            Assert.AreEqual(canvas.Lines.Count, 0);

            fileActions.Import(path);

            Assert.AreEqual(canvas.Elements.Count, 2);
            Assert.AreEqual(canvas.Lines.Count, 1);

            File.Delete(path);
        }
    }
}
