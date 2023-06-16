using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToExample : MonoBehaviour {
    public bool canMenu = true;
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene("InimigoTeste");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && canMenu) {
            SceneManager.LoadScene("Menu");
        }
    }
}
