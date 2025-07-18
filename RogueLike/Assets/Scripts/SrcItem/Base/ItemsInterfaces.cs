using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff {
    int getIdItem();
    void initStaff(StaffBase staffBase, GameObject playerLinked);
    void attack(GameObject grimore, Vector2 direction);
    GameObject GetPlayerLinked();
}
public interface IConsumable {
    int getIdItem();
    void init(ConsumableBase consumableBase);
    void action();
    bool isExist();
}
public interface IEquipment
{
    void init();
    void update();
}
public interface IGrimore
{
    int getIdItem();
    GrimoreBase getGrimoreBase();
    void initGrimore(GrimoreBase grimoreBase);
    void conjureMagic(Vector2 direction);
}

public interface IMagic {
    void addConfigInMagic(Transform center);
    void castMagic(Vector2 direction);
}