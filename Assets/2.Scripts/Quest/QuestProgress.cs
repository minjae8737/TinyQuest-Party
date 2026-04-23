using System.Collections.Generic;
using UnityEngine;

public class QuestProgress
{
    private Dictionary<string, long> counter = new();

    public void Init()
    {
        counter = new();
    }

    public void Add(string key, long count = 1L)
    {
        counter.TryGetValue(key, out long counts);
        counter[key] = counts + count;
        Debug.Log($"Add (key {key}/value {counts + count})");
    }

    public long GetCount(string key)
    {
        Debug.Log($"GetCount {key}");

        counter.TryGetValue(key, out long count);
        return count;
    }
}
