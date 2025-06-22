using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[CreateAssetMenu(fileName = "Item", menuName = "Base/Item")]
public class ItemBase : ScriptableObject {
    public int id;
    public String NameItem;
    public String Description;
    public TypeItem TypeItem;
    public Sprite Sprite;
    public float chanceToDrop;
}

public enum TypeItem
{
    Staff,
    Grimore,
    Equipment,
    Comsumable,
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

