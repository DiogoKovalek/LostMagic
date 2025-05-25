using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaff
{
    void attack();
    void defense();
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
