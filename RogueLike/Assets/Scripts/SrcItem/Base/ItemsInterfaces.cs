using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff {
    int getIdItem();
    void initStaff(StaffBase staffBase, GameObject playerLinked);
    void attack(GameObject grimore);
    GameObject GetPlayerLinked();
}
public interface IConsumable
{
    void init();
}
public interface IEquipment
{
    void init();
    void update();
}
public interface IGrimore
{
    int getIdItem();
    void initGrimore(GrimoreBase grimoreBase);
    void magic(GameObject staff);
}

public interface IMagic {
    void magic(GameObject staff);
    Magic getEMagic();
}
public interface IProject {
    void initMoviment(Vector2 direction);
    void insertProjectBase(ProjectileBase pb, GameObject player);
}