using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter;
using Filter.Attributs;
using Filter.Model;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new FilterContext();

            var test = new TestController();

            var m = EmitGenerator<IController>.GenerateProxy(test);
            m.Add(1, 2);
            m.Add(1, 2, 3);
            m.Add(1, 2, 3, 4);
            m.AddException();
            m.HelloWorld();


            Console.WriteLine("End...");
            Console.ReadKey();
        }
    }

    public class TestController : IController
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

        [CustomErrorFilter]
        public int Add(int a, int b, int c, int d)
        {
            throw new NotImplementedException();
        }

        [CustomErrorFilter]
        public int Add(int a, int b, int c, int d, int e)
        {
            throw new NotImplementedException();
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
            if (parameters != null)
                Console.WriteLine($"异常过滤器：{nameof(CustomErrorFilterAttribute)}, Param:{string.Join(", ", parameters?.Select(p => p.ToString()))},Exception:{ex}");
            else
                Console.WriteLine($"异常过滤器：{nameof(CustomErrorFilterAttribute)}, Exception:{ex}");
            Console.ResetColor();
        }
    }
}
