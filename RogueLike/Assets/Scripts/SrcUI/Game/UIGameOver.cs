using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour, ISetScreen {

    private InputActions input;
    private bool actEnter;

    public GameObject textoEnter;
    private float timeFadeButton = 0.3f;
    
    void Update() {
        getInput();
        if (actEnter) {
            SceneManager.LoadScene("MenuPrincipal");
            input.Disable();
        }
    }
    void getInput() {
        actEnter = input.GameOver.Enter.WasPressedThisFrame();
    }
    public void SetScreen(bool active = false, bool input = false) {
        this.gameObject.SetActive(active);
        if (active == true) {
            StartCoroutine(fadeButton());
        }

        if(this.input == null) this.input = new InputActions();
        if (input) {
            this.input.GameOver.Enable();
        }
        else {
            this.input.GameOver.Disable();
        }
    }
    private IEnumerator fadeButton() {
        textoEnter.SetActive(!textoEnter.activeSelf);
        yield return new WaitForSeconds(timeFadeButton);
    }
}
