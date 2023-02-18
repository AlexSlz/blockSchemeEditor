using blockSchemeEditor.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockSchemeEditor.Commands
{
    internal interface ICommand
    {
        string CommandName { get; }
        void Execute();
        void Undo();
    }
}
