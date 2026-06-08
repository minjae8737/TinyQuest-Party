using Firebase.Firestore;

[FirestoreData]
public class CurrencySaveData
{
    [FirestoreProperty] 
    public long Gold { get; set; }
    [FirestoreProperty] 
    public long Exp { get; set; }

    public CurrencySaveData() {}

    public CurrencySaveData(long gold, long exp)
    {
        Gold = gold;
        Exp = exp;
    }

    public static CurrencySaveData Create()
    {
        return new CurrencySaveData(
            gold: 0L,
            exp: 0L
        );
    }
        
}
        
