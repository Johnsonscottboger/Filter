using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Filter.Attributs;
using Filter.Model;

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

        /// <summary>
        /// 初始化过滤器
        /// </summary>
        /// <param name="filters">方法过滤器</param>
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

        public void Call<T>(
            Action<T> action,
            T p1)
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

        public void Call<T1, T2>(
            Action<T1, T2> action,
            T1 p1, T2 p2)
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

        public void Call<T1, T2, T3>(
            Action<T1, T2, T3> action,
            T1 p1, T2 p2, T3 p3)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3);
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

        public void Call<T1, T2, T3, T4>(
            Action<T1, T2, T3, T4> action,
            T1 p1, T2 p2, T3 p3, T4 p4)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4);
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

        public void Call<T1, T2, T3, T4, T5>(
            Action<T1, T2, T3, T4, T5> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5);
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

        public void Call<T1, T2, T3, T4, T5, T6>(
            Action<T1, T2, T3, T4, T5, T6> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7>(
            Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
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

        public void Call<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16)
        {
            Init(action.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(action.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                action?.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
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

        public TResult Call<TResult>(Func<TResult> func)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            try
            {
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(null);
                });

                //执行
                result = func.Invoke();
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
                this._executedFileters?.ForEach(p => p.Execute(null, result));
            }
            return result;
        }

        public TResult Call<TResult, T1>(
           Func<T1, TResult> func,
           T1 p1)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2>(
           Func<T1, T2, TResult> func,
           T1 p1, T2 p2)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3>(
           Func<T1, T2, T3, TResult> func,
           T1 p1, T2 p2, T3 p3)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4>(
           Func<T1, T2, T3, T4, TResult> func,
           T1 p1, T2 p2, T3 p3, T4 p4)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4, T5>(
           Func<T1, T2, T3, T4, T5, TResult> func,
           T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4, T5, T6>(
            Func<T1, T2, T3, T4, T5, T6, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7>(
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }

        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
        }


        public TResult Call<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> func,
            T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9, T10 p10, T11 p11, T12 p12, T13 p13, T14 p14, T15 p15, T16 p16)
        {
            TResult result = default(TResult);
            Init(func.Method.GetCustomAttributes(typeof(FilterAttribute), true));
            MethodParameters[] methodParameters = null;
            try
            {
                methodParameters = CreateMethodParameters(func.Method, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
                //执行前
                this._executingFilters?.ForEach(p =>
                {
                    p.Execute(methodParameters);
                });

                //执行
                result = func.Invoke(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16);
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
                this._executedFileters?.ForEach(p => p.Execute(methodParameters, result));
            }
            return result;
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
