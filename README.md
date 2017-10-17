## 方法过滤器
### 使用`Emit`和注解属性`Attribute`实现

### 使用方式

### 1. 自定义方法过滤器
可分别定义方法**执行前过滤器**, 方法**执行结束过滤器**, 方法**异常过滤器**

* 执行前过滤器继承 `ExecutingFilterAttribute` 抽象类, 实现 `Execute` 抽象方法, 参数 `parameters` 为运行时拦截方法的参数列表

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
* 执行后过滤器继承 `ExecutedFilterAttribute` 抽象类, 实现 `Execute` 抽象方法,其中泛型参数 `returned` 为拦截方法返回值, 对于无返回值的方法(void),此参数默认为数字 `0`

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
* 异常过滤器继承 `ErrorFilterAttribute` 抽象类, 实现 `Execute` 抽象方法, 其中参数 `ex` 为拦截方法抛出的未处理异常

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


### 2. 定义接口

        public interface IRepository
        {
            void Add<T>(T entity);

            void Remove<T>(T entity);

            void Update<T>(T entity, Func<T, bool> predicate);
        }

### 3. 定义接口实现类, 并在拦截方法上运用自定义过滤器

    public class Repository : IRepository
    {
        private List<string> list = new List<string>();

        //运用过滤器
        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Add<T>(T entity)
        {
            list.Add(entity.ToString());
            Console.WriteLine($"Added: Count-{list.Count}");
        }

        //运用过滤器
        [CustomExecutingFilter]
        [CustomExecutedFilter]
        [CustomErrorFilter]
        public void Remove<T>(T entity)
        {
            list.Remove(entity.ToString());
            Console.WriteLine($"Removed: Count-{list.Count}");
        }

        //运用过滤器
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

### 4. 调用

        //实例化接口实现类
        var repo = new Repository();
        //动态生成代理
        var repoProxy = EmitGenerator<IRepository>.GenerateProxy(repo);

        //使用代理调用方法
        repoProxy.Add("Hello");
        repoProxy.Add("World");
        repoProxy.Remove("World");
        repoProxy.Update("A,ning", p => p == "World");   //将会抛出列表中不存在项异常

### 5. 运行结果

![](http://images2017.cnblogs.com/blog/764262/201710/764262-20171016154754021-166164329.png)
