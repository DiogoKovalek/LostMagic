using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileBank {
    private static GameObject projectile;
    private static ProjectileBase[] projectsBase;
    public static void IntiProjectileBank() {
        projectile = Resources.Load<GameObject>("LoadPrefab/Projects/ProjectileBase");
        projectsBase = Resources.LoadAll<ProjectileBase>("SrcObj/Projectiles");
    }

    public static GameObject GetProject(String nameProject) {
        ProjectileBase pb = GetProjectBaseByName(nameProject);
        if (pb != null) {
            GameObject obj = projectile;
            obj.GetComponent<Project>().insertProjectBase(pb);
            return obj;
        }
        return null;
    }
    private static ProjectileBase GetProjectBaseByName(String nameProject) {
        Debug.Log(projectsBase.Length);
        foreach (var b in projectsBase) {
            Debug.Log(b.name);
            if (b.name == nameProject) return b;
        }
        return null;
    }
}
