//using System;

//namespace JBLib
//{
//    internal class NamedFunction<T>
//        where T : Delegate
//    {
//        protected NamedFunction(string name, T function)
//        {
//            Name = name;
//            this.function = function;
//        }

//        internal string Name { get; }

//        protected T function;

//        internal bool IsNamed(string name)
//            => Name.Equals(name, StringComparison.OrdinalIgnoreCase);

//        internal double Evaluate(params double[] args)
//            => (double)function.DynamicInvoke(args);
//    }
//}
