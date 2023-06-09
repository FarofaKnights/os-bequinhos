using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaTrigger : MonoBehaviour {
    public GameObject indicador;
    bool playerNaArea = false;

    Porta porta;

    void Awake() {
        porta = transform.parent.GetComponent<Porta>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = true;

            if (indicador != null) {
                indicador.SetActive(true);
            }
        } else if (other.gameObject.tag == "Inimigo") {
            // porta.Coletar();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = false;

            if (indicador != null) {
                indicador.SetActive(false);
            }
        }
    }

    void Update() {
        if (playerNaArea && Input.GetKeyDown(KeyCode.E)) {
            if (porta != null && !porta.coletada) {
                porta.Coletar();
            }
        }

        if (indicador != null && playerNaArea) {
            indicador.transform.LookAt(Camera.main.transform);
        }
    }
}
