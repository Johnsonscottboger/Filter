using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter.Model;

namespace Filter.Attributs
{
    /// <summary>
    /// 方法执行结束过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public abstract class ExecutedFilterAttribute : FilterAttribute
    {
        /// <summary>
        /// 运用此注解的方法，执行结束时
        /// </summary>
        /// <typeparam name="TReturn">方法返回类型</typeparam>
        /// <param name="parameters">方法参数</param>
        /// <param name="returned">方法返回值</param>
        public abstract void Execute<TReturn>(MethodParameters[] parameters, TReturn returned);
    }
}
