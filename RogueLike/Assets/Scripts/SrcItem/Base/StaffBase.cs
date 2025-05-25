using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff", menuName = "Base/Item/Staff")]
public class StaffBase : ItemBase
{
    public Element Element;
    public int MFire;
    public int MWater;
    public int MWind;
    public int MEarth;
    public int MVoid;
    public StaffScript staffScript;
}
public enum StaffScript{

}
