using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Base/Item/Equipment")]
public class EquipmentBase : ItemBase
{
    public int HP;
    public int Mana;
    public int Atack;
    public int Speed;
    public int Luck;
    public int MFire;
    public int MWater;
    public int MWind;
    public int MEarth;
    public int MVoid;
    public EquipmentScript equipmentScript;
}
public enum EquipmentScript {
    none
}
