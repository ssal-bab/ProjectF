using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShibaInspector.Collections
{
    [Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new();
    [SerializeField] private List<TValue> values = new();

    [SerializeField] private bool shouldApply;
    [SerializeField] private bool notApplied;

    public void OnBeforeSerialize()
    {
        if(shouldApply == false)
            return;

        keys.Clear();
        values.Clear();

        foreach(var pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }

        shouldApply = false;
    }

    public void OnAfterDeserialize()
    {
        Clear();
        
        for(int i = 0; i < keys.Count; i++)
        {
            if(!ContainsKey(keys[i]))
                Add(keys[i], values[i]);
        }
    }
}
}