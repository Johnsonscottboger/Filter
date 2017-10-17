using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter.Model;

namespace Filter.Attributes
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class ErrorFilterAttribute : FilterAttribute
    {
        /// <summary>
        /// 运用此注解的方法，抛出未处理异常时执行
        /// </summary>
        /// <param name="parameters">方法参数</param>
        /// <param name="ex">异常</param>
        public abstract void Execute(MethodParameters[] parameters, Exception ex);
    }
}
