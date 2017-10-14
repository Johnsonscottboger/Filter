using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Filter
{
    public class EmitGenerator<TInterface>
    {
        private static Type _typeofFilterContext = typeof(FilterContext);
        private static MethodInfo[] _filterContextMethods = _typeofFilterContext.GetMethods();
        private static Dictionary<int, MethodInfo> _filterContextMethodDict = new Dictionary<int, MethodInfo>();
        private static int _hashOfCallA = "CallA".GetHashCode() % 4 * 36;
        private static int _hashOfCallF = "CallF".GetHashCode() % 4 * 36;

        static EmitGenerator()
        {
            foreach (var m in _filterContextMethods)
            {
                _filterContextMethodDict[m.Name.GetHashCode() % 4 * 36 + m.GetParameters().Length] = m;
            }
        }

        public EmitGenerator()
        {
            var type = typeof(TInterface);
            if (!type.IsGenericType)
                throw new TypeInitializationException("传入的泛型参数不是接口类型", null);
        }

        /// <summary>
        /// 生成代理
        /// </summary>
        /// <typeparam name="TImplement">代理接口类型</typeparam>
        /// <param name="implent">代理接口实现类型对象</param>
        /// <returns>代理对象</returns>
        public static TInterface GenerateProxy<TImplement>(TImplement implent) where TImplement : class, TInterface
        {
            var interfaceType = typeof(TInterface);
            var interfaceTypeName = interfaceType.Name;
            var nameOfAssembly = $"{interfaceTypeName}ProxyAssembly";
            var nameofModule = $"{interfaceTypeName}ProxyModule";
            var nameOfType = $"{interfaceTypeName}Proxy";

            var typeofTImplement = typeof(TImplement);

            var assemblyName = new AssemblyName(nameOfAssembly);
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assembly.DefineDynamicModule(nameofModule, $"{nameOfAssembly}.dll");
            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public, null, new Type[] { interfaceType });
            typeBuilder.AddInterfaceImplementation(interfaceType);
            typeBuilder.DefineGenericParameters("TImplement");

            //定义变量
            var fieldFilterContext = typeBuilder.DefineField("_filterContext", _typeofFilterContext, FieldAttributes.Static | FieldAttributes.Private);
            var fieldInstance = typeBuilder.DefineField("_instance", typeofTImplement, FieldAttributes.Private);

            //定义构造方法
            #region 无参构造方法
            var ctorDefault = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            var ilOfCtorDefault = ctorDefault.GetILGenerator();

            ilOfCtorDefault.Emit(OpCodes.Ldarg_0);
            ilOfCtorDefault.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            ilOfCtorDefault.Emit(OpCodes.Ldarg_0);
            ilOfCtorDefault.Emit(OpCodes.Newobj, _typeofFilterContext.GetConstructor(new Type[0]));
            ilOfCtorDefault.Emit(OpCodes.Stfld, fieldFilterContext);

            ilOfCtorDefault.Emit(OpCodes.Ldarg_0);
            ilOfCtorDefault.Emit(OpCodes.Newobj, typeofTImplement.GetConstructor(new Type[0]));
            ilOfCtorDefault.Emit(OpCodes.Stfld, fieldInstance);

            ilOfCtorDefault.Emit(OpCodes.Ret);
            #endregion

            #region 实现接口方法
            var methodsOfInterface = interfaceType.GetMethods();
            var length = methodsOfInterface.Length;
            for (var i = 0; i < length; i++)
            {
                var method = methodsOfInterface[i];
                var methodReturnType = method.ReturnType;
                var methodParamsTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var methodBuilder = typeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, methodReturnType, methodParamsTypes);
                var ilOfMethod = methodBuilder.GetILGenerator();

                ilOfMethod.Emit(OpCodes.Ldarg_0);
                ilOfMethod.Emit(OpCodes.Ldfld, fieldFilterContext);
                ilOfMethod.Emit(OpCodes.Ldarg_0);
                ilOfMethod.Emit(OpCodes.Ldfld, fieldInstance);

                ilOfMethod.Emit(OpCodes.Box, typeofTImplement);
                ilOfMethod.Emit(OpCodes.Dup);
                ilOfMethod.Emit(OpCodes.Ldvirtftn, method);
                MethodInfo desMethod = null;
                ConstructorInfo ctor = null;
                if (methodReturnType == typeof(void))
                {
                    ctor = GetActionType(methodParamsTypes).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
                    desMethod = _filterContextMethodDict[_hashOfCallA + methodParamsTypes.Length + 1];
                    if (desMethod.IsGenericMethod)
                        desMethod = desMethod.MakeGenericMethod(methodParamsTypes);
                }
                else
                {
                    ctor = GetFuncType(methodParamsTypes, methodReturnType).GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
                    desMethod = _filterContextMethodDict[_hashOfCallF + methodParamsTypes.Length + 1];
                    if (desMethod.IsGenericMethod)
                        desMethod = desMethod.MakeGenericMethod(Append(methodParamsTypes, methodReturnType).ToArray());
                }

                ilOfMethod.Emit(OpCodes.Newobj, ctor);

                //加载参数
                var plength = methodParamsTypes.Length;

                for (var p = 0; p < plength; p++)
                    ilOfMethod.Emit(OpCodes.Ldarg_S, (short)(p + 1));

                ilOfMethod.Emit(OpCodes.Callvirt, desMethod);
                ilOfMethod.Emit(OpCodes.Ret);
            }
            #endregion

            var type = typeBuilder.CreateType();
            assembly.Save($"{nameOfAssembly}.dll");
            type = type.MakeGenericType(typeofTImplement);
            return (TInterface)Activator.CreateInstance(type);
        }

        private static Type GetFuncType(Type[] paramsTypes, Type returnType)
        {
            var dict = new Dictionary<int, Type>()
            {
                { 0, typeof(Func<>)},
                { 1, typeof(Func<,>)},
                { 2, typeof(Func<,,>)},
                { 3, typeof(Func<,,,>)},
                { 4, typeof(Func<,,,,>)},
                { 5, typeof(Func<,,,,,>)},
                { 6, typeof(Func<,,,,,,>)},
                { 7, typeof(Func<,,,,,,,>)},
                { 8, typeof(Func<,,,,,,,,>)},
                { 9, typeof(Func<,,,,,,,,,>)},
                { 10, typeof(Func<,,,,,,,,,,>)},
                { 11, typeof(Func<,,,,,,,,,,,>)},
                { 12, typeof(Func<,,,,,,,,,,,,>)},
                { 13, typeof(Func<,,,,,,,,,,,,,>)},
                { 14, typeof(Func<,,,,,,,,,,,,,,>)},
                { 15, typeof(Func<,,,,,,,,,,,,,,,>)},
                { 16, typeof(Func<,,,,,,,,,,,,,,,,>)},
            };

            var type = dict[paramsTypes.Length];
            var types = new Type[paramsTypes.Length + 1];
            //for(var i = 0; i < paramsTypes.Length; i++)
            //{
            //    types[i] = paramsTypes[i];
            //}
            //types[paramsTypes.Length] = returnType;

            types = Append(paramsTypes, returnType).ToArray();
            type = type.MakeGenericType(types);
            return type;
        }

        private static Type GetActionType(Type[] paramsTypes)
        {
            var dict = new Dictionary<int, Type>()
            {
                { 0, typeof(Action)},
                { 1, typeof(Action<>)},
                { 2, typeof(Action<,>)},
                { 3, typeof(Action<,,>)},
                { 4, typeof(Action<,,,>)},
                { 5, typeof(Action<,,,,>)},
                { 6, typeof(Action<,,,,,>)},
                { 7, typeof(Action<,,,,,,>)},
                { 8, typeof(Action<,,,,,,,>)},
                { 9, typeof(Action<,,,,,,,,>)},
                { 10, typeof(Action<,,,,,,,,,>)},
                { 11, typeof(Action<,,,,,,,,,,>)},
                { 12, typeof(Action<,,,,,,,,,,,>)},
                { 13, typeof(Action<,,,,,,,,,,,,>)},
                { 14, typeof(Action<,,,,,,,,,,,,,>)},
                { 15, typeof(Action<,,,,,,,,,,,,,,>)},
                { 16, typeof(Action<,,,,,,,,,,,,,,,>)},
            };

            var type = dict[paramsTypes.Length];
            if (type.IsGenericType)
                type = type.MakeGenericType(paramsTypes);
            return type;
        }

        private static IEnumerable<T> Append<T>(IEnumerable<T> array, T item)
        {
            foreach (var i in array)
                yield return i;
            yield return item;
        }
    }
}
