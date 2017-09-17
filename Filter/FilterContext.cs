using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    /// <summary>
    /// 过滤器上下文
    /// </summary>
    public class FilterContext
    {
        private List<ExecutingFilterAttribute> _executingFilters = new List<ExecutingFilterAttribute>();
        private List<ExecutedFilterAttribute> _executedFileters = new List<ExecutedFilterAttribute>();
        private List<ErrorFilterAttribute> _executingError = new List<ErrorFilterAttribute>();

        private void Init(object[] filters)
        {
            this._executingFilters.Clear();
            this._executedFileters.Clear();
            this._executingError.Clear();
            if (filters != null && filters.Length > 0)
            {
                foreach (var filter in filters)
                {
                    if (filter is ExecutingFilterAttribute)
                    {
                        this._executingFilters.Add(filter as ExecutingFilterAttribute);
                        continue;
                    }
                    if (filter is ExecutedFilterAttribute)
                    {
                        this._executedFileters.Add(filter as ExecutedFilterAttribute);
                        continue;
                    }
                    if (filter is ErrorFilterAttribute)
                    {
                        this._executingError.Add(filter as ErrorFilterAttribute);
                        continue;
                    }
                }
            }
        }


        public void Call(Action action)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            try
            {
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(null);
                });

                //执行
                action?.Invoke();
            }
            catch (Exception ex)
            {
                //异常
                if (this._executingError == null || this._executingError.Count == 0)
                    throw;
                else
                    this._executingError?.ForEach(p => p.Execute(null, ex));
            }
            finally
            {
                //执行后
                this._executedFileters?.ForEach(p => p.Execute(null, 0));
            }
        }

        public void Call<T>(Action<T> action, T p1)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1);
            }
            catch (Exception ex)
            {
                //异常
                if (this._executingError == null || this._executingError.Count == 0)
                    throw;
                else
                    this._executingError?.ForEach(p => p.Execute(methodParameters, ex));
            }
            finally
            {
                //执行后
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, 0));
            }
        }

        public void Call<T1, T2>(Action<T1, T2> action, T1 p1, T2 p2)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2);
            }
            catch (Exception ex)
            {
                //异常
                if (this._executingError == null || this._executingError.Count == 0)
                    throw;
                else
                    this._executingError?.ForEach(p => p.Execute(methodParameters, ex));
            }
            finally
            {
                //执行后
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, 0));
            }
        }


        /// <summary>
        /// 创建方法参数对象
        /// </summary>
        /// <param name="invoked">被调用者</param>
        /// <param name="args">实际参数</param>
        /// <returns>方法对象</returns>
        private MethodParameters[] CreateMethodParameters(MethodInfo invoked, params object[] args)
        {
            var parametersOfInvoked = invoked.GetParameters();

            var methodParameters = new MethodParameters[parametersOfInvoked.Length];

            var i = -1;
            foreach (var paramInfo in parametersOfInvoked)
            {
                i++;
                var genericType = typeof(MethodParameter<>).MakeGenericType(paramInfo.ParameterType);
                var methodParamter = Activator.CreateInstance(genericType);
                var props = genericType.GetProperties();
                foreach (var prop in props)
                {
                    if (prop.Name == "Type")
                    {
                        prop.SetValue(methodParamter, paramInfo.ParameterType);
                        continue;
                    }
                    if (prop.Name == "Name")
                    {
                        prop.SetValue(methodParamter, parametersOfInvoked[i].Name);
                        continue;
                    }
                    if (prop.Name == "Value")
                    {
                        prop.SetValue(methodParamter, args?.ElementAtOrDefault(i));
                    }
                }
                methodParameters[i] = methodParamter as MethodParameters;
            }

            return methodParameters;
        }
    }
}
