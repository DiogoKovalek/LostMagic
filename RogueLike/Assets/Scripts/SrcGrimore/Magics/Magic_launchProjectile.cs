using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Magic_launchProjectile : MonoBehaviour, IMagic {
    [SerializeField] Transform posCenter;
    [SerializeField] float rayForSpawn;
    [SerializeField] GameObject prefabProject;
    [SerializeField] float speedProject = 1;
    [SerializeField] bool isPlayer = true;
    private IAtributesComunique atributes;
    private Element element;
    private Transform AbaProject;
    public void addConfigInMagic(Transform center) {
        posCenter = center;
        AbaProject = GameObject.FindAnyObjectByType<Controler>().GetAbaProjects();
        if (isPlayer) {
            element = GetComponent<IGrimore>().getGrimoreBase().Element;
            atributes = center.transform.parent.GetComponent<IAtributesComunique>();
        }
        else {

            element = GetComponent<Enemy>().element;
            atributes = center.GetComponent<IAtributesComunique>();
        }

    }

    public void castMagic(Vector2 direction) {
        GameObject proj = Instantiate(prefabProject, (Vector2)posCenter.position + (direction * rayForSpawn), prefabProject.transform.rotation);
        proj.GetComponent<IProjectBasic>().initMoviment(direction, atributes.CalculateSpeed() + speedProject, atributes.CalculateAtack(element), isPlayer);
        proj.transform.SetParent(AbaProject);
    }
}

public interface IProjectBasic{
    void initMoviment(Vector2 direction, float speed, int atack, bool isPlayer);
}
