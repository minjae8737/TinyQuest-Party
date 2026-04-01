using System;

public enum ArmorType 
{
    Cloth,   // 천
    Leather, // 가죽
    Light,   // 경갑
    Heavy    // 중갑
}

[Serializable]
public class Armor : Equipment
{
}
