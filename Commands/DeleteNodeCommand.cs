using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Commands
{
    internal class DeleteNodeCommand : ICommand
    {
        public string CommandName => "Delete Node";

        private Canvas _canvas;
        private Node _node;
        private List<Line> lines;

        public DeleteNodeCommand(Canvas canvas, Node node)
        {
            _canvas = canvas;
            _node = node;
        }

        public void Execute()
        {
            lines = _canvas.DeleteNode(_node);
            
        }

        public void Undo()
        {
            _canvas.Lines.AddRange(lines);
        }
    }
}
