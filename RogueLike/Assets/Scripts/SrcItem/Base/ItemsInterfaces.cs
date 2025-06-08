using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff
{
    void initStaff(StaffBase staffBase);
    void attack();
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
    void conjure();
}
