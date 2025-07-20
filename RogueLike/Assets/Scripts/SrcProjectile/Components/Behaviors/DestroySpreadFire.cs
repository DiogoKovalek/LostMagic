using UnityEngine;

public class DestroySpreadFire : MonoBehaviour {
    public GameObject PrefFire;
    public float rayForSpread = 3;
    public int qunatFire = 20;

    private int layerWall = 1 << 9;

    private bool isQuitting = false;
    void OnDestroy() {
        if (isQuitting) return;

        for (int i = 0; i < qunatFire; i++) {
            int tent = 0;
            while (tent < 5) {
                Vector2 dir = Random.insideUnitCircle.normalized;
                Vector2 targetPos = (Vector2)transform.position + dir * Random.Range(0, rayForSpread);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayForSpread, layerWall);

                if (!hit) {
                    Instantiate(PrefFire, targetPos, Quaternion.identity);
                    break;
                }
                else {
                    tent++;
                }
            }
        }
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }
}
