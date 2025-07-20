using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour {
    private Enemy enemy;
    private IMagic magic;
    private Rigidbody2D rig;

    //Atack =============================================
    private bool atackMode = false; // modo de ataque
    private bool ableForAtack = true;
    private bool isAtacking = false; // por causa do delay
    public float delayForAtack = 3.0f;
    public int quantMagicsSequence = 1;
    public float delayMagicSequence = 0.1f;
    //===================================================
    // Move Mode ========================================'
    // Os magos terao o seguinte comportamento de movimentacao:
    // Eles vão andar em uma direção aleatoria, detectando obistaculos, quando colidem em um obstaculo,
    // eles mudam de direção, aleatoriamente tambem, essa colisao são paredes, espinhos e outros inimigos,
    // quando um player estiver no raio de ataque, ele entra em modo de ataque, assim se ableForAtack for true,
    // significa que ele pode atacar, ai ele vai parar e lançar as magias, quando terminar de fazer os ataques ele volta a se movimentar,
    public float distanceFromPlayer = 1;
    private float rayVisionObstacle = 0.8f;
    private int layersObstacle = (1 << 15) | (1 << 14) | (1 << 9);
    private Vector2 deslocRayVision = new Vector2(0, -0.275f);
    private Vector2 dir;
    private Vector2[] dirPosible = {new Vector2(0,1), new Vector2(0.5f,0.5f).normalized, new Vector2(1,0), new Vector2(0.5f,-0.5f).normalized,
    new Vector2(0,-1), new Vector2(-0.5f,-0.5f).normalized, new Vector2(-1,0), new Vector2(-0.5f, 0.5f).normalized};

    private Coroutine timerForTradeDir;
    public float minTimeForTradeDir = 1.5f;
    public float maxTimeForTradeDir = 5f;
    //===================================================
    void Awake() {
        enemy = GetComponent<Enemy>();
        magic = GetComponent<IMagic>();
        magic.addConfigInMagic(this.transform);
        rig = GetComponent<Rigidbody2D>();
    }
    void Start() {
        dir = sortDir();
    }
    void FixedUpdate() {
        if (enemy.isFreeForAction) {
            // Verificar se esta liver para atacar
            if (enemy.target != null) {
                RaycastHit2D ray = Physics2D.Raycast(this.transform.position, enemy.directionForPlayer(), enemy.distanceForPlayer(), layersObstacle & ~1 << 14); // uma and de a com b com o b negado
                if (!ray) {
                    atackMode = true;
                }
                else {
                    atackMode = false;
                }
            }

            if (!isAtacking && enemy.target != null) {
                MoveMode();
            }
            if (ableForAtack && atackMode && !isAtacking) {
                CombatMode();
                rig.linearVelocity = new Vector2(0, 0);
            }
        }
        else {
            rig.linearVelocity = Vector2.zero;
        }
    }

    private void MoveMode() {

        if (timerForTradeDir == null) {
            timerForTradeDir = StartCoroutine(TimerForTradeDir());
        }

        //Colisao
        if (Physics2D.Raycast((Vector2)this.transform.position + deslocRayVision, dir, rayVisionObstacle, layersObstacle) == true) {//existe algo no caminho
            StopCoroutine(timerForTradeDir);
            timerForTradeDir = null;
            dir = sortDir();
        }

        rig.linearVelocity = dir * enemy.speed;
    }
    private IEnumerator TimerForTradeDir() {
        float timeForTradeDir = Random.Range(minTimeForTradeDir, maxTimeForTradeDir);
        yield return new WaitForSeconds(timeForTradeDir);
        dir = sortDir();
        timerForTradeDir = null;
    }
    private Vector2 sortDir() {
        Vector2 direction;
        List<Vector2> ableDir = new List<Vector2>();
        foreach (var d in dirPosible) {
            RaycastHit2D ray = Physics2D.Raycast((Vector2)this.transform.position + deslocRayVision, d, rayVisionObstacle, layersObstacle);
            if (ray.collider == null) {
                ableDir.Add(d);
            }
        }
        direction = ableDir.Count > 0 ? ableDir[Random.Range(0, ableDir.Count)] : new Vector2(0, 0);
        return direction;
    }
    private void CombatMode() {
        if (ableForAtack) {
            StartCoroutine(Atack());
        }
    }

    private IEnumerator Atack() {
        isAtacking = true;
        ableForAtack = false;
        for (int i = 0; i < quantMagicsSequence; i++) {
            magic.castMagic(enemy.directionForPlayer());
            yield return new WaitForSeconds(delayMagicSequence);
        }
        isAtacking = false;
        yield return new WaitForSeconds(delayForAtack);
        ableForAtack = true;
    }
}
