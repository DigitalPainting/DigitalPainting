using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public static class ReflectionHelper
{
    static ReflectionHelper() { }
    
    /// <summary>
    /// Find all class of a given type (including by inheritance).
    /// </summary>
    public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
    {
        return typeof(T)
                    .Assembly.GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract);
    }    
    /// <summary>
    /// Find all class of a given type (including by inheritance).
    /// </summary>
    public static IEnumerable<Type> GetEnumerableOfType(Type T)
    {
        return T.Assembly.GetTypes()
                .Where(t => (t == T || t.IsSubclassOf(T)) && !t.IsAbstract);
    }

    /// <summary>
    /// Get all classes, including abstract classes that have a given base class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<Type> GetClassesWithBaseType<T>()
    {
        return typeof(T).Assembly.GetTypes()
            .Where(x => x.IsClass && x != typeof(T) && x.BaseType == typeof(T));
    }

    public static IEnumerable<T> GetEnumerableOfInterfaceImplementors<T>() where T : class
    {
        return from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.GetInterfaces().Contains(typeof(T))
                                 && t.GetConstructor(Type.EmptyTypes) != null
                        select Activator.CreateInstance(t) as T;
    }

    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        Type baseType = givenType.BaseType;
        if (baseType == null) return false;

        return IsAssignableToGenericType(baseType, genericType);
    }
}