using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey,TValue>,ISerializationCallbackReceiver
{
    List<TKey> keyList = new();
    List<TValue> valueList = new();
    public void OnBeforeSerialize()
    {
        keyList.Clear();
        valueList.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keyList.Add(pair.Key);
            valueList.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keyList.Count != valueList.Count)
        {
            Debug.Log("error in count of serializable dictionary");
            return;
        }
        for (int i = 0; i < keyList.Count; i++)
        {
            this.Add(keyList[i], valueList[i]);
        }
    }
}
