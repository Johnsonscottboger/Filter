using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filter;
using Filter.Attributes;
using Filter.Model;
using Test;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //实例化接口实现类
            var repo = new Repository();
            //动态生成代理
            var repoProxy = EmitGenerator<IRepository>.GenerateProxy(repo);

            //使用代理调用方法
            repoProxy.Add("Hello");
            repoProxy.Add("World");
            repoProxy.Remove("World");
            repoProxy.Update("A,ning", p => p == "World");   //将会抛出列表中不存在项异常








            //var repoGeneric = new RepositoryGeneric<string>();
            //var repoGenProxy = EmitGenerator<IRepositoryGeneric<string>>.GenerateProxy(repoGeneric);
            ////使用代理调用方法
            //repoGenProxy.Add("Hello");
            //repoGenProxy.Add("World");
            //repoGenProxy.Remove("World");
            //repoGenProxy.Update("A,ning", p => p == "World");   //将会抛出列表中不存在项异常



            Console.WriteLine("End...");
            Console.ReadKey();
        }
    }

    public class Repository : IRepository
    {
        private List<string> list = new List<string>();

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Add<T>(T entity)
        {
            list.Add(entity.ToString());
            Console.WriteLine($"Added: Count-{list.Count}");
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Remove<T>(T entity)
        {
            list.Remove(entity.ToString());
            Console.WriteLine($"Removed: Count-{list.Count}");
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Update<T>(T entity, Func<T, bool> predicate)
        {
            var item = list.FirstOrDefault(p => predicate(entity));
            if (item == null)
                throw new Exception("列表中不存在更新的项");
            list.Remove(item);
            list.Add(entity.ToString());
        }
    }

    public class RepositoryGeneric<T> : IRepositoryGeneric<T>
    {
        private List<string> list = new List<string>();

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Add(T entity)
        {
            list.Add(entity.ToString());
            Console.WriteLine($"Added: Count-{list.Count}");
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Remove(T entity)
        {
            list.Remove(entity.ToString());
            Console.WriteLine($"Removed: Count-{list.Count}");
        }

        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Update(T entity, Func<T, bool> predicate)
        {
            var item = list.FirstOrDefault(p => predicate(entity));
            if (item == null)
                throw new Exception("列表中不存在更新的项");
            list.Remove(item);
            list.Add(entity.ToString());
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
            Console.WriteLine("=====================================================================");
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
