using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public string Name;
    public int ID;
    public float HungerMax;
    public float HungerDecrease;
    public int InitNumber;
}

public class ItemData
{
    public string Name;
    public int ID;
    public string Info;
    public int Rare;
}

public class FoodMenu
{
    public string Name;
    public int ID;
    public int[] TargetID;
    public string Info;
    public eUseTarget UseTarget;
}

public class FoodMakeType
{
    public int TargetID;
    public int[] TypeValue;
}

public class ItemMakingInfo
{
    public int TargetID;
    public int[] NeedID;
    public int[] NeedNumber;
}

public class VirusData
{
    public string Name;
    public int ID;
    public float IncubationPeriod;
    public int Rank;
    public float AnalysisRate;
    public float PowerConsumption;
}

public enum ePlantGrowthType
{
    Early,
    MidTerm,
    LastPeriod,
    Rotten
}

public enum eBehaviorPattern
{
    Idle,
    Move,
    Eat,
    Die
}

public enum eAnimalGrowthType
{
    Baby,
    Adult,
    Old,

    max
}

public enum eAnimalKind
{
    Herbivore,
    Carnivore,

    max
}

public enum eFoodType
{
    Raw,
    Fried,
    Steamed
}

public enum eUseTarget
{
    Stamina,
    Hunger,
    HP
}

public enum eItemType
{
    Drop,
    Use,
    Equip
}