using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtivarSeNoTrigger : MonoBehaviour {
    public GameObject[] ativar;
    public bool comecaDesativado = true;

    void Start() {
        if (comecaDesativado) {
            foreach (GameObject obj in ativar) {
                obj.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            foreach (GameObject obj in ativar) {
                obj.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            foreach (GameObject obj in ativar) {
                obj.SetActive(false);
            }
        }
    }

}
