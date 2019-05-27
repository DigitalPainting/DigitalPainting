using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }
    
    /// <summary>
    /// Find all class of a given type (including by inheritance).
    /// </summary>
    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
    {
        return typeof(T)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);
    }

    public static IEnumerable<T> GetEnumerableOfInterfaceImplementors<T>() where T : class
    {
        return from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetInterfaces().Contains(typeof(T))
                                 && t.GetConstructor(Type.EmptyTypes) != null
                        select Activator.CreateInstance(t) as T;
    }
}