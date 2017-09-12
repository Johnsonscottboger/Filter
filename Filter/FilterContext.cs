using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public class FilterContext
    {
        public void Process(Action action, Type controllerType = null)
        {
            var filters = action.Method.GetCustomAttributes(typeof(FilterAttribute), true);
            var executingFilters = new List<ExecutingFilterAttribute>();
            var executedFileters = new List<ExecutedFilterAttribute>();
            var executingError = new List<ErrorFilterAttribute>();

            if(filters != null && filters.Length >0)
            {
                foreach(var filter in filters)
                {
                    if(filter is ExecutingFilterAttribute)
                    {
                        executingFilters.Add(filter as ExecutingFilterAttribute);
                        continue;
                    }
                    if(filter is ExecutedFilterAttribute)
                    {
                        executedFileters.Add(filter as ExecutedFilterAttribute);
                        continue;
                    }
                    if(filter is ErrorFilterAttribute)
                    {
                        executingError.Add(filter as ErrorFilterAttribute);
                        continue;
                    }
                }
            }

            IController controller = null;
            if (controllerType != null)
            {
                controller = Activator.CreateInstance(controllerType) as IController;
            }

            try
            {
                //执行前
                controller?.OnExecuting();
                executingFilters?.ForEach(p => p.Execute<object>(null));

                //执行
                action?.Invoke();

                //执行后
                controller?.OnExecuted();
                executedFileters?.ForEach(p => p.Execute<object>(null));
            }
            catch (Exception ex)
            {
                controller?.OnError(ex);
                executingError?.ForEach(p => p.Execute<object>(null, ex));
            }
        }
    }
}
