using System;
using System.Linq;
using System.Reflection;

namespace _010Proxy.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<T>(this Type type, out T attribute) where T : class
        {
            attribute = null;

            if (type.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T attr)
            {
                attribute = attr;
                return true;
            }

            return false;
        }

        public static bool HasAttribute<T>(this FieldInfo fieldInfo, out T attribute) where T : class
        {
            attribute = null;

            if (fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() is T attr)
            {
                attribute = attr;
                return true;
            }

            return false;
        }

        // public static bool Has
    }
}
