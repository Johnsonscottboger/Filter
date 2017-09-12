using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public class ControllerBase : IController
    {
        public virtual void OnError(Exception ex)
        {

        }

        public virtual void OnExecuted()
        {

        }

        public virtual void OnExecuting()
        {

        }
    }
}
