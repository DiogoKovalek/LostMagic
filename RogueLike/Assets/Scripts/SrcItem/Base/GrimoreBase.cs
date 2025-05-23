using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grimore", menuName = "Base/Item/Grimore")]
public class GrimoreBase : ItemBase
{
    public int MFire;
    public int MWater;
    public int MWind;
    public int MEarth;
    public int MVoid;
    public GrimoreScript grimoreScript;
}
public enum GrimoreScript
{
    
}
