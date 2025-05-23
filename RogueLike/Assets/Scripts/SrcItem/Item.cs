using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemBase itemBase;
    
    void Awake()
    {
        switch (itemBase.TypeItem)
        {
            case TypeItem.Staff:
                break;
            case TypeItem.Grimore:
                break;
            case TypeItem.Equipment:
                break;
            case TypeItem.Comsumable:
                break;
        }
    }
}
