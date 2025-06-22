using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Base/Item/Equipment")]
public class EquipmentBase : ItemBase {
    public CategoryEquipment CategoryEquipment;
    public int HP;
    public int Mana;
    public int ManaPerSecond;
    public int Atack;
    public int Speed;
    public int Luck;
    public int MFire;
    public int MWater;
    public int MWind;
    public int MEarth;
    public int MVoid;
    public EquipmentScript equipmentScript;

    private void OnValidate() {
        TypeItem = TypeItem.Equipment;
    }
}
public enum CategoryEquipment{
    Hat,
    Necklace,
    Cloak,
    Pants,
    Boot,
    Ring
}
public enum EquipmentScript {
    none
}
