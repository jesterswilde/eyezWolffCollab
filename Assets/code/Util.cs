using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Util
{
    public static TValue GetValueOrDefault<TKey, TValue>
    (this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        TValue ret;
        // Ignore return value
        dictionary.TryGetValue(key, out ret);
        return ret;
    }
    public static void ForI(Action<int> _action, int _times)
    {
        for (int i = 0; i < _times; i++)
        {
            _action(i);
        }
    }
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> _list, Action<T> _action)
    {
        foreach (T _item in _list)
        {
            _action(_item);
        }
        return _list;
    }
}
