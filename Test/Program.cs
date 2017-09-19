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

            var add1 = context.Call(test.Add, 1, 2);
            Console.WriteLine($"Add1 Result: {add1}");

            var add2 = context.Call(test.Add, 1, 2, 3);
            Console.WriteLine($"Add2 Result: {add2}");

            var add3 = context.Call(test.AddException);
            Console.WriteLine($"Add3 Result: {add3}");

            context.Call(test.ThrowException);

            Console.WriteLine("End...");
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
        [CustomExecutedFilter]
        [CustomErrorFilter]
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
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void HelloWorld(string name, DateTime dateTime)
        {
            Console.WriteLine($"Hello World {name} {dateTime}");
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public int Add(int a, int b, int c)
        {
            return a + b + c;
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public int AddException()
        {
            throw new NotSupportedException();
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void ThrowException()
        {
            throw new ArgumentException("参数异常");
        }
    }

    /// <summary>
    /// 自定义执行前过滤器
    /// </summary>
    public class CustomExecutingFilterAttribute : ExecutingFilterAttribute
    {
        public string Name { get; set; }

        public override void Execute(MethodParameters[] parameters)
        {
            if (parameters != null)
                Console.WriteLine($"执行前过滤器：{nameof(CustomExecutingFilterAttribute)}, Data:{this.Name}, Param:{string.Join(", ", parameters?.Select(p => p.ToString()))}");
            else
                Console.WriteLine($"执行前过滤器：{nameof(CustomExecutingFilterAttribute)}, Data:{this.Name}");
        }
    }

    /// <summary>
    /// 自定义执行后过滤器
    /// </summary>
    public class CustomExecutedFilterAttribute : ExecutedFilterAttribute
    {
        public override void Execute<TReturn>(MethodParameters[] parameters, TReturn returned)
        {
            if (parameters != null)
                Console.WriteLine($"执行后过滤器：{nameof(CustomExecutedFilterAttribute)},Param:{string.Join(", ", parameters?.Select(p => p.ToString()))}, Return:{returned}");
            else
                Console.WriteLine($"执行后过滤器：{nameof(CustomExecutedFilterAttribute)},Return:{returned}");
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
            Console.ForegroundColor = ConsoleColor.Red;
            if(parameters != null)
                Console.WriteLine($"异常过滤器：{nameof(CustomErrorFilterAttribute)}, Param:{string.Join(", ", parameters?.Select(p => p.ToString()))},Exception:{ex}");
            else
                Console.WriteLine($"异常过滤器：{nameof(CustomErrorFilterAttribute)}, Exception:{ex}");
            Console.ResetColor();
        }
    }
}
