using System;

[Serializable]
public class Item
{
    public string DataId;
    public int Stack;

    public Item()
    {
    }

    public Item(string dataId, int stack = 1)
    {
        DataId = dataId;
        Stack = stack;
    }

    public void AddItem(int amount)
    {
        Stack += 1;
    }
}