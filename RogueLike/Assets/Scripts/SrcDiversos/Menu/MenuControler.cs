using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControler : MonoBehaviour {
    public void OnStartGame() {
        SceneManager.LoadScene("Game");
    }
    public void OnExitGame() {
        Application.Quit();
    }
}
