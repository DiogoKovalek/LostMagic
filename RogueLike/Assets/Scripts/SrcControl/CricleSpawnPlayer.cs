using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CricleSpawnPlayer : MonoBehaviour {
    private float timeToStay = 2.0f;
    [SerializeField] GameObject prefPlayer;

    IEnumerator Start() {
        yield return new WaitForSeconds(timeToStay);
        Instantiate(prefPlayer, transform.position, transform.rotation);
    }
}
