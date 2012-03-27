using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Simple.Validation.DataAnnotations
{

    /// <summary>
    /// Extension methods for objects in the System.Reflection namespace.
    /// </summary>
    internal static class ReflectionExtensions
    {
        public static IEnumerable<T> GetCustomAttributes<T>(this Type source, bool inherit) where T : System.Attribute
        {
            if (source == null)
                return null;
            var results = source.GetCustomAttributes(typeof(T), inherit).Cast<T>();
            return results;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this PropertyInfo source, bool inherit) where T : System.Attribute
        {
            if (source == null)
                return null;
            var results = source.GetCustomAttributes(typeof(T), inherit).Cast<T>();
            return results;
        }
    }
}