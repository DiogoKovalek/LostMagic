using System.Collections;
using UnityEngine;

public class Quicksand : MonoBehaviour {
    public float timeToDesapear = 10.0f;
    private float timeForInitDesapearPorcent = 0.2f;

    private bool ableForStun = true;

    void Start() {
        StartCoroutine(TimerForDestroy());
    }

    private IEnumerator TimerForDestroy() {
        float timer1 = timeToDesapear - timeForInitDesapearPorcent * timeToDesapear;
        yield return new WaitForSeconds(timer1);

        ableForStun = false;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;

        float timerDuration = timeToDesapear - timer1;
        float timer = timerDuration;

        while (timer > 0) {
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / timerDuration);
            color.a = alpha;
            spriteRenderer.color = color;
            yield return null;
        }

        Destroy(this.gameObject);
    }
    void OnTriggerStay2D(Collider2D other) {
        if (ableForStun && (other.transform.tag == "Enemy" || other.transform.tag == "Player")) {
            other.GetComponent<StatusManager>().AplicateStatus(Element.Earth);
        }
    }
}
