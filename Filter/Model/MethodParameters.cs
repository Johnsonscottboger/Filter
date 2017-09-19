using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter.Model
{
    /// <summary>
    /// 方法参数
    /// </summary>
    public class MethodParameters
    {
        
    }

    public class MethodParameter<T> : MethodParameters
    {
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 实参
        /// </summary>
        public T Value { get; set; }

        public override string ToString()
        {
            return string.Format("Type:{0}; Name:{1}; Value:{2}", Type.ToString(), Name.ToString(), Value.ToString());
        }
    }
}
