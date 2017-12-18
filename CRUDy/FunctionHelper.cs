using System;

namespace CRUDy
{
    public static class FunctionHelper
    {
        public static U Apply<T, U>(this T value, Func<T, U> func)
        {
            return func(value);
        }

        public static T Apply<T>(this T value, Action<T>action)
        {
            action(value);
            return value;
        }

        public static T Identity<T>(T input) => input;

        public static void NoOp<T>(T input)
        {
        }

        public static void NoOp()
        {
        }
    }
}
