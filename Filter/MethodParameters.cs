using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public class MethodParameters
    {
        
    }

    /// <summary>
    /// 节点
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MethodParameter<T> : MethodParameters
    {
        public Type Type { get; set; }

        public string Name { get; set; }

        public T Value { get; set; }

        public override string ToString()
        {
            return string.Format("Type:{0}; Name:{1}; Value:{2}", Type.ToString(), Name.ToString(), Value.ToString());
        }
    }
}
