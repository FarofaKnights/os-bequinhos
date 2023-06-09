using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IrPara : MonoBehaviour {
    public string cena;

    public void Ir() {
        SceneManager.LoadScene(cena);
    }

    public void Ir(string cena) {
        SceneManager.LoadScene(cena);
    }

    public void Sair() {
        Application.Quit();
    }
}
