using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScenesManager : MonoBehaviour {

    public void LoadMainScene () {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMenuScene () {
        SceneManager.LoadScene("MenuScene");
    }

    public void Exit () {
        Application.Quit();
    }

}
