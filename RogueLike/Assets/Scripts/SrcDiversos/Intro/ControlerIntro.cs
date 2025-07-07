using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlerIntro : MonoBehaviour {
    public GameObject logoUTFPR;

    public float fadeDuration = 2.0f;

    IEnumerator Start() {
        GameObject img;
        SpriteRenderer spRen;
        yield return new WaitForSeconds(1.0f);
        #region UTFPR
        img = Instantiate(logoUTFPR);
        spRen = img.GetComponent<SpriteRenderer>();

        Color color = spRen.color;
        color.a = 0f;
        spRen.color = color;

        float timer = 0f;

        while (timer < fadeDuration) {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            color.a = alpha;
            spRen.color = color;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        timer = fadeDuration;

        while (timer > 0) {
            timer -= Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            color.a = alpha;
            spRen.color = color;
            yield return null;
        }
        #endregion
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("MenuPrincipal");
    }
}
