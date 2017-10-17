using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter.Model;

namespace Filter.Attributes
{
    /// <summary>
    /// 方法执行前过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ExecutingFilterAttribute : FilterAttribute
    {
        /// <summary>
        /// 运用此注解的方法，开始执行事
        /// </summary>
        /// <param name="param">方法参数</param>
        public abstract void Execute(MethodParameters[] param);
    }
}
