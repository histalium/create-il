using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CreateIl
{
    class Program
    {
        static void Main(string[] args)
        {
            Add2();
            HelloWorld();
        }

        static void HelloWorld()
        {
            var assemblyName = new AssemblyName
            {
                Name = "IlTest"
            };

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("IlTestModule");
            var typeBuilder = moduleBuilder.DefineType("HelloWorld", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod("Print", MethodAttributes.Public | MethodAttributes.Static);

            var il = methodBuilder.GetILGenerator();
            il.Emit(OpCodes.Ldstr, "Hello World!");
            var parameterTypes = new Type[1];
            parameterTypes[0] = typeof(string);

            var consoleType = typeof(Console);
            var methodInfo = consoleType.GetMethod(nameof(Console.WriteLine), parameterTypes);
            il.Emit(OpCodes.Call, methodInfo);
            il.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();

            type.GetMethod("Print").Invoke(null, null);
        }

        static void Add2()
        {
            var assemblyName = new AssemblyName
            {
                Name = "IlTest"
            };

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

            var moduleBuilder = assemblyBuilder.DefineDynamicModule("IlTestModule");
            var typeBuilder = moduleBuilder.DefineType("HelloWorld", TypeAttributes.Public);
            var methodBuilder = typeBuilder.DefineMethod("Add2", MethodAttributes.Public | MethodAttributes.Static, typeof(int), new[] { typeof(int) });
            
            var il = methodBuilder.GetILGenerator();
            //il.DeclareLocal(typeof(int));
            //il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, 2);
            il.Emit(OpCodes.Add);
            //il.Emit(OpCodes.Stloc_0);
            //il.Emit(OpCodes.Ldloc_0);
            //il.Emit(OpCodes.Stloc_1);
            //il.Emit(OpCodes.Ldloc_1);
            il.Emit(OpCodes.Ret);

            var type = typeBuilder.CreateType();

            var sum = type.GetMethod("Add2").Invoke(null, new object[] { 3 });
        }

    }
}
