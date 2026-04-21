using System.Collections.Generic;

public class QuestProgress
{
    public long Count;

    private Dictionary<string, long> counter = new();

    public void Init()
    {
        counter = new();
    }

    public void Add(string key, long count = 1L)
    {
        counter.TryGetValue(key, out long counts);
        counter[key] = counts + count;
    }
}
