using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {
    private Enemy enemy;
    private IMagic magic;
    public LayerMask layerObstacle;

    //Atack =============================================
    private bool atackMode = false; // modo de ataque
    private bool isAtacking = false; // por causa do delay
    public float delayForAtack = 3.0f;
    public int quantMagicsSequence = 1;
    public float delayMagicSequence = 0.1f;
    //===================================================
    
    void Awake() {
        enemy = GetComponent<Enemy>();
        magic = GetComponent<IMagic>();
    }

    void FixedUpdate() {
        // Verificar se esta liver para atacar
        if (enemy.target != null) {
            RaycastHit2D ray = Physics2D.Raycast(this.transform.position, enemy.directionForPlayer(), enemy.distanceForPlayer(), layerObstacle);
            if (!ray) {
                atackMode = true;
            }
            else {
                atackMode = false;
            }
        }

        if (!isAtacking && !atackMode) {
            MoveMode();
        }
        else if (!isAtacking && atackMode) {
            CombatMode();
        }


    }
    private void MoveMode() {

    }
    private void CombatMode() {
        if (!isAtacking) {
            Debug.Log("Atacando");
            StartCoroutine(Atack());
        }
    }

    private IEnumerator Atack() {
        isAtacking = true;
        for (int i = 0; i < quantMagicsSequence; i++) {
            magic.castMagic(enemy.directionForPlayer());
            yield return new WaitForSeconds(delayMagicSequence);
        }
        yield return new WaitForSeconds(delayForAtack);
        isAtacking = false;
    }
}
