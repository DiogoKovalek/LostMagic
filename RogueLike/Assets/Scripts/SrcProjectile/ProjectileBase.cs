using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pojectile", menuName = "Projectile/Projectile")]
public class ProjectileBase : ScriptableObject {
    public String NameProject;
    public Sprite Sprite; // Trocar depois para animator
    public Element element;
}
