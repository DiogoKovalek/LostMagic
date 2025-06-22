using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectileBank {
    private static GameObject projectile;
    private static ProjectileBase[] projectsBase;
    public static void IntiProjectileBank() {
        projectile = Resources.Load<GameObject>("LoadPrefab/Projects/ProjectileBase");
        projectsBase = Resources.LoadAll<ProjectileBase>("ScrObj/Projectiles");
    }

    public static ProjectileBase GetProject(String nameProject) {
        ProjectileBase pb = GetProjectBaseByName(nameProject);
        if (pb != null) {
            return pb;
        }
        return null;
    }
    public static GameObject GetMoldProject() {
        return projectile;
    }
    private static ProjectileBase GetProjectBaseByName(String nameProject) {
        foreach (var b in projectsBase) {
            if (b.NameProject == nameProject) return b;
        }
        return null;
    }
}
