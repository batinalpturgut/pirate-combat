using System;
using System.Collections.Generic;

namespace Root.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        public static void Remove<T>(this Stack<T> stack, Func<T, bool> condition)
        {
            if (stack == null)
            {
                throw new ArgumentNullException(nameof(stack));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            Stack<T> tempStack = new Stack<T>();

            while (stack.Count > 0)
            {
                T item = stack.Pop();
                if (condition(item))
                {
                    break;
                }

                tempStack.Push(item);
            }

            while (tempStack.Count > 0)
            {
                stack.Push(tempStack.Pop());
            }
        }
    }
}