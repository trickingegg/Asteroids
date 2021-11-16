using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    private class Bucket
    {
        public readonly Stack<GameObject> Pooled;
        public readonly HashSet<GameObject> Used;

        public Bucket()
        {
            Pooled = new Stack<GameObject>();
            Used = new HashSet<GameObject>();
        }
    }

    public static Pool Instance => _instance.Value;
    private static readonly Lazy<Pool> _instance = new Lazy<Pool>(Factory);

    private readonly Dictionary<string, Bucket> _registry = new Dictionary<string, Bucket>();

    private static Pool Factory()
    {
        var result = new GameObject("Pool").AddComponent<Pool>();
        result.gameObject.SetActive(false);
        return result;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Register(GameObject original, int count = 4)
    {
        var key = original.name;
        RegisterKeyIfNeeded(key);

        var pool = _registry[key].Pooled;
        for (var i = 0; i < count; i++)
        {
            var instance = Instantiate(original, transform);
            pool.Push(instance);
        }
    }

    public T Get<T>(GameObject original) where T : Component
    {
        var go = Get(original);
        return go.GetComponent<T>();
    }

    public T Get<T>(GameObject original, Transform parent) where T : Component
    {
        var go = Get(original, parent);
        return go.GetComponent<T>();
    }

    public GameObject Get(GameObject original, Transform parent)
    {
        var go = Get(original);
        var trans = go.transform;
        var originalScale = trans.localScale;
        trans.SetParent(parent);
        trans.localScale = originalScale;
        return go;
    }

    public GameObject Get(GameObject original)
    {
        var key = original.name;
        RegisterKeyIfNeeded(key);

        var bucket = _registry[key];
        var instance = bucket.Pooled.Count > 0
          ? bucket.Pooled.Pop()
          : Instantiate(original, transform);

        bucket.Used.Add(instance);
        return instance;
    }

    public void Recycle(Component instance) => Recycle(instance.gameObject);

    public void Recycle(Transform instance) => Recycle(instance.gameObject);

    public void Recycle(GameObject instance)
    {
        if (ReferenceEquals(instance, null))
            return;

        foreach (var bucket in _registry.Values)
        {
            if (!bucket.Used.Remove(instance))
                continue;

            bucket.Pooled.Push(instance);
            instance.SetActive(false);
            instance.transform.SetParent(transform);
            break;
        }
    }

    private void RegisterKeyIfNeeded(string key)
    {
        if (!_registry.ContainsKey(key))
            _registry.Add(key, new Bucket());
    }
}
