using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pojectile", menuName = "Base/Projectile/Projectile")]
public class ProjectileBase : ScriptableObject {
    public String NameProject;
    public RuntimeAnimatorController runAnim;
    public Element element;
    public TypeMoveProject typeMoveProject;
    public float speed = 8f;
    public float size = 1f;
    public float coliderSize = 0.2f;
}
public enum TypeMoveProject {
    forward,
    autoRemote
}