using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoad : MonoBehaviour, ISetScreen {
    public GameObject ManaLoad;
    private float rotationSpeed = 90;
    public Image Background;
    private float BackgroundDuration = 2.0f;
    private float TimeMinInLoadScreen = 3.0f;
    private bool finishCreateLevel = false;
    private float BackgroundDurationForInitiLevel = 2.0f;
    //Event System ============================
    public delegate void AbleForSpawnPlayer();
    public event AbleForSpawnPlayer AbledForSpawnPlayer;
    //=========================================
    void OnDisable() {
        finishCreateLevel = false;
    }
    public void SetScreen(bool active = false, bool input = false) {
        this.gameObject.SetActive(active);
        if (active) {
            StartCoroutine(InitLoadScreen());
        }
    }
    private IEnumerator InitLoadScreen() {
        Color color = Background.color;
        color.a = 0f;
        Background.color = color;

        #region darken Screen
        float timer = 0f;

        while (timer < BackgroundDuration) {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / BackgroundDuration);
            color.a = alpha;
            Background.color = color;
            yield return null;
        }
        #endregion

        #region Load Screen
        timer = 0;
        ManaLoad.SetActive(true);
        while (!(finishCreateLevel && timer > TimeMinInLoadScreen)) {
            timer += Time.deltaTime;
            ManaLoad.transform.Rotate(0, 0, (-rotationSpeed * Time.deltaTime) % 360);
            yield return null;
        }
        ManaLoad.SetActive(false);
        #endregion

        #region lighten Screen
        timer = BackgroundDurationForInitiLevel;

        while (timer > 0) {
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / BackgroundDuration);
            color.a = alpha;
            Background.color = color;
            yield return null;
        }
        AbledForSpawnPlayer();
        #endregion
    }
    public float OnGettTimeForInitLoad() {
        return BackgroundDuration;
    }
    public void OnAbleForLightenScreen() {
        finishCreateLevel = true;
    }

}
