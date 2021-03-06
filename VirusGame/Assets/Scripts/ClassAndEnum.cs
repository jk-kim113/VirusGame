﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public string Name;
    public int ID;
    public float HungerMax;
    public float HungerDecrease;
    public float HP;
    public int InitNumber;
}

public class DropItemData
{
    public string Name;
    public int ID;
    public string Info;
    public int Rare;
    public eDropItemSubject Subject;
}

public class EquipItemData
{
    public string Name;
    public int ID;
    public string Info;
    public int Attack;
    public eEquipType EquipType;
}

public class UseItemData
{
    public string Name;
    public int ID;
    public string Info;
    public eUseTarget UseTarget;
    public int[] TypeValue;
}

public class ItemMakingInfo
{
    public eItemType ItemType;
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
    public float CureProbability;
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
    Damage,
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
    Equip,
    Drug,

    max
}

public enum eAnimalDeathType
{
    Natural,
    Virus
}

public enum eDropItemSubject
{
    Grass,
    Tree,
    Goat,
    Bear,
    Crocodile,
    Cattle,
    Wolf,
    Deer
}

public enum eEffectType
{
    CollectPlant        = 0,
    HitAnimal
}

public enum eEquipType
{
    Weapon,
    Beaker,
    Syringe
}