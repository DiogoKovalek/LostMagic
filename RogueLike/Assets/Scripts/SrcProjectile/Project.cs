using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Project : MonoBehaviour {
    private ProjectileBase projectBase;
    void Start() {

    }
    void Update() {

    }
    public void insertProjectBase(ProjectileBase pb) {
        projectBase = pb;
        gameObject.GetComponent<SpriteRenderer>().sprite = projectBase.Sprite;
    }
}
