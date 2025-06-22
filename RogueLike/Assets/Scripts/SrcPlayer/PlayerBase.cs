using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Base/Player")]
public class PlayerBase : ScriptableObject {
    public int maxLife;
    public int maxMana;
    public int manaPerSecond;
    public int atack;
    public float speed;
    public int luck;
    public int MFire;
    public int MWater;
    public int MWind;
    public int MEarth;
    public int MVoid;
}
