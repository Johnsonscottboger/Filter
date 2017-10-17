using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Filter.Attributes;
using Filter.Model;

namespace Filter
{
    /// <summary>
    /// 使用Emit生成器
    /// </summary>
    public class EmitGenerator<TInterface>
    {
        /// <summary>
        /// 生成代理
        /// </summary>
        /// <typeparam name="TInterface">代理对象需要实现的接口</typeparam>
        /// <typeparam name="TImplement">代理目标对象类型</typeparam>
        /// <param name="implement">代理目标对象实例</param>
        /// <returns>实现了代理目标所实现的接口的代理对象</returns>
        public static TInterface GenerateProxy<TImplement>(TImplement implement) where TImplement : class, TInterface
        {
            if (implement == null)
                throw new ArgumentNullException(nameof(implement));
            var interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
                throw new ArgumentException("传入的TInterface参数不是interface");
            var interfaceTypeName = interfaceType.Name;
            var nameOfAssembly = $"Filter.{interfaceTypeName}ProxyAssembly";
            var nameofModule = $"Filter.{interfaceTypeName}ProxyModule";
            var nameOfType = $"Filter.{interfaceTypeName}Proxy";
            var typeofTImplement = typeof(TImplement);
            if (!typeofTImplement.IsClass)
                throw new ArgumentException("传入的TImplement参数不是class");

            var assemblyName = new AssemblyName(nameOfAssembly);
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            var moduleBuilder = assembly.DefineDynamicModule(nameofModule, $"{nameOfAssembly}.dll");
            var typeBuilder = moduleBuilder.DefineType(nameOfType, TypeAttributes.Public, null, new Type[] { interfaceType });
            var genericTypeParamBuilds = typeBuilder.DefineGenericParameters("TImplement");


            //定义泛型约束
            for (var i = 0; i < genericTypeParamBuilds.Length; i++)
            {
                var genTypeParamBuilder = genericTypeParamBuilds[i];
                genTypeParamBuilder.SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint);
                genTypeParamBuilder.SetInterfaceConstraints(interfaceType);
            }


            //定义变量
            var fieldInstance = typeBuilder.DefineField("_instance", interfaceType, FieldAttributes.Private);
            //定义构造方法
            #region 无参构造方法
            var ctorDefault = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, null);
            var ilOfCtorDefault = ctorDefault.GetILGenerator();

