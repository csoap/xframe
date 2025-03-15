using System.Collections.Generic;
using UnityEngine;
using System;
using UObject = UnityEngine.Object;

public class PrefabBinder : MonoBehaviour
{
    [Serializable]
    public class Item
    {
        public string name;
        public UObject obj;
    }

    private Dictionary<string, UObject> _itemDic = new Dictionary<string, UObject>();
    public Item[] items = new Item[0];

    private void Awake() 
    {
        foreach (var item in items)
        {
            // 允许覆盖重复键
            _itemDic[item.name] = item.obj;
        }
    }

    public UObject GetObj(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("PrefabBinder: Name is null or empty.");
            return null;
        }
        _itemDic.TryGetValue(name, out UObject obj);
        return obj;
    }

    public T GetObj<T>(string name) where T : UObject
    {
        UObject obj = GetObj(name);
        if (obj == null)
        {
            Debug.LogError($"PrefabBinder: Object '{name}' not found.");
            return default;
        }

        if (obj is T target)
        {
            return target;
        }
        else
        {
            Debug.LogError($"PrefabBinder: Object '{name}' is type {obj.GetType()}, not {typeof(T)}.");
            return default;
        }
    }
}