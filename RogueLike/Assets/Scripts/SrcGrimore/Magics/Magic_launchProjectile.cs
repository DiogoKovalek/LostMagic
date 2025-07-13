using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_launchProjectile : MonoBehaviour, IMagic {
    [SerializeField] Transform posCenter;
    [SerializeField] float rayForSpawn;
    [SerializeField] GameObject prefabProject;
    [SerializeField] float speedProject = 1;
    [SerializeField] bool isPlayer = true;
    private IAtributesComunique atributesPlayer;
    private Element element;
    public void addConfigInMagic(Transform center) {
        posCenter = center;
        element = GetComponent<IGrimore>().getGrimoreBase().Element;
        atributesPlayer = center.transform.parent.GetComponent<IAtributesComunique>();
    }

    public void castMagic(Vector2 direction) {
        GameObject proj = Instantiate(prefabProject, (Vector2) posCenter.position + (direction * rayForSpawn), prefabProject.transform.rotation);
        proj.GetComponent<SimpleProject>().initMoviment(direction, atributesPlayer.CalculateSpeed() + speedProject, atributesPlayer.CalculateAtack(element), isPlayer);
    }
}
