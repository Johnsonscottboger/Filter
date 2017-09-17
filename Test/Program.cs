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

            context.Call(test.HelloWorld);

            context.Call(test.HelloWorld, "A,ning");

            context.Call(test.HelloWorld, "A,ning", DateTime.Now);


            //context.Process(test.ThrowException);

            Console.WriteLine("End...");
            Print(Console.ReadLine());

            Console.ReadKey();
        }

        public static void Print(string line)
        {
            Console.WriteLine(line);
        }
    }

    public class TestController
    {
        [CustomExecutingFilter]
        public string Name { get; set; }

        [CustomExecutingFilter]
        public void HelloWorld()
        {
            Console.WriteLine("Hello World");
        }

        [CustomExecutingFilter(Name = "名称")]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void HelloWorld(string name)
        {
            Console.WriteLine($"Hello World {name}");
        }

        [CustomExecutingFilter]
        public void HelloWorld(string name, DateTime dateTime)
        {
            Console.WriteLine($"Hello World {name} {dateTime}");
        }

        //[CustomExecutingFilter]
        //[CustomExecutedFilter]
        //[CustomErrorFilter]
        //public void ThrowException()
        //{
        //    throw new ArgumentException("参数异常");
        //}
    }

    /// <summary>
    /// 自定义执行前过滤器
    /// </summary>
    public class CustomExecutingFilterAttribute : ExecutingFilterAttribute
    {
        public string Name { get; set; }

        public override void Execute(MethodParameters[] parameters)
        {
            if(parameters != null)
                Console.WriteLine($"自定义过滤器：{nameof(CustomExecutingFilterAttribute)}, Data:{this.Name}, Param:{string.Join(", ", parameters?.Select(p => p.ToString()))}");
            else
                Console.WriteLine($"自定义过滤器：{nameof(CustomExecutingFilterAttribute)}, Data:{this.Name}");
        }
    }

    /// <summary>
    /// 自定义执行后过滤器
    /// </summary>
    public class CustomExecutedFilterAttribute : ExecutedFilterAttribute
    {
        public override void Execute<TReturn>(MethodParameters[] parameters, TReturn returned)
        {
            Console.WriteLine($"自定义过滤器：{nameof(CustomExecutedFilterAttribute)},Param:{string.Join(", ", parameters?.Select(p => p.ToString()))}, Return:{returned}");
            Console.WriteLine("=====================================================================\r\n");
        }
    }

    /// <summary>
    /// 自定义错误过滤器
    /// </summary>
    public class CustomErrorFilterAttribute : ErrorFilterAttribute
    {
        public override void Execute(MethodParameters[] parameters, Exception ex)
        {
            Console.WriteLine($"自定义过滤器：{nameof(CustomErrorFilterAttribute)}, Param:{string.Join(", ", parameters?.Select(p => p.ToString()))},Exception:{ex}");
        }
    }
}
