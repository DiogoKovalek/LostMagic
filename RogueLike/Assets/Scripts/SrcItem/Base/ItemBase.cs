using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Base/Item")]
public class ItemBase : ScriptableObject
{
    public String NameItem;
    public String Description;
    public TypeItem TypeItem;
    public Sprite Sprite;
}

public enum TypeItem
{
    Staff,
    Grimore,
    Equipment,
    Comsumable
}
public enum Element
{
    Void,
    Earth,
    Fire,
    Wind,
    Water,
    Magma,
    Electric,
    Ice,
    Leaf,
    Blood,
    Iron,
    Soul,
    Poison
}
public enum categoryEquipment{
    Hat,
    Necklace,
    Cloak,
    Pants,
    Boot,
    Ring
}
