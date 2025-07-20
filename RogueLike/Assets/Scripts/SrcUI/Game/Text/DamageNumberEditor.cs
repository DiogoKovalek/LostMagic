
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageNumberEditor : MonoBehaviour {
    public TextMeshPro texto;
    public float forceMove;
    public float maxAngle = 75;
    public float minAngle = 30;
    public float timeForDestroy = 2;
    private Color32 colorFire = new Color32(255, 249, 131, 255);
    private Color32 colorWater = new Color32(0, 111, 198, 255);
    private Color32 colorWind = new Color32(0, 214, 183, 255);
    private Color32 colorEarth = new Color32(39, 35, 60, 255);
    private Color32 colorVoid = new Color32(0, 186, 239, 255);

    private Rigidbody2D rig;
    public void Init(string damage, Element element) {
        switch (element) {
            case Element.Fire:
                texto.color = colorFire;
                break;
            case Element.Water:
                texto.color = colorWater;
                break;
            case Element.Wind:
                texto.color = colorWind;
                break;
            case Element.Earth:
                texto.color = colorEarth;
                break;
            case Element.Void:
                texto.color = colorVoid;
                break;
            case Element.Iron:
                texto.color = Color.grey;
                break;
        }
        texto.text = damage;

        rig = GetComponent<Rigidbody2D>();

        rig.AddForce(sortDirection() * forceMove, ForceMode2D.Impulse);
        StartCoroutine(TimerForDestroy());
    }
    private Vector2 sortDirection() {
        float dir;
        float rad;
        int aux = Random.Range(0, 2);

        dir = aux == 0 ? Random.Range(minAngle, maxAngle) : Random.Range(180 - minAngle, 180 - maxAngle);
        rad = dir * Mathf.Deg2Rad;

        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    private IEnumerator TimerForDestroy() {
        yield return new WaitForSeconds(timeForDestroy);
        Destroy(this.gameObject);
    }
}
