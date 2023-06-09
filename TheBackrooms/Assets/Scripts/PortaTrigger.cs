using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaTrigger : MonoBehaviour {
    bool playerNaArea = false;

    Porta porta;

    void Awake() {
        porta = transform.parent.GetComponent<Porta>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = true;

            porta.UpdateDisponibilidade(!playerNaArea);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerNaArea = false;

            porta.UpdateDisponibilidade(!playerNaArea);
        }
    }
}
