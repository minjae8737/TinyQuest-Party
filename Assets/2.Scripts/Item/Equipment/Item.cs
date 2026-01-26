public class Item
{
    public long Id;
    public string DataId;
    public int Stack;

    public Item()
    {
    }

    public Item(long id, string dataId, int stack = 1)
    {
        Id = id;
        DataId = dataId;
        Stack = stack;
    }
}