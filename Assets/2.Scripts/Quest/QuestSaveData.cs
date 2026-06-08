using System.Collections.Generic;

public class QuestSaveData
{
    public int mainQuestIdx;
    public Dictionary<string, long> counter = new();

    public QuestSaveData(int curMainQuestIdx, Dictionary<string, long> savedCounter)
    {
        mainQuestIdx = curMainQuestIdx;
        counter = savedCounter;
    }
}
