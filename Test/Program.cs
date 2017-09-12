using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new FilterContext();

            var test = new TestController();

            context.Process(test.HelloWord, typeof(TestController));

            context.Process(test.ThrowException);

            Console.WriteLine("End...");
            Console.ReadLine();
        }
    }

    public class TestController : ControllerBase
    {
        public override void OnExecuting()
        {
            Console.WriteLine($"{nameof(TestController)}.{nameof(OnExecuting)}");
        }

        public override void OnExecuted()
        {
            Console.WriteLine($"{nameof(TestController)}.{nameof(OnExecuted)}");
        }

        public override void OnError(Exception ex)
        {
            Console.WriteLine($"{nameof(TestController)}.{nameof(OnError)}:{ex.Message}");
        }

        [CustomExecutingFilter]
        [CustomExecutingFilter]
        [CustomExecutedFilter]
        public void HelloWord()
        {
            Console.WriteLine("Hello World");
        }


        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void ThrowException()
        {
            throw new ArgumentException("参数异常");
        }
    }

    public class CustomExecutingFilterAttribute : ExecutingFilterAttribute
    {
        public override void Execute<T>(T data)
        {
            Console.WriteLine($"自定义过滤器：{nameof(CustomExecutingFilterAttribute)}, Data:{data}");
        }
    }

    public class CustomExecutedFilterAttribute : ExecutedFilterAttribute
    {
        public override void Execute<T>(T data)
        {
            Console.WriteLine($"自定义过滤器：{nameof(CustomExecutedFilterAttribute)}, Data:{data}");
        }
    }

    public class CustomErrorFilterAttribute : ErrorFilterAttribute
    {
        public override void Execute<T>(T data, Exception ex)
        {
            Console.WriteLine($"自定义过滤器：{nameof(CustomErrorFilterAttribute)}, Data:{data}, Exception:{ex}");
        }
    }
}
