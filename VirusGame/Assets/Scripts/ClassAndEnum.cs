﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public string Name;
    public int ID;
    public int InitNumber;
}

public class ItemData
{
    public string Name;
    public int ID;
    public string Info;
    public int Rare;
}

public class CombItem
{
    public string Name;
    public int ID;
    public string Content;
}

public class ItemMakingInfo
{
    public int TargetID;
    public int[] NeedID;
    public int[] NeedNumber;
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
