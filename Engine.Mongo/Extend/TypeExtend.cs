using System;
using System.Collections;

namespace Engine.Mongo.Extend
{
    public static class TypeExtend
    {

        public static bool IsComplex(this Type type)
        {
            bool flag = type.IsClass && !type.IsPrimitive && type != typeof(string);
            return flag;
        }

        public static bool IsList(this Type type)
        {
            if (!type.IsComplex())
                return false;
            return typeof(IList).IsAssignableFrom(type);
        }
        /// <summary>
        /// 只包含string字段和基础类型即为简单类型
        /// </summary>
        public static bool IsListChildPrimitive(this Type type)
        {
            if (!type.IsList())
                return false;
            bool flag = true;
            foreach (var element in type.GenericTypeArguments)
            {
                foreach (var field in element.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    flag = flag && !field.PropertyType.IsComplex();
            }
            return flag;
        }

        public static bool IsListChildComplex(this Type type)
        {
            if (!type.IsList())
                return false;
            bool flag = true;
            foreach (var element in type.GenericTypeArguments)
                flag = flag && element.IsPrimitive;
            return !flag;
        }
    }
}