            ilOfCtorDefault.Emit(OpCodes.Ldarg_0);
            ilOfCtorDefault.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

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
                var hasReturnValue = methodReturnType != typeof(void);
                var methodParamsTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();
                var methodBuilder = typeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual, CallingConventions.Standard, methodReturnType, methodParamsTypes);
                if (method.IsGenericMethod)
                {
                    methodBuilder.DefineGenericParameters(method.GetGenericArguments().Select(p => p.Name).ToArray());
                }
                var ilOfMethod = methodBuilder.GetILGenerator();
                #region 方法内容
                ilOfMethod.Emit(OpCodes.Nop);
                //定义局部变量
                LocalBuilder resultLocal;
                if (hasReturnValue)
                {
                    resultLocal = ilOfMethod.DeclareLocal(methodReturnType);
                    if (methodReturnType.IsValueType)
                    {
                        ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                        ilOfMethod.Emit(OpCodes.Stloc_0);
                    }
                    else
                    {
                        ilOfMethod.Emit(OpCodes.Ldnull);
                        ilOfMethod.Emit(OpCodes.Stloc_0);
                    }
                }
                else
                {
                    resultLocal = ilOfMethod.DeclareLocal(typeof(int));
                }

                ilOfMethod.DeclareLocal(typeof(List<ExecutingFilterAttribute>));
                ilOfMethod.DeclareLocal(typeof(List<ExecutedFilterAttribute>));
                ilOfMethod.DeclareLocal(typeof(List<ErrorFilterAttribute>));

                ilOfMethod.Emit(OpCodes.Newobj, typeof(List<ExecutingFilterAttribute>).GetConstructor(new Type[0]));
                ilOfMethod.Emit(OpCodes.Stloc_1);
                ilOfMethod.Emit(OpCodes.Newobj, typeof(List<ExecutedFilterAttribute>).GetConstructor(new Type[0]));
                ilOfMethod.Emit(OpCodes.Stloc_2);
                ilOfMethod.Emit(OpCodes.Newobj, typeof(List<ErrorFilterAttribute>).GetConstructor(new Type[0]));
                ilOfMethod.Emit(OpCodes.Stloc_3);

                var getTypeFromHandleMI = typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Public | BindingFlags.Static);

                //获取method
                ilOfMethod.Emit(OpCodes.Ldtoken, typeofTImplement);
                ilOfMethod.Emit(OpCodes.Call, getTypeFromHandleMI);

                ilOfMethod.Emit(OpCodes.Ldstr, method.Name);
                ilOfMethod.Emit(OpCodes.Ldc_I4, methodParamsTypes.Length);
                ilOfMethod.Emit(OpCodes.Newarr, typeof(Type));
                for (var p = 0; p < methodParamsTypes.Length; p++)
                {
                    ilOfMethod.Emit(OpCodes.Dup);
                    ilOfMethod.Emit(OpCodes.Ldc_I4, p);
                    ilOfMethod.Emit(OpCodes.Ldtoken, methodParamsTypes[p]);
                    ilOfMethod.Emit(OpCodes.Call, getTypeFromHandleMI);
                    ilOfMethod.Emit(OpCodes.Stelem_Ref);
                }


                ilOfMethod.Emit(OpCodes.Call, typeof(EmitGenerator<>).GetMethod("GetInstanceMethod", BindingFlags.Public | BindingFlags.Static));
                var methodInfoLocal = ilOfMethod.DeclareLocal(typeof(MethodInfo));
                ilOfMethod.Emit(OpCodes.Stloc_S, methodInfoLocal);

                ilOfMethod.Emit(OpCodes.Ldloc_S, methodInfoLocal);
                ilOfMethod.Emit(OpCodes.Ldloc_1);
                ilOfMethod.Emit(OpCodes.Ldloc_2);
                ilOfMethod.Emit(OpCodes.Ldloc_3);

                ilOfMethod.Emit(OpCodes.Call, typeof(EmitGenerator<>).GetMethod("InitFilters", BindingFlags.Public | BindingFlags.Static));
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldnull);
                var methodParametersLocal = ilOfMethod.DeclareLocal(typeof(MethodParameters[]));
                ilOfMethod.Emit(OpCodes.Stloc_S, methodParametersLocal);

                //try
                ilOfMethod.BeginExceptionBlock();
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldloc_S, methodInfoLocal);
                ilOfMethod.Emit(OpCodes.Ldc_I4, methodParamsTypes.Length);

                ilOfMethod.Emit(OpCodes.Newarr, typeof(object));
                int p1 = 1;
                for (int p = 0; p < methodParamsTypes.Length; p++, p1++)
                {
                    var item = methodParamsTypes[p];
                    ilOfMethod.Emit(OpCodes.Dup);
                    ilOfMethod.Emit(OpCodes.Ldc_I4, p);
                    ilOfMethod.Emit(OpCodes.Ldarg, (p1));
                    if (item.IsValueType)
                    {
                        ilOfMethod.Emit(OpCodes.Box, item);  //值类型装箱
                    }
                    ilOfMethod.Emit(OpCodes.Stelem_Ref);
                }
                ilOfMethod.Emit(OpCodes.Call, typeof(EmitGenerator<>).GetMethod("GetMethodParmaters", BindingFlags.Public | BindingFlags.Static));
                ilOfMethod.Emit(OpCodes.Stloc_S, methodParametersLocal);
                //执行前 for 循环
                var iLocal = ilOfMethod.DeclareLocal(typeof(int));  //变量i
                var conditionLable = ilOfMethod.DefineLabel();      //条件
                var trueLable = ilOfMethod.DefineLabel();           //true
                ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                ilOfMethod.Emit(OpCodes.Stloc_S, iLocal);           //V6
                ilOfMethod.Emit(OpCodes.Br_S, conditionLable);
                ilOfMethod.MarkLabel(trueLable);
                //循环体
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldloc_1);
                ilOfMethod.Emit(OpCodes.Ldloc_S, iLocal);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ExecutingFilterAttribute>).GetMethod("get_Item"));
                ilOfMethod.Emit(OpCodes.Ldloc_S, methodParametersLocal);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(ExecutingFilterAttribute).GetMethod("Execute"));
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Nop);
                //i++
                ilOfMethod.Emit(OpCodes.Ldloc_S, iLocal);
                ilOfMethod.Emit(OpCodes.Ldc_I4_1);
                ilOfMethod.Emit(OpCodes.Add);
                ilOfMethod.Emit(OpCodes.Stloc_S, iLocal);
                //condition
                ilOfMethod.MarkLabel(conditionLable);
                ilOfMethod.Emit(OpCodes.Ldloc_S, iLocal);
                ilOfMethod.Emit(OpCodes.Ldloc_1);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ExecutingFilterAttribute>).GetMethod("get_Count"));
                ilOfMethod.Emit(OpCodes.Clt);
                var v7Local = ilOfMethod.DeclareLocal(typeof(bool));
                ilOfMethod.Emit(OpCodes.Stloc_S, v7Local);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v7Local);
                ilOfMethod.Emit(OpCodes.Brtrue_S, trueLable);
                //执行前 for循环结束

                //执行
                ilOfMethod.Emit(OpCodes.Ldarg_0);
                ilOfMethod.Emit(OpCodes.Ldfld, fieldInstance);
                for (var p = 0; p < methodParamsTypes.Length; p++)
                {
                    ilOfMethod.Emit(OpCodes.Ldarg, (p + 1));
                }


                MethodInfo m;
                if (method.IsGenericMethod)
                {
                    m = typeofTImplement.GetMethods().FirstOrDefault(p => p.Name == method.Name && p.GetParameters().Length == methodParamsTypes.Length);
                }
                else
                {
                    m = typeofTImplement.GetMethod(method.Name, methodParamsTypes);
                }
                ilOfMethod.Emit(OpCodes.Callvirt, m);
                if (hasReturnValue)
                    ilOfMethod.Emit(OpCodes.Stloc_0);
                else
                    ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Nop);

                //Catch
                ilOfMethod.BeginCatchBlock(typeof(Exception));
                var exLocal = ilOfMethod.DeclareLocal(typeof(Exception));
                ilOfMethod.Emit(OpCodes.Stloc_S, exLocal);
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldloc_3);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ErrorFilterAttribute>).GetMethod("get_Count"));
                ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                ilOfMethod.Emit(OpCodes.Ceq);
                var v9Local = ilOfMethod.DeclareLocal(typeof(bool));
                ilOfMethod.Emit(OpCodes.Stloc_S, v9Local);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v9Local);

                var tLable = ilOfMethod.DefineLabel();
                var fLable = ilOfMethod.DefineLabel();
                ilOfMethod.Emit(OpCodes.Brfalse_S, fLable);         //如果false跳转
                ilOfMethod.Emit(OpCodes.Rethrow);
                ilOfMethod.MarkLabel(fLable);                       //false
                //异常 for循环
                var conditionLable2 = ilOfMethod.DefineLabel();     //条件
                var trueLable2 = ilOfMethod.DefineLabel();          //true
                ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                var v10Local = ilOfMethod.DeclareLocal(typeof(int));
                ilOfMethod.Emit(OpCodes.Stloc_S, v10Local);
                ilOfMethod.Emit(OpCodes.Br_S, conditionLable2);
                ilOfMethod.MarkLabel(trueLable2);
                //循环体
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldloc_3);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v10Local);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ErrorFilterAttribute>).GetMethod("get_Item"));
                ilOfMethod.Emit(OpCodes.Ldloc_S, methodParametersLocal);
                ilOfMethod.Emit(OpCodes.Ldloc_S, exLocal);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(ErrorFilterAttribute).GetMethod("Execute"));
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Nop);
                //i++
                ilOfMethod.Emit(OpCodes.Ldloc_S, v10Local);
                ilOfMethod.Emit(OpCodes.Ldc_I4_1);
                ilOfMethod.Emit(OpCodes.Add);
                ilOfMethod.Emit(OpCodes.Stloc_S, v10Local);
                //condition
                ilOfMethod.MarkLabel(conditionLable2);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v10Local);
                ilOfMethod.Emit(OpCodes.Ldloc_3);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ErrorFilterAttribute>).GetMethod("get_Count"));
                ilOfMethod.Emit(OpCodes.Clt);
                var v11Local = ilOfMethod.DeclareLocal(typeof(bool));
                ilOfMethod.Emit(OpCodes.Stloc_S, v11Local);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v11Local);
                ilOfMethod.Emit(OpCodes.Brtrue_S, trueLable2);
                ilOfMethod.Emit(OpCodes.Nop);
                //异常 for循环结束

                //finally
                ilOfMethod.BeginFinallyBlock();
                ilOfMethod.Emit(OpCodes.Nop);
                //执行结束 for循环
                var conditionLable3 = ilOfMethod.DefineLabel();      //条件
                var trueLable3 = ilOfMethod.DefineLabel();           //true
                ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                var v12Local = ilOfMethod.DeclareLocal(typeof(int));
                ilOfMethod.Emit(OpCodes.Stloc_S, v12Local);
                ilOfMethod.Emit(OpCodes.Br_S, conditionLable3);
                ilOfMethod.MarkLabel(trueLable3);
                //循环体
                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Ldloc_2);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v12Local);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ExecutedFilterAttribute>).GetMethod("get_Item"));
                ilOfMethod.Emit(OpCodes.Ldloc_S, methodParametersLocal);
                if (hasReturnValue)
                {
                    ilOfMethod.Emit(OpCodes.Ldloc_0);
                    ilOfMethod.Emit(OpCodes.Callvirt, typeof(ExecutedFilterAttribute).GetMethod("Execute").MakeGenericMethod(methodReturnType));
                }
                else
                {
                    ilOfMethod.Emit(OpCodes.Ldc_I4_0);
                    ilOfMethod.Emit(OpCodes.Callvirt, typeof(ExecutedFilterAttribute).GetMethod("Execute").MakeGenericMethod(typeof(int)));
                }

                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.Emit(OpCodes.Nop);
                //i++
                ilOfMethod.Emit(OpCodes.Ldloc_S, v12Local);
                ilOfMethod.Emit(OpCodes.Ldc_I4_1);
                ilOfMethod.Emit(OpCodes.Add);
                ilOfMethod.Emit(OpCodes.Stloc_S, v12Local);
                //condition
                ilOfMethod.MarkLabel(conditionLable3);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v12Local);
                ilOfMethod.Emit(OpCodes.Ldloc_2);
                ilOfMethod.Emit(OpCodes.Callvirt, typeof(List<ExecutedFilterAttribute>).GetMethod("get_Count"));
                ilOfMethod.Emit(OpCodes.Clt);
                var v13Local = ilOfMethod.DeclareLocal(typeof(bool));
                ilOfMethod.Emit(OpCodes.Stloc_S, v13Local);
                ilOfMethod.Emit(OpCodes.Ldloc_S, v13Local);
                ilOfMethod.Emit(OpCodes.Brtrue, trueLable3);
                //执行结束 for循环结束

                ilOfMethod.Emit(OpCodes.Nop);
                ilOfMethod.EndExceptionBlock();

                if (hasReturnValue)
                {
                    ilOfMethod.Emit(OpCodes.Ldloc_0);
                    ilOfMethod.Emit(OpCodes.Stloc, resultLocal);

                    var returnLable = ilOfMethod.DefineLabel();
                    ilOfMethod.Emit(OpCodes.Br_S, returnLable);

                    ilOfMethod.MarkLabel(returnLable);
                    ilOfMethod.Emit(OpCodes.Ldloc_S, resultLocal);
                }

                //结束
                ilOfMethod.Emit(OpCodes.Ret);
                #endregion
            }

            #endregion
            var type = typeBuilder.CreateType();
            //assembly.Save($"{nameOfAssembly}.dll");
            type = type.MakeGenericType(typeofTImplement);
            return (TInterface)Activator.CreateInstance(type);
        }

        #region 
        public static MethodInfo GetInstanceMethod(Type type, string name, Type[] paramTypes)
        {
            return type.GetMethods().FirstOrDefault(p => p.Name == name && p.GetParameters().Length == paramTypes.Length);
        }

        public static void InitFilters(MethodInfo methodInfo, List<ExecutingFilterAttribute> executingFilters,
                                                         List<ExecutedFilterAttribute> executedFilters,
                                                         List<ErrorFilterAttribute> executingErrors)
        {
            var filters = methodInfo.GetCustomAttributes(typeof(FilterAttribute), false);
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    if (filter is ExecutingFilterAttribute)
                    {
                        executingFilters.Add(filter as ExecutingFilterAttribute);
                        continue;
                    }
                    if (filter is ExecutedFilterAttribute)
                    {
                        executedFilters.Add(filter as ExecutedFilterAttribute);
                        continue;
                    }
                    if (filter is ErrorFilterAttribute)
                    {
                        executingErrors.Add(filter as ErrorFilterAttribute);
                        continue;
                    }
                }
            }
        }

        public static MethodParameters[] GetMethodParmaters(MethodInfo methodInfo, object[] args)
        {
            var paramsOfMethod = methodInfo.GetParameters();
            var methodParameters = new MethodParameters[paramsOfMethod.Length];
            var i = -1;
            foreach (var paramInfo in paramsOfMethod)
            {
                i++;
                var genericType = typeof(MethodParameter<>).MakeGenericType(args[i].GetType());
                var methodParamter = Activator.CreateInstance(genericType);
                var props = genericType.GetProperties();
                foreach (var prop in props)
                {
                    if (prop.Name == "Type")
                    {
                        prop.SetValue(methodParamter, args[i].GetType());
                        continue;
                    }
                    if (prop.Name == "Name")
                    {
                        prop.SetValue(methodParamter, paramsOfMethod[i].Name);
                        continue;
                    }
                    if (prop.Name == "Value")
                    {
                        prop.SetValue(methodParamter, args[i]);
                    }
                }
                methodParameters[i] = methodParamter as MethodParameters;
            }
            return methodParameters;
        }
        #endregion
    }
}
