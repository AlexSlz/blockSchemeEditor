using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace blockSchemeEditor.Commands
{
    internal class DeleteCommand : ICommand
    {
        public string CommandName => "Delete Element";

        private Canvas _canvas;
        private List<ElementObject> _elements;
        private List<Line> _lines;
        public DeleteCommand(Canvas canvas, List<ElementObject> elements)
        {
            _canvas = canvas;
            _lines = new List<Line>(canvas.Lines);
            _elements = new List<ElementObject>(elements);
        }

        public void Execute()
        {
            _canvas.DeleteElements(_elements);
        }

        public void Undo()
        {
            _elements.ForEach(element =>
            {
                _canvas.AddElement(element);
            });
            _lines.ForEach(line =>
            {
                _canvas.Lines.Remove(line);
                _canvas.Lines.Add(line);
            });
        }
    }
}
