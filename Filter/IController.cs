using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    internal interface IController
    {
        void OnExecuting();

        void OnExecuted();

        void OnError(Exception ex);
    }
}
