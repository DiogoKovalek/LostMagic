using System.Collections;
using System.Threading;
using UnityEngine;

public class WaterShild : MonoBehaviour {
    private StatusManager statusManager;
    public float timeDuration;
    private float timeForBlink = 1.5f;
    private float timeBetweenBlink = 0.2f;
    IEnumerator Start() {
        statusManager = transform.parent.GetComponentInChildren<StatusManager>();
        statusManager.AplicateStatus(Element.Water);
        statusManager.setAbleForRemoveWet(false);
        Player iPlayer = transform.parent.GetComponent<Player>();
        if (iPlayer != null) iPlayer.delayInvunerableByTime(timeDuration);
        yield return new WaitForSeconds(timeDuration - timeForBlink);
        float timer = 0;
        SpriteRenderer spRen = GetComponent<SpriteRenderer>();
        while (timer <= timeForBlink) {
            spRen.enabled = !spRen.enabled;
            timer += timeBetweenBlink;
            yield return new WaitForSeconds(timeBetweenBlink);
        }
        statusManager.setAbleForRemoveWet(true);
        Destroy(this.gameObject);
    }
    void OnDisable() {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
