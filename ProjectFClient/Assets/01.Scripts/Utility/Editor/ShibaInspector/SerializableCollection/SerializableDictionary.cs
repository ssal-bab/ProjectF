using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShibaInspector.Collections
{
    [Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] 
    [HideInInspector]
    public List<TKey> keys = new();
    [SerializeField] 
    [HideInInspector]
    public List<TValue> values = new();

    public Dictionary<TKey, TValue>  myDictionary = new Dictionary<TKey, TValue>();

    public TValue this[TKey key]
    {
        get{ return myDictionary[key]; }
        set{ myDictionary[key] = value; }
    }

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        // For each key/value pair in the dictionary, add the key to the keys list and the value to the values list
        foreach (var kvp in myDictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        myDictionary = new Dictionary<TKey, TValue>();

        // Loop through the list of keys and values and add each key/value pair to the dictionary
        for (int i = 0; i != Math.Min(keys.Count, values.Count); i++)
        {
            myDictionary.Add(keys[i], values[i]);
        }
    }
}
}