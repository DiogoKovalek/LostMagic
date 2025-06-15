using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff
{
    void initStaff(StaffBase staffBase);
    void attack(GameObject grimore);
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
    void initGrimore(GrimoreBase grimoreBase);
    void magic(GameObject staff);
}

public interface IMagic {
    void magic(GameObject staff);
    Magic getEMagic();
}